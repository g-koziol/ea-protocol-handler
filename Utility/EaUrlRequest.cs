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

namespace EA.ProtocolHandler.Utility
{
	/// <summary>
	/// Class that handles Url requests
	/// </summary>
	/// <remarks>
	/// No logic provided, simply a property bag.
	/// </remarks>
	public class EaUrlRequest
	{
		/// <summary>
		/// Gets or sets the Connection string
		/// </summary>
		/// <remarks>
		/// May be String.Empty, null or populated
		/// </remarks>
		public string ConnectionString { get; set; }
	
		/// <summary>
		/// Gets or sets the name of the EA repository
		/// </summary>
		/// <remarks>
		/// EA repositories are usually assigned a name when the connection string is defined
		/// </remarks>
		public string RepositoryName { get; set; }

		/// <summary>
		/// Gets or sets the id of the EA artifact
		/// </summary>
		/// <remarks>
		/// A Guid is string format than is the unique id of the EA artifact
		/// </remarks>
		public string ArtifactId { get; set; }
	}

}
