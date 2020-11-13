using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.ComponentModel;
using System.Text.RegularExpressions;


namespace JiME.Views
{
	/// <summary>
	/// Interaction logic for MultiEventWindow.xaml
	/// </summary>
	public partial class MultiEventWindow : Window, INotifyPropertyChanged
	{
		string oldName;

		public Scenario scenario { get; set; }
		public MultiEventInteraction interaction { get; set; }
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

		public MultiEventWindow( Scenario s, MultiEventInteraction inter = null )
		{
			InitializeComponent();
			DataContext = this;

			scenario = s;
			cancelButton.Visibility = inter == null ? Visibility.Visible : Visibility.Collapsed;
			interaction = inter ?? new MultiEventInteraction( "New Multi-Event" );

			triggerRB.IsChecked = interaction.usingTriggers;
			eventRB.IsChecked = !interaction.usingTriggers;

			meTriggerBox.IsEnabled = triggerRB.IsChecked.Value;
			meEventBox.IsEnabled = eventRB.IsChecked.Value;
			eventbox.IsEnabled = !interaction.isSilent;

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

		private void EditFlavorButton_Click( object sender, RoutedEventArgs e )
		{
			TextEditorWindow tw = new TextEditorWindow( scenario, EditMode.Flavor, interaction.textBookData );
			if ( tw.ShowDialog() == true )
			{
				interaction.textBookData.pages = tw.textBookController.pages;
			}
		}

		private void EditEventButton_Click( object sender, RoutedEventArgs e )
		{
			TextEditorWindow tw = new TextEditorWindow( scenario, EditMode.Progress, interaction.eventBookData );
			if ( tw.ShowDialog() == true )
			{
				interaction.eventBookData.pages = tw.textBookController.pages;
			}
		}

		private void OkButton_Click( object sender, RoutedEventArgs e )
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

			interaction.usingTriggers = triggerRB.IsChecked.Value;

			closing = true;
			DialogResult = true;
		}

		private void CancelButton_Click( object sender, RoutedEventArgs e )
		{
			closing = true;
			DialogResult = false;
		}

		private void Window_Closing( object sender, CancelEventArgs e )
		{
			if ( !closing )
				e.Cancel = true;
		}

		bool TryClosing()
		{
			//check for dupe name
			if ( interaction.dataName == "New Multi-Event" || scenario.interactionObserver.Count( x => x.dataName == interaction.dataName ) > 1 )
			{
				MessageBox.Show( "Give this Event a unique name.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error );
				return false;
			}

			return true;
		}

		private void Window_ContentRendered( object sender, System.EventArgs e )
		{
			nameTB.Focus();
			nameTB.SelectAll();
			triggerCB.SelectedIndex = 0;
			eventCB.SelectedIndex = 0;
		}

		private void addMainTriggerAfterButton_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				interaction.triggerAfterName = tw.triggerName;
			}
		}

		private void addMainTriggerButton_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				interaction.triggerName = tw.triggerName;
			}
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

		void PropChanged( string name )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
		}

		private void removeTriggerButton_Click( object sender, RoutedEventArgs e )
		{
			string sel = ( (Button)sender ).DataContext as string;
			if ( interaction.triggerList.Contains( sel ) )
				interaction.triggerList.Remove( sel );
		}

		private void addSelectedTriggerButton_Click( object sender, RoutedEventArgs e )
		{
			string t = triggerCB.SelectedValue as string;
			if ( !interaction.triggerList.Contains( t ) )
			{
				interaction.triggerList.Add( t );
			}
		}

		private void AddTriggerButton_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				if ( !interaction.triggerList.Contains( tw.triggerName ) )
					interaction.triggerList.Add( tw.triggerName );
			}
		}

		private void removeEventButton_Click( object sender, RoutedEventArgs e )
		{
			string sel = ( (Button)sender ).DataContext as string;
			if ( interaction.eventList.Contains( sel ) )
				interaction.eventList.Remove( sel );
		}

		private void addSelectedEventButton_Click( object sender, RoutedEventArgs e )
		{
			string t = eventCB.SelectedValue as string;
			if ( !interaction.eventList.Contains( t ) )
			{
				interaction.eventList.Add( t );
			}
		}

		private void triggerRB_Click( object sender, RoutedEventArgs e )
		{
			meTriggerBox.IsEnabled = true;
			meEventBox.IsEnabled = false;
		}

		private void eventRB_Click( object sender, RoutedEventArgs e )
		{
			meTriggerBox.IsEnabled = false;
			meEventBox.IsEnabled = true;
		}

		private void silenCB_Click( object sender, RoutedEventArgs e )
		{
			eventbox.IsEnabled = !( (CheckBox)sender ).IsChecked.Value;
		}

		private void eventCB_SelectionChanged( object sender, SelectionChangedEventArgs e )
		{
			addSelectedEventButton.IsEnabled = eventCB.SelectedIndex != 0;
		}

		private void triggerCB_SelectionChanged( object sender, SelectionChangedEventArgs e )
		{
			addSelectedTriggerButton.IsEnabled = triggerCB.SelectedIndex != 0;
		}

		private void tokenTypeClick( object sender, RoutedEventArgs e )
		{
			RadioButton rb = e.Source as RadioButton;
			if ( ( (string)rb.Content ) == "Person" )
				personType.Visibility = Visibility.Visible;
			else
				personType.Visibility = Visibility.Collapsed;
		}
	}
}
