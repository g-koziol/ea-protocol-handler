namespace EA.ProtocolHandler.AddIn
{
	partial class OptionsForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( OptionsForm ) );
			this.radFromCurrent = new System.Windows.Forms.RadioButton();
			this.radUserDefined = new System.Windows.Forms.RadioButton();
			this.radDefault = new System.Windows.Forms.RadioButton();
			this.txtUserDefined = new System.Windows.Forms.TextBox();
			this.lblConnectionString = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			this.SuspendLayout();
			// 
			// radFromCurrent
			// 
			resources.ApplyResources( this.radFromCurrent, "radFromCurrent" );
			this.helpProvider.SetHelpString( this.radFromCurrent, resources.GetString( "radFromCurrent.HelpString" ) );
			this.radFromCurrent.Name = "radFromCurrent";
			this.helpProvider.SetShowHelp( this.radFromCurrent, ( (bool) ( resources.GetObject( "radFromCurrent.ShowHelp" ) ) ) );
			this.radFromCurrent.TabStop = true;
			this.radFromCurrent.UseVisualStyleBackColor = true;
			// 
			// radUserDefined
			// 
			resources.ApplyResources( this.radUserDefined, "radUserDefined" );
			this.helpProvider.SetHelpString( this.radUserDefined, resources.GetString( "radUserDefined.HelpString" ) );
			this.radUserDefined.Name = "radUserDefined";
			this.helpProvider.SetShowHelp( this.radUserDefined, ( (bool) ( resources.GetObject( "radUserDefined.ShowHelp" ) ) ) );
			this.radUserDefined.TabStop = true;
			this.radUserDefined.UseVisualStyleBackColor = true;
			this.radUserDefined.CheckedChanged += new System.EventHandler( this.radUserDefined_CheckedChanged );
			// 
			// radDefault
			// 
			resources.ApplyResources( this.radDefault, "radDefault" );
			this.helpProvider.SetHelpString( this.radDefault, resources.GetString( "radDefault.HelpString" ) );
			this.radDefault.Name = "radDefault";
			this.helpProvider.SetShowHelp( this.radDefault, ( (bool) ( resources.GetObject( "radDefault.ShowHelp" ) ) ) );
			this.radDefault.TabStop = true;
			this.radDefault.UseVisualStyleBackColor = true;
			// 
			// txtUserDefined
			// 
			resources.ApplyResources( this.txtUserDefined, "txtUserDefined" );
			this.helpProvider.SetHelpString( this.txtUserDefined, resources.GetString( "txtUserDefined.HelpString" ) );
			this.txtUserDefined.Name = "txtUserDefined";
			this.helpProvider.SetShowHelp( this.txtUserDefined, ( (bool) ( resources.GetObject( "txtUserDefined.ShowHelp" ) ) ) );
			// 
			// lblConnectionString
			// 
			resources.ApplyResources( this.lblConnectionString, "lblConnectionString" );
			this.lblConnectionString.Name = "lblConnectionString";
			// 
			// btnOK
			// 
			resources.ApplyResources( this.btnOK, "btnOK" );
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
			// 
			// btnCancel
			// 
			resources.ApplyResources( this.btnCancel, "btnCancel" );
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler( this.btnCancel_Click );
			// 
			// OptionsForm
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources( this, "$this" );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add( this.btnCancel );
			this.Controls.Add( this.btnOK );
			this.Controls.Add( this.lblConnectionString );
			this.Controls.Add( this.txtUserDefined );
			this.Controls.Add( this.radDefault );
			this.Controls.Add( this.radUserDefined );
			this.Controls.Add( this.radFromCurrent );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.HelpButton = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsForm";
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RadioButton radFromCurrent;
		private System.Windows.Forms.RadioButton radUserDefined;
		private System.Windows.Forms.RadioButton radDefault;
		private System.Windows.Forms.TextBox txtUserDefined;
		private System.Windows.Forms.Label lblConnectionString;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.HelpProvider helpProvider;
	}
}