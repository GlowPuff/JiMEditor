using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.ComponentModel;

namespace JiME.Views
{
	/// <summary>
	/// Interaction logic for BattleTileEditor.xaml
	/// </summary>
	public partial class BattleTileEditor : Window, INotifyPropertyChanged
	{
		BattleTile _selectedLeft;

		int selectedWallIndex;
		int wallValue;
		Path selectedWall;

		public Scenario scenario { get; set; }
		public Chapter chapter { get; set; }

		public BattleTile selectedLeft
		{
			get => _selectedLeft;
			set
			{
				_selectedLeft = value;
				PropChanged( "selectedLeft" );
			}
		}

		public BattleTileEditor( Scenario s, Chapter c )
		{
			InitializeComponent();
			DataContext = this;

			scenario = s;
			chapter = c;

			foreach ( FrameworkElement el in canvas.Children )
			{
				if ( el.DataContext.ToString().Contains( "wall" ) )
					Canvas.SetZIndex( el, 1000 );
				if ( el.DataContext.ToString() == "wall0" )
				{
					( (Path)el ).Stroke = new SolidColorBrush( Color.FromArgb( 255, 255, 0, 0 ) );
					selectedWall = (Path)el;
				}
			}
			selectedLeft = chapter.tileObserver[0] as BattleTile;
			( (Path)canvas.Children[0] ).Stroke = Brushes.Red;
			Canvas.SetZIndex( (FrameworkElement)canvas.Children[0], 100 );

			selectedWallIndex = 0;
			wallValue = scenario.wallTypes[0];
			SetRadioButtons();
			FillWallColors();
			FillRegionColors();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OkButton_Click( object sender, RoutedEventArgs e )
		{
			if ( !TryClose() )
				return;
			DialogResult = true;
		}

		bool TryClose()
		{
			if ( selectedLeft.terrainToken > 0 )
			{
				if ( selectedLeft.tokenTrigger == "None" )
				{
					MessageBox.Show( "'Trigger On Token Interaction' cannot be set to None.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error );
					return false;
				}
			}
			return true;
		}

		private void Canvas_MouseDown( object sender, MouseButtonEventArgs e )
		{
			string dc = (string)( (FrameworkElement)e.Source ).DataContext;

			if ( dc.Contains( "wall" ) )//wall tile
			{
				selectedWallIndex = int.Parse( dc.Substring( 4 ) );
				wallValue = scenario.wallTypes[selectedWallIndex];
				selectedWall = (Path)e.Source;
				SetRadioButtons();

				//unselect
				foreach ( FrameworkElement el in canvas.Children )
				{
					if ( el.DataContext.ToString().Contains( "wall" ) )
					{
						( (Path)el ).Stroke = new SolidColorBrush( Color.FromArgb( (byte)( .15f * 255f ), 255, 255, 255 ) );
					}
				}
				//select
				( (Path)e.Source ).Stroke = new SolidColorBrush( Color.FromArgb( 255, 255, 0, 0 ) );
			}
			else//region tile
			{
				int idx = int.Parse( (string)( (FrameworkElement)e.Source ).DataContext );

				selectedLeft = null;
				selectedLeft = chapter.tileObserver[idx] as BattleTile;
				foreach ( FrameworkElement el in canvas.Children )
				{
					if ( !el.DataContext.ToString().Contains( "wall" ) )
					{
						//unselect
						( (Path)el ).Stroke = Brushes.White;
						//( (Path)el ).Fill = (SolidColorBrush)FindResource( "bgColorLight" );
						Canvas.SetZIndex( el, 0 );
					}
				}
				Canvas.SetZIndex( (FrameworkElement)e.Source, 100 );
				( (Path)canvas.Children[idx] ).Stroke = Brushes.Red;
				FillRegionColors();
			}
		}

		private void AddTriggerButton_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				selectedLeft.triggerName = tw.triggerName;
			}
		}

		private void AddTokenTrigger_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				selectedLeft.tokenTrigger = tw.triggerName;
			}
		}

		private void RadioButton_Click( object sender, RoutedEventArgs e )
		{
			wallValue = int.Parse( ( (RadioButton)sender ).DataContext.ToString() );
			scenario.wallTypes[selectedWallIndex] = wallValue;
			FillWallColors();
		}

		void FillWallColors()
		{
			switch ( wallValue )
			{
				case 0:
					selectedWall.Fill = (SolidColorBrush)FindResource( "wallNone" );
					break;
				case 1:
					selectedWall.Fill = (SolidColorBrush)FindResource( "wallBrown" );
					break;
				case 2:
					selectedWall.Fill = (SolidColorBrush)FindResource( "wallRiver" );
					break;
			}
		}

		void FillRegionColors()
		{
			for ( int i = 0; i < chapter.tileObserver.Count; i++ )
			{
				switch ( ( (BattleTile)chapter.tileObserver[i] ).terrainToken )
				{
					case 0:
						( (Path)canvas.Children[i] ).Fill = (SolidColorBrush)FindResource( "bgColorLight" );
						break;
					case 1:
						( (Path)canvas.Children[i] ).Fill = (SolidColorBrush)FindResource( "pit" );
						break;
					case 2:
						( (Path)canvas.Children[i] ).Fill = (SolidColorBrush)FindResource( "mist" );
						break;
					case 3:
						( (Path)canvas.Children[i] ).Fill = (SolidColorBrush)FindResource( "barrels" );
						break;
					case 4:
						( (Path)canvas.Children[i] ).Fill = (SolidColorBrush)FindResource( "table" );
						break;
					case 5:
						( (Path)canvas.Children[i] ).Fill = (SolidColorBrush)FindResource( "firepit" );
						break;
					case 6:
						( (Path)canvas.Children[i] ).Fill = (SolidColorBrush)FindResource( "statue" );
						break;
				}
			}
		}

		void SetRadioButtons()
		{
			wNone.IsChecked = wallValue == 0;
			wWall.IsChecked = wallValue == 1;
			wRiver.IsChecked = wallValue == 2;
		}

		private void TokenCB_SelectionChanged( object sender, SelectionChangedEventArgs e )
		{
			FillRegionColors();
		}

		void PropChanged( string name )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
		}

		private void Window_Closing( object sender, CancelEventArgs e )
		{
			e.Cancel = !TryClose();
		}
	}
}
