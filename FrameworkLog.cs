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

namespace Canonic.EAPlugin.ProtocolHandler
{
    /// <summary>
    /// This was originally a robust proprietary logging framework.
    /// </summary>
    internal class FrameworkLog
    {
		/// <summary>
		/// Logger instance for this class type
		/// </summary>
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        static public void Write(string text)
        {
			// For now, just generate all logging at Trace level
			logger.Trace( text );
        }
    }
}
