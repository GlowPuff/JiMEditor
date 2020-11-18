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
				tokenHelp.Items.OfType<TabItem>().ToArray()[tab].IsSelected = true;
			}
			else if ( helpType == HelpType.Grouping )
			{
				Title = "Help On Token Interaction Groups";
				groupHelp.Visibility = Visibility.Visible;
				groupHelp.Items.OfType<TabItem>().ToArray()[tab].IsSelected = true;
			}
			else if ( helpType == HelpType.Enemies )
			{
				Title = "Help On Enemy Damage and Difficulty";
				threatHelp.Visibility = Visibility.Visible;
				threatHelp.Items.OfType<TabItem>().ToArray()[tab].IsSelected = true;
			}
			else if ( helpType == HelpType.Triggers )
			{
				Title = "Help On Triggers";
				triggerHelp.Visibility = Visibility.Visible;
				triggerHelp.Items.OfType<TabItem>().ToArray()[tab].IsSelected = true;
			}
		}

		private void okButton_Click( object sender, RoutedEventArgs e )
		{
			DialogResult = true;
		}
	}
}
