using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace JiME.Views
{
	/// <summary>
	/// Interaction logic for TileEditorWindow.xaml
	/// </summary>
	public partial class TileEditorWindow : Window, INotifyPropertyChanged
	{
		HexTile _selected;
		public HexTile selected
		{
			get => _selected;
			set
			{
				_selected = value;
				PropChanged( "selected" );
			}
		}
		public Chapter chapter { get; set; }
		public Scenario scenario { get; set; }

		bool dragging;

		public event PropertyChangedEventHandler PropertyChanged;

		public TileEditorWindow( Scenario s, Chapter c = null )
		{
			InitializeComponent();
			DataContext = this;

			scenario = s;
			chapter = c ?? new Chapter( "New Chapter" );
			selected = null;

			//rehydrate existing tiles in this chapter
			for ( int i = 0; i < chapter.tileObserver.Count; i++ )
			{
				( (HexTile)chapter.tileObserver[i] ).Rehydrate( canvas );
				( (HexTile)chapter.tileObserver[i] ).ChangeColor( i );
			}

			editTokenButton.IsEnabled = !chapter.usesRandomGroups;
			disabledMessage.Visibility = chapter.usesRandomGroups ? Visibility.Visible : Visibility.Collapsed;
			//SourceInitialized += ( x, y ) =>
			//{
			//	this.HideMinimizeAndMaximizeButtons();
			//};
		}

		int GetUnusedColor()
		{
			int[] used = chapter.tileObserver.Select( x => ( (HexTile)x ).color ).ToArray();
			for ( int i = 0; i < chapter.tileObserver.Count; i++ )
			{
				if ( i < used.Length && !used.Contains( i ) )
					return i;
			}
			return chapter.tileObserver.Count;
		}

		void PropChanged( string name )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
		}

		private void OkButton_Click( object sender, RoutedEventArgs e )
		{
			DialogResult = true;
		}

		private void Canvas_MouseUp( object sender, MouseButtonEventArgs e )
		{
			dragging = false;
		}

		private void Canvas_MouseMove( object sender, MouseEventArgs e )
		{
			if ( dragging && selected != null )
			{
				selected.Drag( e, canvas );
			}
		}

		public Rect GetBounds( FrameworkElement of, FrameworkElement from )
		{
			// Might throw an exception if of and from are not in the same visual tree
			GeneralTransform transform = of.TransformToVisual( from );

			return transform.TransformBounds( new Rect( 0, 0, of.ActualWidth, of.ActualHeight ) );
		}

		private void Window_PreviewKeyDown( object sender, KeyEventArgs e )
		{
			if ( selected != null )
			{
				if ( e.Key == Key.PageUp )
					selected.Rotate( 60, canvas );
				else if ( e.Key == Key.PageDown )
					selected.Rotate( -60, canvas );
				else if ( e.Key == Key.Delete )
				{
					scenario.globalTilePool.Add( selected.idNumber );
					canvas.Children.Remove( selected.hexPathShape );
					chapter.RemoveTile( selected );
					selected = null;
					//sort list
					TileSorter sorter = new TileSorter();
					List<int> foo = scenario.globalTilePool.ToList();
					foo.Sort( sorter );
					scenario.globalTilePool.Clear();
					foreach ( int s in foo )
						scenario.globalTilePool.Add( s );
					tilePool.SelectedIndex = 0;
				}
			}
		}

		private void AddTileButton_Click( object sender, RoutedEventArgs e )
		{
			if ( tilePool.SelectedIndex == -1 || chapter.tileObserver.Count == 5 )
				return;

			foreach ( HexTile tt in chapter.tileObserver )
			{
				tt.Unselect();
			}

			int color = GetUnusedColor();
			HexTile hex = new HexTile( (int)tilePool.SelectedItem );
			chapter.AddTile( hex );
			hex.ChangeColor( color );
			canvas.Children.Add( hex.hexPathShape );
			selected = hex;
			selected.Select();
			radioA.IsChecked = selected.tileSide == "A";
			radioB.IsChecked = selected.tileSide == "B";
			inChapterCB.SelectedIndex = chapter.tileObserver.Count - 1;
			scenario.globalTilePool.Remove( (int)tilePool.SelectedItem );
		}

		private void removeTileButton_Click( object sender, RoutedEventArgs e )
		{
			if ( selected != null )
			{
				scenario.globalTilePool.Add( selected.idNumber );
				canvas.Children.Remove( selected.hexPathShape );
				chapter.RemoveTile( selected );
				selected = null;
				//sort list
				TileSorter sorter = new TileSorter();
				List<int> foo = scenario.globalTilePool.ToList();
				foo.Sort( sorter );
				scenario.globalTilePool.Clear();
				foreach ( int s in foo )
					scenario.globalTilePool.Add( s );
				tilePool.SelectedIndex = 0;
			}
		}

		//private void AddEventButton_Click( object sender, RoutedEventArgs e )
		//{
		//	//EventEditorWindow ew = new EventEditorWindow( scenario );
		//	TriggerEditorWindow ew = new TriggerEditorWindow( scenario );
		//	if ( ew.ShowDialog() == true )
		//	{
		//		scenario.AddTrigger( ew.triggerName );
		//		int idx = int.Parse( (string)( (Button)sender ).DataContext );
		//		selected.triggerNames[idx] = ew.triggerName;
		//		selected = selected;
		//	}
		//}

		/// <summary>
		/// tiles in chapter CB
		/// </summary>
		private void ComboBox_SelectionChanged( object sender, SelectionChangedEventArgs e )
		{
			foreach ( HexTile tt in chapter.tileObserver )
			{
				tt.Unselect();
			}

			selected = null;

			var s = ( (ComboBox)sender ).SelectedItem as HexTile;
			if ( s != null )
			{
				var t = ( from HexTile tile in chapter.tileObserver where s.GUID == tile.GUID select tile ).FirstOr( null );
				if ( t != null )
				{
					selected = t;
					selected.Select();
					radioA.IsChecked = selected.tileSide == "A";
					radioB.IsChecked = selected.tileSide == "B";
				}
			}
		}

		private void Window_Closing( object sender, CancelEventArgs e )
		{
			canvas.Children.Clear();
		}

		private void editTokenButton_Click( object sender, RoutedEventArgs e )
		{
			selected.Unselect();
			TokenEditorWindow tw = new TokenEditorWindow( selected, scenario );
			tw.ShowDialog();
			selected = null;
		}

		private void canvas_MouseLeftButtonDown( object sender, MouseButtonEventArgs e )
		{
			if ( e.ClickCount == 1 )
			{
				foreach ( HexTile tt in chapter.tileObserver )
					tt.Unselect();

				selected = null;

				if ( e.Source is Path )
				{
					Path path = e.Source as Path;
					selected = path.DataContext as HexTile;
					selected.Select();
					dragging = true;
					selected.SetClickV( e, canvas );
					inChapterCB.SelectedItem = selected;
					radioA.IsChecked = selected.tileSide == "A";
					radioB.IsChecked = selected.tileSide == "B";
					//Debug.Log( selected );
				}
			}

			//double click
			else if ( e.ChangedButton == MouseButton.Left && e.ClickCount == 2 )
			{
				dragging = false;
				if ( selected == null )
					return;

				TokenEditorWindow tw = new TokenEditorWindow( selected, scenario );
				selected.Unselect();
				selected = null;
				tw.ShowDialog();
			}
		}

		private void radioA_Click( object sender, RoutedEventArgs e )
		{
			if ( selected != null )
			{
				if ( selected.tileSide == "A" )
					return;
				canvas.Children.Remove( selected.hexPathShape );
				selected.ChangeTileSide( "A", canvas );
			}
		}

		private void radioB_Click( object sender, RoutedEventArgs e )
		{
			if ( selected != null )
			{
				if ( selected.tileSide == "B" )
					return;
				canvas.Children.Remove( selected.hexPathShape );
				selected.ChangeTileSide( "B", canvas );
			}
		}

		private void tileGalleryButton_Click( object sender, RoutedEventArgs e )
		{
			GalleryWindow gw = new GalleryWindow( scenario, chapter.tileObserver.Count );
			if ( gw.ShowDialog() == true && gw.selectedData.Length > 0 )
			{
				foreach ( HexTile tt in chapter.tileObserver )
				{
					tt.Unselect();
				}

				foreach ( var t in gw.selectedData )
				{
					int color = GetUnusedColor();
					HexTile hex = new HexTile( t.Item1 );
					hex.ChangeColor( color );
					hex.ChangeTileSide( t.Item2, canvas );
					chapter.AddTile( hex );
					selected = hex;
					selected.Select();
					radioA.IsChecked = selected.tileSide == "A";
					radioB.IsChecked = selected.tileSide == "B";
					inChapterCB.SelectedIndex = chapter.tileObserver.Count - 1;
					scenario.globalTilePool.Remove( t.Item1 );
				}
			}
		}

		private void addExploredTriggerButton_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				selected.triggerName = tw.triggerName;
			}
		}
	}
}