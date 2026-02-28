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
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

using Canonic.EAPlugin.Interface;
using Canonic.EAPlugin.Utility;


namespace Canonic.EAPlugin.ProtocolHandler
{
    public class Plugin : IPlugin
    {
        // TODO: Remove expiration code
        private const string BETA_EXPIRES = "December 31, 2010 23:59:59";

        EA.Repository rep = null;

        static Plugin()
        {
        }

        public Plugin()
        {
        }

        string IPlugin.Name
        {
            get { return "ea:// (Public Beta)"; }
        }

        string IPlugin.Version
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

                return fvi.FileVersion;
            }
        }

        string IPlugin.Description
        {
            get { return "Handles the ea:// protocol."; }
        }

        MenuEntry IPlugin.MainMenu
        {
            get
            {
                MenuEntry me = new MenuEntry("ea:// Protocol", new string[] { "About" });
                return me;
            }
        }

        private MenuEntry BuildMenuEntry()
        {
            MenuEntry me;

            me = new MenuEntry("ea:// URL");

            return me;
        }

        MenuEntry IPlugin.TreeViewMenu
        {
            get
            {
                return BuildMenuEntry();
            }
        }

        MenuEntry IPlugin.DiagramMenu
        {
            get
            {
                return BuildMenuEntry();
            }
        }

        private bool Expired
        {
            get
            {
                DateTime expires = DateTime.Parse(BETA_EXPIRES);

                return (DateTime.Now > expires);
            }
        }

        void IPlugin.DoWork(EA.Repository rep, string menuLoc, string itemName)
        {
            try
            {
                this.rep = rep;

                if (Expired)
                {
                    Message("This BETA version has expired.  Please visit http://canoniccorp.com to download the current version.", MessageSeverity.Error);
                    return;
                }

                if (menuLoc == "MainMenu" && itemName == "About")
                {
                    string text = "This plugin handles the ea:// protocol, a component of Canonic's " +
                        "Model Driven Business® approach to software development. " + Environment.NewLine + Environment.NewLine +
                        "For the current version of the ea:// protocol handler, " +
                        "or to learn more about Model Driven Business®, visit http://canoniccorp.com";

                    FrmAbout frmAbout = new FrmAbout(text);
                    frmAbout.ShowDialog();

                }
                else
                {
                    // This should be a provider from the CanonicPlugins directory
                    DisplayUrl(rep, menuLoc, itemName);
                }

                this.rep = null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("CodeGen Plugin IPlugin.DoWork()", ex);
            }
        }

        private void DisplayUrl(EA.Repository rep, string menuLoc, string menuItemName)
        {
            string url = null;

            switch (menuLoc)
            {
                case "Diagram":
                    url = GenUrl(rep, menuLoc);
                    break;
                case "TreeView":
                    url = GenUrl(rep, menuLoc);
                    break;
                default:
                    Message("Please select a diagram, diagram element, or element from the Project Browser to make an ea:// URL",
                        MessageSeverity.Warning);
                    break;
            }

            if (url != null)
            {
                
                FrmUrl frmUrl = new FrmUrl(url);
                frmUrl.ShowDialog();
            }
        }

        private string GenUrl(EA.Repository rep, string menuLoc)
        {
            EA.ObjectType itemType;
            string contextItemName;
            string type;
            string stereoType;
            int packageId;
            string guid;
            int objectId;
            string stereoTypeEx;
            string status;

            string url = null;

            Util.GetItemDetails(ref rep, out itemType, out contextItemName, out type, out stereoType,
                out packageId, out guid, out objectId, out stereoTypeEx, out status);

            if (itemType == EA.ObjectType.otElement)
            {
                if (menuLoc == "Diagram")
                {
                    Message("Diagram URLs are not supported in this BETA version.  This URL will select the element in the Project Browser.",
                                            MessageSeverity.Warning);
                }

                url = Encryption.EaUrl.MakeElementUrl(rep, guid);
            }
            else
            {
                Message("Only Elements are supported in this BETA version.",
                                        MessageSeverity.Warning);
            }

            return url;
        }

        /// <summary>
        /// Severity of a message displayed by Message
        /// </summary>
        public enum MessageSeverity
        {
            /// <summary>
            /// Not set
            /// </summary>
            Unknown = 0,
            /// <summary>
            /// Informative
            /// </summary>
            Info = 1,
            /// <summary>
            /// Warning
            /// </summary>
            Warning = 2,
            /// <summary>
            /// Error
            /// </summary>
            Error = 3
        }

        private void Message(string message, MessageSeverity severity)
        {
            System.Media.SoundPlayer sp = new System.Media.SoundPlayer();

            System.Media.SystemSound sound;
            MessageBoxIcon icon;
            string title;

            switch (severity)
            {
                #region Sound & Icon assignments
                case MessageSeverity.Error:
                    sound = System.Media.SystemSounds.Exclamation;
                    icon = MessageBoxIcon.Error;
                    title = "ERROR";
                    break;
                case MessageSeverity.Warning:
                    sound = System.Media.SystemSounds.Hand;
                    icon = MessageBoxIcon.Hand;
                    title = "WARNING";
                    break;
                case MessageSeverity.Info:
                    sound = System.Media.SystemSounds.Asterisk;
                    icon = MessageBoxIcon.Information;
                    title = "INFO";
                    break;
                default:
                    sound = System.Media.SystemSounds.Beep;
                    icon = MessageBoxIcon.Asterisk;
                    title = "MESSAGE";
                    break;
                #endregion
            }

            sound.Play();

            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }
    }
}
