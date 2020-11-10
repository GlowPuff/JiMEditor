using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace JiME
{
	/// <summary>
	/// A standalone mission, or a single mission in a campaign
	/// </summary>
	public class Scenario : INotifyPropertyChanged
	{
		string _scenarioName, _fileName, _objectiveName, _fileVersion, _specialInstructions;
		bool _isDirty, _scenarioTypeJourney, _useTileGraphics;
		int _threatMax, _loreReward, _xpReward, _shadowFear, _loreStartValue;
		int[] _wallTypes;
		Guid _scenarioGUID, _campaignGUID;
		//titleChangedToken is ONLY used to trigger the window Title converter
		Tuple<bool, string, Guid, ProjectType> _titleChangedToken;
		public int[] wallTypes
		{
			get => _wallTypes;
			set
			{
				_wallTypes = value;
				PropChanged( "wallTypes" );
			}
		}

		public string saveDate { get; set; }
		#region propchanged
		public Tuple<bool, string, Guid, ProjectType> titleChangedToken
		{
			get => _titleChangedToken;
			set
			{
				if ( _titleChangedToken != value )
				{
					_titleChangedToken = value;
					PropChanged( "titleChangedToken" );
				}
			}
		}
		public bool isDirty
		{
			get => _isDirty;
			set
			{
				if ( value != _isDirty )
				{
					_isDirty = value;
					PropChanged( "isDirty" );
				}
			}
		}
		public bool scenarioTypeJourney
		{
			get => _scenarioTypeJourney;
			set
			{
				if ( value != _scenarioTypeJourney )
				{
					_scenarioTypeJourney = value;
					PropChanged( "scenarioTypeJourney" );
				}
			}
		}
		/// <summary>
		/// just the file NAME, not the path
		/// </summary>
		public string fileName
		{
			get => _fileName;
			set
			{
				if ( value != _fileName )
				{
					_fileName = value;
					PropChanged( "fileName" );
				}
			}
		}
		public string fileVersion
		{
			get => _fileVersion;
			set
			{
				if ( value != _fileVersion )
				{
					_fileVersion = value;
					PropChanged( "fileVersion" );
				}
			}
		}
		public string scenarioName
		{
			get => _scenarioName;
			set
			{
				if ( _scenarioName != value )
				{
					_scenarioName = value;
					PropChanged( "scenarioName" );
				}
			}
		}
		public string objectiveName
		{
			get => _objectiveName;
			set
			{
				_objectiveName = value;
				PropChanged( "objectiveName" );
			}
		}
		public int threatMax
		{
			get => _threatMax;
			set
			{
				if ( value != _threatMax )
				{
					_threatMax = value;
					PropChanged( "threatMax" );
				}
			}
		}
		public bool threatNotUsed { get; set; }
		public ProjectType projectType { get; set; }
		public TextBookData introBookData { get; set; }
		public int loreReward
		{
			get => _loreReward;
			set { _loreReward = value; PropChanged( "loreReward" ); }
		}
		public int loreStartValue
		{
			get => _loreStartValue;
			set { _loreStartValue = value; PropChanged( "loreStartValue" ); }
		}
		public int xpReward
		{
			get => _xpReward;
			set { _xpReward = value; PropChanged( "xpReward" ); }
		}
		public int shadowFear
		{
			get => _shadowFear;
			set { _shadowFear = value; PropChanged( "shadowFear" ); }
		}
		public Guid scenarioGUID
		{
			get => _scenarioGUID;
			set
			{
				_scenarioGUID = value;
				PropChanged( "scenarioGUID" );
			}
		}
		public Guid campaignGUID
		{
			get => _campaignGUID;
			set
			{
				_campaignGUID = value;
				PropChanged( "campaignGUID" );
			}
		}
		public string specialInstructions
		{
			get => _specialInstructions;
			set
			{
				_specialInstructions = value;
				PropChanged( "specialInstructions" );
			}
		}
		public bool useTileGraphics
		{
			get => _useTileGraphics;
			set
			{
				_useTileGraphics = value;
				PropChanged( "useTileGraphics" );
			}
		}
		#endregion

		public ObservableCollection<IInteraction> interactionObserver { get; set; }
		public ObservableCollection<Trigger> triggersObserver { get; set; }
		public ObservableCollection<Objective> objectiveObserver { get; set; }
		public ObservableCollection<TextBookData> resolutionObserver { get; set; }
		public ObservableCollection<Threat> threatObserver { get; set; }
		public ObservableCollection<Chapter> chapterObserver { get; set; }
		public ObservableCollection<int> globalTilePool { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public Scenario() : this( "Click Text To Edit Scenario Title" )
		{
			CreateDefaults();
		}

		public Scenario( string name )
		{
			scenarioName = name;
			isDirty = true;
			projectType = ProjectType.Standalone;
			titleChangedToken = new Tuple<bool, string, Guid, ProjectType>( true, string.Empty, Guid.NewGuid(), ProjectType.Standalone );

			interactionObserver = new ObservableCollection<IInteraction>();
			triggersObserver = new ObservableCollection<Trigger>();
			objectiveObserver = new ObservableCollection<Objective>();
			resolutionObserver = new ObservableCollection<TextBookData>();
			threatObserver = new ObservableCollection<Threat>();
			chapterObserver = new ObservableCollection<Chapter>();
			globalTilePool = new ObservableCollection<int>( Utils.LoadTiles() );
			Utils.LoadHexData();
		}

		/// <summary>
		/// Load in data from FileManager
		/// </summary>
		public static Scenario CreateInstance( FileManager fm )
		{
			Scenario s = new Scenario();
			s.scenarioName = fm.scenarioName;
			s.fileName = fm.fileName;
			s.fileVersion = fm.fileVersion;
			s.saveDate = fm.saveDate;
			s.projectType = fm.projectType;
			s.objectiveName = fm.objectiveName;
			s.interactionObserver = new ObservableCollection<IInteraction>( fm.interactions );
			s.triggersObserver = new ObservableCollection<Trigger>( fm.triggers );
			s.objectiveObserver = new ObservableCollection<Objective>( fm.objectives );
			s.resolutionObserver = new ObservableCollection<TextBookData>( fm.resolutions );
			s.threatObserver = new ObservableCollection<Threat>( fm.threats );
			s.chapterObserver = new ObservableCollection<Chapter>( fm.chapters );
			s.globalTilePool = new ObservableCollection<int>( fm.globalTiles );
			s.introBookData = fm.introBookData;
			s.threatMax = fm.threatMax;
			s.threatNotUsed = fm.threatNotUsed;
			s.scenarioTypeJourney = fm.scenarioTypeJourney;
			s.loreReward = fm.loreReward;
			s.xpReward = fm.xpReward;
			s.shadowFear = fm.shadowFear;
			s.loreStartValue = fm.loreStartValue;
			s.scenarioGUID = fm.scenarioGUID;
			s.campaignGUID = fm.campaignGUID;
			s.specialInstructions = fm.specialInstructions ?? "";
			s.useTileGraphics = fm.useTileGraphics;

			if ( s.scenarioGUID.ToString() == "00000000-0000-0000-0000-000000000000" )
				s.scenarioGUID = Guid.NewGuid();

			//sort the observer lists by name
			//List<IInteraction> sorted = s.interactionObserver.OrderBy( key => key.dataName != "None" ).ThenBy( key => key.dataName ).ToList();
			//for ( int i = 0; i < sorted.Count; i++ )
			//	s.interactionObserver[i] = sorted[i];

			//List<Trigger> trigsorted = s.triggersObserver.OrderBy( key => key.dataName != "None" ).ThenBy( key => key.dataName ).ToList();
			//for ( int i = 0; i < trigsorted.Count; i++ )
			//	s.triggersObserver[i] = trigsorted[i];

			//List<Objective> objsorted = s.objectiveObserver.OrderBy( key => key.dataName != "None" ).ThenBy( key => key.dataName ).ToList();
			//for ( int i = 0; i < objsorted.Count; i++ )
			//	s.objectiveObserver[i] = objsorted[i];

			return s;
		}

		/// <summary>
		/// Triggers the window Title binding converter to update, sets isDirty=false
		/// </summary>
		public void TriggerTitleChange( bool dirty = false )
		{
			isDirty = dirty;
			titleChangedToken = new Tuple<bool, string, Guid, ProjectType>( isDirty, fileName, Guid.NewGuid(), projectType );
		}

		void CreateDefaults()
		{
			scenarioGUID = Guid.NewGuid();
			campaignGUID = Guid.NewGuid();
			specialInstructions = "";
			threatMax = 60;
			scenarioTypeJourney = true;
			objectiveName = "None";
			loreReward = loreStartValue = xpReward = 0;
			shadowFear = 2;
			fileVersion = Utils.formatVersion;
			useTileGraphics = true;

			introBookData = new TextBookData( "Default Introduction Text" );
			introBookData.pages.Add( "Default Introduction text.\n\nThis text is displayed at the beginning of the Scenario to describe the mission and Objectives.\n\nScenarios have one Introduction Text." );

			TextBookData data = new TextBookData( "Default Resolution" );
			data.pages.Add( "Default resolution text.  This text is displayed when the Scenario has ended.\n\nDifferent Resolutions can be shown based on a Trigger that gets set during the Scenario, depending on player actions.\n\nScenarios have at least one Resolution." );
			data.triggerName = "Scenario Ended";
			AddResolution( data );

			//Always have one EMPTY Trigger / Objective / Interaction
			triggersObserver.Add( Trigger.EmptyTrigger() );
			//triggersObserver.Add( Trigger.RandomTrigger() );
			objectiveObserver.Add( Objective.EmptyObjective() );
			interactionObserver.Add( NoneInteraction.EmptyInteraction() );
			AddTrigger( "Scenario Ended" );
			AddTrigger( "Objective Complete" );
			//AddInteraction( Interaction.EmptyInteraction() );

			//default objective - always at least 1 in the scenario
			Objective obj = new Objective( "Default Objective" ) { triggerName = "Objective Complete" };
			objectiveObserver.Add( obj );

			//starting chapter - always at least one in the scenario
			Chapter chapter = new Chapter( "Start" ) { isEmpty = true };
			chapterObserver.Add( chapter );

			wallTypes = new int[22];
			for ( int i = 0; i < 22; i++ )
				wallTypes[i] = 0;//0=none, 1=wall, 2=river
		}

		public void WipeChapters()
		{
			chapterObserver.Clear();
			Chapter chapter = new Chapter( "Start" ) { isEmpty = true };
			chapterObserver.Add( chapter );
		}

		void PropChanged( string name )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
		}

		public void AddChapter( Chapter chapter )
		{
			chapterObserver.Add( chapter );
		}

		public void AddInteraction( IInteraction interaction )
		{
			switch ( interaction.interactionType )
			{
				case InteractionType.Branch:
					interactionObserver.Add( interaction );
					break;
				case InteractionType.Text:
					interactionObserver.Add( interaction );
					break;
				case InteractionType.Threat:
					interactionObserver.Add( interaction );
					break;
				case InteractionType.StatTest:
					interactionObserver.Add( interaction );
					break;
				case InteractionType.Decision:
					interactionObserver.Add( interaction );
					break;
				case InteractionType.Darkness:
					interactionObserver.Add( interaction );
					break;
				case InteractionType.MultiEvent:
					interactionObserver.Add( interaction );
					break;
				case InteractionType.Persistent:
					interactionObserver.Add( interaction );
					break;
				case InteractionType.Conditional:
					interactionObserver.Add( interaction );
					break;
				case InteractionType.Dialog:
					interactionObserver.Add( interaction );
					break;
				case InteractionType.Replace:
					interactionObserver.Add( interaction );
					break;
				default:
					throw new Exception( "Interaction type not supported: " + interaction.interactionType );
			}

			//sort by name
			//List<IInteraction> sorted = interactionObserver.OrderBy( key => key.dataName != "None" ).ThenBy( key => key.dataName ).ToList();
			//for ( int i = 0; i < sorted.Count; i++ )
			//	interactionObserver[i] = sorted[i];
		}

		public bool AddTrigger( string name, bool isMulti = false )
		{
			if ( ( from Trigger foo in triggersObserver where foo.dataName == name select foo ).Count() == 0 )
			{
				Trigger t = new Trigger( name );
				t.isMultiTrigger = isMulti;
				triggersObserver.Add( t );

				//sort by name
				//List<Trigger> trigsorted = triggersObserver.OrderBy( key => key.dataName != "None" ).ThenBy( key => key.dataName ).ToList();
				//for ( int i = 0; i < trigsorted.Count; i++ )
				//	triggersObserver[i] = trigsorted[i];

				return true;
			}
			return false;
		}

		public void AddObjective( Objective objective )
		{
			objectiveObserver.Add( objective );
			//sort by name
			//List<Objective> objsorted = objectiveObserver.OrderBy( key => key.dataName != "None" ).ThenBy( key => key.dataName ).ToList();
			//for ( int i = 0; i < objsorted.Count; i++ )
			//	objectiveObserver[i] = objsorted[i];
		}

		public bool AddResolution( TextBookData data )
		{
			if ( ( from foo in resolutionObserver where foo.dataName == data.dataName select foo ).Count() == 0 )
			{
				resolutionObserver.Add( data );
				return true;
			}
			return false;
		}

		public void RemoveData<T>( T item )
		{
			if ( ( (ICommonData)item ).isEmpty )
				return;

			if ( item is IInteraction )
				interactionObserver.Remove( item as IInteraction );
			else if ( item is Trigger )
				triggersObserver.Remove( item as Trigger );
			else if ( item is Objective )
				objectiveObserver.Remove( item as Objective );
			else if ( item is TextBookData )
				resolutionObserver.Remove( item as TextBookData );
			else if ( item is Threat )
				threatObserver.Remove( item as Threat );
			else if ( item is Chapter )
				chapterObserver.Remove( item as Chapter );
		}

		/// <summary>
		/// Renames the Trigger (if new name doesn't exist) and updates all data collections so they point to the new name
		/// </summary>
		public bool RenameTrigger( string oldName, string newName, bool isMulti )
		{
			//bail out if new name already exists
			if ( triggersObserver.Count( t => t.dataName == newName ) > 0 )
				return false;

			foreach ( var obj in objectiveObserver )
				obj.RenameTrigger( oldName, newName );

			foreach ( var obj in resolutionObserver )
				if ( obj.triggerName == oldName )
					obj.triggerName = newName;

			foreach ( var obj in interactionObserver )
				obj.RenameTrigger( oldName, newName );

			foreach ( var obj in threatObserver )
				if ( obj.triggerName == oldName )
					obj.triggerName = newName;

			foreach ( var obj in chapterObserver )
				obj.RenameTrigger( oldName, newName, scenarioTypeJourney );

			//finally rename the trigger object itself
			triggersObserver.Where( t => t.dataName == oldName ).First().dataName = newName;
			triggersObserver.Where( t => t.dataName == oldName ).First().isMultiTrigger = isMulti;

			return true;
		}

		/// <summary>
		/// Check if named trigger is being used anywhere
		/// </summary>
		public Tuple<string, string> IsTriggerUsed( string name )
		{
			if ( triggersObserver.Count > 0 )
			{
				var used = interactionObserver.IsTriggerUsed( name );
				if ( used != null )
					return used;
				used = triggersObserver.IsTriggerUsed( name );
				if ( used != null )
					return used;
				used = objectiveObserver.IsTriggerUsed( name );
				if ( used != null )
					return used;
				used = resolutionObserver.IsTriggerUsed( name );
				if ( used != null )
					return used;

				//Threats no longer use Triggers, they use Events
				//used = threatObserver.IsTriggerUsed( name );
				//if ( used != null )
				//	return used;

				used = chapterObserver.IsTriggerUsed( name );
				if ( used != null )
					return used;
			}
			return null;
		}

		public bool isEventUsed( string name )
		{
			//check Threats
			return true;
		}

		/// <summary>
		/// Checks for duplicate name usage within same object type
		/// </summary>
		public bool IsDuplicate( ICommonData data )
		{
			if ( data is IInteraction )
			{
				for ( int i = 0; i < interactionObserver.Count; i++ )
				{
					if ( interactionObserver[i].dataName == data.dataName
					&& interactionObserver[i].GUID != data.GUID )
						return true;
				}
			}
			else if ( data is Trigger )
			{
				for ( int i = 0; i < triggersObserver.Count; i++ )
				{
					if ( triggersObserver[i].dataName == data.dataName
					&& triggersObserver[i].GUID != data.GUID )
						return true;
				}
			}
			else if ( data is Objective )
			{
				for ( int i = 0; i < objectiveObserver.Count; i++ )
				{
					if ( objectiveObserver[i].dataName == data.dataName
					&& objectiveObserver[i].GUID != data.GUID )
						return true;
				}
			}
			else if ( data is TextBookData )
			{
				for ( int i = 0; i < resolutionObserver.Count; i++ )
				{
					if ( resolutionObserver[i].dataName == data.dataName
					&& resolutionObserver[i].GUID != data.GUID )
						return true;
				}
			}
			else if ( data is Threat )
			{
				for ( int i = 0; i < threatObserver.Count; i++ )
				{
					if ( threatObserver[i].dataName == data.dataName
					&& threatObserver[i].GUID != data.GUID )
						return true;
				}
			}
			else if ( data is Chapter )
			{
				for ( int i = 0; i < chapterObserver.Count; i++ )
				{
					if ( chapterObserver[i].dataName == data.dataName
					&& chapterObserver[i].GUID != data.GUID )
						return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Updates any token or threat that references this interaction to use the new name.
		/// Updates Tokens if the event tokentype/personType changed.
		/// Updates Tokens if event is no longer a token interaction.
		/// Removes Event from Threat if Event becomes a token interaction.
		/// </summary>
		public void UpdateEventReferences( string oldName, IInteraction interaction )
		{
			//go through all the assigned tokens in hextiles and update their tokentype and triggerName
			foreach ( var chapter in chapterObserver )
			{
				foreach ( var tile in chapter.tileObserver )
				{
					if ( tile is HexTile )
					{
						foreach ( var token in ( (HexTile)tile ).tokenList )
						{
							if ( token.triggerName == oldName )
							{
								//update token type
								token.tokenType = interaction.tokenType;
								//update person type
								token.personType = interaction.personType;
								//rename
								token.triggerName = interaction.dataName;
								//remove event if it's no longer a token interaction
								if ( !interaction.isTokenInteraction )
								{
									token.triggerName = "None";
									( (HexTile)tile ).tokenList.Remove( token );
									break;
								}
							}
						}
					}
				}
			}

			//rename scenario threats that references this event
			foreach ( var threat in threatObserver )
			{
				if ( threat.triggerName == oldName )
				{
					threat.triggerName = interaction.dataName;
					//remove event if it's become a token interaction
					if ( interaction.isTokenInteraction )
						threat.triggerName = "None";
				}
			}
		}
	}
}
