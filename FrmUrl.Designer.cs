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
namespace Canonic.EAPlugin.ProtocolHandler
{
    partial class FrmUrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUrl));
            this.pbCanonic = new System.Windows.Forms.PictureBox();
            this.pbPoweredBy = new System.Windows.Forms.PictureBox();
            this.btnOK = new Glass.GlassButton();
            this.lblAbout = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.btnClipboard = new Glass.GlassButton();
            ((System.ComponentModel.ISupportInitialize)(this.pbCanonic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPoweredBy)).BeginInit();
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
            // pbPoweredBy
            // 
            this.pbPoweredBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPoweredBy.Image = ((System.Drawing.Image)(resources.GetObject("pbPoweredBy.Image")));
            this.pbPoweredBy.Location = new System.Drawing.Point(261, 12);
            this.pbPoweredBy.Name = "pbPoweredBy";
            this.pbPoweredBy.Size = new System.Drawing.Size(141, 50);
            this.pbPoweredBy.TabIndex = 2;
            this.pbPoweredBy.TabStop = false;
            this.pbPoweredBy.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(242, 183);
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
            this.lblAbout.Location = new System.Drawing.Point(23, 30);
            this.lblAbout.Name = "lblAbout";
            this.lblAbout.Size = new System.Drawing.Size(164, 32);
            this.lblAbout.TabIndex = 6;
            this.lblAbout.Text = "URL Result";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Your ea:// URL";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(15, 101);
            this.txtUrl.Multiline = true;
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(384, 67);
            this.txtUrl.TabIndex = 8;
            // 
            // btnClipboard
            // 
            this.btnClipboard.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClipboard.Location = new System.Drawing.Point(84, 183);
            this.btnClipboard.Name = "btnClipboard";
            this.btnClipboard.Size = new System.Drawing.Size(84, 23);
            this.btnClipboard.TabIndex = 9;
            this.btnClipboard.Text = "&Clipboard";
            this.btnClipboard.Click += new System.EventHandler(this.btnClipboard_Click);
            // 
            // FrmUrl
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(411, 218);
            this.Controls.Add(this.btnClipboard);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblAbout);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pbPoweredBy);
            this.Controls.Add(this.pbCanonic);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUrl";
            this.Text = "Your ea:// URL";
            this.Load += new System.EventHandler(this.FrmAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbCanonic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPoweredBy)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbCanonic;
        private System.Windows.Forms.PictureBox pbPoweredBy;
        private Glass.GlassButton btnOK;
        private System.Windows.Forms.Label lblAbout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUrl;
        private Glass.GlassButton btnClipboard;
    }
}