using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace JiME
{
	public class ThreatInteraction : INotifyPropertyChanged, ICommonData, IInteraction
	{
		//common
		string _dataName, _triggerName, _triggerAfterName;
		bool _isTokenInteraction;
		int _loreReward;
		TokenType _tokenType;

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

		//Threat
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

		public event PropertyChangedEventHandler PropertyChanged;

		public ThreatInteraction( string name, bool random )
		{
			interactionType = InteractionType.Threat;
			dataName = name;
			GUID = Guid.NewGuid();
			isEmpty = false;
			triggerName = "None";
			triggerAfterName = "None";
			isTokenInteraction = false;
			tokenType = TokenType.None;
			textBookData = new TextBookData();
			textBookData.pages.Add( "Default Flavor Text\n\nUse this text to describe the Event situation and present choices, depending on the type of Event this is." );
			eventBookData = new TextBookData();
			eventBookData.pages.Add( "Default Event Text.\n\nThis text is shown after the Event is triggered. Use it to tell about the actual event that has been triggered Example: Describe a Monster Threat, present a Test, describe a Decision, etc." );
			loreReward = 0;

			isReuseable = false;
			monsterCollection = new ObservableCollection<Monster>();
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

		public void AddMonster( Monster m )
		{
			monsterCollection.Add( m );
		}
	}
}
