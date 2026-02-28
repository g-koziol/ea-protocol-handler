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
using System.Net;
using System.IO;

namespace EA.Core.Utility
{
    /// <summary>
    /// Encapsulates access to online lookup of information about Canonic software.
    /// </summary>
    static public class SoftwareInfo
    {
        static string versionUrl = _getUrl();
       
        /// </summary>
        /// <returns></returns>
        static private string _getUrl()
        {
            string _productionUrl = "http://eaprotocol.sourceforge.net";
            string _defaultUrl = _productionUrl;

#if DEBUG 
            _defaultUrl = _debugURL;

            try //put in tryc/catch just in case .GetHostName would fail 
            {
                string _hostname = System.Net.Dns.GetHostName().ToUpper();
                switch (_hostname)
                {
                    case "PG": //PGs laptop 080123
                        _defaultUrl = _productionUrl;
                        break;
                    case "RAVEN-V32": //PGs desktop 080123
                        _defaultUrl = _productionUrl;
                        break;
                    case "ERIC-LAPTOP": //Creative name, eh?
                        _defaultUrl = _productionUrl;
                        break;
                    default:
                        break;
                }
            }
            catch
            {
            }
#endif

        return _defaultUrl;
        }

/*        /// <summary>
        /// Discovers the most current version of the given software.
        /// </summary>
        /// <param name="productName">Name of the software as specified in the installer
        /// that can be downloaded from the Canonic website.  The name can be found by
        /// right-clicking on the file in Windows Explorer, General tab, Description.</param>
        /// <returns>The current version of the product, or string.Empty if no match found.</returns>
        static public string CurrentVersion(string productName)
        {
            string result = string.Empty;
            string version = string.Empty;

            try
            {
                WebResponse response;
                WebRequest request = System.Net.HttpWebRequest.Create(versionUrl + productName);
                request.Timeout = 3000;
                response = request.GetResponse();
                using (StreamReader sr =
                   new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    // Close and clean up the StreamReader
                    sr.Close();
                }

                string[] parts = result.Split(new string[] { "<body>", "</body>" }, StringSplitOptions.None);

                if (parts.Length == 3)
                {

                    version = parts[1].Replace("\r", "").Replace("\n", "").Replace(" ", "");
                    if (version == "?") // Response from aspx page if error or could not find product
                    {
                        version = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
#warning We should log the exception ... need the generic logging framework.
            }

            return version;

        }*/
    }
}
