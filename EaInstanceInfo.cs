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
    public class EaInstanceInfo
    {
        private int processId;

        public int ProcessId
        {
            get { return processId; }
        }
        private EA.App app;

        public EA.App App
        {
            get { return app; }
        }

        public EaInstanceInfo(EA.App app, int processId)
        {
            this.app = app;
            this.processId = processId;
        }
    }
}
