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
namespace EA.Plugin.ProtocolHandler
{
    partial class FrmAbout
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAbout));
            this.pbCanonic = new System.Windows.Forms.PictureBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblYourVersion = new System.Windows.Forms.Label();
            this.lblCurrentVersion = new System.Windows.Forms.Label();
            this.btnOK = new Glass.GlassButton();
            this.lblAbout = new System.Windows.Forms.Label();
            this.lnkCanonic = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pbCanonic)).BeginInit();
            this.SuspendLayout();
            // 
            // pbCanonic
            // 
            this.pbCanonic.Image = ((System.Drawing.Image)(resources.GetObject("pbCanonic.Image")));
            this.pbCanonic.Location = new System.Drawing.Point(-2, -1);
            this.pbCanonic.Name = "pbCanonic";
            this.pbCanonic.Size = new System.Drawing.Size(415, 83);
            this.pbCanonic.TabIndex = 0;
            this.pbCanonic.TabStop = false;
            this.pbCanonic.Paint += new System.Windows.Forms.PaintEventHandler(this.pbCanonic_Paint);
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(106)))), ((int)(((byte)(154)))));
            this.lblDescription.Location = new System.Drawing.Point(12, 100);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(390, 110);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "About text... (set in code)";
            // 
            // lblYourVersion
            // 
            this.lblYourVersion.AutoSize = true;
            this.lblYourVersion.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYourVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(106)))), ((int)(((byte)(154)))));
            this.lblYourVersion.Location = new System.Drawing.Point(13, 257);
            this.lblYourVersion.Name = "lblYourVersion";
            this.lblYourVersion.Size = new System.Drawing.Size(91, 14);
            this.lblYourVersion.TabIndex = 3;
            this.lblYourVersion.Text = "Your Version:";
            // 
            // lblCurrentVersion
            // 
            this.lblCurrentVersion.AutoSize = true;
            this.lblCurrentVersion.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(106)))), ((int)(((byte)(154)))));
            this.lblCurrentVersion.Location = new System.Drawing.Point(13, 280);
            this.lblCurrentVersion.Name = "lblCurrentVersion";
            this.lblCurrentVersion.Size = new System.Drawing.Size(179, 14);
            this.lblCurrentVersion.TabIndex = 4;
            this.lblCurrentVersion.Text = "Current Version: (checking)";
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(160, 312);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblAbout
            // 
            this.lblAbout.AutoSize = true;
            this.lblAbout.BackColor = System.Drawing.Color.Transparent;
            this.lblAbout.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAbout.Location = new System.Drawing.Point(9, 29);
            this.lblAbout.Name = "lblAbout";
            this.lblAbout.Size = new System.Drawing.Size(94, 32);
            this.lblAbout.TabIndex = 6;
            this.lblAbout.Text = "About";
            // 
            // lnkCanonic
            // 
            this.lnkCanonic.AutoSize = true;
            this.lnkCanonic.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkCanonic.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(106)))), ((int)(((byte)(154)))));
            this.lnkCanonic.Location = new System.Drawing.Point(116, 220);
            this.lnkCanonic.Name = "lnkCanonic";
            this.lnkCanonic.Size = new System.Drawing.Size(0, 14);
            this.lnkCanonic.TabIndex = 7;
            this.lnkCanonic.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCanonic_LinkClicked);
            // 
            // FrmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(411, 347);
            this.Controls.Add(this.lnkCanonic);
            this.Controls.Add(this.lblAbout);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblCurrentVersion);
            this.Controls.Add(this.lblYourVersion);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.pbCanonic);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAbout";
            this.Text = "About ea:// Protocol Handler";
            this.Load += new System.EventHandler(this.FrmAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbCanonic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbCanonic;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblYourVersion;
        private System.Windows.Forms.Label lblCurrentVersion;
        private Glass.GlassButton btnOK;
        private System.Windows.Forms.Label lblAbout;
        private System.Windows.Forms.LinkLabel lnkCanonic;
    }
}