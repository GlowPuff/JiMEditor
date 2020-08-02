using System.ComponentModel;
using System.Windows;

namespace JiME
{
	public class BattleTile : INotifyPropertyChanged, ITile
	{
		int _terrainToken;
		string _triggerName, _tokenTrigger;
		bool _isGrassyLeft, _isGrassyRight;

		public int terrainToken
		{
			get => _terrainToken;
			set
			{
				_terrainToken = value;
				PropChanged( "terrainToken" );
			}
		}
		public string triggerName
		{
			get => _triggerName;
			set
			{
				_triggerName = value;
				PropChanged( "triggerName" );
			}
		}
		public string tokenTrigger
		{
			get => _tokenTrigger;
			set
			{
				_tokenTrigger = value;
				PropChanged( "tokenTrigger" );
			}
		}
		public bool isGrassyLeft
		{
			get => _isGrassyLeft;
			set
			{
				_isGrassyLeft = value;
				PropChanged( "isGrassyLeft" );
			}
		}
		public bool isGrassyRight
		{
			get => _isGrassyRight;
			set
			{
				_isGrassyRight = value;
				PropChanged( "isGrassyRight" );
			}
		}
		public TileType tileType { get; set; }
		public int idNumber { get; set; }
		public Vector position { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public BattleTile()
		{
			tileType = TileType.Battle;
			//None, Pit, Mist, Barrels, Table, FirePit, Statue 
			terrainToken = 0;
			isGrassyLeft = isGrassyRight = true;
			triggerName = "None";
			tokenTrigger = "None";
		}

		public void RenameTrigger( string oldName, string newName )
		{
			if ( triggerName == oldName )
				triggerName = newName;
			if ( tokenTrigger == oldName )
				tokenTrigger = newName;
		}

		void PropChanged( string name )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
		}
	}
}
