/*
   Copyright 2009 Canonic Corp

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
using System.Globalization;
using System.Text;

namespace EA.ProtocolHandler.Utility
{
	/// <summary>
	/// Base class for a EaURL type in support of all artifacts
	/// </summary>
	/// <remarks>
	/// Marked as abstract to force implementation of derived types
	/// </remarks>
	public abstract class EaUrl
	{
		/// <summary>
		/// Logger instance for this class type
		/// </summary>
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Makes the required URL for the specific artifact type
		/// </summary>
		/// <remarks>
		/// Marked as abstract to force implementation in derived class
		/// </remarks>
		/// <param name="request">The request object</param>
		/// <returns>URL for the artifact type</returns>
		public abstract string Create( EaUrlRequest request );

		/// <summary>
		/// Common method for building a URL from a request and artifact type
		/// </summary>
		/// <param name="request">URL Request object</param>
		/// <param name="prefix">Artifact type</param>
		/// <returns>EA Handler URL</returns>
		protected string MakeUrl( EaUrlRequest request, string prefix )
		{
			try
			{
				#region TraceBeginElement
				logger.Trace( @"Begin:MakeUrl ( request[EA.ProtocolHandler.Utility.EaUrlRequest]:'{0}', prefix[string]:'{1}' )", request, prefix );
				#endregion // TraceBeginElement

				string ret;
				string queryChar = "&";

				StringBuilder sb = new StringBuilder();

				// Create the URL with protocol and repository
				sb.AppendFormat( "ea://{0}", request.RepositoryName );

				// If the connection string is the current one encrypt it
				if( !String.IsNullOrEmpty( request.ConnectionString ) )
				{
					logger.Debug( "Processing current connection string: {0}", request.ConnectionString );

					string enc = Binary.Encrypt( request.ConnectionString );
					sb.Append( "?c=" ).Append( System.Web.HttpUtility.UrlEncode( enc ) );
				}
				else if( !String.IsNullOrEmpty( request.ConnectionString ) )
				{
					logger.Debug( "Processing user supplied connection string: {0}", request.ConnectionString );
					sb.Append( "?c=" ).Append( System.Web.HttpUtility.UrlEncode( request.ConnectionString ) );
				}
				else
				{
					logger.Debug( "Connection string not supplied" );
					queryChar = "?";
				}

				// Add the query to the end of the URL
				sb.Append( queryChar ).Append( prefix ).Append( "=" ).Append( Binary.CompactGuid( request.ArtifactId ) );

				ret = sb.ToString();

				#region TraceReturnElement
				logger.Trace( "Return:MakeUrl [System.String] : '{0}'", ( ret != null ) ? ret.ToString() : "null" );
				#endregion // TraceReturnElement
				return ret;
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( String.Format( CultureInfo.CurrentCulture, @"Exception:MakeUrl ( request[EA.ProtocolHandler.Utility.EaUrlRequest]:'{0}', prefix[string]:'{1}' )", request, prefix ), systemException );
				#endregion // TraceException
				throw;
			}
			finally
			{
				#region TraceEndElement
				logger.Trace( @"End:MakeUrl ( request[EA.ProtocolHandler.Utility.EaUrlRequest]:'{0}', prefix[string]:'{1}' )", request, prefix );
				#endregion // TraceEndElement
			}
		}
	}

	/// <summary>
	/// Implementation of a EaURL type for Diagram artifacts
	/// </summary>
	public class EaDiagramUrl : EaUrl
	{
		/// <summary>
		/// Makes the required URL for the specific artifact type
		/// </summary>
		/// <param name="request">The request object</param>
		/// <returns>URL for the artifact type</returns>
		public override string Create( EaUrlRequest request )
		{
			return MakeUrl( request, "d" );
		}
	}

	/// <summary>
	/// Implementation of a EaURL type for Package artifacts
	/// </summary>
	public class EaPackageUrl : EaUrl
	{
		/// <summary>
		/// Makes the required URL for the specific artifact type
		/// </summary>
		/// <param name="request">The request object</param>
		/// <returns>URL for the artifact type</returns>
		public override string Create( EaUrlRequest request )
		{
			return MakeUrl( request, "p" );
		}
	}

	/// <summary>
	/// Implementation of a EaURL type for Element artifacts
	/// </summary>
	public class EaElementUrl : EaUrl
	{
		/// <summary>
		/// Makes the required URL for the specific artifact type
		/// </summary>
		/// <param name="request">The request object</param>
		/// <returns>URL for the artifact type</returns>
		public override string Create( EaUrlRequest request )
		{
			return MakeUrl( request, "e" );
		}
	}
}
