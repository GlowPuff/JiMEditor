using System.ComponentModel;

namespace JiME
{
	public class DialogInteraction : InteractionBase, INotifyPropertyChanged, ICommonData
	{
		//dialog
		string _choice1, _choice2, _choice3, _c1Trigger, _c2Trigger, _c3Trigger, _c1Text, _c2Text, _c3Text, _persistentText;
		bool _isPersistent;

		public string choice1
		{
			get => _choice1;
			set
			{
				_choice1 = value;
				NotifyPropertyChanged( "choice1" );
			}
		}
		public string choice2
		{
			get => _choice2;
			set
			{
				_choice2 = value;
				NotifyPropertyChanged( "choice2" );
			}
		}
		public string choice3
		{
			get => _choice3;
			set
			{
				_choice3 = value;
				NotifyPropertyChanged( "choice3" );
			}
		}
		public string c1Trigger
		{
			get => _c1Trigger;
			set
			{
				_c1Trigger = value;
				NotifyPropertyChanged( "c1Trigger" );
			}
		}
		public string c2Trigger
		{
			get => _c2Trigger;
			set
			{
				_c2Trigger = value;
				NotifyPropertyChanged( "c2Trigger" );
			}
		}
		public string c3Trigger
		{
			get => _c3Trigger;
			set
			{
				_c3Trigger = value;
				NotifyPropertyChanged( "c3Trigger" );
			}
		}
		public string c1Text
		{
			get => _c1Text;
			set
			{
				_c1Text = value;
				NotifyPropertyChanged( "c1Text" );
			}
		}
		public string c2Text
		{
			get => _c2Text;
			set
			{
				_c2Text = value;
				NotifyPropertyChanged( "c2Text" );
			}
		}
		public string c3Text
		{
			get => _c3Text;
			set
			{
				_c3Text = value;
				NotifyPropertyChanged( "c3Text" );
			}
		}
		public bool isPersistent
		{
			get => _isPersistent;
			set
			{
				_isPersistent = value;
				NotifyPropertyChanged( "isPersistent" );
			}
		}
		public string persistentText
		{
			get => _persistentText;
			set
			{
				_persistentText = value;
				NotifyPropertyChanged( "persistentText" );
			}
		}

		public DialogInteraction( string name ) : base( name )
		{
			interactionType = InteractionType.Dialog;

			choice1 = "Choice 1";
			choice2 = "Choice 2";
			choice3 = "Choice 3";
			c1Text = c2Text = c3Text = persistentText = "";
			c1Trigger = c2Trigger = c3Trigger = "None";
			isPersistent = false;
		}

		new public void RenameTrigger( string oldName, string newName )
		{
			base.RenameTrigger( oldName, newName );

			if ( c1Trigger == oldName )
				c1Trigger = newName;
			if ( c2Trigger == oldName )
				c2Trigger = newName;
			if ( c3Trigger == oldName )
				c3Trigger = newName;
		}
	}
}
