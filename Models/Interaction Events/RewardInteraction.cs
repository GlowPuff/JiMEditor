using System.Collections.Generic;
using System.ComponentModel;


namespace JiME
{
	public class RewardInteraction : InteractionBase, INotifyPropertyChanged, ICommonData
	{
		int _rewardLore, _rewardXP, _rewardThreat;

		public int rewardLore
		{
			get => _rewardLore;
			set
			{
				_rewardLore = value;
				NotifyPropertyChanged( "rewardLore" );
			}
		}
		public int rewardXP
		{
			get => _rewardXP;
			set
			{
				_rewardXP = value;
				NotifyPropertyChanged( "rewardXP" );
			}
		}
		public int rewardThreat
		{
			get => _rewardThreat;
			set
			{
				_rewardThreat = value;
				NotifyPropertyChanged( "rewardThreat" );
			}
		}

		public RewardInteraction( string name ) : base( name )
		{
			interactionType = InteractionType.Reward;

			rewardLore = rewardXP = rewardThreat = 0;
			//default blank event text
			eventBookData.pages = new List<string>();
			eventBookData.pages.Add( "" );
		}
	}
}
