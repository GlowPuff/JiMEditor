using System;
using System.ComponentModel;

namespace JiME
{
	public class DecisionInteraction : INotifyPropertyChanged, ICommonData, IInteraction
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

		//Decision
		string _c1t, _c2t, _c3t;
		public string choice1 { get; set; }
		public string choice2 { get; set; }
		public string choice3 { get; set; }
		public bool isThreeChoices { get; set; }
		public string choice1Trigger
		{
			get => _c1t;
			set
			{
				if ( _c1t != value )
				{
					_c1t = value;
					NotifyPropertyChanged( "choice1Trigger" );
				}
			}
		}
		public string choice2Trigger
		{
			get => _c2t;
			set
			{
				if ( _c2t != value )
				{
					_c2t = value;
					NotifyPropertyChanged( "choice2Trigger" );
				}
			}
		}
		public string choice3Trigger
		{
			get => _c3t;
			set
			{
				if ( _c3t != value )
				{
					_c3t = value;
					NotifyPropertyChanged( "choice3Trigger" );
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public DecisionInteraction( string name )
		{
			interactionType = InteractionType.Decision;
			dataName = name;
			GUID = Guid.NewGuid();
			isEmpty = false;
			triggerName = "None";
			triggerAfterName = "None";
			isTokenInteraction = false;
			tokenType = TokenType.None;
			personType = PersonType.Human;
			textBookData = new TextBookData();
			textBookData.pages.Add( "Default Flavor Text\n\nUse this text to describe the Event situation and present choices, depending on the type of Event this is." );
			eventBookData = new TextBookData();
			eventBookData.pages.Add( "Default Event Text.\n\nThis text is shown after the Event is triggered. Use it to tell about the actual event that has been triggered Example: Describe an Enemy Threat, present a Test, describe a Decision, etc." );
			loreReward = 0;

			isThreeChoices = false;
			choice1 = "Choice One";
			choice2 = "Choice Two";
			choice3 = "Choice Three";
			choice1Trigger = choice2Trigger = choice3Trigger = "None";
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
