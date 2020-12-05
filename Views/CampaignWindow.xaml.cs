using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO.Compression;

namespace JiME.Views
{
	/// <summary>
	/// Interaction logic for NewCampaignWindow.xaml
	/// </summary>
	public partial class CampaignWindow : Window
	{
		public Campaign campaign { get; set; }

		string campaignFolder;

		public CampaignWindow( Campaign c = null )
		{
			InitializeComponent();
			DataContext = this;

			cancelButton.Visibility = c == null ? Visibility.Visible : Visibility.Collapsed;
			deleteCampaign.Visibility = c == null ? Visibility.Collapsed : Visibility.Visible;

			if ( c == null )
			{
				Title = "Campaign Manager - New Campaign";
			}
			else
			{
				Title = "Campaign Manager - Edit Campaign - " + c.campaignName;
			}
			campaign = c ?? new Campaign();
			campaignNameTB.Focus();

			//string mydocs = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments );

			campaignFolder = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ), "Your Journey", campaign.campaignGUID.ToString() );

			cGUID.Text = "Campaign GUID:  "
			+ campaign.campaignGUID.ToString();

			//update controls
			cTriggersCombo.SelectedIndex = 0;
			if ( campaign.storyText.Trim() == "" )
				storySet.Text = "The Story Text is not set.";
			else
				storySet.Text = "The Story Text is set.";

