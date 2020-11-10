using System.ComponentModel;

namespace JiME
{
	public class BranchInteraction : InteractionBase, INotifyPropertyChanged, ICommonData
	{
		bool _branchTestEvent;

		//Story branch
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
		}
	}
}
