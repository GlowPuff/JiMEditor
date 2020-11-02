using System;
using System.ComponentModel;

namespace JiME
{
	public class DialogInteraction : INotifyPropertyChanged, ICommonData, IInteraction
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

		//dialog
		string _choice1, _choice2, _choice3, _c1Trigger, _c2Trigger, _c3Trigger, _c1Text, _c2Text, _c3Text, _persistentText;
		bool _isPersistent;

		public string choice1
		{
			get => _choice1;
			set
			{
				_choice1 = value;
				NotifyPropertyChanged( "choice1" );
			}
		}
		public string choice2
		{
			get => _choice2;
			set
			{
				_choice2 = value;
				NotifyPropertyChanged( "choice2" );
			}
		}
		public string choice3
		{
			get => _choice3;
			set
			{
				_choice3 = value;
				NotifyPropertyChanged( "choice3" );
			}
		}
		public string c1Trigger
		{
			get => _c1Trigger;
			set
			{
				_c1Trigger = value;
				NotifyPropertyChanged( "c1Trigger" );
			}
		}
		public string c2Trigger
		{
			get => _c2Trigger;
			set
			{
				_c2Trigger = value;
				NotifyPropertyChanged( "c2Trigger" );
			}
		}
		public string c3Trigger
		{
			get => _c3Trigger;
			set
			{
				_c3Trigger = value;
				NotifyPropertyChanged( "c3Trigger" );
			}
		}
		public string c1Text
		{
			get => _c1Text;
			set
			{
				_c1Text = value;
				NotifyPropertyChanged( "c1Text" );
			}
		}
		public string c2Text
		{
			get => _c2Text;
			set
			{
				_c2Text = value;
				NotifyPropertyChanged( "c2Text" );
			}
		}
		public string c3Text
		{
			get => _c3Text;
			set
			{
				_c3Text = value;
				NotifyPropertyChanged( "c3Text" );
			}
		}
		public bool isPersistent
		{
			get => _isPersistent;
			set
			{
				_isPersistent = value;
				NotifyPropertyChanged( "isPersistent" );
			}
		}
		public string persistentText
		{
			get => _persistentText;
			set
			{
				_persistentText = value;
				NotifyPropertyChanged( "persistentText" );
			}
		}

		public DialogInteraction( string name )
		{
			interactionType = InteractionType.Dialog;
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

			choice1 = "Choice 1";
			choice2 = "Choice 2";
			choice3 = "Choice 3";
			c1Text = c2Text = c3Text = persistentText = "";
			c1Trigger = c2Trigger = c3Trigger = "None";
			isPersistent = false;
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

			if ( c1Trigger == oldName )
				c1Trigger = newName;
			if ( c2Trigger == oldName )
				c2Trigger = newName;
			if ( c3Trigger == oldName )
				c3Trigger = newName;
		}
	}
}
