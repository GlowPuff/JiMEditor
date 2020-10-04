using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json;

namespace JiME
{
	/// <summary>
	/// A Chapter contains a batch of Tiles. Each Chapter leads to the next by activating a Trigger
	/// </summary>
	public class Chapter : INotifyPropertyChanged, ICommonData
	{
		//common
		string _dataName;

		public string dataName
		{
			get => _dataName;
			set
			{
				if ( _dataName != value )
				{
					_dataName = value;
					PropChanged( "dataName" );
				}
			}
		}
		public Guid GUID { get; set; }
		public bool isEmpty { get; set; }
		public string triggerName { get; set; }

		//vars
		bool _noFlavorText, _isRandomTiles, _isPreExplored, _usesRandomGroups, _isDynamic;
		string _triggeredBy, _exploreTrigger, _randomInteractionGroup, _attachHint;
		int _randomInteractionGroupCount;

		public bool noFlavorText
		{
			get => _noFlavorText;
			set
			{
				_noFlavorText = value;
				PropChanged( "noFlavorText" );
			}
		}
		public TextBookData flavorBookData { get; set; }
		public string triggeredBy
		{
			get => _triggeredBy;
			set
			{
				_triggeredBy = value;
				PropChanged( "triggeredBy" );
			}
		}
		public string exploreTrigger
		{
			get => _exploreTrigger;
			set
			{
				_exploreTrigger = value;
				PropChanged( "exploreTrigger" );
			}
		}
		public bool isRandomTiles
		{
			get => _isRandomTiles;
			set
			{
				_isRandomTiles = value;
				PropChanged( "isRandomTiles" );
			}
		}
		[JsonConverter( typeof( TileConverter ) )]
		public ObservableCollection<ITile> tileObserver { get; set; }
		public string randomInteractionGroup
		{
			get => _randomInteractionGroup;
			set
			{
				_randomInteractionGroup = value;
				PropChanged( "randomInteractionGroup" );
			}
		}
		public int randomInteractionGroupCount
		{
			get => _randomInteractionGroupCount;
			set
			{
				_randomInteractionGroupCount = value;
				PropChanged( "randomInteractionGroupCount" );
			}
		}
		public bool isPreExplored
		{
			get => _isPreExplored;
			set { _isPreExplored = value; PropChanged( "isPreExplored" ); }
		}
		public bool usesRandomGroups
		{
			get => _usesRandomGroups;
			set
			{
				_usesRandomGroups = value;
				PropChanged( "usesRandomGroups" );
			}
		}
		public bool isDynamic
		{
			get => _isDynamic;
			set
			{
				_isDynamic = value;
				PropChanged( "isDynamic" );
			}
		}
		public string attachHint
		{
			get => _attachHint;
			set
			{
				_attachHint = value;
				PropChanged( "attachHint" );
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public Chapter( string name )
		{
			dataName = name;
			GUID = Guid.NewGuid();
			isEmpty = false;
			noFlavorText = true;
			flavorBookData = new TextBookData();
			flavorBookData.pages.Add( "This optional text is shown when any Tile in this Chapter is first explored. It is only shown once." );
			exploreTrigger = triggeredBy = triggerName = "None";
			tileObserver = new ObservableCollection<ITile>();
			//randomTilePool = new ObservableCollection<int>();
			randomInteractionGroup = "None";
			if ( name == "Start" )
				isPreExplored = true;
			else
				isPreExplored = false;
			usesRandomGroups = false;
			isDynamic = false;
			attachHint = "None";
		}

		//public Chapter CreateDefault()
		//{
		//	return new Chapter( "Start" )
		//	{
		//		isEmpty = true,
		//		isPreExplored = true,
		//		isDynamic = false
		//	};
		//}

		public void AddTile( HexTile t )
		{
			tileObserver.Add( t );
		}

		public void RemoveTile( HexTile t )
		{
			tileObserver.Remove( t );
		}

		public void RenameTrigger( string oldName, string newName, bool isJourney )
		{
			if ( triggerName == oldName )
				triggerName = newName;
			if ( triggeredBy == oldName )
				triggeredBy = newName;
			if ( exploreTrigger == oldName )
				exploreTrigger = newName;

			//rename tiles

		}

		public void ToJourneyTile()
		{
			tileObserver = new ObservableCollection<ITile>();
		}

		public void ToBattleTile()
		{
			tileObserver = new ObservableCollection<ITile>();
			for ( int i = 0; i < 10; i++ )
			{
				tileObserver.Add( new BattleTile() );
			}
		}

		void PropChanged( string name )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
		}
	}
}
