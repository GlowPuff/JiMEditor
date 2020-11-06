using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using JiME.Views;

namespace JiME
{
	/// <summary>
	/// Interaction logic for ProjectWindow.xaml
	/// </summary>
	public partial class ProjectWindow : Window, INotifyPropertyChanged
	{
		//scenarioName, fileName
		public ObservableCollection<ProjectItem> projectCollection;

		public event PropertyChangedEventHandler PropertyChanged;

		public bool scrollVisible { get; set; }

		public ProjectWindow()
		{
			InitializeComponent();
			DataContext = this;

			scrollVisible = false;

			projectCollection = new ObservableCollection<ProjectItem>();
			projectLV.ItemsSource = projectCollection;

			//poll Project folder for files and populate Recent list
			var projects = FileManager.GetProjects();
			if ( projects != null )
			{
				foreach ( ProjectItem pi in projects )
				{
					projectCollection.Add( pi );
				}
			}
			else
			{
				throw new Exception( "Could not properly load scenario projects." );
			}

			formatVersion.Text = "Scenario Format Version: v." + Utils.formatVersion;
		}

		void debug()
		{
			projectCollection.Add( new ProjectItem() { Title = "A Worthy Journey", Date = "7/3/2019", Description = "Some text here.", projectType = ProjectType.Standalone } );
			projectCollection.Add( new ProjectItem() { Title = "A Worthy Journey", Date = "7/3/2019", Description = "Some text here.", projectType = ProjectType.Campaign } );
			projectCollection.Add( new ProjectItem() { Title = "A Worthy Journey", Date = "7/3/2019", Description = "Some text here.", projectType = ProjectType.Standalone } );
			projectCollection.Add( new ProjectItem() { Title = "A Worthy Journey", Date = "7/3/2019", Description = "Some text here.", projectType = ProjectType.Campaign } );
			projectCollection.Add( new ProjectItem() { Title = "A Worthy Journey", Date = "7/3/2019", Description = "Some text here.", projectType = ProjectType.Standalone } );
			projectCollection.Add( new ProjectItem() { Title = "A Worthy Journey", Date = "7/3/2019", Description = "Some text here.", projectType = ProjectType.Standalone } );
		}

		private void ProjectLV_MouseEnter( object sender, System.Windows.Input.MouseEventArgs e )
		{
			scrollVisible = true;
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( "scrollVisible" ) );
		}

		private void ProjectLV_MouseLeave( object sender, System.Windows.Input.MouseEventArgs e )
		{
			scrollVisible = false;
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( "scrollVisible" ) );
		}

		private void CancelButton_Click( object sender, RoutedEventArgs e )
		{
			Application.Current.Shutdown();
		}

		private void ScrollViewer_PreviewMouseWheel( object sender, System.Windows.Input.MouseWheelEventArgs e )
		{
			ScrollViewer scv = (ScrollViewer)sender;
			scv.ScrollToVerticalOffset( scv.VerticalOffset - e.Delta / 10 );
			e.Handled = true;
		}

		private void ScenarioButton_Click( object sender, RoutedEventArgs e )
		{
			MainWindow mainWindow = new MainWindow( Guid.Empty );
			mainWindow.Show();
			Close();
		}

		private void CampaignButton_Click( object sender, RoutedEventArgs e )
		{
			CampaignWindow campaignWindow = new CampaignWindow();
			campaignWindow.Show();
			Close();
		}

		private void ProjectLV_SelectionChanged( object sender, SelectionChangedEventArgs e )
		{
			ProjectItem item = ( (ListView)e.Source ).SelectedItem as ProjectItem;
			if ( item != null && item.projectType == ProjectType.Standalone )
			{
				var project = FileManager.LoadProject( item.fileName );
				if ( project != null )
				{
					MainWindow mainWindow = new MainWindow( project );
					mainWindow.Show();
					Close();
				}
			}
			else if ( item != null && item.projectType == ProjectType.Campaign )
			{
				//load the campaign object from item.filename
				Campaign c = FileManager.LoadCampaign( item.fileName );
				if ( c != null )
				{
					CampaignWindow cw = new CampaignWindow( c );
					cw.Show();
					Close();
				}
			}
		}

		private void Window_MouseDown( object sender, System.Windows.Input.MouseButtonEventArgs e )
		{
			this.DragMove();
		}
	}
}