			//update scenario names
			for ( int i = 0; i < campaign.scenarioCollection.Count; i++ )
			{
				var item = campaign.scenarioCollection[i];
				Scenario scenario = FileManager.LoadProjectFromPath( campaignFolder, item.fileName );
				if ( scenario != null )
				{
					campaign.scenarioCollection[i].scenarioName = scenario.scenarioName;
				}
			}
			SaveCampaign();
		}

		private void removeScenario_Click( object sender, RoutedEventArgs e )
		{
			bool doDelete = false;
			if ( System.Windows.Input.Keyboard.IsKeyDown( System.Windows.Input.Key.LeftCtrl ) || System.Windows.Input.Keyboard.IsKeyDown( System.Windows.Input.Key.RightCtrl ) )
				doDelete = true;

			string basePath = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ), "Your Journey" );
			CampaignItem ci = (CampaignItem)( (Button)sender ).DataContext;


			if ( doDelete )
			{
				var res = MessageBox.Show( "Are you sure you want to PERMANENTLY DELETE the Scenario?\r\n\r\n" + ci.scenarioName + "\r\n\r\n" + Path.Combine( campaignFolder, ci.fileName ), "Confirm Scenario Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question );
				if ( res == MessageBoxResult.Yes )
				{
					campaign.scenarioCollection.Remove( ci );
					FileInfo fi = new FileInfo( Path.Combine( campaignFolder, ci.fileName ) );
					fi.Delete();
					SaveCampaign();
				}
			}
			else
			{
				campaign.scenarioCollection.Remove( ci );
				SaveCampaign();
				//move scenario file back into main project folder
				FileInfo fi = new FileInfo( Path.Combine( campaignFolder, ci.fileName ) );
				string moveto = Path.Combine( Path.Combine( basePath, ci.fileName ) );
				fi.MoveTo( moveto );
				//open the scenario and set its campaign GUID to empty
				Scenario scenario = FileManager.LoadProject( ci.fileName );
				if ( scenario != null )
				{
					scenario.campaignGUID = Guid.Empty;
					scenario.projectType = ProjectType.Standalone;
					FileManager fm = new FileManager( scenario );
					fm.Save();
				}
				else
				{
					MessageBox.Show( "Could not open the Scenario to reset its Campaign setting to Empty.\r\nTried to modify: " + campaignFolder + ci.fileName, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
				}
			}
		}

		private void okButton_Click( object sender, RoutedEventArgs e )
		{
			SaveCampaign();
			ProjectWindow pw = new ProjectWindow();
			pw.Show();
			Close();
		}

		private void cancelButton_Click( object sender, RoutedEventArgs e )
		{
			string basePath = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ), "Your Journey" );

			//can only cancel if creating a NEW campaign
			//check if any scenarios have been added and move them back into the base folder
			DirectoryInfo di = new DirectoryInfo( campaignFolder );
			if ( di.Exists )
			{
				FileInfo[] files = di.GetFiles().Where( file => file.Extension == ".jime" ).ToArray();
				foreach ( FileInfo fi in files )
				{
					//load the scenario and reset its campaign GUID, resave it
					Scenario s = FileManager.LoadProjectFromPath( campaignFolder, fi.Name );
					s.campaignGUID = Guid.Empty;
					FileManager fm = new FileManager( s );
					fm.Save();//this will save it to base folder
				}
				//finally, remove the campaign folder
				if ( Directory.Exists( campaignFolder ) )
					Directory.Delete( campaignFolder, true );
			}

			ProjectWindow pw = new ProjectWindow();
			pw.Show();
			Close();
		}

		private void createNew_Click( object sender, RoutedEventArgs e )
		{
			//create a new scenario and set its campaign GUID
			Scenario scenario = new Scenario();
			scenario.campaignGUID = campaign.campaignGUID;
			scenario.projectType = ProjectType.Campaign;
			bool doEdit = false;
			if ( System.Windows.Input.Keyboard.IsKeyDown( System.Windows.Input.Key.LeftCtrl ) || System.Windows.Input.Keyboard.IsKeyDown( System.Windows.Input.Key.RightCtrl ) )
				doEdit = true;

			//ask for scenario filename and save it
			FileManager fm = new FileManager( scenario );
			if ( fm.SaveAs( campaignFolder ) )
			{
				//add it to campaign
				CampaignItem ci = new CampaignItem();
				ci.scenarioName = scenario.scenarioName;
				ci.fileName = fm.fileName;
				campaign.scenarioCollection.Add( ci );

				scenario.fileName = fm.fileName;
				//save the campaign
				SaveCampaign();
				//open the new scenario in the Editor if CTRL held
				if ( doEdit )
				{
					//add campaign triggers to the scenario
					foreach ( Trigger t in campaign.triggerCollection )
						scenario.triggersObserver.Add( t );
					MainWindow mainWindow = new MainWindow( scenario );
					mainWindow.Show();
					Close();
				}
			}
		}

		private void addExisting_Click( object sender, RoutedEventArgs e )
		{
			string basePath = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ), "Your Journey" );

			//check regular scenario project path
			if ( !Directory.Exists( basePath ) )
			{
				var di = Directory.CreateDirectory( basePath );
				if ( di == null )
				{
					MessageBox.Show( "Could not create the scenario project folder.\r\nTried to create: " + basePath, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
					return;
				}
			}

			OpenFileDialog ofd = new OpenFileDialog();
			ofd.DefaultExt = ".jime";
			ofd.Title = "Add Scenario to Campaign";
			ofd.Filter = "Journey File (*.jime)|*.jime";
			ofd.InitialDirectory = basePath;
			if ( ofd.ShowDialog() == true )
			{
				var s = FileManager.LoadProject( ofd.SafeFileName );
				if ( s != null )
				{
					CampaignItem ci = new CampaignItem();
					ci.scenarioName = s.scenarioName;
					ci.fileName = ofd.SafeFileName;
					campaign.scenarioCollection.Add( ci );

					//save campaign
					SaveCampaign();
					//move scenario file from project folder to campaign folder
					FileInfo fi = new FileInfo( ofd.FileName );
					string moveto = Path.Combine( campaignFolder, ofd.SafeFileName );
					fi.MoveTo( moveto );
					//open the scenario and add the campaign GUID to it, resave
					Scenario scenario = FileManager.LoadProjectFromPath( campaignFolder, ci.fileName );
					if ( scenario != null )
					{
						scenario.campaignGUID = campaign.campaignGUID;
						scenario.projectType = ProjectType.Campaign;
						FileManager fm = new FileManager( scenario );
						fm.Save();
					}
					else
					{
						MessageBox.Show( "Could not modify the Scenario to use a Campaign.\r\nTried to modify: " + campaignFolder + ci.fileName, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
					}
				}
			}
		}

		private void moveDown_Click( object sender, RoutedEventArgs e )
		{
			CampaignItem ci = ( (Button)sender ).DataContext as CampaignItem;
			int idx = campaign.scenarioCollection.IndexOf( ci );
			if ( idx >= campaign.scenarioCollection.Count - 1 )
				return;
			CampaignItem next = campaign.scenarioCollection[idx + 1];
			campaign.scenarioCollection[idx] = next;
			campaign.scenarioCollection[idx + 1] = ci;
		}

		private void moveUp_Click( object sender, RoutedEventArgs e )
		{
			CampaignItem ci = ( (Button)sender ).DataContext as CampaignItem;
			int idx = campaign.scenarioCollection.IndexOf( ci );
			if ( idx == 0 )
				return;
			CampaignItem prev = campaign.scenarioCollection[idx - 1];
			campaign.scenarioCollection[idx] = prev;
			campaign.scenarioCollection[idx - 1] = ci;
		}

		//private void campaignNameTB_TextChanged( object sender, TextChangedEventArgs e )
		//{
		//	if ( campaign.campaignName.Trim() == "" )
		//	{
		//		addExisting.IsEnabled = false;
		//		createNew.IsEnabled = false;
		//	}
		//	else
		//	{
		//		addExisting.IsEnabled = true;
		//		createNew.IsEnabled = true;
		//	}
		//}

		private void SaveCampaign()
		{
			if ( !Directory.Exists( campaignFolder ) )
			{
				var di = Directory.CreateDirectory( campaignFolder );
				if ( di == null )
				{
					MessageBox.Show( "Could not create the Campaign project folder.\r\nTried to create: " + campaignFolder, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
					return;
				}
			}

			campaign.fileVersion = Utils.formatVersion;

			string output = JsonConvert.SerializeObject( campaign, Formatting.Indented );
			string outpath = Path.Combine( campaignFolder, campaign.campaignGUID.ToString() + ".json" );
			try
			{
				using ( var stream = File.CreateText( outpath ) )
				{
					stream.Write( output );
				}
			}
			catch ( Exception e )
			{
				MessageBox.Show( "Could not save the Campaign file.\r\n\r\nException:\r\n" + e.Message, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
			}
		}

		private void editSelected_Click( object sender, RoutedEventArgs e )
		{
			SaveCampaign();

			CampaignItem ci = ( (Button)sender ).DataContext as CampaignItem;
			//load the scenario to make sure its campaign GUID is set
			Scenario scenario = FileManager.LoadProjectFromPath( campaignFolder, ci.fileName );
			scenario.campaignGUID = campaign.campaignGUID;
			if ( scenario != null )
			{
				//...and save it back
				FileManager fm = new FileManager( scenario );
				fm.Save();
				//add campaign triggers to the scenario
				foreach ( Trigger t in campaign.triggerCollection )
					scenario.triggersObserver.Add( t );
				//then open it in Editor
				MainWindow mainWindow = new MainWindow( scenario );
				mainWindow.Show();
				Close();
			}
		}

		private void deleteCampaign_Click( object sender, RoutedEventArgs e )
		{
			var res = MessageBox.Show( "Are you sure you want to permanently delete this Campaign and its shareable Package, if it exists?\r\n\r\nTHIS ACTION CANNOT BE UNDONE.\r\n\r\nAny individual Scenarios added to this Campaign WILL NOT BE DELETED - they will be moved back to the base folder.", "Permanently Delete This Campaign And Its Data?", MessageBoxButton.YesNo );
			if ( res == MessageBoxResult.Yes )
			{
				cancelButton_Click( null, null );
			}
		}

		private void Window_ContentRendered( object sender, EventArgs e )
		{
			campaignNameTB.SelectAll();
		}

		private void packageCampaign_Click( object sender, RoutedEventArgs e )
		{
			if ( campaign.campaignName.Trim() == "" )
			{
				MessageBox.Show( "The Campaign must have a name before it can be packaged.", "Package Error", MessageBoxButton.OK, MessageBoxImage.Error );
				return;
			}

			if ( campaign.scenarioCollection.Count == 0 )
			{
				MessageBox.Show( "This Campaign doesn't contain any Scenarios for packaging.", "Package Error", MessageBoxButton.OK, MessageBoxImage.Error );
				return;
			}

			try
			{
				//remove existing package if it exists
				FileInfo fi = new FileInfo( Path.Combine( campaignFolder, campaign.campaignName + ".zip" ) );
				if ( fi.Exists )
					fi.Delete();

				using ( FileStream zipPath = new FileStream( Path.Combine( campaignFolder, campaign.campaignName + ".zip" ), FileMode.OpenOrCreate ) )
				{
					using ( ZipArchive archive = new ZipArchive( zipPath, ZipArchiveMode.Update ) )
					{
						//add the campaign metadata
						archive.CreateEntryFromFile( Path.Combine( campaignFolder, campaign.campaignGUID.ToString() + ".json" ), campaign.campaignGUID.ToString() + ".json" );

						//add the scenarios
						foreach ( CampaignItem item in campaign.scenarioCollection )
						{
							archive.CreateEntryFromFile( Path.Combine( campaignFolder, item.fileName ), item.fileName );
						}
					}
					MessageBox.Show( "Campaign Package successfully created.\r\n\r\n", "Campaign Packaging Successful", MessageBoxButton.OK, MessageBoxImage.Information );
				}
			}
			catch ( Exception ex )
			{
				MessageBox.Show( "Could not create the Package ZIP.\r\n\r\nException:\r\n" + ex.Message, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
			}
		}

		private void addTrigger_Click( object sender, RoutedEventArgs e )
		{
			CampaignTriggerEditor tw = new CampaignTriggerEditor( campaign );
			if ( tw.ShowDialog() == true )
			{
				Trigger t = new Trigger( tw.triggerName );
				t.isMultiTrigger = tw.isMulti;
				t.isCampaignTrigger = true;
				t.isMultiTrigger = true;
				campaign.triggerCollection.Add( t );
				cTriggersCombo.SelectedIndex = cTriggersCombo.Items.Count - 1;
			}
		}

		private void delTrigger_Click( object sender, RoutedEventArgs e )
		{
			Trigger selected = (Trigger)cTriggersCombo.SelectedItem;
			campaign.triggerCollection.Remove( selected );
			cTriggersCombo.SelectedIndex = 0;
		}

		private void storyButton_Click( object sender, RoutedEventArgs e )
		{
			TextBookData data = new TextBookData();
			data.pages.Add( campaign.storyText );

			TextEditorWindow tw = new TextEditorWindow( null, EditMode.Story, data );
			if ( tw.ShowDialog() == true )
			{
				campaign.storyText = tw.textBookController.pages[0].Trim();
				;
			}

			if ( campaign.storyText == "" )
				storySet.Text = "The Story Text is not set.";
			else
				storySet.Text = "The Story Text is set.";
		}

		private void help_Click( object sender, RoutedEventArgs e )
		{
			HelpWindow hw = new HelpWindow( HelpType.Triggers, 2 );
			hw.ShowDialog();
		}

		private void openExplorer_Click( object sender, RoutedEventArgs e )
		{
			System.Diagnostics.Process.Start( campaignFolder );
		}
	}
}
