using System.Windows;

namespace JiME.Views
{
	/// <summary>
	/// Interaction logic for MonsterEditorWindow.xaml
	/// </summary>
	public partial class MonsterEditorWindow : Window
	{
		public Monster monster { get; set; }

		public MonsterEditorWindow( Monster m = null )
		{
			InitializeComponent();
			DataContext = this;

			cancelButton.Visibility = m == null ? Visibility.Visible : Visibility.Collapsed;
			monster = m ?? new Monster( "" );

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
	}
}
