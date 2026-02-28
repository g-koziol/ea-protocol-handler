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
using System.Collections.Generic;
using System.Text;

namespace EA.Plugin.ProtocolHandler
{
    /// <summary>
    /// URL Processing Exception.
    /// </summary>
    public class UrlProcessingException : Exception
    {
        private string url;

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url
        {
            get { return url; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlProcessingException"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="message">The message.</param>
        public UrlProcessingException(string url, string message)
            : base(message)
        {
            this.url = url;
        }
    }
}
