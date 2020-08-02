using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace JiME.Views
{
	public partial class HelpWindow : Window
	{
		public HelpWindow( HelpType helpType, int tab = 0 )
		{
			InitializeComponent();

			if ( helpType == HelpType.Token )
			{
				Title = "Help On Tokens and Events";
				tokenHelp.Visibility = Visibility.Visible;
				groupHelp.Visibility = Visibility.Collapsed;
				tokenHelp.Items.OfType<TabItem>().ToArray()[tab].IsSelected = true;
			}
			else if ( helpType == HelpType.Grouping )
			{
				Title = "Help On Token Interaction Groups";
				tokenHelp.Visibility = Visibility.Collapsed;
				groupHelp.Visibility = Visibility.Visible;
				groupHelp.Items.OfType<TabItem>().ToArray()[tab].IsSelected = true;
			}
		}

		private void okButton_Click( object sender, RoutedEventArgs e )
		{
			DialogResult = true;
		}
	}
}
