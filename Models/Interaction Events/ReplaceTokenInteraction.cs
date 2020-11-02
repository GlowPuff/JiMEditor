using System;
using System.ComponentModel;

namespace JiME
{
	public class ReplaceTokenInteraction : INotifyPropertyChanged, ICommonData, IInteraction
	{
		//common
		string _dataName, _triggerName, _triggerAfterName;
		bool _isTokenInteraction;
		int _loreReward;
		TokenType _tokenType;
		PersonType _personType;

		public string dataName
		{
			get { return _dataName; }
			set
			{
				if ( _dataName != value )
				{
					_dataName = value;
					NotifyPropertyChanged( "dataName" );
				}
			}
		}
		public Guid GUID { get; set; }
		public bool isEmpty { get; set; }
		public string triggerName
		{
			get => _triggerName;
			set
			{
				_triggerName = value;
				NotifyPropertyChanged( "triggerName" );
			}
		}
		public string triggerAfterName
		{
			get => _triggerAfterName;
			set
			{
				_triggerAfterName = value;
				NotifyPropertyChanged( "triggerAfterName" );
			}
		}
		public bool isTokenInteraction
		{
			get => _isTokenInteraction;
			set
			{
				_isTokenInteraction = value;
				NotifyPropertyChanged( "isTokenInteraction" );
			}
		}
		public TokenType tokenType
		{
			get => _tokenType;
			set
			{
				_tokenType = value;
				NotifyPropertyChanged( "tokenType" );
			}
		}
		public PersonType personType
		{
			get => _personType;
			set { _personType = value; NotifyPropertyChanged( "personType" ); }
		}
		public TextBookData textBookData { get; set; }
		public TextBookData eventBookData { get; set; }
		public int loreReward
		{
			get => _loreReward;
			set
			{
				_loreReward = value;
				NotifyPropertyChanged( "loreReward" );
			}
		}

		//IInteraction properties
		public InteractionType interactionType { get; set; }
		public event PropertyChangedEventHandler PropertyChanged;

		//replace event
		string _eventToReplace, _replaceWithEvent;
		bool _noText;
		Guid _replaceWithGUID;

		public string eventToReplace
		{
			get => _eventToReplace;
			set
			{
				_eventToReplace = value;
				NotifyPropertyChanged( "eventToReplace" );
			}
		}
		public string replaceWithEvent
		{
			get => _replaceWithEvent;
			set
			{
				_replaceWithEvent = value;
				NotifyPropertyChanged( "replaceWithEvent" );
			}
		}
		public bool noText
		{
			get => _noText;
			set
			{
				_noText = value;
				NotifyPropertyChanged( "noText" );
			}
		}
		public Guid replaceWithGUID
		{
			get => _replaceWithGUID;
			set
			{
				_replaceWithGUID = value;
				NotifyPropertyChanged( "replaceWithGUID" );
			}
		}

		public ReplaceTokenInteraction( string name )
		{
			interactionType = InteractionType.Replace;
			dataName = name;
			GUID = Guid.NewGuid();
			isEmpty = false;
			triggerName = "None";
			triggerAfterName = "None";
			isTokenInteraction = false;
			tokenType = TokenType.Search;
			personType = PersonType.Human;
			textBookData = new TextBookData();
			textBookData.pages.Add( "Default Flavor Text\n\nUse this text to describe the Event situation and present choices, depending on the type of Event this is." );
			eventBookData = new TextBookData();
			eventBookData.pages.Add( "Default Event Text.\n\nThis text is shown after the Event is triggered. Use it to tell about the actual event that has been triggered Example: Describe an Enemy Threat, present a Test, describe a Decision, etc." );
			loreReward = 0;

			eventToReplace = replaceWithEvent = "None";
			noText = true;
			replaceWithGUID = Guid.Empty;
		}

		public void NotifyPropertyChanged( string propName )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propName ) );
		}

		public void RenameTrigger( string oldName, string newName )
		{
			if ( triggerName == oldName )
				triggerName = newName;

			if ( triggerAfterName == oldName )
				triggerAfterName = newName;
		}
	}
}
