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

namespace Canonic.EAPlugin.Interface
{
    #region Delegate Defs
    public delegate void EABroadcastBasic(EA.Repository rep);
    public delegate void EABroadcastInfo(EA.Repository rep, EA.EventProperties info);
    public delegate void EABroadcastGuid(EA.Repository rep, string guid, EA.ObjectType ot);
    #endregion

    #region Delegates
    public class EABroadcastEvents
    {
        public static event EABroadcastBasic FileOpenDelegate;
        static public void FileOpen(EA.Repository rep)
        {
            if (FileOpenDelegate != null) FileOpenDelegate(rep);
        }

        public static event EABroadcastBasic FileCloseDelegate;
        static public void FileClose(EA.Repository rep)
        {
            if (FileCloseDelegate != null) FileCloseDelegate(rep);
        }

        public static event EABroadcastInfo OnPreDeleteElementDelegate;
        public static void OnPreDeleteElement(EA.Repository rep, EA.EventProperties info)
        {
            if (OnPreDeleteElementDelegate != null) OnPreDeleteElementDelegate(rep, info);
        }

        public static event EABroadcastInfo OnPreDeleteConnectorDelegate;
        public static void OnPreDeleteConnector(EA.Repository rep, EA.EventProperties info)
        {
            if (OnPreDeleteConnectorDelegate != null) OnPreDeleteConnectorDelegate(rep, info);
        }

        public static event EABroadcastInfo OnPreDeleteDiagramDelegate;
        public static void OnPreDeleteDiagram(EA.Repository rep, EA.EventProperties info)
        {
            if (OnPreDeleteDiagramDelegate != null) OnPreDeleteDiagramDelegate(rep, info);
        }

        public static event EABroadcastInfo OnPreDeletePackageDelegate;
        public static void OnPreDeletePackage(EA.Repository rep, EA.EventProperties info)
        {
            if (OnPreDeletePackageDelegate != null) OnPreDeletePackageDelegate(rep, info);
        }

        public static event EABroadcastInfo OnPreNewElementDelegate;
        public static void OnPreNewElement(EA.Repository rep, EA.EventProperties info)
        {
            if (OnPreNewElementDelegate != null) OnPreNewElementDelegate(rep, info);
        }

        public static event EABroadcastInfo OnPreNewConnectorDelegate;
        public static void OnPreNewConnector(EA.Repository rep, EA.EventProperties info)
        {
            if (OnPreNewConnectorDelegate != null) OnPreNewConnectorDelegate(rep, info);
        }

        public static event EABroadcastInfo OnPreNewAttributeDelegate;
        public static void OnPreNewAttribute(EA.Repository rep, EA.EventProperties info)
        {
            if (OnPreNewAttributeDelegate != null) OnPreNewAttributeDelegate(rep, info);
        }

        public static event EABroadcastInfo OnPreNewMethodDelegate;
        public static void OnPreNewMethod(EA.Repository rep, EA.EventProperties info)
        {
            if (OnPreNewMethodDelegate != null) OnPreNewMethodDelegate(rep, info);
        }

        public static event EABroadcastInfo OnPreNewPackageDelegate;
        public static void OnPreNewPackage(EA.Repository rep, EA.EventProperties info)
        {
            if (OnPreNewPackageDelegate != null) OnPreNewPackageDelegate(rep, info);
        }

        public static event EABroadcastInfo OnPostNewElementDelegate;
        public static void OnPostNewElement(EA.Repository rep, EA.EventProperties info)
        {
            if (OnPostNewElementDelegate != null) OnPostNewElementDelegate(rep, info);
        }

        public static event EABroadcastInfo OnPostNewConnectorDelegate;
        public static void OnPostNewConnector(EA.Repository rep, EA.EventProperties info)
        {
            if (OnPostNewConnectorDelegate != null) OnPostNewConnectorDelegate(rep, info);
        }

        public static event EABroadcastInfo OnPostNewAttributeDelegate;
        public static void OnPostNewAttribute(EA.Repository rep, EA.EventProperties info)
        {
            if (OnPostNewAttributeDelegate != null) OnPostNewAttributeDelegate(rep, info);
        }

        public static event EABroadcastInfo OnPostNewMethodDelegate;
        public static void OnPostNewMethod(EA.Repository rep, EA.EventProperties info)
        {
            if (OnPostNewMethodDelegate != null) OnPostNewMethodDelegate(rep, info);
        }

        public static event EABroadcastInfo OnPostNewPackageDelegate;
        public static void OnPostNewPackage(EA.Repository rep, EA.EventProperties info)
        {
            if (OnPostNewPackageDelegate != null) OnPostNewPackageDelegate(rep, info);
        }

        public static event EABroadcastInfo OnPreDeleteTechnologyDelegate;
        public static void OnPreDeleteTechnology(EA.Repository rep, EA.EventProperties info)
        {
            if (OnPreDeleteTechnologyDelegate != null) OnPreDeleteTechnologyDelegate(rep, info);
        }

        public static event EABroadcastInfo OnDeleteTechnologyDelegate;
        public static void OnDeleteTechnology(EA.Repository rep, EA.EventProperties info)
        {
            if (OnDeleteTechnologyDelegate != null) OnDeleteTechnologyDelegate(rep, info);
        }

        public static event EABroadcastInfo OnImportTechnologyDelegate;
        public static void OnImportTechnology(EA.Repository rep, EA.EventProperties info)
        {
            if (OnImportTechnologyDelegate != null) OnImportTechnologyDelegate(rep, info);
        }

        public static event EABroadcastGuid OnContextItemChangedDelegate;
        public static void OnContextItemChanged(EA.Repository rep, string guid, EA.ObjectType ot)
        {
            if (OnContextItemChangedDelegate != null) OnContextItemChangedDelegate(rep, guid, ot);
        }

        public static event EABroadcastGuid OnContextItemDoubleClickedDelegate;
        public static void OnContextItemDoubleClicked(EA.Repository rep, string guid, EA.ObjectType ot)
        {
            if (OnContextItemDoubleClickedDelegate != null) OnContextItemDoubleClickedDelegate(rep, guid, ot);
        }

        public static event EABroadcastGuid OnNotifyContextItemModifiedDelegate;
        public static void OnNotifyContextItemModified(EA.Repository rep, string guid, EA.ObjectType ot)
        {
            if (OnNotifyContextItemModifiedDelegate != null) OnNotifyContextItemModifiedDelegate(rep, guid, ot);
        }

        public static event EABroadcastGuid OnPostTransformDelegate;
        public static void OnPostTransform(EA.Repository rep, string guid, EA.ObjectType ot)
        {
            if (OnPostTransformDelegate != null) OnPostTransformDelegate(rep, guid, ot);
        }
    }
    #endregion

}
