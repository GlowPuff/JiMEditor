using System.ComponentModel;

namespace JiME
{
	public class PersistentTokenInteraction : InteractionBase, INotifyPropertyChanged, ICommonData
	{
		string _eventToActivate, _alternativeTextTrigger;
		public TextBookData alternativeBookData { get; set; }
		public string eventToActivate
		{
			get { return _eventToActivate; }
			set { _eventToActivate = value; NotifyPropertyChanged( "eventToActivate" ); }
		}
		public string alternativeTextTrigger
		{
			get { return _alternativeTextTrigger; }
			set { _alternativeTextTrigger = value; NotifyPropertyChanged( "alternativeTextTrigger" ); }
		}

		public PersistentTokenInteraction( string name ) : base( name )
		{
			interactionType = InteractionType.Persistent;

			alternativeBookData = new TextBookData();
			alternativeBookData.pages.Add( "This text will be shown persistently every time a player inspects this Event's Token, but only after the 'Alternative Flavor Text Trigger' has been set." );
			alternativeTextTrigger = "None";
			eventToActivate = "None";
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
