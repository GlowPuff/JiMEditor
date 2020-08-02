/*using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace JiME
{
	public class Interaction : INotifyPropertyChanged, ICommonData
	{
		//common
		string _dataName, _triggerName;
		bool _isRandom, _isTokenInteraction, _isFromThreatThreshold;
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
		public InteractionType interactionType { get; set; }
		public string triggerName
		{
			get => _triggerName;
			set
			{
				_triggerName = value;
				NotifyPropertyChanged( "triggerName" );
			}
		}
		public string triggerAfterName { get; set; }
		public bool isRandom
		{
			get => _isRandom;
			set
			{
				_isRandom = value;
				NotifyPropertyChanged( "isRandom" );
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
		public bool isFromThreatThreshold
		{
			get => _isFromThreatThreshold;
			set
			{
				_isFromThreatThreshold = value;
				NotifyPropertyChanged( "isFromThreatThreshold" );
			}
		}
		public TextBookData eventBookData { get; set; }

		//Attribute Test
		string _failTrigger, _successTrigger;
		public Ability testAttribute { get; set; }
		public bool isCumulative { get; set; }
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
		public TextBookData textBookData { get; set; }
		public TextBookData passBookData { get; set; }
		public TextBookData failBookData { get; set; }
		public TextBookData progressBookData { get; set; }

		//Monster
		bool _isReuseable;
		public bool isReuseable
		{
			get => _isReuseable;
			set
			{
				_isReuseable = value;
				NotifyPropertyChanged( "isReuseable" );
			}
		}
		public ObservableCollection<Monster> monsterCollection { get; set; }

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

		//Story branch
		//bool _branchTestEvent;
		string _triggerNotSetTrigger;
		public string triggerTest { get; set; }
		public string triggerIsSet { get; set; }
		public string triggerNotSet { get; set; }
		public string triggerIsSetTrigger { get; set; }
		public string triggerNotSetTrigger
		{
			get => _triggerNotSetTrigger;
			set
			{
				_triggerNotSetTrigger = value;
				NotifyPropertyChanged( "triggerNotSetTrigger" );
			}
		}
		public bool branchTestEvent { get; set; }
		//{
		//	get => _branchTestEvent;
		//	set
		//	{
		//		_branchTestEvent = value;
		//		NotifyPropertyChanged( "branchTestEvent" );
		//	}
		//}

		public event PropertyChangedEventHandler PropertyChanged;

		public Interaction( string name, bool random )
		{
			monsterCollection = new ObservableCollection<Monster>();
			GUID = Guid.NewGuid();
			dataName = name;
			triggerName = "None";
			triggerAfterName = "None";
			successTrigger = "None";
			failTrigger = "None";
			triggerTest = "None";
			triggerIsSet = triggerNotSet = "None";
			triggerIsSetTrigger = triggerNotSetTrigger = "None";
			interactionType = InteractionType.Text;
			isEmpty = false;
			isCumulative = false;
			isReuseable = false;
			isTokenInteraction = false;
			isFromThreatThreshold = false;
			isRandom = random;
			successValue = 0;
			testAttribute = Ability.Might;
			textBookData = new TextBookData();
			textBookData.pages.Add( "Default Flavor Text\n\nUse this text to describe the Event situation and present choices, depending on the type of Event this is." );
			passBookData = new TextBookData();
			passBookData.pages.Add( "Default Pass Text.\n\nThis text is shown if the player(s) Pass the test." );
			failBookData = new TextBookData();
			failBookData.pages.Add( "Default Fail Text.\n\nThis text is shown if the player(s) Fail the test." );
			progressBookData = new TextBookData();
			progressBookData.pages.Add( "Default Progress Text.\n\nThis text is shown if the Test is Cumulative and the current value is greater than 0 but less than the Success Value for completion. Use it as a way to indicate progress towards competing the Test." );
			eventBookData = new TextBookData();
			eventBookData.pages.Add( "Default Event Text.\n\nThis text is shown after the Event is triggered. Use it to tell about the actual event that has been triggered Example: Describe a Monster Threat, present a Test, describe a Decision, etc." );
			isThreeChoices = false;
			choice1 = "Choice One";
			choice2 = "Choice Two";
			choice3 = "Choice Three";
			choice1Trigger = choice2Trigger = choice3Trigger = "None";
			branchTestEvent = true;
		}

		public void AddMonster( Monster m )
		{
			monsterCollection.Add( m );
		}

		public void RenameTrigger( string oldName, string newName )
		{
			if ( triggerName == oldName )
				triggerName = newName;

			if ( triggerAfterName == oldName )
				triggerAfterName = newName;

			if ( successTrigger == oldName )
				successTrigger = newName;
			if ( failTrigger == oldName )
				failTrigger = newName;

			if ( choice1Trigger == oldName )
				choice1Trigger = newName;
			if ( choice2Trigger == oldName )
				choice2Trigger = newName;
			if ( choice3Trigger == oldName )
				choice3Trigger = newName;
		}

		public void NotifyPropertyChanged( string propName )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propName ) );
		}

		public static Interaction EmptyInteraction()
		{
			return new Interaction( "None", false ) { isEmpty = true };
		}
	}
}
*/