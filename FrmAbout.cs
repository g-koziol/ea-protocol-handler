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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

using EA.Core.Utility;

namespace EA.Plugin.ProtocolHandler
{
    public partial class FrmAbout : Form
    {
        public FrmAbout(string text)
        {
            InitializeComponent();

            lblDescription.Text = text;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://eaprotocol.sourceforge.net");
        }

        private void FrmAbout_Load(object sender, EventArgs e)
        {
            AssemblyName assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
            Version ver = assemblyName.Version;
            string yourVersion = ver.ToString();
            lblYourVersion.Text = "Your Version: " + yourVersion;
            
            // Possibly put this in another thread for better UI response...
            FileVersionInfo fi = FileVersionInfo.GetVersionInfo(assemblyName.CodeBase.Replace("file:///", ""));

            //string currentVersion = SoftwareInfo.CurrentVersion(fi.ProductName);
            string currentVersion = "unavailable";

            if (currentVersion == string.Empty)
            {
                lblCurrentVersion.Text = "Current Version: (Unknown)";
            }
            else
            {
                lblCurrentVersion.Text = "Current Version: " + currentVersion;
                if (currentVersion != yourVersion)
                {
                    lblCurrentVersion.Font = new Font(lblCurrentVersion.Font, FontStyle.Bold);
                }
            }

            lnkCanonic.Links.Add(0, lnkCanonic.Text.Length, "Based on work of Canonic Corp. (http://canoniccorp.com)");

        }

        private void pbCanonic_Paint(object sender, PaintEventArgs e)
        {
            foreach (Control C in this.Controls)
            {
                if (C is Label)
                {
                    Label lbl = (Label)C;
                    if (lbl.BackColor == Color.Transparent)
                    {
                        lbl.Visible = false;
                        e.Graphics.DrawString(lbl.Text, lbl.Font, new
                        SolidBrush(lbl.ForeColor), lbl.Left - pbCanonic.Left, lbl.Top - pbCanonic.Top);
                    }
                }
            }
        }

        private void lnkCanonic_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Determine which link was clicked within the LinkLabel.
            lnkCanonic.Links[lnkCanonic.Links.IndexOf(e.Link)].Visited = true;
            // Display the appropriate link based on the value of the 
            // LinkData property of the Link object.
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }


    }
}