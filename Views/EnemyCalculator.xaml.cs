using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace JiME.Views
{
	/// <summary>
	/// Interaction logic for EnemyCalculator.xaml
	/// </summary>
	public partial class EnemyCalculator : Window, INotifyPropertyChanged
	{
		enum DifficultyMode { Easy, Normal, Hard }
		SimulatorData simulatorData;
		string _scaledPoints;
		int _selectedDifficulty, _selectedPlayers, _modPoints;
		int numSingleGroups, lastEnemyIndex;
		float _leftOvers;
		Random random = new Random();

		public ObservableCollection<MonsterSim> calculatedItems { get; set; }
		public int selectedPlayers
		{
			get => _selectedPlayers;
			set
			{
				_selectedPlayers = value;
				PropChanged( "selectedPlayers" );
			}
		}
		public float leftOvers
		{
			get => _leftOvers;
			set
			{
				_leftOvers = value;
				PropChanged( "leftOvers" );
			}
		}
		public int modPoints
		{
			get => _modPoints;
			set
			{
				_modPoints = value;
				PropChanged( "modPoints" );
			}
		}
		public int selectedDifficulty
		{
			get => _selectedDifficulty;
			set
			{
				_selectedDifficulty = value;
				PropChanged( "selectedDifficulty" );
			}
		}
		public string scaledPoints
		{
			get => _scaledPoints;
			set
			{
				_scaledPoints = value;
				PropChanged( "scaledPoints" );
			}
		}

		int[] MonsterCost = new int[7] { 3, 3, 6, 6, 7, 9, 7 };
		int[] ModCost = new int[3] { 1, 2, 1 };
		string[] modNames = new string[3] { "Large", "Bloodthirsty", "Armored" };

		public event PropertyChangedEventHandler PropertyChanged;

		public EnemyCalculator( SimulatorData simData )
		{
			InitializeComponent();

			calculatedItems = new ObservableCollection<MonsterSim>();
			DataContext = this;


			simulatorData = simData;
			selectedDifficulty = 1;
			selectedPlayers = 0;
			basePoints.Text = "Base Pool Points: " + simData.poolPoints;
			bias.Text = "Difficulty Bias: " + simData.difficultyBias.ToString();
		}

		void PropChanged( string name )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
		}

		private void okButton_Click( object sender, RoutedEventArgs e )
		{
			DialogResult = true;
		}

		private void rerollBtn_Click( object sender, RoutedEventArgs e )
		{
			calculatedItems.Clear();
			numSingleGroups = 0;
			lastEnemyIndex = -1;//avoid repeat enemy types if possible
			leftOvers = 0;
			modPoints = 0;

			float poolCount = CalculateScaledPoints();
			float starting = poolCount;

			//if no enemies checked, returns 1000
			int lowestCost = LowestRequestedEnemyCost();
			if ( lowestCost == 1000 )
			{
				MessageBox.Show( "There are no Enemies included in the Pool." );
				return;
			}

			if ( poolCount < lowestCost )
			{
				MessageBox.Show( "There aren't enough Pool Points to generate any Enemies given the current parameters." );
				return;
			}

			Debug.Log( "***" );
			//generate all the random monster groups possible
			List<MonsterSim> mList = new List<MonsterSim>();
			while ( poolCount >= lowestCost )//lowest enemy cost
			{
				Tuple<MonsterSim, float> generated = GenerateMonster( poolCount );
				if ( generated.Item1.dataName != "modifier" )
				{
					poolCount = Math.Max( 0f, poolCount - generated.Item2 );
					mList.Add( generated.Item1 );
				}
				else
				{
					//use dummy point
					poolCount = Math.Max( 0f, poolCount - 1f );
				}
			}

			//recalculate points left over
			poolCount = starting;
			foreach ( MonsterSim sim in mList )
				poolCount -= sim.cost;

			//if enough points left for more enemies, add them
			if ( poolCount > lowestCost )
			{
				foreach ( MonsterSim sim in mList )
				{
					//% chance to add another
					if ( random.Next( 100 ) < 50 && sim.count < 2 && sim.singlecost <= poolCount )
					{
						poolCount = Math.Max( 0f, poolCount - sim.singlecost );
						sim.count++;
						sim.cost += sim.singlecost;
					}
				}
			}

			//add modifiers with any leftover points
			if ( mList.Count > 0 && poolCount > 0 )
			{
				foreach ( MonsterSim sim in mList )
				{
					int mod = random.Next( 4 );//3=none
					if ( mod != 3 && ModCost[mod] <= poolCount )
					{
						poolCount -= ModCost[mod];
						sim.modList.Add( modNames[mod] );
						modPoints += ModCost[mod];
						Debug.Log( "mod added: " + modNames[mod] );
					}
				}
			}

			//round it to 2 decimal places
			leftOvers = (float)Math.Round( poolCount * 100f ) / 100f;

			//finally add finished groups to collection
			foreach ( MonsterSim ms in mList )
				calculatedItems.Add( ms );
		}

		/// <summary>
		/// returns the lowest enemy cost in includedEnemies
		/// </summary>
		private int LowestRequestedEnemyCost()
		{
			int cost = 1000;

			for ( int i = 0; i < simulatorData.includedEnemies.Length; i++ )
			{
				if ( simulatorData.includedEnemies[i] )
				{
					if ( MonsterCost[i] < cost )
					{
						cost = MonsterCost[i];
					}
				}
			}

			return cost;
		}

		/// <summary>
		/// returns monster type and group cost
		/// </summary>
		Tuple<MonsterSim, float> GenerateMonster( float points )
		{
			//monster type/cost
			List<Tuple<MonsterType, float>> mList = new List<Tuple<MonsterType, float>>();
			//create list of enemy candidates
			for ( int i = 0; i < simulatorData.includedEnemies.Length; i++ )
			{
				//skip using enemy if it was already used last iteration
				if ( simulatorData.includedEnemies.Count( x => x ) > 1 && lastEnemyIndex == i )
					continue;

				//includedEnemies lines up with MonsterType enum and MonsterCost array
				if ( simulatorData.includedEnemies[i] && points >= MonsterCost[i] )
				{
					mList.Add( new Tuple<MonsterType, float>( (MonsterType)i, MonsterCost[i] ) );
				}
			}

			//how many
			if ( mList.Count > 0 )//sanity check
			{
				//pick 1 at random
				int pick = random.Next( 0, mList.Count );

				MonsterSim ms = MonsterFactory( mList[pick].Item1 );
				//.6 of cost per count above 1
				float groupcost = mList[pick].Item2;
				int upTo = groupcost <= points ? 1 : 0;
				for ( int i = 0; i < 2; i++ )
				{
					if ( ( groupcost + .6f * mList[pick].Item2 ) <= points )
					{
						upTo += 1;
						groupcost += .6f * mList[pick].Item2;
					}
				}

				int count = random.Next( 1, upTo + 1 );
				//avoid a bunch of 1 enemy groups
				if ( count == 1 && numSingleGroups >= 1 )
				{
					if ( count + 1 <= upTo )//if room to add 1 more...
					{
						//50% chance add another or use the points for modifiers
						if ( random.Next( 100 ) > 50 || modPoints > 3 )
							count += 1;
						else
						{
							MonsterSim skip = new MonsterSim() { dataName = "modifier", cost = 1 };
							return new Tuple<MonsterSim, float>( skip, 0 );
						}
					}
					else//no more room, 30% to add a modifier point instead
					{
						MonsterSim skip = new MonsterSim() { dataName = "modifier", cost = 0 };
						return new Tuple<MonsterSim, float>( skip, random.Next( 100 ) > 30 ? 1 : 0 );
					}
				}

				if ( count == 1 )
					numSingleGroups++;

				groupcost = mList[pick].Item2 + ( ( count - 1 ) * ( .6f * mList[pick].Item2 ) );
				lastEnemyIndex = (int)mList[pick].Item1;
				ms.count = count;
				ms.singlecost = mList[pick].Item2;
				ms.cost = groupcost;
				return new Tuple<MonsterSim, float>( ms, mList[pick].Item2 * count );
			}
			else
			{
				MonsterSim skip = new MonsterSim() { dataName = "modifier", cost = 1 };
				return new Tuple<MonsterSim, float>( skip, 1 );
			}
		}

		private float CalculateScaledPoints()
		{
			float difficultyScale = 0;
			int bias = 0;

			//set the base pool
			float poolCount = simulatorData.poolPoints;

			//set the difficulty bias
			if ( simulatorData.difficultyBias == DifficultyBias.Light )
				bias = 3;
			else if ( simulatorData.difficultyBias == DifficultyBias.Medium )
				bias = 5;
			else if ( simulatorData.difficultyBias == DifficultyBias.Heavy )
				bias = 7;

			//set the difficulty scale
			if ( selectedDifficulty == 0 )//easy
				difficultyScale = -.25f;
			else if ( selectedDifficulty == 2 )//hard
				difficultyScale = .5f;

			//modify pool based on hero count above 1 and bias
			poolCount += ( selectedPlayers ) * bias;

			//modify pool based on difficulty scale
			poolCount += poolCount * difficultyScale;

			scaledPoints = poolCount.ToString();
			leftOvers = modPoints = 0;

			return poolCount;
		}

		private void playerCB_SelectionChanged( object sender, System.Windows.Controls.SelectionChangedEventArgs e )
		{
			calculatedItems.Clear();
			CalculateScaledPoints();
		}

		private void diffCB_SelectionChanged( object sender, System.Windows.Controls.SelectionChangedEventArgs e )
		{
			calculatedItems.Clear();
			CalculateScaledPoints();
		}

		public static MonsterSim MonsterFactory( MonsterType mType )
		{
			//light=2, medium=3, heavy=4
			int mHealth = 0, /*mDamage = 0, mSpeed = 0,*/ armor = 0, sorc = 0;

			switch ( mType )
			{
				case MonsterType.GoblinScout:
					mHealth = 3;
					//mDamage = 2;
					//mSpeed = 2;
					armor = 1;
					break;
				case MonsterType.Ruffian:
					mHealth = 5;
					//mDamage = 2;
					//mSpeed = 2;
					armor = 0;
					break;
				case MonsterType.OrcHunter:
					mHealth = 5;
					//mDamage = 3;
					//mSpeed = 1;
					armor = 1;
					break;
				case MonsterType.OrcMarauder:
					mHealth = 5;
					//mDamage = 3;
					//mSpeed = 1;
					armor = 2;
					break;
				case MonsterType.Warg:
					mHealth = 8;
					//mDamage = 3;
					//mSpeed = 3;
					armor = 1;
					break;
				case MonsterType.HillTroll:
					mHealth = 14;
					//mDamage = 4;
					//mSpeed = 1;
					armor = 2;
					break;
				case MonsterType.Wight:
					mHealth = 6;
					//mDamage = 4;
					//mSpeed = 1;
					sorc = 3;
					break;
			}

			return new MonsterSim()
			{
				dataName = mType.ToString(),
				health = mHealth,
				//damage = mDamage,
				shieldValue = armor,
				sorceryValue = sorc,
			};
		}
	}
}
