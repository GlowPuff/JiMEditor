using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace JiME.Views
{
	/// <summary>
	/// Interaction logic for MonsterEditorWindow.xaml
	/// </summary>
	public partial class MonsterEditorWindow : Window
	{
		public Monster monster { get; set; }
		public DefaultStats defaultStats { get; set; }

		int Light { get { return 2; } }
		int Medium { get { return 3; } }
		int Heavy { get { return 4; } }

		public MonsterEditorWindow( Monster m = null )
		{
			InitializeComponent();
			DataContext = this;

			cancelButton.Visibility = m == null ? Visibility.Visible : Visibility.Collapsed;
			monster = m ?? new Monster( "" );

			if ( monster.defaultStats )
			{
				stats1.IsEnabled = false;
				stats2.IsEnabled = false;
			}
			FillDefaultStats( monster.monsterType.ToString() );

			//negated radio buttons
			mightRB.IsChecked = monster.negatedBy == Ability.Might;
			agilityRB.IsChecked = monster.negatedBy == Ability.Agility;
			wisdomRB.IsChecked = monster.negatedBy == Ability.Wisdom;
			spiritRB.IsChecked = monster.negatedBy == Ability.Spirit;
			witRB.IsChecked = monster.negatedBy == Ability.Wit;
			//monster type radio buttons
			ruffianRB.IsChecked = monster.monsterType == MonsterType.Ruffian;
			hunterRB.IsChecked = monster.monsterType == MonsterType.OrcHunter;
			orcRB.IsChecked = monster.monsterType == MonsterType.OrcMarauder;
			wargRB.IsChecked = monster.monsterType == MonsterType.Warg;
			trollRB.IsChecked = monster.monsterType == MonsterType.HillTroll;
			wightRB.IsChecked = monster.monsterType == MonsterType.Wight;
			goblinRB.IsChecked = monster.monsterType == MonsterType.GoblinScout;
		}

		private void OkButton_Click( object sender, RoutedEventArgs e )
		{
			TryClose();
			DialogResult = true;
		}

		private void CancelButton_Click( object sender, RoutedEventArgs e )
		{
			DialogResult = false;
		}

		void TryClose()
		{
			if ( ruffianRB.IsChecked == true )
				monster.monsterType = MonsterType.Ruffian;
			else if ( hunterRB.IsChecked == true )
				monster.monsterType = MonsterType.OrcHunter;
			else if ( orcRB.IsChecked == true )
				monster.monsterType = MonsterType.OrcMarauder;
			else if ( wargRB.IsChecked == true )
				monster.monsterType = MonsterType.Warg;
			else if ( trollRB.IsChecked == true )
				monster.monsterType = MonsterType.HillTroll;
			else if ( wightRB.IsChecked == true )
				monster.monsterType = MonsterType.Wight;
			else if ( goblinRB.IsChecked == true )
				monster.monsterType = MonsterType.GoblinScout;

			if ( mightRB.IsChecked == true )
				monster.negatedBy = Ability.Might;
			else if ( agilityRB.IsChecked == true )
				monster.negatedBy = Ability.Agility;
			else if ( wisdomRB.IsChecked == true )
				monster.negatedBy = Ability.Wisdom;
			else if ( spiritRB.IsChecked == true )
				monster.negatedBy = Ability.Spirit;
			else if ( witRB.IsChecked == true )
				monster.negatedBy = Ability.Wit;

			if ( string.IsNullOrEmpty( nameTB.Text.Trim() ) )
				monster.dataName = Monster.monsterNames[(int)monster.monsterType];
		}

		private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
		{
			TryClose();
		}

		private void useDefaultCB_Click( object sender, RoutedEventArgs e )
		{
			if ( ( (CheckBox)sender ).IsChecked == true )
			{
				stats1.IsEnabled = false;
				stats2.IsEnabled = false;
				if ( ruffianRB.IsChecked == true )
					monster.monsterType = MonsterType.Ruffian;
				else if ( hunterRB.IsChecked == true )
					monster.monsterType = MonsterType.OrcHunter;
				else if ( orcRB.IsChecked == true )
					monster.monsterType = MonsterType.OrcMarauder;
				else if ( wargRB.IsChecked == true )
					monster.monsterType = MonsterType.Warg;
				else if ( trollRB.IsChecked == true )
					monster.monsterType = MonsterType.HillTroll;
				else if ( wightRB.IsChecked == true )
					monster.monsterType = MonsterType.Wight;
				else if ( goblinRB.IsChecked == true )
					monster.monsterType = MonsterType.GoblinScout;
				FillDefaultStats( monster.monsterType.ToString() );
			}
			else
			{
				stats1.IsEnabled = true;
				stats2.IsEnabled = true;
			}
		}

		private void FillDefaultStats( string enemy )
		{
			defaultStats = Utils.defaultStats.Where( x => x.name == enemy ).First();
			special.Text = defaultStats.special;

			if ( monster.defaultStats )
			{
				monster.health = defaultStats.health;
				monster.movementValue = defaultStats.speed == "light" ? Light - 1 : ( defaultStats.speed == "medium" ? Medium - 1 : Heavy - 1 );
				monster.damage = defaultStats.damage == "light" ? Light : ( defaultStats.damage == "medium" ? Medium : Heavy );
				monster.fear = defaultStats.damage == "light" ? Light : ( defaultStats.damage == "medium" ? Medium : Heavy );
				monster.shieldValue = defaultStats.armor;
				monster.sorceryValue = defaultStats.sorcery;
			}
		}

		private void monsterType_Click( object sender, RoutedEventArgs e )
		{
			string enemy = ( (RadioButton)sender ).Content as string;
			enemy = enemy.Replace( " ", "" );
			FillDefaultStats( enemy );
		}
	}
}
