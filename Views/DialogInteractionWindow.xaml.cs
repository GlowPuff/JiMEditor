using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace JiME.Views
{
	/// <summary>
	/// Interaction logic for DialogInteractionWindow.xaml
	/// </summary>
	public partial class DialogInteractionWindow : Window, INotifyPropertyChanged
	{
		string oldName;
		public Scenario scenario { get; set; }
		public DialogInteraction interaction { get; set; }
		bool closing = false;

		public event PropertyChangedEventHandler PropertyChanged;
		bool _isThreatTriggered;
		public bool isThreatTriggered
		{
			get => _isThreatTriggered;
			set
			{
				_isThreatTriggered = value;
				PropChanged( "isThreatTriggered" );
			}
		}

		public DialogInteractionWindow( Scenario s, DialogInteraction inter = null )
		{
			InitializeComponent();
			DataContext = this;

			scenario = s;
			cancelButton.Visibility = inter == null ? Visibility.Visible : Visibility.Collapsed;
			interaction = inter ?? new DialogInteraction( "New Dialog Event" );

			isThreatTriggered = scenario.threatObserver.Any( x => x.triggerName == interaction.dataName );
			if ( isThreatTriggered )
			{
				addMainTriggerButton.IsEnabled = false;
				triggeredByCB.IsEnabled = false;
				isTokenCB.IsEnabled = false;
				interaction.isTokenInteraction = false;
			}

			if ( interaction.isTokenInteraction && interaction.tokenType == TokenType.Person )
				personType.Visibility = Visibility.Visible;
			humanRadio.IsChecked = interaction.personType == PersonType.Human;
			elfRadio.IsChecked = interaction.personType == PersonType.Elf;
			hobbitRadio.IsChecked = interaction.personType == PersonType.Hobbit;
			dwarfRadio.IsChecked = interaction.personType == PersonType.Dwarf;

			personRadio.IsChecked = interaction.tokenType == TokenType.Person;
			searchRadio.IsChecked = interaction.tokenType == TokenType.Search;
			darkRadio.IsChecked = interaction.tokenType == TokenType.Darkness;
			threatRadio.IsChecked = interaction.tokenType == TokenType.Threat;

			oldName = interaction.dataName;
		}

		void PropChanged( string name )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
		}

		private void okButton_Click( object sender, RoutedEventArgs e )
		{
			if ( !TryClosing() )
				return;

			if ( searchRadio.IsChecked.HasValue && searchRadio.IsChecked.Value )
				interaction.tokenType = TokenType.Search;
			if ( personRadio.IsChecked.HasValue && personRadio.IsChecked.Value )
				interaction.tokenType = TokenType.Person;
			if ( darkRadio.IsChecked.HasValue && darkRadio.IsChecked.Value )
				interaction.tokenType = TokenType.Darkness;
			if ( threatRadio.IsChecked.HasValue && threatRadio.IsChecked.Value )
				interaction.tokenType = TokenType.Threat;

			if ( humanRadio.IsChecked == true )
				interaction.personType = PersonType.Human;
			if ( elfRadio.IsChecked == true )
				interaction.personType = PersonType.Elf;
			if ( hobbitRadio.IsChecked == true )
				interaction.personType = PersonType.Hobbit;
			if ( dwarfRadio.IsChecked == true )
				interaction.personType = PersonType.Dwarf;

			scenario.UpdateEventReferences( oldName, interaction );

			closing = true;
			DialogResult = true;
		}

		private void Window_Closing( object sender, CancelEventArgs e )
		{
			if ( !closing )
				e.Cancel = true;
		}

		bool TryClosing()
		{
			//check for dupe name
			if ( interaction.dataName == "New Dialog Event" || scenario.interactionObserver.Count( x => x.dataName == interaction.dataName ) > 1 )
			{
				MessageBox.Show( "Give this Event a unique name.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error );
				return false;
			}

			return true;
		}

		private void Window_ContentRendered( object sender, EventArgs e )
		{
			nameTB.Focus();
			nameTB.SelectAll();
		}

		private void story1_Click( object sender, RoutedEventArgs e )
		{
			TextBookData tbd = new TextBookData( "Dialog Text" );
			tbd.pages.Add( interaction.c1Text );

			TextEditorWindow te = new TextEditorWindow( scenario, EditMode.Dialog, tbd );
			if ( te.ShowDialog() == true )
			{
				interaction.c1Text = te.textBookController.pages[0];
			}
		}

		private void story2_Click( object sender, RoutedEventArgs e )
		{
			TextBookData tbd = new TextBookData( "Dialog Text" );
			tbd.pages.Add( interaction.c2Text );

			TextEditorWindow te = new TextEditorWindow( scenario, EditMode.Dialog, tbd );
			if ( te.ShowDialog() == true )
			{
				interaction.c2Text = te.textBookController.pages[0];
			}
		}

		private void story3_Click( object sender, RoutedEventArgs e )
		{
			TextBookData tbd = new TextBookData( "Dialog Text" );
			tbd.pages.Add( interaction.c3Text );

			TextEditorWindow te = new TextEditorWindow( scenario, EditMode.Dialog, tbd );
			if ( te.ShowDialog() == true )
			{
				interaction.c3Text = te.textBookController.pages[0];
			}
		}

		private void addTrigger1Button_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				interaction.c1Trigger = tw.triggerName;
			}
		}

		private void addTrigger2Button_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				interaction.c2Trigger = tw.triggerName;
			}
		}

		private void addTrigger3Button_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				interaction.c3Trigger = tw.triggerName;
			}
		}

		private void cancelButton_Click( object sender, RoutedEventArgs e )
		{
			closing = true;
			DialogResult = false;
		}

		private void tokenHelp_Click( object sender, RoutedEventArgs e )
		{
			HelpWindow hw = new HelpWindow( HelpType.Token, 1 );
			hw.ShowDialog();
		}

		private void groupHelp_Click( object sender, RoutedEventArgs e )
		{
			HelpWindow hw = new HelpWindow( HelpType.Grouping );
			hw.ShowDialog();
		}

		private void nameTB_TextChanged( object sender, TextChangedEventArgs e )
		{
			interaction.dataName = ( (TextBox)sender ).Text;
			Regex rx = new Regex( @"\sGRP\d+$" );
			MatchCollection matches = rx.Matches( interaction.dataName );
			if ( matches.Count > 0 )
				groupInfo.Text = "This Event is in the following group: " + matches[0].Value.Trim();
			else
				groupInfo.Text = "This Event is in the following group: None";
		}

		private void tokenTypeClick( object sender, RoutedEventArgs e )
		{
			RadioButton rb = e.Source as RadioButton;
			if ( ( (string)rb.Content ) == "Person" )
				personType.Visibility = Visibility.Visible;
			else
				personType.Visibility = Visibility.Collapsed;
		}

		private void isTokenCB_Click( object sender, RoutedEventArgs e )
		{
			if ( isTokenCB.IsChecked == true )
			{
				interaction.triggerName = "None";
				personType.Visibility = personRadio.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
			}
			else
				personType.Visibility = Visibility.Collapsed;
		}

		private void addMainTriggerButton_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				interaction.triggerName = tw.triggerName;
			}
		}

		private void addMainTriggerAfterButton_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				interaction.triggerAfterName = tw.triggerName;
			}
		}

		private void EditFlavorButton_Click( object sender, RoutedEventArgs e )
		{
			TextEditorWindow tw = new TextEditorWindow( scenario, EditMode.Flavor, interaction.textBookData );
			if ( tw.ShowDialog() == true )
			{
				interaction.textBookData.pages = tw.textBookController.pages;
				flavorTB.Text = tw.textBookController.pages[0];
			}
		}

		private void EditEventButton_Click( object sender, RoutedEventArgs e )
		{
			TextEditorWindow tw = new TextEditorWindow( scenario, EditMode.Progress, interaction.eventBookData );
			if ( tw.ShowDialog() == true )
			{
				interaction.eventBookData.pages = tw.textBookController.pages;
				eventTB.Text = tw.textBookController.pages[0];
			}
		}

		private void editPersText_Click( object sender, RoutedEventArgs e )
		{
			TextBookData tbd = new TextBookData( "Dialog Text" );
			tbd.pages.Add( interaction.persistentText );

			TextEditorWindow te = new TextEditorWindow( scenario, EditMode.Persistent, tbd );
			if ( te.ShowDialog() == true )
			{
				interaction.persistentText = te.textBookController.pages[0];
			}
		}
	}
}
