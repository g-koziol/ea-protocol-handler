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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace EA.Plugin.ProtocolHandler
{
    /// <summary>
    /// Running Object Table wrapper.
    /// </summary>
	/// <remarks>
	/// The Running Object Table (ROT) is a table containing entries for each registered
	/// COM object that is current running. It's commonly used for invocation or interprocess
	/// communication.
	/// </remarks>
    internal static class RunningObjectTable
    {
		/// <summary>
		/// Logger instance for this class type
		/// </summary>
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Returns the ROT content as a Dictionary.
		/// </summary>
		/// <remarks>
		/// Entries in the ROT are identified by their unique name (Dictionary key).
		/// </remarks>
		/// <returns>Dictionary containing items from the ROT or an empty Dictionary should there be no
		/// items in the ROT.</returns>
		public static Dictionary<string,object> GetRunningObjectTable()
		{
			IRunningObjectTable runningObjectTable = null;
			IEnumMoniker enumMoniker = null;

			try
			{
				#region TraceBeginElement
				logger.Trace( "Begin:GetRunningObjectTable" );
				#endregion // TraceBeginElement

				// Create a default collection for return
				Dictionary<string, object> retCollection = new Dictionary<string, object>();

				IntPtr count = IntPtr.Zero;
				IMoniker[] monikers = new IMoniker[1];

				// Grab the ROT instance
				GetRunningObjectTable( 0, out runningObjectTable );

				// Iterate through the ROT entries
				runningObjectTable.EnumRunning( out enumMoniker );
				enumMoniker.Reset();
				while( enumMoniker.Next( 1, monikers, count ) == 0 )
				{
					// Obtain the name of the running object
					IBindCtx bindCtx;
					CreateBindCtx( 0, out bindCtx );
					string objectName;
					monikers[0].GetDisplayName( bindCtx, null, out objectName );

					// Obtain the COM object reference
					object objectReference;
					runningObjectTable.GetObject( monikers[0], out objectReference );

					// Add the item to the collection keyed by the object name
					retCollection[objectName] = objectReference;

					// Release the bound object
					bindCtx.ReleaseBoundObjects();
				}

				#region TraceReturnElement
				logger.Trace( "Return:GetRunningObjectTable [System.Collections.Generic.Dictionary<System.String,System.Object>] : '{0}'", retCollection.Count );
				#endregion // TraceReturnElement
				return retCollection;
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
				// Deference the COM objects
				if( runningObjectTable != null )
				{
					Marshal.FinalReleaseComObject( runningObjectTable );
				}
				if( enumMoniker != null )
				{
					Marshal.FinalReleaseComObject( enumMoniker );
				}

				#region TraceEndElement
				logger.Trace( "End:GetRunningObjectTable" );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Returns a pointer to the IRunningObjectTable interface on the local running object table (ROT).
		/// </summary>
		/// <param name="reserved">reserved</param>
		/// <param name="pRunningObjectTable">Reference to the ROT</param>
		/// <returns>Standard COM result</returns>
		[DllImport( "ole32.dll" )]
		public static extern int GetRunningObjectTable( int reserved, out IRunningObjectTable pRunningObjectTable );

		/// <summary>
		/// Returns a pointer to an implementation of IBindCtx (a bind context object).
		/// This object stores information about a particular moniker-binding operation.
		/// </summary>
		/// <param name="reserved">reserved</param>
		/// <param name="pbindCtx"></param>
		/// <returns>Standard COM result</returns>
		[DllImport( "ole32.dll" )]
		public static extern int CreateBindCtx( int reserved, out IBindCtx pbindCtx );
    }
}