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
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EA.ProtocolHandler.AddIn.Properties;
using EA.ProtocolHandler.Utility;
using System.Reflection;
using System.Diagnostics;

namespace EA.ProtocolHandler.AddIn
{
	/// <summary>
	/// Main class for the Protocol Handler AddIn for EA
	/// </summary>
	/// <remarks>
	/// Provides the basic implementation for the AddIn's menu
	/// </remarks>
	[ComVisible( true )]
	public class Main
	{
		/// <summary>
		/// Logger instance for this class type
		/// </summary>
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Called when EA loads
		/// </summary>
		/// <remarks>
		/// Perform initialisation actions which in this case are none but at least the
		/// AddIn 'Connect' event will be logged.
		/// </remarks>
		/// <param name="repository">EA repository reference</param>
		/// <returns>A string (the AddIn name)</returns>
		public string EA_Connect( Repository repository )
		{
			try
			{
				#region TraceBeginElement
				logger.Trace( @"Begin:EA_Connect ( repository[EA.Repository]:'{0}' )", repository );
				#endregion // TraceBeginElement

				#region TraceReturnElement
				logger.Trace( "Return:EA_Connect [System.String] : '{0}'", ( Resources.AddInName != null ) ? Resources.AddInName.ToString() : "null" );
				#endregion // TraceReturnElement
				return Resources.AddInName;
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( String.Format( CultureInfo.CurrentCulture, @"Exception:EA_Connect ( repository[EA.Repository]:'{0}' )", repository ), systemException );
				#endregion // TraceException
				throw;
			}
			finally
			{
				#region TraceEndElement
				logger.Trace( @"End:EA_Connect ( repository[EA.Repository]:'{0}' )", repository );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Called when user Click Add-Ins Menu [item].
		/// </summary>
		/// <remarks>
		/// Adds the required menu item to the required menus. EA doesn't have a very extensible
		/// model for 
		/// </remarks>
		/// <param name="repository">EA repository reference</param>
		/// <param name="location">Parent menu item</param>
		/// <param name="menuName">Sub-menu name</param>
		/// <returns>Menu item</returns>
		public object EA_GetMenuItems( Repository repository, string location, string menuName )
		{
			try
			{
				#region TraceBeginElement
				logger.Trace( @"Begin:EA_GetMenuItems ( repository[EA.Repository]:'{0}', location[string]:'{1}', menuName[string]:'{2}' )", repository, location, menuName );
				#endregion // TraceBeginElement

				List<string> retMenuItems = new List<string>();

				if( String.IsNullOrEmpty( menuName ) )
				{
					// Top level menu item required
					retMenuItems.Add( Resources.RootMenu );
				}
				else if( String.Compare( menuName, Resources.RootMenu ) == 0 )
				{
					// Add sub-menus for AddIn
					retMenuItems.Add( Resources.DiagramMenuItem );
					retMenuItems.Add( Resources.PackageMenuItem );
					retMenuItems.Add( Resources.ElementMenuItem );

					if( !IsContextMenu( location ) )
					{
						logger.Debug( "Menu is not a context menu, adding additional items" );

						// Do not add the following to context menus
						retMenuItems.Add( Resources.SeparatorMenuItem );
						retMenuItems.Add( Resources.OptionsMenuItem );
						retMenuItems.Add( Resources.SeparatorMenuItem );
						retMenuItems.Add( Resources.AboutMenuItem );
					}
				}
				else
				{
					// Invalid menu id
					logger.Error( Resources.InvalidMenuItem, location, menuName );
				}

				#region TraceReturnElement
				logger.Trace( "Return:EA_GetMenuItems [System.Object] : '{0}'", retMenuItems.Count );
				#endregion // TraceReturnElement
				return (String[]) retMenuItems.ToArray();
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( String.Format( CultureInfo.CurrentCulture, @"Exception:EA_GetMenuItems ( repository[EA.Repository]:'{0}', location[string]:'{1}', menuName[string]:'{2}' )", repository, location, menuName ), systemException );
				#endregion // TraceException#region TraceException
				throw;
			}
			finally
			{
				#region TraceEndElement
				logger.Trace( @"End:EA_GetMenuItems ( repository[EA.Repository]:'{0}', location[string]:'{1}', menuName[string]:'{2}' )", repository, location, menuName );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Called once Menu has been opened to see what menu items are active.
		/// </summary>
		/// <remarks>
		/// Enables the AddIn to show menu items that are either enabled or not and for those
		/// that require it whether it is checked
		/// </remarks>
		/// <param name="repository">EA repository</param>
		/// <param name="location">Parent menu item</param>
		/// <param name="menuName">Sub-menu name</param>
		/// <param name="itemName">Menu item</param>
		/// <param name="isEnabled">Ref param for menu item's enabled state</param>
		/// <param name="isChecked">Ref param for menu item's checked state</param>
		public void EA_GetMenuState( Repository repository, string location, string menuName, string itemName, ref bool isEnabled, ref bool isChecked )
		{
			try
			{
				#region TraceBeginElement
				logger.Trace( @"Begin:EA_GetMenuState ( repository[EA.Repository]:'{0}', location[string]:'{1}', menuName[string]:'{2}', itemName[string]:'{3}', isEnabled[bool]:'{4}', isChecked[bool]:'{5}' )", repository, location, menuName, itemName, isEnabled, isChecked );
				#endregion // TraceBeginElement

				if( String.Compare( itemName, Resources.DiagramMenuItem, StringComparison.OrdinalIgnoreCase ) == 0 )
				{
					// Check availability of Diagram menu item
					isEnabled = IsProjectOpen( repository ) && !( GetDiagramId( repository ) == String.Empty );
				}
				else if( String.Compare( itemName, Resources.PackageMenuItem, StringComparison.OrdinalIgnoreCase ) == 0 )
				{
					// Check availability of Package menu item
					isEnabled = IsProjectOpen( repository ) && !( GetPackageId( repository ) == String.Empty );
				}
				else if( String.Compare( itemName, Resources.ElementMenuItem, StringComparison.OrdinalIgnoreCase ) == 0 )
				{
					// Check availability of Element menu item
					isEnabled = IsProjectOpen( repository ) && !( GetElementId( repository ) == String.Empty );
				}
				else if( !IsContextMenu( location ) )
				{
					if( String.Compare( itemName, Resources.OptionsMenuItem, StringComparison.OrdinalIgnoreCase ) == 0 )
					{
						// Always allow the Options menu item
						isEnabled = true;
					}
					else if( String.Compare( itemName, Resources.AboutMenuItem, StringComparison.OrdinalIgnoreCase ) == 0 )
					{
						// Always allow the About menu item
						isEnabled = true;
					}
				}
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( String.Format( CultureInfo.CurrentCulture, @"Exception:EA_GetMenuState ( repository[EA.Repository]:'{0}', location[string]:'{1}', menuName[string]:'{2}', itemName[string]:'{3}', isEnabled[bool]:'{4}', isChecked[bool]:'{5}' )", repository, location, menuName, itemName, isEnabled, isChecked ), systemException );
				#endregion // TraceException
				throw;
			}
			finally
			{
				#region TraceEndElement
				logger.Trace( @"End:EA_GetMenuState ( repository[EA.Repository]:'{0}', location[string]:'{1}', menuName[string]:'{2}', itemName[string]:'{3}', isEnabled[bool]:'{4}', isChecked[bool]:'{5}' )", repository, location, menuName, itemName, isEnabled, isChecked );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Called when user makes a selection in the menu.
		/// </summary>
		/// <remarks>
		/// This is your main exit point to the rest of your Add-in
		/// </remarks>
		/// <param name="repository">EA repository</param>
		/// <param name="location">Parent menu item</param>
		/// <param name="menuName">Sub-menu name</param>
		/// <param name="itemName">Menu item</param>
		public void EA_MenuClick( Repository repository, string location, string menuName, string itemName )
		{
			// Obtani the current cursor;
			Cursor oldCursor = Cursor.Current;

			try
			{
				#region TraceBeginElement
				logger.Trace( @"Begin:EA_MenuClick ( repository[EA.Repository]:'{0}', location[string]:'{1}', menuName[string]:'{2}', itemName[string]:'{3}' )", repository, location, menuName, itemName );
				#endregion // TraceBeginElement

				// Set the cursor to busy
				Cursor.Current = Cursors.AppStarting;

				if( String.Compare( itemName, Resources.DiagramMenuItem ) == 0 )
				{
					// Process request for a diagram
					string diagramId = GetDiagramId( repository );
					if( diagramId != String.Empty )
					{
						// Create the handler URL and place it on the clipboard
						Clipboard.SetDataObject( new EaDiagramUrl().Create( EaUrlRequestAdapter( repository, diagramId ) ) );
					}
				}
				else if( String.Compare( itemName, Resources.PackageMenuItem ) == 0 )
				{
					// Process request for an package
					string packageId = GetPackageId( repository );
					if( packageId != String.Empty )
					{
						// Create the handler URL and place it on the clipboard
						Clipboard.SetDataObject( new EaPackageUrl().Create( EaUrlRequestAdapter( repository, packageId ) ) );
					}
				}
				else if( String.Compare( itemName, Resources.ElementMenuItem ) == 0 )
				{
					// Process request for an element
					string elementId = GetElementId( repository );
					if( elementId != String.Empty )
					{
						// Create the handler URL and place it on the clipboard
						Clipboard.SetDataObject( new EaElementUrl().Create( EaUrlRequestAdapter( repository, elementId ) ) );
					}
				}
				else if( String.Compare( itemName, Resources.OptionsMenuItem ) == 0 )
				{
					// Show the options dialog for this AddIn
					OptionsForm optionsForm = new OptionsForm();
					optionsForm.ShowDialog();
				}
				else if( String.Compare( itemName, Resources.AboutMenuItem ) == 0 )
				{
					logger.Debug( "Processing request for About" );

					// Extract the product version from the current assembly
					string versionInfo = FileVersionInfo.GetVersionInfo( Assembly.GetExecutingAssembly().Location ).ProductVersion;

					// Show some basic information about the AddIn
					MessageBox.Show( String.Format( Resources.AboutInfo, Resources.AddInName, Environment.NewLine, versionInfo ), String.Format( Resources.AboutCaption, Resources.AddInName ), MessageBoxButtons.OK, MessageBoxIcon.Information );
				}
				else
				{
					// Invalid menu item received
					logger.Error( Resources.InvalidMenuItem, menuName, itemName );
				}
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( String.Format( CultureInfo.CurrentCulture, @"Exception:EA_MenuClick ( repository[EA.Repository]:'{0}', location[string]:'{1}', menuName[string]:'{2}', itemName[string]:'{3}' )", repository, location, menuName, itemName ), systemException );
				#endregion // TraceException
				throw;
			}
			finally
			{
				// Reset the cursor back to what it was when we started
				Cursor.Current = oldCursor;

				#region TraceEndElement
				logger.Trace( @"End:EA_MenuClick ( repository[EA.Repository]:'{0}', location[string]:'{1}', menuName[string]:'{2}', itemName[string]:'{3}' )", repository, location, menuName, itemName );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Helper method to get the id of current Diagram
		/// </summary>
		/// <remarks>
		/// If a diagram is open, that id is returned. If not a further check
		/// is made against the tree to see if the currently selected item is a
		/// diagram. If so that id is returned.
		/// </remarks>
		/// <param name="repository">EA repository reference</param>
		/// <returns>Id of the opne/selected diagram. String.Empty if none.</returns>
		/// <change>Adam Hearn, 2009-10-29, 2888697 - Process tree selections before active diagram.</change>
		private static string GetDiagramId( Repository repository )
		{
			try
			{
				// First off, check for a diagram selected in the tree
				object selectedObject;
				ObjectType objectType = repository.GetTreeSelectedItem( out selectedObject );
				if( objectType == ObjectType.otDiagram )
				{
					#region TraceReturnElement
					logger.Trace( "Return:GetDiagramId [System.String] : '{0}'", ( ( (EA.Diagram) selectedObject ).DiagramGUID != null ) ? ( (EA.Diagram) selectedObject ).DiagramGUID.ToString() : "null" );
					#endregion // TraceReturnElement
					return ( (Diagram) selectedObject ).DiagramGUID;
				}

				// Now check for an open diagram
				if( repository.GetCurrentDiagram() != null )
				{
					#region TraceReturnElement
					logger.Trace( "Return:GetDiagramId [System.String] : '{0}'", ( repository.GetCurrentDiagram().DiagramGUID != null ) ? repository.GetCurrentDiagram().DiagramGUID.ToString() : "null" );
					#endregion // TraceReturnElement
					return repository.GetCurrentDiagram().DiagramGUID;
				}

				#region TraceReturnElement
				logger.Trace( "Return:GetDiagramId [System.String] : '{0}'", ( String.Empty != null ) ? String.Empty.ToString() : "null" );
				#endregion // TraceReturnElement
				return String.Empty;
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( String.Format( CultureInfo.CurrentCulture, @"Exception:GetDiagramId ( repository[EA.Repository]:'{0}' )", repository ), systemException );
				#endregion // TraceException
				throw;
			}
			finally
			{
				#region TraceEndElement
				logger.Trace( @"End:GetDiagramId ( repository[EA.Repository]:'{0}' )", repository );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Helper method to get the id of current Element
		/// </summary>
		/// <remarks>
		/// A check is made against the tree to see if the currently selected item is
		/// an element. If so that id is returned.
		/// </remarks>
		/// <param name="repository">EA repository reference</param>
		/// <returns>Id of the opne/selected element. String.Empty if none.</returns>
		private static string GetElementId( Repository repository )
		{
			try
			{
				// Need to check for a selected element in the tree
				object selectedObject;
				ObjectType objectType = repository.GetTreeSelectedItem( out selectedObject );
				if( objectType == ObjectType.otElement )
				{
					#region TraceReturnElement
					logger.Trace( "Return:GetElementId [System.String] : '{0}'", ( ( (EA.Element) selectedObject ).ElementGUID != null ) ? ( (EA.Element) selectedObject ).ElementGUID.ToString() : "null" );
					#endregion // TraceReturnElement
					return ( (Element) selectedObject ).ElementGUID;
				}

				#region TraceReturnElement
				logger.Trace( "Return:GetElementId [System.String] : '{0}'", ( String.Empty != null ) ? String.Empty.ToString() : "null" );
				#endregion // TraceReturnElement
				return String.Empty;
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( String.Format( CultureInfo.CurrentCulture, @"Exception:GetElementId ( repository[EA.Repository]:'{0}' )", repository ), systemException );
				#endregion // TraceException
				throw;
			}
			finally
			{
				#region TraceEndElement
				logger.Trace( @"End:GetElementId ( repository[EA.Repository]:'{0}' )", repository );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Helper method to get the id of current Package
		/// </summary>
		/// <remarks>
		/// The selected package is requested from the tree. The package itself may
		/// not be selected but the API returns the package that is the parent of the
		/// selected item.
		/// </remarks>
		/// <param name="repository">EA repository reference</param>
		/// <returns>Id of the opne/selected element. String.Empty if none.</returns>
		private static string GetPackageId( Repository repository )
		{
			try
			{
				#region TraceBeginElement
				logger.Trace( @"Begin:GetPackageId ( repository[EA.Repository]:'{0}' )", repository );
				#endregion // TraceBeginElement

				// Check for a selected package in the tree
				Package package = repository.GetTreeSelectedPackage();
				if( package != null )
				{
					#region TraceReturnElement
					logger.Trace( "Return:GetPackageId [System.String] : '{0}'", ( package.PackageGUID != null ) ? package.PackageGUID.ToString() : "null" );
					#endregion // TraceReturnElement
					return package.PackageGUID;
				}

				#region TraceReturnElement
				logger.Trace( "Return:GetPackageId [System.String] : '{0}'", ( String.Empty != null ) ? String.Empty.ToString() : "null" );
				#endregion // TraceReturnElement
				return String.Empty;
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( String.Format( CultureInfo.CurrentCulture, @"Exception:GetPackageId ( repository[EA.Repository]:'{0}' )", repository ), systemException );
				#endregion // TraceException
				throw;
			}
			finally
			{
				#region TraceEndElement
				logger.Trace( @"End:GetPackageId ( repository[EA.Repository]:'{0}' )", repository );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Helper method to determine if there is an active project or not
		/// </summary>
		/// <remarks>
		/// Uses the EA repository object to determine if a project has been loaded or not.
		/// </remarks>
		/// <param name="repository">EA repository reference</param>
		/// <returns>true if open</returns>
		private static bool IsProjectOpen( Repository repository )
		{
			try
			{
				return null != repository.Models;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Helper method to adapt a request for an artifact from a repository
		/// into a UrlRequest object
		/// </summary>
		/// <param name="repository">EA repository</param>
		/// <param name="artifactId">Id of the artifict required</param>
		/// <returns>Populate EaUrlRequest instance</returns>
		private static EaUrlRequest EaUrlRequestAdapter( Repository repository, string artifactId )
		{
			try
			{
				#region TraceBeginElement
				logger.Trace( @"Begin:EaUrlRequestAdapter ( repository[EA.Repository]:'{0}', artifactId[string]:'{1}' )", repository, artifactId );
				#endregion // TraceBeginElement

				string connectionString = String.Empty;
				string repositoryName = Resources.DefaultRespositoryName;

				// Parse the repository connection string
				string[] parts = repository.ConnectionString.Split( new string[] { " --- " }, StringSplitOptions.None );

				// Obtain the AddIn preferences
				Preferences preferences = Preferences.Read();
				if( preferences.UseCurrent )
				{
					if( parts.Length > 1 )
					{
						// Repository name was available, set value
						repositoryName = parts[0];
					}
					else if( parts.Length == 1 )
					{
						// Special processing for an EAP model
						if( parts[parts.Length - 1].ToLower().EndsWith( String.Format( ".{0}", Resources.EAPRepositoryName ) ) )
						{
							// It's an EAP file
							repositoryName = Resources.EAPRepositoryName;
						}
					}

					// Default, use connection string from model
					connectionString = parts[parts.Length - 1];
				}
				else
				{
					if( !String.IsNullOrEmpty( preferences.UserDefinedKey ) )
					{
						// User defined repository name based on value of 'key'
						repositoryName = preferences.UserDefinedKey;
					}
				}

				// Create the new instance of the request object
				EaUrlRequest retRequest = new EaUrlRequest()
				{
					RepositoryName = repositoryName,
					ConnectionString = connectionString,
					ArtifactId = artifactId
				};

				#region TraceReturnElement
				logger.Trace( "Return:EaUrlRequestAdapter [Canonic.Encryption.EaUrlRequest] : '{0}'", ( retRequest != null ) ? retRequest.ToString() : "null" );
				#endregion // TraceReturnElement
				return retRequest;
			}
			catch( Exception systemException )
			{
				#region TraceException
				logger.ErrorException( String.Format( CultureInfo.CurrentCulture, @"Exception:EaUrlRequestAdapter ( repository[EA.Repository]:'{0}', artifactId[string]:'{1}' )", repository, artifactId ), systemException );
				#endregion // TraceException
				throw;
			}
			finally
			{
				#region TraceEndElement
				logger.Trace( @"End:EaUrlRequestAdapter ( repository[EA.Repository]:'{0}', artifactId[string]:'{1}' )", repository, artifactId );
				#endregion // TraceEndElement
			}
		}

		/// <summary>
		/// Helper method to determine if the menu item is considered a context menu
		/// </summary>
		/// <param name="location">Menu name</param>
		/// <returns>true when menu is a context menu, false if not</returns>
		private static bool IsContextMenu( string location )
		{
			// Is this a context menu
			return ( String.Compare( location, "TreeView", StringComparison.OrdinalIgnoreCase ) == 0 ) ||
				   ( String.Compare( location, "Diagram", StringComparison.OrdinalIgnoreCase ) == 0 );
		}
	}
}
