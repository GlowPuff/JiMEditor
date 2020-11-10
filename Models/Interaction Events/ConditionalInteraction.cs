using System.Collections.ObjectModel;
using System.ComponentModel;

namespace JiME
{
	public class ConditionalInteraction : InteractionBase, INotifyPropertyChanged, ICommonData
	{
		string _finishedTrigger;

		public ObservableCollection<string> triggerList { get; set; }
		public string finishedTrigger
		{
			get { return _finishedTrigger; }
			set
			{
				_finishedTrigger = value;
				NotifyPropertyChanged( "finishedTrigger" );
			}
		}

		public ConditionalInteraction( string name ) : base( name )
		{
			interactionType = InteractionType.Conditional;

			triggerList = new ObservableCollection<string>();
			finishedTrigger = "None";
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
