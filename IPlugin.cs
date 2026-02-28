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
    /// <summary>
    /// Represents the interface that a plugin must implement in order to be loaded by the Canonic
    /// EA Plugin Framework.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Name of the plugin.
        /// </summary>
        string Name {get;}
        /// <summary>
        /// Version of the plugin.
        /// </summary>
        string Version { get;}
        /// <summary>
        /// Description of the plugin.
        /// </summary>
        string Description { get;}
        /// <summary>
        /// MenuEntry (containing main menu and sub menus) to be shown on EA's main Add-Ins menu.
        /// </summary>
        MenuEntry MainMenu { get;}
        /// <summary>
        /// MenuEntry (containing main menu and sub menus) to be shown on EA's treeview (a.k.a project browser).
        /// </summary>
        MenuEntry TreeViewMenu { get;}
        /// <summary>
        /// MenuEntry (containing main menu and sub menus) to be shown on EA diagram menus.
        /// </summary>
        MenuEntry DiagramMenu { get;}
        /// <summary>
        /// Method invoked by the Canonic EA Plugin Framework to do work.
        /// </summary>
        /// <param name="rep">Current EA repository.</param>
        /// <param name="menuLoc">Menu location in EA (MainMenu, Treeview, Diagram).</param>
        /// <param name="itemName">Text of the menu item.</param>
        /// <remarks>EA 6.5 (build 805) has a bug that sometimes sets the incorrect menu location
        /// after a project has been loaded.  The Canonic EA Plugin Framework attempts to
        /// determine the correct menu location.</remarks>
        void DoWork(EA.Repository rep, string menuLoc, string itemName);
    }
}
