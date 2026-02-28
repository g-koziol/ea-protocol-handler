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

namespace EA.Plugin.ProtocolHandler
{
    public partial class FrmError : Form
    {
        private string error = string.Empty;
        private string explanation = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmError"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="explanation">The explanation.</param>
        /// <param name="url">The URL.</param>
        public FrmError(string error, string explanation, string url)
        {
            explanation += Environment.NewLine + Environment.NewLine + "URL: " + url;
            Init(error, explanation);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmError"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="explanation">The explanation.</param>
        public FrmError(string error, string explanation)
        {
            Init(error, explanation);
        }

        private void Init(string error, string explanation)
        {
            this.error = error;
            this.explanation = explanation;

            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://eaprotocol.sourceforge.net");
        }

        private void FrmError_Load(object sender, EventArgs e)
        {
            StringBuilder html = new StringBuilder();

            html.AppendLine("<html><body style='font-family: verdana; font-size:9pt; color: #2a6a9a'>");
            html.AppendLine("<b>Problem</b><br/>");
            html.AppendLine(error);
            html.AppendLine("<br/><br/><b>Explanation</b><br/>");
            html.AppendLine(explanation);
            html.AppendLine("</body></html>");

            webInfo.DocumentText = html.ToString();
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

        private void btnAbout_Click(object sender, EventArgs e)
        {
            string text = "This plugin handles the ea:// protocol." + Environment.NewLine + Environment.NewLine +
                "For the current version of the ea:// protocol handler, visit:";

            FrmAbout about = new FrmAbout(text);
            about.ShowDialog();
        }


    }
}