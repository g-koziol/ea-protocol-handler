/*
   Copyright 2009 Canonic Corp and Oliver Fels

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using EA.Core.Encryption;
using Microsoft.Win32;

namespace EA.Plugin.ProtocolHandler
{
    /// <summary>
    /// This class accepts URL requests in the format ea:// and redirects them to EA.
    /// 
    /// Requires the following registry entries:
    /// <example>
    /// [HKEY_CLASSES_ROOT]
    /// [ea]
    ///     (Default) = "URL:ea Protocol Handler"
    ///     URL Protocol = ""
    ///     [DefaultIcon]
    ///         (Default) = "C:\Program Files\Sparx Systems\EA\CanonicPlugins\Canonic.EAPlugin.ProtocolHandler.exe”
    ///     [shell]
    ///         [open]
    ///             [command]
    ///                 (Default) = "C:\Program Files\Sparx Systems\EA\CanonicPlugins\Canonic.EAPlugin.ProtocolHandler.exe "%1""
    /// </example>
    /// </summary>
    /// <remarks>Test URL: ea://EaMercuryIntegration?c=%2fgH%2bjil1ZOsHDun78LPBQlRSjB%2fJGN%2fc9RFs6xEVfsqnUqntn5aJYBDaebMdyumonnRjbNK%2fc9g%2bC0VtuW87WQ%3d%3d&e=8759F3EF2FB64b30AFA03025C17E77DB
    /// </remarks>
    /// <change> Oliver Fels, 2009-10-13 Additional feature: Make the connection string configurable if required</change>
    /// <change> Oliver Fels, 2009-10-15 Additional feature: Added a configurable default connection string</change>
    public class Program
    {
		/// <summary>
		/// Logger instance for this class type
		/// </summary>
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        #region COM Interop
        public static Guid IUnknown = new Guid("00000000-0000-0000-C000-000000000046");

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        private const int SW_HIDE = 0;
        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_SHOWNOACTIVATE = 4;
        private const int SW_RESTORE = 9;
        private const int SW_SHOWDEFAULT = 10;
        #endregion

        private const string ERROR_CAPTION = "Error in ea:// Handler";

        private enum ArtifactType
        {
            None = 0,
            Element = 1,
            Package = 2,
            Diagram = 3
        }

        /// <summary>
        /// Accepts a request in one of the formats:
        /// ea://RespositoryFriendlyName?c=Base64EncryptedConnectionString&e=ElementGuid
        /// ea://RespositoryFriendlyName?c=Base64EncryptedConnectionString&p=PackageGuid
        /// ea://RespositoryFriendlyName?c=Base64EncryptedConnectionString&d=DiagramGuid
        /// and opens EA, selecting the given GUID in the Project Browser (TreeView).
        /// All GUIDs are formatted without additional punctuation such as {-}
        /// </summary>
        /// <param name="args">Arguments from Windows (should be ea:// formatted URL)</param>
        [STAThread]
        static void Main(string[] args)
        {
#if DEBUG
            /* Create a listener that outputs to the console screen, and 
             * add it to the debug listeners. */
            TextWriterTraceListener tr2 = new TextWriterTraceListener(System.IO.File.CreateText("c:\\Output.txt"));
            Debug.Listeners.Add(tr2);
