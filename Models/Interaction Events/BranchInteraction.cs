using System.Collections.Generic;
using System.ComponentModel;

namespace JiME
{
	public class BranchInteraction : InteractionBase, INotifyPropertyChanged, ICommonData
	{
		bool _branchTestEvent;

		//Story branch
		string _triggerNotSetTrigger;
		public string triggerTest { get; set; }
		public string triggerIsSet { get; set; }//event name
		public string triggerNotSet { get; set; }//event name
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
		public bool branchTestEvent
		{
			get { return _branchTestEvent; }
			set
			{
				_branchTestEvent = value;
				NotifyPropertyChanged( "branchTestEvent" );
			}
		}

		public BranchInteraction( string name ) : base( name )
		{
			interactionType = InteractionType.Branch;

			triggerTest = triggerIsSet = triggerNotSet = triggerIsSetTrigger = triggerNotSetTrigger = "None";
			branchTestEvent = true;
			//default blank event text so it activates silently
			eventBookData.pages = new List<string>();
			eventBookData.pages.Add( "" );
		}

		new public void RenameTrigger( string oldName, string newName )
		{
			base.RenameTrigger( oldName, newName );

			if ( triggerIsSetTrigger == oldName )
				triggerIsSetTrigger = newName;

			if ( triggerNotSetTrigger == oldName )
				triggerNotSetTrigger = newName;

			if ( triggerTest == oldName )
				triggerTest = newName;
		}
	}
}
