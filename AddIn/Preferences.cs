/*
   Copyright 2009 Adam Hearn

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
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

namespace EA.ProtocolHandler.AddIn
{
	/// <summary>
	/// Encapsulated preferences object used for the AddIn
	/// </summary>
	/// <remarks>
	/// Should be serializeable using the XmlSerializer
	/// </remarks>
	public class Preferences
	{
		/// <summary>
		/// Logger instance for this class type
		/// </summary>
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Name of the preferences file
		/// </summary>
		private const string PreferencesFile = "EAProtocolHandlerPreferences.xml";

		/// <summary>
		/// Gets or sets the connection type property
		/// </summary>
		/// <remarks>
		/// When true, the current repository's connection string will be used. When false,
		/// will mean a default repository name will be applied unless UserDefinedKey is set.
		/// </remarks>
		public bool UseCurrent { get; set; }

		/// <summary>
		/// Gets or sets the user defined key
		/// </summary>
		/// <remarks>
		/// When set, the EA Protocol Handler will use this value as a key to
		/// lookup a specific connection string
		/// </remarks>
		public string UserDefinedKey { get; set; }

		/// <summary>
		/// Initializes a new instance of the Preferences class
		/// </summary>
		/// <remarks>
		/// The default constructor initializes any fields to their default values.
		/// </remarks>
		public Preferences()
		{
			try
			{
				#region TraceBeginElement
				logger.Trace( "Begin:Preferences" );
				#endregion // TraceBeginElement

				UseCurrent = true;
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
				logger.Trace( "End:Preferences" );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Read the AddIn preferences data
		/// </summary>
		/// <remarks>
		/// Preferences are stored in a file placed in Isolated storage. The
		/// file contains an XML serialised copy of the data.
		/// </remarks>
		/// <returns>A retrieved preferences instance</returns>
		public static Preferences Read()
		{
			try
			{
				#region TraceBeginElement
				logger.Trace( "Begin:Read" );
				#endregion // TraceBeginElement

				Preferences newPreferences = null;

				using( IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore( IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null ) )
				{
					logger.Debug( "Attached to Isolated storage, reading settings using XmlSerializer" );
					try
					{
						using( StreamReader streamReader = new StreamReader( new IsolatedStorageFileStream( PreferencesFile, FileMode.Open, isoStore ) ) )
						{
							XmlSerializer xml = new XmlSerializer( typeof( Preferences ) );
							newPreferences = (Preferences) xml.Deserialize( streamReader );
						}
					}
					catch( FileNotFoundException fileNotFoundException )
					{
						logger.InfoException( "Failed to read settings", fileNotFoundException );

						// Probably the first time it's run, create default preferences
						newPreferences = new Preferences();
					}
				}

				#region TraceReturnElement
				logger.Trace( "Return:Read [EA.ProtocolHandler.AddIn.Preferences] : '{0}'", ( newPreferences != null ) ? newPreferences.ToString() : "null" );
				#endregion // TraceReturnElement
				return newPreferences;
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
				logger.Trace( "End:Read" );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Persist the preferences
		/// </summary>
		/// <remarks>
		/// Preferences are stored in a file placed in Isolated storage. The
		/// file contains an XML serialised copy of the data.
		/// </remarks>
		public void Write()
		{
			try
			{
				#region TraceBeginElement
				logger.Trace( "Begin:Write" );
				#endregion // TraceBeginElement

				logger.Debug( "Persisting preferences object: {0}", ToString() );
				using( IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore( IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null ) )
				{
					logger.Debug( "Attached to Isolated storage, writing settings using XmlSerializer" );
					using( StreamWriter streamWriter = new StreamWriter( new IsolatedStorageFileStream( PreferencesFile, FileMode.Create, isoStore ) ) )
					{
						XmlSerializer xml = new XmlSerializer( typeof( Preferences ) );
						xml.Serialize( streamWriter, this );
					}
				}
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
				logger.Trace( "End:Write" );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Returns a System.String that represents the current System.Object.
		/// </summary>
		/// <remarks>
		/// Provided to support logging of the content. Not logged itself.
		/// </remarks>
		/// <returns>A System.String that represents the current System.Object.</returns>
		public override string ToString()
		{
			return String.Format( "Use current : {0}, User defined key: {1}", UseCurrent, ( String.IsNullOrEmpty( UserDefinedKey ) ) ? "null" : UserDefinedKey );
		}
	}
}