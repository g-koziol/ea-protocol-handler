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
    public class MenuEntry
    {
        private string mainMenu;
        private string[] subMenus;

        /// <summary>
        /// Name of the main menu for this MenuEntry;
        /// </summary>
        public string MainMenu
        {
            get
            {
                return mainMenu;
            }
        }

        /// <summary>
        /// Sub menu items of this MenuEntry.
        /// </summary>
        public string[] SubMenus
        {
            get
            {
                return subMenus;
            }
        }

        /// <summary>
        /// Indicates whether the MenuEntry has sub items.
        /// </summary>
        public bool HasSubItems
        {
            get
            {
                if (subMenus == null) return false;
                return subMenus.Length > 0;
            }
        }

        /// <summary>
        /// Initializes a new MenuEntry with only a main menu item.
        /// </summary>
        /// <param name="item">Main menu name.</param>
        public MenuEntry(string item)
            : this(item, null)
        {
        }

        /// <summary>
        /// Initializes a new MenuEntry with a main menu item and one or more sub menu items.
        /// </summary>
        /// <param name="item">Main menu name.</param>
        /// <param name="subItems">Array of sub-menu names.</param>
        public MenuEntry(string item, string[] subItems)
        {
            if (subItems != null && subItems.Length > 0)
            {
                this.mainMenu = "-" + item;
            }
            else
            {
                this.mainMenu = item;
            }

            this.subMenus = subItems;
        }

        /// <summary>
        /// Determines whether a MainMenu string has been specified.
        /// </summary>
        public bool IsNull
        {
            get { return mainMenu == null; }
        }

        /// <summary>
        /// Initializes a new MenuEntry from an existing one, appending a code to each menu item and
        /// sub menu item.
        /// </summary>
        /// <param name="me">Existing MenuEntry</param>
        /// <param name="code">Code to append</param>
        public MenuEntry(MenuEntry me, string code)
        {
            if (me == null)
            {
                this.mainMenu = null;
                this.subMenus = null;
            }
            else
            {
                this.mainMenu = me.MainMenu + code;

                if (me.SubMenus == null)
                {
                    this.subMenus = null;
                }
                else
                {
                    this.subMenus = new string[me.SubMenus.Length];

                    for (int i = 0; i < me.SubMenus.Length; i++)
                    {
                        this.subMenus[i] = me.SubMenus[i] + code;
                    }
                }
            }
        }
    }
}
