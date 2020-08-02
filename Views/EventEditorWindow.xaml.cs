using System.Windows;
using System.Windows.Controls;

namespace JiME.Views
{
	/// <summary>
	/// Interaction logic for EventEditorWindow.xaml
	/// </summary>
	public partial class EventEditorWindow : Window
	{
		public Scenario scenario { get; set; }
		public Interaction interaction { get; set; }
		bool closing = false;

		public EventEditorWindow( Scenario s, Interaction inter = null, bool fromThreat = false )
		{
			InitializeComponent();
			DataContext = this;

			scenario = s;
			cancelButton.Visibility = inter == null ? Visibility.Visible : Visibility.Collapsed;
			interaction = inter ?? new Interaction( "New Event", false );

			if ( fromThreat )
				interaction.isFromThreatThreshold = true;
			//radio buttons
			textRB.IsChecked = interaction.interactionType == InteractionType.Text;
			threatRB.IsChecked = interaction.interactionType == InteractionType.Threat;
			testRB.IsChecked = interaction.interactionType == InteractionType.StatTest;
			decisionRB.IsChecked = interaction.interactionType == InteractionType.Decision;
			branchRB.IsChecked = interaction.interactionType == InteractionType.Branch;
			//attribute page
			mightRB.IsChecked = interaction.testAttribute == Ability.Might;
			agilityRB.IsChecked = interaction.testAttribute == Ability.Agility;
			spiritRB.IsChecked = interaction.testAttribute == Ability.Spirit;
			wisdomRB.IsChecked = interaction.testAttribute == Ability.Wisdom;
			witRB.IsChecked = interaction.testAttribute == Ability.Wit;

			//if ( textRB.IsChecked.Value )
			//	flavorbox.Visibility = Visibility.Collapsed;

			if ( interaction.isFromThreatThreshold )
			{
				triggeredByCB.IsEnabled = false;
				isEventBox.Visibility = Visibility.Collapsed;
				isRandomBox.Visibility = Visibility.Collapsed;
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
			tw.ShowDialog();
			interaction.eventBookData.pages = tw.textBookController.pages;
		}

		private void AddMonsterButton_Click( object sender, RoutedEventArgs e )
		{
			MonsterEditorWindow me = new MonsterEditorWindow();
			if ( me.ShowDialog() == true )
			{
				interaction.AddMonster( me.monster );
			}
		}

		private void EditButton_Click( object sender, RoutedEventArgs e )
		{
			Monster m = ( (Button)sender ).DataContext as Monster;
			MonsterEditorWindow me = new MonsterEditorWindow( m );
			me.ShowDialog();
		}

		private void DeleteButton_Click( object sender, RoutedEventArgs e )
		{
			Monster m = ( (Button)sender ).DataContext as Monster;
			interaction.monsterCollection.Remove( m );
		}

		private void EditProgress_Click( object sender, RoutedEventArgs e )
		{
			TextEditorWindow tw = new TextEditorWindow( scenario, EditMode.Progress, interaction.progressBookData );
			tw.ShowDialog();
			interaction.progressBookData.pages = tw.textBookController.pages;
		}

		private void EditFail_Click( object sender, RoutedEventArgs e )
		{
			TextEditorWindow tw = new TextEditorWindow( scenario, EditMode.Fail, interaction.failBookData );
			tw.ShowDialog();
			interaction.failBookData.pages = tw.textBookController.pages;
		}

		private void EditPass_Click( object sender, RoutedEventArgs e )
		{
			TextEditorWindow tw = new TextEditorWindow( scenario, EditMode.Pass, interaction.passBookData );
			tw.ShowDialog();
			interaction.passBookData.pages = tw.textBookController.pages;
		}

		private void AddTrigger1Button_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				interaction.choice1Trigger = tw.triggerName;
			}
		}

		private void AddTrigger2Button_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				interaction.choice2Trigger = tw.triggerName;
			}
		}

		private void AddTrigger3Button_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				interaction.choice3Trigger = tw.triggerName;
			}
		}

		private void AddTriggerPassButton_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				interaction.successTrigger = tw.triggerName;
			}
		}

		private void AddTriggerFailButton_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				interaction.failTrigger = tw.triggerName;
			}
		}

		private void Window_ContentRendered( object sender, System.EventArgs e )
		{
			nameTB.Focus();
			nameTB.SelectAll();
		}

		bool TryClosing()
		{
			if ( textRB.IsChecked == true )
				interaction.interactionType = InteractionType.Text;
			else if ( decisionRB.IsChecked == true )
				interaction.interactionType = InteractionType.Decision;
			else if ( testRB.IsChecked == true )
				interaction.interactionType = InteractionType.StatTest;
			else if ( threatRB.IsChecked == true )
				interaction.interactionType = InteractionType.Threat;
			else if ( branchRB.IsChecked == true )
				interaction.interactionType = InteractionType.Branch;

			//var retval = ErrorChecker.CheckInteraction( interaction );
			//if ( !retval.Item1 )
			//{
			//	MessageBox.Show( retval.Item2, retval.Item3, MessageBoxButton.OK, MessageBoxImage.Error );
			//	return false;
			//}

			if ( interaction.isTokenInteraction && ( interaction.triggerName == "None" || interaction.triggerName.Contains( "Random" ) ) )
			{
				MessageBox.Show( "A Token Interaction cannot have 'Triggered By' set to 'None' or 'Random Event Trigger'.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error );
				return false;
			}

			return true;
		}

		private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
		{
			if ( !closing && !TryClosing() )
				e.Cancel = true;
			else if ( !closing )
				DialogResult = false;
		}

		private void OkButton_Click( object sender, RoutedEventArgs e )
		{
			if ( !TryClosing() )
				return;
			closing = true;
			DialogResult = true;
		}

		private void CancelButton_Click( object sender, RoutedEventArgs e )
		{
			closing = true;
			DialogResult = false;
		}

		private void RB_Click( object sender, RoutedEventArgs e )
		{
			//if ( !interaction.isFromThreatThreshold )
			//{
			//	string txt = ( (RadioButton)sender ).Content as string;
			//	if ( txt == "Text Popup" )
			//		flavorbox.Visibility = Visibility.Collapsed;
			//	else
			//		flavorbox.Visibility = Visibility.Visible;
			//}
			//else
			//{
			//	flavorbox.Visibility = Visibility.Collapsed;
			//}
		}

		//triggered by
		private void ComboBox_SelectionChanged( object sender, SelectionChangedEventArgs e )
		{
			//if ( !interaction.isFromThreatThreshold )
			//{
			//	if ( interaction.triggerName == "None" )
			//		eventbox.Visibility = Visibility.Visible;
			//	else
			//		eventbox.Visibility = Visibility.Collapsed;
			//}
			//else
			//	flavorbox.Visibility = Visibility.Collapsed;
		}

		private void RandomCB_Click( object sender, RoutedEventArgs e )
		{
			interaction.triggerName = "None";
		}

		private void AddTriggerButton_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
				interaction.triggerTest = tw.triggerName;
		}

		private void AddTriggerButton2_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
				interaction.triggerIsSetTrigger = tw.triggerName;
		}

		private void AddTriggerButton3_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
				interaction.triggerNotSetTrigger = tw.triggerName;
		}

		private void triggerCB_SelectionChanged( object sender, SelectionChangedEventArgs e )
		{
			//don't let "Random Event" be selected
			if ( ( (ComboBox)sender ).SelectedIndex == 1 )
				( (ComboBox)sender ).SelectedIndex = 0;
		}
	}
}
