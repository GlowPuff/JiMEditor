using System.Windows;

namespace JiME.Views
{
	/// <summary>
	/// Interaction logic for TriggerEditorWindow.xaml
	/// </summary>
	public partial class TriggerEditorWindow : Window
	{
		public string triggerName { get; set; }
		public Trigger newTrigger;

		Scenario scenario;
		bool isNew = false;
		string oldName;

		/// <summary>
		/// Edit existing Trigger
		/// </summary>
		public TriggerEditorWindow( Scenario s, string editName )
		{

			InitializeComponent();
			DataContext = this;
			scenario = s;
			nameTB.Text = editName;
			nameTB.SelectAll();
			oldName = editName;
		}

		/// <summary>
		/// Create new Trigger
		/// </summary>
		public TriggerEditorWindow( Scenario s ) : this( s, "" )
		{
			isNew = true;
		}

		private void OkButton_Click( object sender, RoutedEventArgs e )
		{
			triggerName = nameTB.Text.Trim();
			newTrigger = new Trigger( nameTB.Text );

			if ( oldName == triggerName )
			{
				DialogResult = false;
				return;
			}

			if ( triggerName != string.Empty )
			{
				if ( !isNew )//renaming
				{
					if ( triggerName != string.Empty )
						if ( scenario.RenameTrigger( oldName, triggerName ) )
						{
							DialogResult = true;
							return;
						}
				}

				if ( triggerName.Contains( "Random" ) )
				{
					MessageBox.Show( "The word 'Random' cannot be used in the Trigger Name.", "Invalid Trigger Name", MessageBoxButton.OK, MessageBoxImage.Error );
					return;
				}

				if ( scenario.AddTrigger( triggerName ) )//new trigger
					DialogResult = true;
				else
				{
					MessageBox.Show( "A Trigger with this name already exists.", "Invalid Trigger Name", MessageBoxButton.OK, MessageBoxImage.Error );
					nameTB.Focus();
				}
			}
			else
			{
				MessageBox.Show( "The Trigger Name cannot be empty.", "Invalid Trigger Name", MessageBoxButton.OK, MessageBoxImage.Error );
				nameTB.Focus();
			}
		}

		private void CancelButton_Click( object sender, RoutedEventArgs e )
		{
			DialogResult = false;
		}

		private void Window_ContentRendered( object sender, System.EventArgs e )
		{
			nameTB.Focus();
		}
	}
}
