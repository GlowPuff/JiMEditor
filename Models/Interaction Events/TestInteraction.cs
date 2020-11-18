using System;
using System.ComponentModel;

namespace JiME
{
	public class TestInteraction : InteractionBase, INotifyPropertyChanged, ICommonData
	{
		//Attribute Test
		string _failTrigger, _successTrigger;
		bool _noAlternate, _passFail;
		int _rewardLore, _rewardXP, _rewardThreat, _failThreat;

		public Ability testAttribute { get; set; }
		public Ability altTestAttribute { get; set; }
		public bool isCumulative { get; set; }
		public bool passFail
		{
			get => _passFail;
			set
			{
				_passFail = value;
				NotifyPropertyChanged( "passFail" );
			}
		}
		public int successValue { get; set; }
		public string successTrigger
		{
			get => _successTrigger;
			set
			{
				if ( _successTrigger != value )
				{
					_successTrigger = value;
					NotifyPropertyChanged( "successTrigger" );
				}
			}
		}
		public string failTrigger
		{
			get => _failTrigger;
			set
			{
				if ( _failTrigger != value )
				{
					_failTrigger = value;
					NotifyPropertyChanged( "failTrigger" );
				}
			}
		}
		public bool noAlternate
		{
			get => _noAlternate;
			set
			{
				_noAlternate = value;
				NotifyPropertyChanged( "noAlternate" );
			}
		}
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
		public int failThreat
		{
			get => _failThreat;
			set
			{
				_failThreat = value;
				NotifyPropertyChanged( "failThreat" );
			}
		}

		public TextBookData passBookData { get; set; }
		public TextBookData failBookData { get; set; }
		public TextBookData progressBookData { get; set; }

		public TestInteraction( string name ) : base( name )
		{
			interactionType = InteractionType.StatTest;

			successTrigger = "None";
			failTrigger = "None";
			successValue = 0;
			isCumulative = passFail = false;
			testAttribute = Ability.Might;
			altTestAttribute = Ability.Might;
			noAlternate = true;

			passBookData = new TextBookData();
			passBookData.pages.Add( "Default Pass Text.\n\nThis text is shown if the player(s) Pass the test." );
			failBookData = new TextBookData();
			failBookData.pages.Add( "Default Fail Text.\n\nThis text is shown if the player(s) Fail the test." );
			progressBookData = new TextBookData();
			progressBookData.pages.Add( "Default Progress Text.\n\nThis text is shown if the Test is Cumulative and the current value is greater than 0 but less than the Success Value for completion. Use it as a way to indicate progress towards competing the Test." );
		}

		new public void RenameTrigger( string oldName, string newName )
		{
			base.RenameTrigger( oldName, newName );

			if ( successTrigger == oldName )
				successTrigger = newName;

			if ( failTrigger == oldName )
				failTrigger = newName;
		}
	}
}
