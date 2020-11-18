using System.ComponentModel;

namespace JiME
{
	public class DecisionInteraction : InteractionBase, INotifyPropertyChanged, ICommonData
	{
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

		public DecisionInteraction( string name ) : base( name )
		{
			interactionType = InteractionType.Decision;

			isThreeChoices = false;
			choice1 = "Choice One";
			choice2 = "Choice Two";
			choice3 = "Choice Three";
			choice1Trigger = choice2Trigger = choice3Trigger = "None";
		}

		new public void RenameTrigger( string oldName, string newName )
		{
			base.RenameTrigger( oldName, newName );

			if ( choice1Trigger == oldName )
				choice1Trigger = newName;

			if ( choice2Trigger == oldName )
				choice2Trigger = newName;

			if ( choice3Trigger == oldName )
				choice3Trigger = newName;
		}
	}
}
