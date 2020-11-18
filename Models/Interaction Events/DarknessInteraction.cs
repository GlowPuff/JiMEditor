using System.ComponentModel;

namespace JiME
{
	public class DarknessInteraction : InteractionBase, INotifyPropertyChanged, ICommonData
	{
		public DarknessInteraction( string name ) : base( name )
		{
			interactionType = InteractionType.Darkness;
		}
	}
}
