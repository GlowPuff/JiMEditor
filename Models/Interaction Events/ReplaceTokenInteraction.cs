using System;
using System.ComponentModel;

namespace JiME
{
	public class ReplaceTokenInteraction : InteractionBase, INotifyPropertyChanged, ICommonData
	{
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

		public ReplaceTokenInteraction( string name ) : base( name )
		{
			interactionType = InteractionType.Replace;

			eventToReplace = replaceWithEvent = "None";
			noText = true;
			replaceWithGUID = Guid.Empty;
		}

		new public void RenameTrigger( string oldName, string newName )
		{
			if ( triggerName == oldName )
				triggerName = newName;

			if ( triggerAfterName == oldName )
				triggerAfterName = newName;
		}
	}
}
