using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace JiME
{
	public class ThreatInteraction : InteractionBase, INotifyPropertyChanged, ICommonData
	{
		string _triggerDefeatedName;
		bool[] _includedEnemies;
		int _basePoolPoints;
		DifficultyBias _difficultyBias;

		public string triggerDefeatedName
		{
			get { return _triggerDefeatedName; }
			set
			{
				_triggerDefeatedName = value;
				NotifyPropertyChanged( "triggerDefeatedName" );
			}
		}
		public int basePoolPoints
		{
			get => _basePoolPoints;
			set
			{
				_basePoolPoints = value;
				NotifyPropertyChanged( "basePoolPoints" );
			}
		}
		public bool[] includedEnemies
		{
			get => _includedEnemies;
			set
			{
				_includedEnemies = value;
				NotifyPropertyChanged( "includedEnemies" );
			}
		}
		public DifficultyBias difficultyBias
		{
			get => _difficultyBias;
			set
			{
				_difficultyBias = value;
				NotifyPropertyChanged( "difficultyBias" );
			}
		}

		public ObservableCollection<Monster> monsterCollection { get; set; }

		public ThreatInteraction( string name ) : base( name )
		{
			interactionType = InteractionType.Threat;

			triggerDefeatedName = "None";
			includedEnemies = new bool[7].Fill( true );
			basePoolPoints = 10;
			difficultyBias = DifficultyBias.Medium;

			monsterCollection = new ObservableCollection<Monster>();
		}

		new public void RenameTrigger( string oldName, string newName )
		{
			if ( triggerName == oldName )
				triggerName = newName;

			if ( triggerAfterName == oldName )
				triggerAfterName = newName;

			if ( triggerDefeatedName == oldName )
				triggerDefeatedName = newName;
		}

		public void AddMonster( Monster m )
		{
			monsterCollection.Add( m );
		}
	}
}
