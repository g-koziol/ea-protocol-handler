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
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;
using EA.Core;

namespace EA.Plugin.ProtocolHandler
{
	/// <summary>
	/// The central purpose of this class is to locate an appropriate instance of EA if one
	/// is running, and if not launch a new one.  Appropriate means that the repository 
	/// indicated in the URL is currently loaded.
	/// </summary>
	/// <remarks>
	/// The Running Object Table (ROT) is used to enumerate any existing instances of EA.
	/// </remarks>
	public static class EaInterop
	{
		/// <summary>
		/// Logger instance for this class type
		/// </summary>
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Prefix used for Sparx Systems EA COM object
		/// </summary>
		private const string EA_OBJECT_PREFIX = "!Sparx.EA.App:";

		/// <summary>
		/// Create a new instance of EA
		/// </summary>
		/// <remarks>
		/// A new instance of EA is required when any active instances do not have the
		/// required project open.
		/// </remarks>
		/// <param name="path">Path to EA application</param>
		/// <returns>Reference to a new instance of EA</returns>
		public static EaInstanceInfo NewEaInstance( string friendlyname, string path )
		{
			try
			{
				#region TraceBeginElement
				logger.Trace( @"Begin:NewEaInstance ( path[string]:'{0}' )", path );
				#endregion // TraceBeginElement

				int maxSecWait = 5;
				int mSecDelay = 100;

				// Obtain the EA installation path
				string installPath = EaInstallPath();

				// Append the 'well-known' EA process name and start it
				logger.Trace( "Starting EA process." );
				Process eaProc = Process.Start( installPath + @"\ea.exe" );

				// TODO Sleep for a short while to let the EA process initialise itself into the ROT
				Thread.Sleep( 5000 );

				DateTime start = DateTime.Now;
				EaInstanceInfo info = null;

				while( info == null )
				{
					logger.Debug( "Waiting for EA to initialize." );

					// We know the id of the process just started, obtain the ROT entry for it
					info = GetAppByProcessId( eaProc.Id );
					if( info == null )
					{
						logger.Debug( "Process still not running." );

						// Determine if we should give up waiting for the process to initialise
						TimeSpan ts = new TimeSpan( DateTime.Now.Ticks - start.Ticks );
						if( ts.TotalSeconds >= maxSecWait )
						{
							logger.Error( "Process {0} started but EA either not running or has unexpected context.", eaProc.Id );
							Error.Show( "EA did not start in time.", "EA was not running (or was running a different " +
								"repository).  The ea:// protocol handler tried to start it but it did not come up " +
								"properly after waiting " + maxSecWait.ToString() + " seconds." );

							#region TraceReturnElement
							logger.Trace( "Return:NewEaInstance [EA.Plugin.ProtocolHandler.EaInstanceInfo] : 'null'" );
							#endregion // TraceReturnElement
							return null;
						}

						// Wait for a short duration
						Thread.Sleep( mSecDelay );
					}
				}

				try
				{
					// Open the required project
					logger.Trace( "Getting repository and opening project." );
					EA.Repository rep = info.App.Repository;
					rep.OpenFile( GetFullConnectionString( friendlyname, path ) );
				}
				catch( COMException comex )
				{
					logger.FatalException( "COM exception whilst accessing repository: {0}", comex );
					if( comex.Message == "Not a valid file name." )
					{
						Error.Show( "The EA repository '" + path + "' could not be opened.",
							"The connection string or file name is not available or is invalid." );
					}
					else
					{
						Error.Show( "There was a problem opening the EA repository '" + path + "'",
							comex.Message );
					}

					#region TraceReturnElement
					logger.Trace( "Return:NewEaInstance [EA.Plugin.ProtocolHandler.EaInstanceInfo] : 'null'" );
					#endregion // TraceReturnElement
					return null;
				}

				#region TraceReturnElement
				logger.Trace( "Return:NewEaInstance [EA.Plugin.ProtocolHandler.EaInstanceInfo] : '{0}'", ( info != null ) ? info.ToString() : "null" );
				#endregion // TraceReturnElement
				return info;
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( String.Format( CultureInfo.CurrentCulture, @"Exception:NewEaInstance ( path[string]:'{0}' )", path ), systemException );
				#endregion // TraceException
				throw;
			}
			finally
			{
				#region TraceEndElement
				logger.Trace( @"End:NewEaInstance ( path[string]:'{0}' )", path );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Retrieves the required EA process using it's PID
		/// </summary>
		/// <remarks>
		/// Uses the ROT to find the EA App object that will have been registrered.
		/// </remarks>
		/// <param name="pid">Id of the EA process to locate</param>
		/// <returns>A populated EaInstanceInfo when the process is located. null when not.</returns>
		public static EaInstanceInfo GetAppByProcessId( int pid )
		{
			try
			{
				#region TraceBeginElement
				logger.Trace( @"Begin:GetAppByProcessId ( pid[int]:'{0}' )", pid );
				#endregion // TraceBeginElement

				// Obtain the ROT
				Dictionary<string, object> rotNames = RunningObjectTable.GetRunningObjectTable(); // .GetRunningCOMObjectNames();
				List<object> eaAppObjects = new List<object>();

				string desiredAppObjectName = EA_OBJECT_PREFIX + pid.ToString();
				if( rotNames.ContainsKey( desiredAppObjectName ) )
				{
					// Grab the application reference from the ROT using the object name
					EA.App app = (EA.App) rotNames[desiredAppObjectName];

					#region TraceReturnElement
					logger.Trace( "Return:GetAppByProcessId [EA.Plugin.ProtocolHandler.EaInstanceInfo] : Process found" );
					#endregion // TraceReturnElement
					return new EaInstanceInfo( app, pid );
				}

				logger.Warn( "GetAppByProcessId NOT FOUND" );

				#region TraceReturnElement
				logger.Trace( "Return:GetAppByProcessId [EA.Plugin.ProtocolHandler.EaInstanceInfo] : 'null'" );
				#endregion // TraceReturnElement
				return null;
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( String.Format( CultureInfo.CurrentCulture, @"Exception:GetAppByProcessId ( pid[int]:'{0}' )", pid ), systemException );
				#endregion // TraceException
				throw;
			}
			finally
			{
				#region TraceEndElement
				logger.Trace( @"End:GetAppByProcessId ( pid[int]:'{0}' )", pid );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Retrieves the required EA process using it's PID
		/// </summary>
		/// <remarks>
		/// Uses the ROT to find the EA App object that will have been registrered.
		/// </remarks>
		/// <param name="pid">Id of the EA process to locate</param>
		/// <returns>A populated EaInstanceInfo when the process is located. null when not.</returns>
		public static EaInstanceInfo GetAppByConnectionName( string friendlyName, string connectionString )
		{
			try
			{
				#region TraceBeginElement
				logger.Trace( @"Begin:GetAppByConnectionName ( friendlyName[string]:'{0}', connectionString[string]:'{1}' )", friendlyName, connectionString );
				#endregion // TraceBeginElement

				// Obtain the ROT
				Dictionary<string, object> rotNames = RunningObjectTable.GetRunningObjectTable();
				List<EaInstanceInfo> eaProcInfos = new List<EaInstanceInfo>();

				string fullConnection = GetFullConnectionString( friendlyName, connectionString );
				logger.Debug( "Scanning ROT for EA instance." );
				foreach( string name in rotNames.Keys )
				{
					if( name.StartsWith( EA_OBJECT_PREFIX ) )
					{
						// We've located an instance of EA. Add it's details into the collection to scan later
						logger.Debug( "Located ROT entry for EA, extract the process PID" );
						int pid = int.Parse( name.Substring( EA_OBJECT_PREFIX.Length ) );
						EA.App app = (EA.App) rotNames[name];
						eaProcInfos.Add( new EaInstanceInfo( app, pid ) );
					}
				}
				
				// Declare variable used when finding EA instances that are not attached to a repository
				EaInstanceInfo notConnectedAppinfo = null;

				// Now scan the EA instance collection looking for an instance connected to the required repository
				logger.Trace( "Now trying to locate a matching connection strings for any matching EA instance." );
				foreach( EaInstanceInfo info in eaProcInfos )
				{
					EA.App app = info.App;
					if( app != null )
					{
						// Extract the connection string
						string instanceConnectionString = app.Repository.ConnectionString;
						if( String.IsNullOrEmpty( instanceConnectionString ) )
						{
							// EA running but not connected to a repository
							notConnectedAppinfo = info;
							continue;
						}
						if( fullConnection.EndsWith( instanceConnectionString ) )
						{
							logger.Trace( "Found an instance that is connected to the required repository." );
							#region TraceReturnElement
							logger.Trace( "Return:GetAppByConnectionName [EA.Plugin.ProtocolHandler.EaInstanceInfo] : '{0}'", ( info != null ) ? info.ToString() : "null" );
							#endregion // TraceReturnElement
							return info;
						}
					}
				}

				if( notConnectedAppinfo != null )
				{
					logger.Trace( "Found at least one EA instance that is not connected, take over the session." );

					// Found at least one EA instance that is not connected, recycle the process
					notConnectedAppinfo.App.Repository.OpenFile( fullConnection );

					#region TraceReturnElement
					logger.Trace( "Return:GetAppByConnectionName [EA.Plugin.ProtocolHandler.EaInstanceInfo] : '{0}'", ( notConnectedAppinfo != null ) ? notConnectedAppinfo.ToString() : "null" );
					#endregion // TraceReturnElement
					return notConnectedAppinfo;
				}

				logger.Warn( "Found no instances that are connected to the required repository." );
				#region TraceReturnElement
				logger.Trace( "Return:GetAppByConnectionName [EA.Plugin.ProtocolHandler.EaInstanceInfo] : 'null'" );
				#endregion // TraceReturnElement
				return null;
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( String.Format( CultureInfo.CurrentCulture, @"Exception:GetAppByConnectionName ( friendlyName[string]:'{0}', connectionString[string]:'{1}' )", friendlyName, connectionString ), systemException );
				#endregion // TraceException
				throw;
			}
			finally
			{
				#region TraceEndElement
				logger.Trace( @"End:GetAppByConnectionName ( friendlyName[string]:'{0}', connectionString[string]:'{1}' )", friendlyName, connectionString );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Helper method to obtain the installation path for EA
		/// </summary>
		/// <remarks>
		/// Uses the Registry so it's assumed the caller has read access to the required keys
		/// and that EA has been run at least once.
		/// </remarks>
		/// <returns>EA installation path or String.Empty if this cannot be determined</returns>
		private static string EaInstallPath()
		{
			try
			{
				#region TraceBeginElement
				logger.Trace( "Begin:EaInstallPath" );
				#endregion // TraceBeginElement

				// Look in the registry
				string keyPath = @"Software\Sparx Systems\EA400\EA\";
				using( RegistryKey regKey = Registry.CurrentUser.OpenSubKey( keyPath ) )
				{
					string value = regKey.GetValue( "Install Path" ) as string;
					if( !String.IsNullOrEmpty( value ) )
					{
						#region TraceReturnElement
						logger.Trace( "Return:EaInstallPath [System.String] : '{0}'", ( value != null ) ? value.ToString() : "null" );
						#endregion // TraceReturnElement
						return value;
					}
				}

				// If we've got here the required registry entry was not found
				Error.Show( "Could not find the Install Path registry key for EA", "Please ensure that EA has been properly install and works in this PC." );

				#region TraceReturnElement
				logger.Trace( "Return:EaInstallPath [System.String] : '{0}'", ( String.Empty != null ) ? String.Empty.ToString() : "null" );
				#endregion // TraceReturnElement
				return string.Empty;
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( systemException.Message, systemException );
				#endregion // TraceException
				throw;
			}
			finally
			{
				#region TraceEndElement
				logger.Trace( "End:EaInstallPath" );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Helper method to build a EA repository connection string
		/// </summary>
		/// <remarks>
		/// Only used for 
		/// </remarks>
		/// <param name="friendlyName"></param>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		private static string GetFullConnectionString( string friendlyName, string connectionString )
		{
			if( ( String.IsNullOrEmpty( friendlyName ) ||
				( String.Compare( friendlyName, "eap", StringComparison.OrdinalIgnoreCase ) == 0 ) ) )
			{
				return connectionString;
			}

			// It's not an EAP, return back a connection string for RDBMS repositories
			return String.Format( "{0} --- {1}", friendlyName, connectionString );
		}
	}
}
