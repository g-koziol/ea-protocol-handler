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

namespace Canonic.EAPlugin.ProtocolHandler
{
    public partial class FrmMakeDict : Form
    {
        public FrmMakeDict()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            string[] lines = txtInput.Text.Split(new string[] { Environment.NewLine }, 
                StringSplitOptions.RemoveEmptyEntries);

            string buffer = string.Empty;
            foreach (string line in lines)
            {
                buffer += MakeBytes(line, true);
                if (buffer.Length >= 80)
                {
                    txtOutput.Text += buffer + Environment.NewLine;
                    buffer = string.Empty;
                }
            }
        }

        private string MakeBytes(string line, bool appendNull)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                char ch = line[i];

                sb.Append("(byte)'");

                if (char.IsLetterOrDigit(ch) || char.IsPunctuation(ch))
                {
                    sb.Append(ch);
                }
                else
                {
                    string x = ((int)ch).ToString("x2");
                    sb.Append(@"\x").Append(x);
                }
                sb.Append("', ");
            }

            if (appendNull)
            {
                sb.Append(@"(byte)'\0', ");
            }

            return sb.ToString();
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();

            for (int i = 0; i < 4; i++)
            {
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < 8; j++)
                {
                    int r = rnd.Next(255);
                    sb.Append((char)(r));
                }
                txtOutput.Text += MakeBytes(sb.ToString(), false) + Environment.NewLine;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtOutput.Text = "";
        }
    }
}