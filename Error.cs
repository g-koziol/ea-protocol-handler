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
using System.Windows.Forms;

namespace EA.Plugin.ProtocolHandler
{
    /// <summary>
    /// General error handler.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Shows the error.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="explanation">The explanation.</param>
        static public void Show(string title, string explanation)
        {
            FrmError error = new FrmError(title, explanation);
            error.ShowDialog();
        }

        /// <summary>
        /// Shows the error.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="explanation">The explanation.</param>
        /// <param name="url">The URL.</param>
        static public void Show(string title, string explanation, string url)
        {
            FrmError error = new FrmError(title, explanation, url);
            error.ShowDialog();
        }
    }
}
