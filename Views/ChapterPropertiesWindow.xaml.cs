using System.Windows;
using System.Linq;
using System.Collections.Generic;

namespace JiME.Views
{
	/// <summary>
	/// Interaction logic for ChapterPropertiesWindow.xaml
	/// </summary>
	public partial class ChapterPropertiesWindow : Window
	{
		public Chapter chapter { get; set; }
		public Scenario scenario { get; set; }
		bool closing = false, currentRandomToggle;

		public ChapterPropertiesWindow( Scenario s, Chapter c = null )
		{
			InitializeComponent();

			SourceInitialized += ( x, y ) =>
			{
				this.HideMinimizeAndMaximizeButtons();
			};

			DataContext = this;

			cancelButton.Visibility = c == null ? Visibility.Visible : Visibility.Collapsed;
			chapter = c ?? new Chapter( "New Chapter" );
			scenario = s;
			currentRandomToggle = chapter.isRandomTiles;
		}

		private void OkButton_Click( object sender, RoutedEventArgs e )
		{
			if ( !TryClose() )
				return;
			closing = true;
			DialogResult = true;
		}

		private void CancelButton_Click( object sender, RoutedEventArgs e )
		{
			closing = true;
			DialogResult = false;
		}

		bool TryClose()
		{
			if ( chapter.dataName != "Start"
				&& ( chapter.triggeredBy == "None" || chapter.triggeredBy == "Trigger Random Event" ) )
			{
				MessageBox.Show( "'Triggered By' cannot be set to None or Trigger Random Event.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error );
				return false;
			}
			else if ( string.IsNullOrEmpty( chapter.dataName.Trim() ) )
			{
				MessageBox.Show( "'Chapter Name' cannot be empty.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error );
				return false;
			}

			return true;
		}

		private void EditFlavorButton_Click( object sender, RoutedEventArgs e )
		{
			TextEditorWindow tw = new TextEditorWindow( scenario, EditMode.Flavor, chapter.flavorBookData );
			if ( tw.ShowDialog() == true )
				chapter.flavorBookData.pages = tw.textBookController.pages;
		}

		private void Window_ContentRendered( object sender, System.EventArgs e )
		{
			nameTB.Focus();
			nameTB.SelectAll();
		}

		private void AddExploreTriggerButton_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				chapter.exploreTrigger = tw.triggerName;
			}
		}

		private void AddTriggerByButton_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				chapter.triggeredBy = tw.triggerName;
			}
		}

		private void TileEditButton_Click( object sender, RoutedEventArgs e )
		{
			if ( chapter.isRandomTiles )
			{
				TilePoolEditorWindow tp = new TilePoolEditorWindow( scenario, chapter );
				tp.ShowDialog();
			}
			else
			{
				TileEditorWindow tw = new TileEditorWindow( scenario, chapter );
				tw.ShowDialog();
			}
		}

		private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
		{
			if ( !closing )
				e.Cancel = !TryClose();
		}

		private void RandomToggleCB_Click( object sender, RoutedEventArgs e )
		{
			if ( chapter.isRandomTiles != currentRandomToggle )
			{
				var ret = MessageBox.Show( "Are you sure you want to toggle between Random Tiles and Fixed Tiles?\n\nALL TILE DATA IN THIS CHAPTER WILL BE RESET IF YOU SWITCH.", "Switch Between Random and Fixed Tiles", MessageBoxButton.YesNo, MessageBoxImage.Question );
				if ( ret == MessageBoxResult.Yes )
				{
					currentRandomToggle = chapter.isRandomTiles;
					if ( chapter.isRandomTiles )
					{
						foreach ( HexTile tile in chapter.tileObserver )
							scenario.globalTilePool.Add( tile.idNumber );
						chapter.tileObserver.Clear();
					}
					else
					{
						foreach ( int tile in chapter.randomTilePool )
							scenario.globalTilePool.Add( tile );
						chapter.randomTilePool.Clear();
					}

					List<int> sorted = scenario.globalTilePool.OrderBy( key => key ).ToList();
					for ( int i = 0; i < sorted.Count; i++ )
						scenario.globalTilePool[i] = sorted[i];
				}
				else
					chapter.isRandomTiles = currentRandomToggle;
			}
		}
	}
}
