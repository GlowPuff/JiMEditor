using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace JiME
{
	public class HexTile : INotifyPropertyChanged, ITile
	{
		string _tileSide, _triggerName;
		bool _isStartTile;

		public double angle { get; set; }
		public int idNumber { get; set; }
		public int tokenCount { get; set; }
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
		public TileType tileType { get; set; }
		public TextBookData flavorBookData { get; set; }
		public ObservableCollection<Token> tokenList { get; set; }
		public bool isStartTile
		{
			get => _isStartTile;
			set
			{
				_isStartTile = value;
				PropChanged( "isStartTile" );
			}
		}
		public int color;
		public Vector hexRoot;
		public string triggerName
		{
			get { return _triggerName; }
			set
			{
				_triggerName = value;
				PropChanged( "triggerName" );
			}
		}

		[JsonIgnore]
		public Path hexPathShape;

		public event PropertyChangedEventHandler PropertyChanged;

		Point clickV;

		public HexTile() { }

		//public HexTile( int n, Vector position, float angle )
		//{
		//	tileType = TileType.Hex;
		//	idNumber = n;
		//	tokenCount = ( n / 100 ) % 10;
		//	GUID = Guid.NewGuid();
		//	tileSide = "A";
		//	this.position = position;
		//	this.angle = angle;
		//}

		public HexTile( int n )
		{
			tileType = TileType.Hex;
			idNumber = n;
			tokenCount = ( n / 100 ) % 10;
			GUID = Guid.NewGuid();
			tileSide = "A";
			position = new Vector( Utils.dragSnapX[5], Utils.dragSnapY[5] );
			tokenList = new ObservableCollection<Token>();
			flavorBookData = new TextBookData( "" );
			flavorBookData.pages.Add( "" );
			isStartTile = false;
			triggerName = "None";

			BuildShape();
			Update();
		}

		void BuildShape()
		{
			//where is this from?????
			//dims: 68.8660278320313,86.4256286621094 of ONE hexagon

			//canvas grid 32x28
			//hexagon ratio 1 : 1.1547005
			//unit dims: 1 x 0.8660254 = hex ratio

			//dims 64 x 55.4256256
			// distance between hextiles = 55.425626

			//hexPathShape = framework.FindResource( "hexpath" ) as Path;
			hexPathShape = new Path();
			hexPathShape.Stroke = Brushes.White;
			hexPathShape.StrokeThickness = 2;
			hexPathShape.Fill = new SolidColorBrush( Color.FromRgb( 70, 70, 74 ) );

			HexTileData hexdata;
			if ( tileSide == "A" )
				hexdata = Utils.hexDictionary[idNumber];
			else
				hexdata = Utils.hexDictionaryB[idNumber];

			//store the FIRST hexagon position (hexRoot) that makes up the tile
			//hexRoot is needed by companion app to calculate offsets
			string[] s = hexdata.coords.Split( ' ' )[0].Split( ',' );
			hexRoot = new Vector( double.Parse( s[0] ), double.Parse( s[1] ) );
			//local coords, example: (0,1)
			Point[] hexPositions = HexTileData.ParseCoords( hexdata.coords );
			Point center = hexPositions[0];
			PathFigure[] hexfigures = new PathFigure[hexdata.tileCount];
			for ( int i = 0; i < hexfigures.Length; i++ )
			{
				hexPositions[i] = RotatePoint( hexPositions[i],
					//new Point( 0, 55.4256256d / 2 ), 
					center,
					angle );
				hexfigures[i] = BuildRegularPolygon( hexPositions[i], 32, 6, 0 );
			}
			//for ( int i = 0; i < hexPositions.Length; i++ )
			//	Debug.Log( hexPositions[i] );
			hexPathShape.Data = new PathGeometry( hexfigures );
			hexPathShape.DataContext = this;

			//Debug.Log( "POS:" + position );
			//Debug.Log( "hexpos:" + hexPositions[0] );
			//string[] array = hexdata.coords.Split( ' ' );
			//string[] xy = array[0].Split( ',' );
			//Debug.Log( "xy: " + xy[0] + ", " + xy[1] );
			//Point p = new Point( double.Parse( xy[0] ), double.Parse( xy[1] ) );
			////p = new Point( p.X / 2, p.Y / 2 );
			//Point c = new Point( 0, 1 );
			//Debug.Log( "POINT: " + c );
			//Vector diff = p - c;
			//diff /= 2;
			//Debug.Log( diff );
			//Point newpos = new Point( diff.X, diff.Y * 55.4256256 );
			//position += new Vector( newpos.X, newpos.Y );
			//Debug.Log( "NEW:" + position );
		}

		PathFigure BuildRegularPolygon( Point c, double r, int numSides, double offsetDegree )
		{
			// c is the center, r is the radius,
			// numSides the number of sides, offsetDegree the offset in Degrees.
			// Do not add the last point.
			double a = Math.PI * offsetDegree / 180.0d;
			double step = 2d * Math.PI / Math.Max( numSides, 3 );

			LineSegment[] segments = new LineSegment[numSides + 1];

			Point cur = c;
			cur.X = c.X + r * Math.Cos( a );
			cur.Y = c.Y + r * Math.Sin( a );
			Point startPoint = cur;
			for ( int i = 0; i < numSides; i++, a += step )
			{
				cur.X = c.X + r * Math.Cos( a );
				cur.Y = c.Y + r * Math.Sin( a );
				//Debug.Log( cur );
				segments[i] = new LineSegment( new Point( cur.X, cur.Y ), true );
			}
			segments[numSides] = new LineSegment( startPoint, true );
			PathFigure figure = new PathFigure( startPoint, segments, false );

			//Debug.Log( "end poly" );
			return figure;

			//StreamGeometry geometry = new StreamGeometry();

			//using ( StreamGeometryContext ctx = geometry.Open() )
			//{
			//	double step = 2d * Math.PI / Math.Max( numSides, 3 );
			//	Point cur = c;

			//	cur.X = c.X + r * Math.Cos( a );
			//	cur.Y = c.Y + r * Math.Sin( a );
			//	ctx.BeginFigure( cur, true, true );

			//	for ( int i = 0; i < numSides; i++, a += step )
			//	{
			//		cur.X = c.X + r * Math.Cos( a );
			//		cur.Y = c.Y + r * Math.Sin( a );
			//		ctx.LineTo( cur, true, false );
			//	}
			//}

			//return geometry;
		}

		public void ChangeColor( int idx )
		{
			color = idx;
			hexPathShape.Fill = Utils.hexColors[Math.Max( idx, 0 )];
		}

		/// <summary>
		/// updates shape position on canvas
		/// </summary>
		void Update()
		{
			hexPathShape.RenderTransformOrigin = new Point( 0, 0 );//.5d, .5d );
			TranslateTransform tf = new TranslateTransform( position.X, position.Y );
			//RotateTransform rt = new RotateTransform( angle );

			TransformGroup grp = new TransformGroup();
			//grp.Children.Add( rt );
			grp.Children.Add( tf );
			hexPathShape.RenderTransform = grp;
		}

		public void Select()
		{
			hexPathShape.Stroke = new SolidColorBrush( Colors.Red );
			Canvas.SetZIndex( hexPathShape, 100 );
		}

		public void Unselect()
		{
			hexPathShape.Stroke = new SolidColorBrush( Colors.White );
			Canvas.SetZIndex( hexPathShape, 0 );
		}

		public void Rehydrate( Canvas canvas )
		{
			BuildShape();
			hexPathShape.DataContext = this;
			canvas.Children.Add( hexPathShape );
			Update();
		}

		public void ChangeTileSide( string side, Canvas canvas )
		{
			tileSide = side;
			position = new Vector( Utils.dragSnapX[5], Utils.dragSnapY[5] );
			angle = 0;
			Rehydrate( canvas );
			ChangeColor( color );
			Select();
		}

		public void Rotate( double amount, Canvas canvas )
		{
			//4,57.7128128
			canvas.Children.Remove( hexPathShape );
			angle += amount;
			angle %= 360;
			Rehydrate( canvas );
			ChangeColor( color );
			Select();
		}

		public void SetClickV( MouseButtonEventArgs e, Canvas canvas )
		{
			clickV = new Point();
			TransformGroup grp = hexPathShape.RenderTransform as TransformGroup;
			if ( grp?.Children.Count == 1 )
			{
				Vector gv = new Vector( ( (TranslateTransform)grp.Children[0] ).X, ( (TranslateTransform)grp.Children[0] ).Y );
				//Debug.Log( "shape position(center):" + gv.X + "," + gv.Y );
				//Debug.Log( $"dims: {hexPathShape.ActualWidth},{hexPathShape.ActualHeight}" );
				clickV = e.GetPosition( canvas );
				clickV.X -= gv.X;
				clickV.Y -= gv.Y;
				//Debug.Log( "clickV:" + clickV );
			}
		}

		public void Drag( MouseEventArgs e, Canvas canvas )
		{
			Vector clickPoint = new Vector( e.GetPosition( canvas ).X - clickV.X, e.GetPosition( canvas ).Y - clickV.Y );

			Vector snapped = new Vector();
			snapped.X = ( from snapx in Utils.dragSnapX where clickPoint.X.WithinTolerance( snapx, Utils.tolerance ) select snapx ).FirstOr( -1 );
			snapped.Y = ( from snapy in Utils.dragSnapY where clickPoint.Y.WithinTolerance( snapy, Utils.tolerance ) select snapy ).FirstOr( -1 );

			position = snapped;

			if ( snapped != new Vector( -1, -1 ) )
				Update();
		}

		Point RotatePoint( Point pointToRotate, Point centerPoint, double angleInDegrees )
		{
			double angleInRadians = angleInDegrees * ( Math.PI / 180 );
			double cosTheta = Math.Cos( angleInRadians );
			double sinTheta = Math.Sin( angleInRadians );
			Point p = new Point
			{
				X =
							(int)
							( cosTheta * ( pointToRotate.X - centerPoint.X ) -
							sinTheta * ( pointToRotate.Y - centerPoint.Y ) + centerPoint.X ),
				Y =
							(int)
							( sinTheta * ( pointToRotate.X - centerPoint.X ) +
							cosTheta * ( pointToRotate.Y - centerPoint.Y ) + centerPoint.Y )
			};

			p.X = ( from snapx in Utils.hexSnapX where p.X.WithinTolerance( snapx, 5 ) select snapx ).FirstOr( -1 );
			p.Y = ( from snapy in Utils.hexSnapY where p.Y.WithinTolerance( snapy, 5 ) select snapy ).FirstOr( -1 );
			return p;
		}

		public void RenameTrigger( string oldName, string newName )
		{
			foreach ( Token t in tokenList )
			{
				if ( t.triggerName == oldName )
					t.triggerName = newName;
			}

			if ( triggerName == oldName )
				triggerName = newName;
		}

		void PropChanged( string name )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
		}
	}
}
