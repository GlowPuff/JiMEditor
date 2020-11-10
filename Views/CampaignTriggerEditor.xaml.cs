using System.Linq;
using System.Windows;

namespace JiME.Views
{
	/// <summary>
	/// Interaction logic for CampaignTriggerEditor.xaml
	/// </summary>
	public partial class CampaignTriggerEditor : Window
	{
		public string triggerName { get; set; }
		public bool isMulti { get; set; }

		Campaign campaign;

		public CampaignTriggerEditor( Campaign c )
		{
			InitializeComponent();
			InitializeComponent();
			DataContext = this;
			nameTB.Text = "";
			nameTB.SelectAll();
			campaign = c;
		}

		private void okButton_Click( object sender, RoutedEventArgs e )
		{
			triggerName = nameTB.Text.Trim();
			if ( triggerName == "" )
			{
				MessageBox.Show( "The name can't be an empty string.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error );
				return;
			}

			triggerName += " [CMPN]";

			if ( campaign.triggerCollection.Any( x => x.dataName == triggerName ) )
			{
				MessageBox.Show( "A Campaign Trigger with this name already exists.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error );
				return;
			}

			isMulti = multiCB.IsChecked.Value;
			DialogResult = true;
		}

		private void cancelButton_Click( object sender, RoutedEventArgs e )
		{
			DialogResult = false;
		}

		private void help_Click( object sender, RoutedEventArgs e )
		{
			HelpWindow hw = new HelpWindow( HelpType.Triggers, 2 );
			hw.ShowDialog();
		}

		private void Window_ContentRendered( object sender, System.EventArgs e )
		{
			nameTB.Focus();
		}
	}
}
