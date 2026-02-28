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
//using Canonic.Core.Utility;
using System.Reflection;
using System.Diagnostics;

namespace Canonic.EAPlugin.ProtocolHandler
{
    public partial class FrmUrl : Form
    {
        public FrmUrl(string url)
        {
            InitializeComponent();

            txtUrl.Text = url;
            txtUrl.SelectAll();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://canoniccorp.com");
        }

        private void FrmAbout_Load(object sender, EventArgs e)
        {
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

        private void btnClipboard_Click(object sender, EventArgs e)
        {
            System.Media.SystemSound sound = System.Media.SystemSounds.Asterisk;
            sound.Play();

            Clipboard.SetDataObject(txtUrl.Text, true, 20, 50);
        }


    }
}