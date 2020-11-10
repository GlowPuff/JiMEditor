using System.Collections.ObjectModel;
using System.ComponentModel;

namespace JiME
{
	public class MultiEventInteraction : InteractionBase, INotifyPropertyChanged, ICommonData
	{
		bool _usingTriggers, _isSilent;
		public ObservableCollection<string> eventList { get; set; }
		public ObservableCollection<string> triggerList { get; set; }
		public bool usingTriggers
		{
			get { return _usingTriggers; }
			set
			{
				_usingTriggers = value;
				NotifyPropertyChanged( "usingTriggers" );
			}
		}
		public bool isSilent
		{
			get { return _isSilent; }
			set
			{
				_isSilent = value;
				NotifyPropertyChanged( "isSilent" );
			}
		}

		public MultiEventInteraction( string name ) : base( name )
		{
			interactionType = InteractionType.MultiEvent;

			eventList = new ObservableCollection<string>();
			triggerList = new ObservableCollection<string>();
			usingTriggers = true;
			isSilent = true;
		}

		new public void RenameTrigger( string oldName, string newName )
		{
			if ( triggerName == oldName )
				triggerName = newName;

			if ( triggerAfterName == oldName )
				triggerAfterName = newName;

			for ( int i = 0; i < triggerList.Count; i++ )
			{
				if ( triggerList[i] == oldName )
					triggerList[i] = newName;
			}
		}
	}
}
