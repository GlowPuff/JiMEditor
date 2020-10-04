using System;
using System.ComponentModel;

namespace JiME
{
	public class TestInteraction : INotifyPropertyChanged, ICommonData, IInteraction
	{
		//common
		string _dataName, _triggerName, _triggerAfterName;
		bool _isTokenInteraction, _passFail;
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
		//public bool isRandom
		//{
		//	get => _isRandom;
		//	set
		//	{
		//		_isRandom = value;
		//		NotifyPropertyChanged( "isRandom" );
		//	}
		//}
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

		//Attribute Test
		string _failTrigger, _successTrigger;
		public Ability testAttribute { get; set; }
		public bool isCumulative { get; set; }
		public bool passFail
		{
			get => _passFail;
			set
			{
				_passFail = value;
				NotifyPropertyChanged( "passFail" );
			}
		}
		public int successValue { get; set; }
		public string successTrigger
		{
			get => _successTrigger;
			set
			{
				if ( _successTrigger != value )
				{
					_successTrigger = value;
					NotifyPropertyChanged( "successTrigger" );
				}
			}
		}
		public string failTrigger
		{
			get => _failTrigger;
			set
			{
				if ( _failTrigger != value )
				{
					_failTrigger = value;
					NotifyPropertyChanged( "failTrigger" );
				}
			}
		}
		public TextBookData passBookData { get; set; }
		public TextBookData failBookData { get; set; }
		public TextBookData progressBookData { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public TestInteraction( string name, bool random )
		{
			interactionType = InteractionType.StatTest;
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

			successTrigger = "None";
			failTrigger = "None";
			successValue = 0;
			isCumulative = passFail = false;
			testAttribute = Ability.Might;
			passBookData = new TextBookData();
			passBookData.pages.Add( "Default Pass Text.\n\nThis text is shown if the player(s) Pass the test." );
			failBookData = new TextBookData();
			failBookData.pages.Add( "Default Fail Text.\n\nThis text is shown if the player(s) Fail the test." );
			progressBookData = new TextBookData();
			progressBookData.pages.Add( "Default Progress Text.\n\nThis text is shown if the Test is Cumulative and the current value is greater than 0 but less than the Success Value for completion. Use it as a way to indicate progress towards competing the Test." );

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