#endif
            string uri = string.Empty;

            args = Environment.GetCommandLineArgs();

			try
			{
				#region TraceBeginElement
				logger.Trace( @"Begin:Main ( args[string[]]:'{0}' )", args );
				#endregion // TraceBeginElement

				if( args.Length == 0 || args[0] == "ea:///" )
				{
					logger.Debug( "No arguments, showing about dialog" );

					string text =
						"This program accepts URL requests in the form: " + Environment.NewLine +
						"ea://RepositoryName?{specifier}" + Environment.NewLine +
						"and attempts to display the requested item in EA.  This capability " +
						"allows Intranet sites (such as portals, issue tracking systems, etc) to " +
						"link directly into EA. For more information visit: http://eaprotocol.sourceforge.net";

					FrmAbout about = new FrmAbout( text );
					about.ShowDialog();
				}
				else
				{
					string req = CombineArgs( args );
					if( !req.StartsWith( "ea://" ) )
					{
						logger.Error( "Request does not begin with ea://" );
						throw new UrlFormatException( req, "Protocol must start with ea://" );
					}

					uri = req.Substring( "ea://".Length );

					string[] uriParts = uri.Split( new char[] { '?' } );
					if( uriParts.Length == 1 )
					{
						logger.Error( "Request does not have query string parameters." );
						throw new UrlFormatException( req, "The URL must have query string parameters." );
					}
					else if( uriParts.Length > 2 )
					{
						logger.Error( "Request had too many '?'" );
						throw new UrlFormatException( req, "There were too many '?' in the URL.  Only one allowed." );
					}

					Binary bin = new Binary();

					Dictionary<string, string> qpDict = new Dictionary<string, string>();
					string[] qpParts = uriParts[1].Split( new char[] { '&' } );
					foreach( string qp in qpParts )
					{
						int eqIdx = qp.IndexOf( '=' );
						string name = qp.Substring( 0, eqIdx );
						string value = qp.Substring( eqIdx + 1 );
						qpDict.Add( name, value );
					}

					// We support 1 or 2 arguments depending on the invocation request
					if( qpDict.Count < 1 || qpDict.Count > 2 )
					{
						logger.Error( "Request has wrong number of parameters: " + qpDict.Count );
						throw new UrlFormatException( req, "The number of parameters specified in the URL is wrong. Valid parameters are " +
							"an encrypted connection string and/or the GUID of an element, package or diagram." );
					}

					string friendlyName = uriParts[0];
					if( friendlyName.EndsWith( "/" ) )
					{
						// MSIE adds a trailing slash
						friendlyName = friendlyName.Substring( 0, friendlyName.Length - 1 );
					}

					// Override the friendly name with the default [from the config file]
					if( ( !String.IsNullOrEmpty( friendlyName ) ) && ( String.Compare( friendlyName, "default", StringComparison.OrdinalIgnoreCase ) == 0 ) )
					{
						friendlyName = ConfigurationManager.AppSettings["default"];
					}

					string connectionString = null;

					if( !qpDict.ContainsKey( "c" ) )
					{
						logger.Debug( "Connection string not provided. Checking configuration file" );

						ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings[friendlyName];
						if( cs != null )
						{
							// Retrieve the connection string, not logged as could include sensitive information
							connectionString = cs.ConnectionString;
						}
					}
					else
					{
						string csEnc = qpDict["c"];
						logger.Debug( "Decoding connection string... '{0}'", csEnc );

						// Decode (if required) the data
						string urlEnc = ( !csEnc.Contains( "%" ) ) ? csEnc : System.Web.HttpUtility.UrlDecode( csEnc );
						logger.Debug( "Decrypting connection string... '{0}'", urlEnc );

						connectionString = bin.Decrypt( urlEnc );
					}

					// Verify that we have a connection string (may not be valid)
					if( String.IsNullOrEmpty( connectionString ) )
					{
						throw new UrlFormatException( req, "Connection string not provided or not configured." );
					}

					ArtifactType at = ArtifactType.None;
					string guid = null;

					if( qpDict.ContainsKey( "d" ) )
					{
						at = ArtifactType.Diagram;
						guid = bin.ExpandGuid( qpDict["d"] );
					}
					else if( qpDict.ContainsKey( "e" ) )
					{
						at = ArtifactType.Element;
						guid = bin.ExpandGuid( qpDict["e"] );
					}
					else if( qpDict.ContainsKey( "p" ) )
					{
						at = ArtifactType.Package;
						guid = bin.ExpandGuid( qpDict["p"] );
					}
					else
					{
						logger.Error( "Artifact type not specified." );
						throw new UrlFormatException( req, "Must specify the GUID of an element, package or diagram." );
					}

					logger.Trace( "Looking for running instances of EA" );

					EA.Repository rep = null;

					// Look for an EA instance already connected to the required repository
					EaInstanceInfo info = EaInterop.GetAppByConnectionName( friendlyName, connectionString );
					if( info != null && info.App != null )
					{
						logger.Debug( "Using existing instance of EA, process id: {0}", info.ProcessId );
						rep = info.App.Repository;
					}
					else
					{
						// Could not find a running instance of EA with the desired connection
						logger.Debug( "Starting new instance of EA." );
						info = EaInterop.NewEaInstance( friendlyName, connectionString );
						if( info != null && info.App != null )
						{
							logger.Debug( "New instance of EA started, process if: {0}", info.ProcessId );
							rep = info.App.Repository;
						}
					}

					// Verify that we have a repository object to use
					if( rep == null )
					{
						logger.Error( "Could not find/start an instance of EA." );
						Error.Show( "Error opening EA.", "Ensure that EA is installed and working properly." );
						return;
					}

					info.App.Visible = true;

					switch( at )
					{
						case ArtifactType.Element:
							try
							{
								EA.Element elem = rep.GetElementByGuid( guid );
								rep.ShowInProjectView( elem );
							}
							catch( Exception systemException )
							{
								logger.Error( "Could not load requested artifact, element id {0}", guid );
								throw new UrlProcessingException( req, "Could not load requested element " + guid + ". " +
									"The GUID of the element you specified may not be valid for the current repository. " +
								systemException.Message );
							}
							break;

						case ArtifactType.Package:
							try
							{
								EA.Package pack = rep.GetPackageByGuid( guid );
								rep.ShowInProjectView( pack );
							}
							catch( Exception systemException )
							{
								logger.Error( "Could not load requested artifact, package id {0}", guid );
								throw new UrlProcessingException( req, "Could not load requested package " + guid + ". " +
									"The GUID of the package you specified may not be valid for the current repository. " +
								systemException.Message );
							}
							break;

						case ArtifactType.Diagram:
							try
							{
								EA.Diagram diag = (EA.Diagram) rep.GetDiagramByGuid( guid );
								rep.ShowInProjectView( diag );
								rep.OpenDiagram( diag.DiagramID );
							}
							catch( Exception systemException )
							{
								logger.Error( "Could not load requested artifact, diagram id {0}", guid );
								throw new UrlProcessingException( req, "Could not load requested diagram " + guid + ". " +
									"The GUID of the diagram you specified may not be valid for the current repository. " +
								systemException.Message );
							}
							break;
						default:
							logger.Error( "Artifact type '{0}' not supported.", at );
							throw new UrlProcessingException( req, "Artifact type not supported." );
					}

					rep.ShowWindow( 1 );

					Process eaProc = Process.GetProcessById( info.ProcessId );
					IntPtr hWindow = eaProc.MainWindowHandle;
					if( IsIconic( hWindow ) )
					{
						ShowWindowAsync( hWindow, SW_RESTORE );
					}
					SetForegroundWindow( hWindow );
				}
			}
			catch( UrlFormatException ufe )
			{
				#region TraceException
				logger.WarnException( String.Format( CultureInfo.CurrentCulture, @"Exception:Main ( args[string[]]:'{0}' ) - Error in URL format: {1} {2}", args, ufe.Message, ufe.Url ), ufe );
				#endregion // TraceException
				Error.Show( "Error in URL format", ufe.Message, ufe.Url );
			}
			catch( UrlProcessingException upe )
			{
				#region TraceException
				logger.WarnException( String.Format( CultureInfo.CurrentCulture, @"Exception:Main ( args[string[]]:'{0}' ) - Error in URL format: {1} {2}", args, upe.Message, upe.Url ), upe );
				#endregion // TraceException
				Error.Show( "Error processing URL", upe.Message, upe.Url );
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( String.Format( CultureInfo.CurrentCulture, @"Exception:Main ( args[string[]]:'{0}' )", args ), systemException );
				#endregion // TraceException
				Error.Show( "There was a problem with the ea:// protocol handler.",
					"This is a catch-all error message.  It is shown because something happened that " +
					"the program does not know how to handle.  The error is: " + Environment.NewLine + Environment.NewLine +
					systemException.Message + Environment.NewLine + Environment.NewLine +
					"URI: " + uri );
			}
			finally
			{
				#region TraceEndElement
				logger.Trace( @"End:Main ( args[string[]]:'{0}' )", args );
				#endregion // TraceEndElement
			}
        }

        /// <summary>
        /// Apparently Windows interprets %20 (space) characters in the URL as a space separator and 
        /// begins a new command line argument.  We want to include the space character in the processed
        /// argument, so recombine the args with a space separator between each one.
        /// </summary>
        /// <param name="args">Array of arguments passed in by Windows.</param>
        /// <returns>Single combined argument.</returns>
        static private string CombineArgs(string[] args)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 1; i < args.Length - 1; i++)
            {
                sb.Append(args[i]).Append(" ");
            }

            sb.Append(args[args.Length - 1]);

            return sb.ToString();
        }
    }
}
