using System;
using System.Windows.Controls;

namespace JiME.UserControls
{
	/// <summary>
	/// Interaction logic for SidebarListView.xaml
	/// </summary>
	public partial class SidebarListView : UserControl
	{
		public string Title { get; set; }
		public Array ListData { get; set; }

		public EventHandler onAddEvent, onRemoveEvent, onSettingsEvent;

		public SidebarListView()
		{
			InitializeComponent();
			DataContext = this;
		}

		private void AddInteraction_Click( object sender, System.Windows.RoutedEventArgs e )
		{
			onAddEvent?.Invoke( sender, e );
		}

		private void Settings_Click( object sender, System.Windows.RoutedEventArgs e )
		{
			onSettingsEvent?.Invoke( sender, e );
		}

		private void RemoveInteraction_Click( object sender, System.Windows.RoutedEventArgs e )
		{
			onRemoveEvent?.Invoke( sender, e );
		}
	}
}
