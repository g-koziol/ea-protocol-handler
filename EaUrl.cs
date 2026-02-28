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

namespace Canonic.Encryption
{
    /// <summary>
    /// Utilities for managing ea:// URLs
    /// </summary>
    public class EaUrl
    {
        /// <summary>
        /// Makes the element URL.
        /// </summary>
        /// <param name="rep">The rep.</param>
        /// <param name="guid">The GUID.</param>
        /// <returns></returns>
        static public string MakeElementUrl(EA.Repository rep, string guid)
        {
            return MakeUrl(rep, "e", guid);
        }

        /// <summary>
        /// Makes the package URL.
        /// </summary>
        /// <param name="rep">The rep.</param>
        /// <param name="guid">The GUID.</param>
        /// <returns></returns>
        static public string MakePackageUrl(EA.Repository rep, string guid)
        {
            return MakeUrl(rep, "p", guid);
        }

        /// <summary>
        /// Makes the diagram URL.
        /// </summary>
        /// <param name="rep">The rep.</param>
        /// <param name="guid">The GUID.</param>
        /// <returns></returns>
        static public string MakeDiagramUrl(EA.Repository rep, string guid)
        {
            return MakeUrl(rep, "d", guid);
        }

        static private string MakeUrl(EA.Repository rep, string prefix, string guid)
        {
            string ret;

            StringBuilder sb = new StringBuilder();
            Binary bin = new Binary();

            string connectionString = rep.ConnectionString;
            string[] parts = connectionString.Split(new string[] { " --- " }, StringSplitOptions.None);
            string enc = bin.Encrypt(parts[1]);

            sb.Append("ea://").Append(System.Web.HttpUtility.UrlEncode(parts[0]));
            sb.Append("?c=").Append(System.Web.HttpUtility.UrlEncode(enc));
            sb.Append("&").Append(prefix).Append("=").Append(bin.CompactGuid(guid)); // Reduce len of GUID?

            ret = sb.ToString();

            return ret;
        }
    }
}
