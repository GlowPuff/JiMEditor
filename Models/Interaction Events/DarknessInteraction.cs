using System.ComponentModel;

namespace JiME
{
	public class DarknessInteraction : InteractionBase, INotifyPropertyChanged, ICommonData
	{
		public DarknessInteraction( string name ) : base( name )
		{
			interactionType = InteractionType.Darkness;
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
