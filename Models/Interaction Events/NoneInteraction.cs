using System;
using System.ComponentModel;

namespace JiME
{
	public class NoneInteraction : InteractionBase, INotifyPropertyChanged, ICommonData
	{
		public NoneInteraction( string name ) : base( name ) { }

		public static NoneInteraction EmptyInteraction()
		{
			NoneInteraction empty = new NoneInteraction( "None" )
			{
				dataName = "None",
				isEmpty = true,
				tokenType = TokenType.None,
				personType = PersonType.Human,
				isTokenInteraction = false
			};
			return empty;
		}
	}
}
