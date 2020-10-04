using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace JiME
{
	public class PersistentTokenInteraction : INotifyPropertyChanged, ICommonData, IInteraction
	{
		//common
		string _dataName, _triggerName, _triggerAfterName, _eventToActivate, _alternativeTextTrigger;
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

		public TextBookData alternativeBookData { get; set; }
		public string eventToActivate
		{
			get { return _eventToActivate; }
			set { _eventToActivate = value; NotifyPropertyChanged( "eventToActivate" ); }
		}
		public string alternativeTextTrigger
		{
			get { return _alternativeTextTrigger; }
			set { _alternativeTextTrigger = value; NotifyPropertyChanged( "alternativeTextTrigger" ); }
		}

		//IInteraction properties
		public InteractionType interactionType { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public PersistentTokenInteraction( string name )
		{
			interactionType = InteractionType.Persistent;
			dataName = name;
			GUID = Guid.NewGuid();
			isEmpty = false;
			triggerName = "None";
			triggerAfterName = "None";
			isTokenInteraction = true;
			tokenType = TokenType.None;
			personType = PersonType.Human;
			textBookData = new TextBookData();
			textBookData.pages.Add( "Default Flavor Text\n\nUse this text to describe the Event situation and present choices, depending on the type of Event this is." );
			eventBookData = new TextBookData();
			eventBookData.pages.Add( "Default Event Text.\n\nThis text is shown after the Event is triggered. Use it to tell about the actual event that has been triggered Example: Describe an Enemy Threat, present a Test, describe a Decision, etc." );
			loreReward = 0;

			alternativeBookData = new TextBookData();
			alternativeBookData.pages.Add( "This text will be shown persistently every time a player inspects this Event's Token, but only after the 'Alternative Flavor Text Trigger' has been set." );
			alternativeTextTrigger = "None";
			eventToActivate = "None";
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
