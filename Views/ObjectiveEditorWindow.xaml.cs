using System.Windows;

namespace JiME.Views
{
	/// <summary>
	/// Interaction logic for ObjectiveWindow.xaml
	/// </summary>
	public partial class ObjectiveEditorWindow : Window
	{
		public Scenario scenario { get; set; }
		public Objective objective { get; set; }
		public string shortName { get; set; }

		public ObjectiveEditorWindow( Scenario s, Objective obj, bool isNew = true )
		{
			InitializeComponent();
			DataContext = this;

			scenario = s;
			objective = obj;

			shortName = obj.dataName;

			//triggerCB.ItemsSource = scenario.triggersObserver;
			//triggerCB.SelectedItem = (Trigger)scenario.GetData<Trigger>( obj.triggerName );

			if ( !isNew )
				cancelButton.Visibility = Visibility.Collapsed;
		}

		private void OkButton_Click( object sender, RoutedEventArgs e )
		{
			objective.dataName = shortName.Trim();

			//check empty string
			if ( string.IsNullOrEmpty( objective.dataName ) || string.IsNullOrEmpty( objective.objectiveReminder ) )
			{
				MessageBox.Show( "The Objective Name and Objective Reminder cannot be empty.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error );
				return;
			}

			//check if name duplicated
			//string ret = scenario.IsDuplicate( objective );
			if ( scenario.IsDuplicate( objective ) )//ret != null )
			{
				MessageBox.Show( $"An Objective with name [{objective.dataName}] already exists.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error );
				return;
			}

			//check if trigger isn't set
			//if ( ( (Trigger)triggerCB.SelectedItem ).dataName == "None"
			//	|| ( (Trigger)triggerCB.SelectedItem ).dataName.Contains( "Random" ) )
			//{
			//	MessageBox.Show( "You must select a Trigger.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error );
			//	return;
			//}

			//objective.triggerName = ( (Trigger)triggerCB.SelectedItem ).dataName;
			DialogResult = true;
		}

		private void CancelButton_Click( object sender, RoutedEventArgs e )
		{
			DialogResult = false;
		}

		private void AddTriggerButton_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				triggerCB.SelectedIndex = scenario.triggersObserver.Count - 1;
			}
		}

		private void addTriggerButton2_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				nextTriggerCB.SelectedIndex = scenario.triggersObserver.Count - 1;
			}
		}

		private void addTriggeredByButton_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				triggeredByCB.SelectedIndex = scenario.triggersObserver.Count - 1;
			}
		}

		private void EditObjectiveButton_Click( object sender, RoutedEventArgs e )
		{
			TextEditorWindow te = new TextEditorWindow( scenario, EditMode.Objective, objective.textBookData );
			if ( te.ShowDialog() == true )
			{
				objective.textBookData.pages = te.textBookController.pages;
			}
		}

		private void Window_ContentRendered( object sender, System.EventArgs e )
		{
			nameTB.Focus();
			nameTB.SelectAll();
		}
	}
}
