using System.Windows;

namespace JiME
{
	public class HexTileData
	{
		public int id { get; set; }
		public string coords { get; set; }
		public float rotation { get; set; }
		public int tileCount;

		public void Init()
		{
			tileCount = coords.Split( ' ' ).Length;
		}

		/// <summary>
		/// parses string of coords into absolute vector positions on the canvas
		/// </summary>
		public static Point[] ParseCoords( string coords )
		{
			Point[] vectors;
			string[] array = coords.Split( ' ' );
			vectors = new Point[array.Length];
			for ( int i = 0; i < array.Length; i++ )
			{
				string[] xy = array[i].Split( ',' );
				double x = Utils.hexSnapX[int.Parse( xy[0] ) + 10];
				double y = Utils.hexSnapY[int.Parse( xy[1] ) + 10];
				vectors[i] = new Point( x, y );
			}
			return vectors;
		}
	}
}
