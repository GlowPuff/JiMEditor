using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JiME
{
	/*
	PathGeometry shape1 = (PathGeometry)Application.Current.FindResource("Shape1");
	PathGeometry shape2 = (PathGeometry)Application.Current.FindResource("Shape2");
	CombinedGeometry combinedGeometry = new CombinedGeometry(
			GeometryCombineMode.Union, shape1, shape2);
	Path combinedPath = new Path();
	combinedPath.Data = combinedGeometry; 
	*/
	public class Tile : INotifyPropertyChanged, ITile
	{
		string[] _tokenType, _eventNames;
		string _tileSide;

		public double angle { get; set; }
		public int idNumber { get; set; }
		public int tokenCount { get; set; }
		public string[] tokenType
		{
			get => _tokenType;
			set
			{
				_tokenType = value;
				PropChanged( "tokenType" );
			}
		}
		public string[] triggerNames
		{
			get => _eventNames;
			set
			{
				_eventNames = value;
				PropChanged( "eventNames" );
			}
		}
		public Guid GUID { get; set; }
		public string tileSide
		{
			get => _tileSide;
			set
			{
				_tileSide = value;
				PropChanged( "tileSide" );
			}
		}
		public Vector position { get; set; }

		Point clickV;
		BitmapImage bm;//, bmSelected;
		double YOffset
		{
			get
			{
				//if ( tokenCount == 1 )
				//	return angle == 0 ? 0 : 2;
				//else if ( tokenCount == 2 )
				//	return angle == 0 ? 0 : 2;
				//else if ( tokenCount == 3 )
				return angle == 0 ? 0 : 2;
				//else
				//	return 0;
			}
		}

		public Image image;

		public event PropertyChangedEventHandler PropertyChanged;

		public Tile( int n, Image img )
		{
			image = img;
			idNumber = n;
			tokenCount = ( n / 100 ) % 10;
			GUID = Guid.NewGuid();

			//bmSelected = new BitmapImage( new Uri( $"pack://application:,,,/Tiles/{n}-selected.png" ) );
			bm = new BitmapImage( new Uri( $"pack://application:,,,/Tiles/{n}.png" ) );

			tileSide = "A";
			//image.Source = bmSelected;
			image.DataContext = this;
			position = new Vector( 0, 0 );
			tokenType = new string[3];
			triggerNames = new string[3];
			for ( int i = 0; i < 3; i++ )
			{
				tokenType[i] = "None";
				triggerNames[i] = "None";
			}

			//overlay = new Rectangle();
			//overlay.Width = image.ActualWidth;
			//overlay.Height = image.ActualHeight;
			//overlay.Fill = Brushes.Red;
			//overlay.OpacityMask = new ImageBrush( bm );
			//Canvas.SetZIndex( overlay, 1000 );
			//canvas.Children.Add( overlay );

			Update();
		}

		public void Rehydrate( Canvas canvas )
		{
			image = new Image();
			bm = new BitmapImage( new Uri( $"pack://application:,,,/Tiles/{idNumber}.png" ) );
			//bmSelected = new BitmapImage( new Uri( $"pack://application:,,,/Tiles/{idNumber}-selected.png" ) );
			image.Source = bm;
			image.DataContext = this;
			canvas.Children.Add( image );
			Update();
		}

		public void Update()
		{
			image.RenderTransformOrigin = new Point( .5d, .5d );
			TranslateTransform tf = new TranslateTransform( position.X, position.Y + YOffset );
			RotateTransform rt = new RotateTransform( angle );

			TransformGroup grp = new TransformGroup();
			grp.Children.Add( rt );
			grp.Children.Add( tf );
			image.RenderTransform = grp;

			//overlay.RenderTransformOrigin = new Point( .5d, .5d );
			//overlay.RenderTransform = grp;
		}

		public void Unselect()
		{
			image.Source = bm;
			Canvas.SetZIndex( image, 0 );
		}

		public void Select()
		{
			//image.Source = bmSelected;
			Canvas.SetZIndex( image, 100 );
		}

		/// <summary>
		/// also calls Update
		/// </summary>
		void SnapPosition()
		{
			Vector snapped = ( from snapV in Utils.gridSnap where position.WithinTolerance( snapV, Utils.tolerance ) select snapV ).FirstOr( new Vector( -1, -1 ) );
			if ( snapped != new Vector( -1, -1 ) )
			{
				position = snapped;
				Update();
			}
		}

		public void Rotate( double amount )
		{
			angle += amount;
			if ( angle % 360 == 0 )
				angle = 0;
			TransformGroup grp = image.RenderTransform as TransformGroup;
			if ( grp?.Children.Count == 2 )
			{
				Vector gv = new Vector( ( (TranslateTransform)grp.Children[1] ).X, ( (TranslateTransform)grp.Children[1] ).Y );
				position = gv;
				SnapPosition();
			}
		}

		public void Nudge( Vector amount )
		{
			//position.X += amount.X;
			//position.Y += amount.Y;
			position = new Vector( position.X + amount.X, position.Y + amount.Y );

			SnapPosition();
		}

		public void SetClickV( System.Windows.Input.MouseButtonEventArgs e, Canvas canvas )
		{
			clickV = new Point();
			TransformGroup grp = image.RenderTransform as TransformGroup;
			if ( grp?.Children.Count == 2 )
			{
				Vector gv = new Vector( ( (TranslateTransform)grp.Children[1] ).X, ( (TranslateTransform)grp.Children[1] ).Y );
				gv.X += image.ActualWidth / 2;
				gv.Y += image.ActualHeight / 2;
				//Debug.Log( "shape position(center):" + gv.X + "," + gv.Y );
				clickV = e.GetPosition( canvas );
				clickV.X -= gv.X;
				clickV.Y -= gv.Y;
				//Debug.Log( "clickV:" + clickV );
				//image.Source = bmSelected;
			}
		}

		public void Drag( System.Windows.Input.MouseEventArgs e, Canvas canvas )
		{
			Vector clickPoint = new Vector( e.GetPosition( canvas ).X - ( image.ActualWidth / 2 ) - clickV.X, e.GetPosition( canvas ).Y - ( image.ActualHeight / 2 ) - clickV.Y );

			Vector snapped = ( from snapV in Utils.gridSnap where clickPoint.WithinTolerance( snapV, Utils.tolerance ) select snapV ).FirstOr( new Vector( -1, -1 ) );

			position = snapped;

			if ( snapped != new Vector( -1, -1 ) )
			{
				Update();
			}
		}

		public void RenameTrigger( string oldName, string newName )
		{
			for ( int i = 0; i < triggerNames.Length; i++ )
			{
				if ( triggerNames[i] == oldName )
					triggerNames[i] = newName;
			}
		}

		void PropChanged( string name )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
		}
	}
}
