using System;
using System.ComponentModel;

namespace JiME
{
	public class Objective : INotifyPropertyChanged, ICommonData
	{
		string _dataName, _eventName, _triggerName, _objectiveReminder, _nextTrigger, _triggeredByName;
		bool _skipSummary;
		int _loreReward;

		public string dataName
		{
			get => _dataName;
			set
			{
				if ( value != _dataName )
				{
					_dataName = value;
					NotifyChange( "dataName" );
				}
			}
		}//description in editor
		public Guid GUID { get; set; }
		public bool isEmpty { get; set; }
		public string eventName
		{
			get => _eventName;
			set
			{
				if ( value != _eventName )
				{
					_eventName = value;
					NotifyChange( "eventName" );
				}
			}
		}
		public string triggerName
		{
			get => _triggerName;
			set
			{
				if ( value != _triggerName )
				{
					_triggerName = value;
					NotifyChange( "triggerName" );
				}
			}
		}
		public string triggeredByName
		{
			get => _triggeredByName;
			set
			{
				if ( value != _triggeredByName )
				{
					_triggeredByName = value;
					NotifyChange( "triggeredByName" );
				}
			}
		}
		public string objectiveReminder
		{
			get { return _objectiveReminder; }
			set
			{
				if ( _objectiveReminder != value )
				{
					_objectiveReminder = value;
					NotifyChange( "objectiveReminder" );
				}
			}
		}
		public bool skipSummary
		{
			get => _skipSummary;
			set
			{
				if ( _skipSummary != value )
				{
					_skipSummary = value;
					NotifyChange( "skipSummary" );
				}
			}
		}
		//public string nextObjective
		//{
		//	get => _nextObjective;
		//	set
		//	{
		//		if ( value != _nextObjective )
		//		{
		//			_nextObjective = value;
		//			NotifyChange( "nextObjective" );
		//		}
		//	}
		//}
		public string nextTrigger
		{
			get => _nextTrigger;
			set
			{
				if ( value != _nextTrigger )
				{
					_nextTrigger = value;
					NotifyChange( "nextTrigger" );
				}
			}
		}
		public TextBookData textBookData { get; set; }
		public int loreReward
		{
			get => _loreReward;
			set
			{
				_loreReward = value;
				NotifyChange( "loreReward" );
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public Objective( string shortname )
		{
			GUID = Guid.NewGuid();
			dataName = shortname;
			eventName = "None";
			triggerName = "None";
			triggeredByName = "None";
			objectiveReminder = "Default Objective Reminder";
			textBookData = new TextBookData( "Default Name" );
			textBookData.pages.Add( "Default Objective Text\n\nEdit this text to provide a summary to players so they understand what they need to accomplish to complete this Objective." );
			//nextObjective = "None";
			nextTrigger = "None";
			loreReward = 0;
		}

		public static Objective EmptyObjective()
		{
			Objective obj = new Objective( "None" );
			obj.textBookData.pages[0] = "None";
			obj.objectiveReminder = "";
			obj.isEmpty = true;
			return obj;
		}

		public void RenameTrigger( string oldName, string newName )
		{
			if ( triggerName == oldName )
				triggerName = newName;

			if ( nextTrigger == oldName )
				nextTrigger = newName;

			if ( triggeredByName == oldName )
				triggeredByName = newName;
		}

		void NotifyChange( string name )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
		}
	}
}
