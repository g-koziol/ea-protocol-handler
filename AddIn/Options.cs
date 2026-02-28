/*
   Copyright 2009 Adam Hearn

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
using System.Globalization;
using System.Windows.Forms;

namespace EA.ProtocolHandler.AddIn
{
	/// <summary>
	/// Options form for EA Protocol Handler AddIn
	/// </summary>
	/// <remarks>
	/// Allows the user to override the default processing of the EA Protocol Handler
	/// Addin.
	/// </remarks>
	public partial class OptionsForm : Form
	{
		/// <summary>
		/// Logger instance for this class type
		/// </summary>
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Gets or sets the AddIn preferences
		/// </summary>
		/// <remarks>
		/// The preferences object is self contained and contains all preferences
		/// </remarks>
		public Preferences AddInPreferences{ get; set; }

		/// <summary>
		/// Initializes a new instance of the OptionsForm class
		/// </summary>
		/// <remarks>
		/// The default constructor initializes any fields to their default values.
		/// </remarks>
		public OptionsForm()
		{
			try
			{
				#region TraceBeginElement
				logger.Trace( "Begin:OptionsForm" );
				#endregion // TraceBeginElement

				InitializeComponent();

				// Load any existing preferences
				AddInPreferences = Preferences.Read();

				// Update the UI from the preferences just loaded
				UpdateUI();
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( systemException.Message, systemException );
				#endregion // TraceException
				throw;
			}
			finally
			{
				#region TraceEndElement
				logger.Trace( "End:OptionsForm" );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Radio button checked changed event handler
		/// </summary>
		/// <remarks>
		/// Manages the accessibility of the user defined text control
		/// </remarks>
		/// <param name="sender">Object that generated the event</param>
		/// <param name="e">Argument data for the event</param>
		private void radUserDefined_CheckedChanged( object sender, EventArgs e )
		{
			try
			{
				#region TraceBeginElement
				logger.Trace( @"Begin:radUserDefined_CheckedChanged ( sender[object]:'{0}', e[System.EventArgs]:'{1}' )", sender, e );
				#endregion // TraceBeginElement

				txtUserDefined.Enabled = radUserDefined.Checked;
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( String.Format( CultureInfo.CurrentCulture, @"Exception:radUserDefined_CheckedChanged ( sender[object]:'{0}', e[System.EventArgs]:'{1}' )", sender, e ), systemException );
				#endregion // TraceException
				throw;
			}
		}

		/// <summary>
		/// Button click event handler
		/// </summary>
		/// <remarks>
		/// Cancel form
		/// </remarks>
		/// <param name="sender">Object that generated the event</param>
		/// <param name="e">Argument data for the event</param>
		private void btnCancel_Click( object sender, EventArgs e )
		{
			try
			{
				#region TraceBeginElement
				logger.Trace( @"Begin:btnCancel_Click ( sender[object]:'{0}', e[System.EventArgs]:'{1}' )", sender, e );
				#endregion // TraceBeginElement

				// Close the form, discarding any updates
				Close();
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( String.Format( CultureInfo.CurrentCulture, @"Exception:btnCancel_Click ( sender[object]:'{0}', e[System.EventArgs]:'{1}' )", sender, e ), systemException );
				#endregion // TraceException
				throw;
			}
			finally
			{
				#region TraceEndElement
				logger.Trace( @"End:btnCancel_Click ( sender[object]:'{0}', e[System.EventArgs]:'{1}' )", sender, e );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Button click event handler
		/// </summary>
		/// <remarks>
		/// Apply user preferences and close form
		/// </remarks>
		/// <param name="sender">Object that generated the event</param>
		/// <param name="e">Argument data for the event</param>
		private void btnOK_Click( object sender, EventArgs e )
		{
			try
			{
				#region TraceBeginElement
				logger.Trace( @"Begin:btnOK_Click ( sender[object]:'{0}', e[System.EventArgs]:'{1}' )", sender, e );
				#endregion // TraceBeginElement

				// Update the preferences object
				UpdatePreferences();

				// Persist the preferences
				AddInPreferences.Write();
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( String.Format( CultureInfo.CurrentCulture, @"Exception:btnOK_Click ( sender[object]:'{0}', e[System.EventArgs]:'{1}' )", sender, e ), systemException );
				#endregion // TraceException
				throw;
			}
			finally
			{
				// Close the form
				Close();

				#region TraceEndElement
				logger.Trace( @"End:btnOK_Click ( sender[object]:'{0}', e[System.EventArgs]:'{1}' )", sender, e );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Update the UI from the preferences
		/// </summary>
		/// <remarks>
		/// Applies the local preferences object to the UI.
		/// </remarks>
		private void UpdateUI()
		{
			try
			{
				#region TraceBeginElement
				logger.Trace( "Begin:UpdateUI" );
				#endregion // TraceBeginElement

				// Configure the UI elements from the preferences object
				radFromCurrent.Checked = AddInPreferences.UseCurrent;
				radDefault.Checked = !AddInPreferences.UseCurrent;
				radUserDefined.Checked = !AddInPreferences.UseCurrent && ( !String.IsNullOrEmpty( AddInPreferences.UserDefinedKey ) );
				if( !String.IsNullOrEmpty( AddInPreferences.UserDefinedKey ) )
				{
					txtUserDefined.Text = AddInPreferences.UserDefinedKey;
				}
				txtUserDefined.Enabled = radUserDefined.Checked;
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( systemException.Message, systemException );
				#endregion // TraceException
				throw;
			}
			finally
			{
				#region TraceEndElement
				logger.Trace( "End:UpdateUI" );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Update the preferences from the UI.
		/// </summary>
		/// <remarks>
		/// Extracts the state of all UI controls and applies them directly
		/// to the local preferences object.
		/// </remarks>
		private void UpdatePreferences()
		{
			try
			{
				#region TraceBeginElement
				logger.Trace( "Begin:UpdatePreferences" );
				#endregion // TraceBeginElement

				// Update the preferences object
				AddInPreferences.UseCurrent = radFromCurrent.Checked;
				AddInPreferences.UserDefinedKey = ( radUserDefined.Checked ) ? txtUserDefined.Text : String.Empty;
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( systemException.Message, systemException );
				#endregion // TraceException
				throw;
			}
			finally
			{
				#region TraceEndElement
				logger.Trace( "End:UpdatePreferences" );
				#endregion // TraceEndElement
			}
		}
	}
}
