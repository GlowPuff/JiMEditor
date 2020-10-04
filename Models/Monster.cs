using System;
using System.ComponentModel;

namespace JiME
{
	public class Monster : INotifyPropertyChanged, ICommonData
	{
		string _dataName, _bonuses;
		int _count, _health, _damage, _fear, _shieldValue, _sorceryValue, _loreReward, _movementValue, _maxMovementValue;
		bool _isLarge, _isBloodThirsty, _isArmored, _isElite, _defaultStats, _isEasy, _isNormal, _isHard;

		public Guid GUID { get; set; }
		public string dataName
		{
			get => _dataName;
			set
			{
				if ( value != _dataName )
				{
					_dataName = value;
					NotifyPropertyChanged( "dataName" );
				}
			}
		}
		public bool isEmpty { get; set; }
		public string triggerName { get; set; }
		public string bonuses
		{
			get
			{
				string b = string.Empty;
				if ( isLarge )
					b += "Large, ";
				if ( isBloodThirsty )
					b += "BloodThirsty, ";
				if ( isArmored )
					b += "Armored, ";
				if ( string.IsNullOrEmpty( b ) )
					return "No Bonuses";
				else
					return b.Substring( 0, b.Length - 2 );
			}
			set
			{
				if ( value != _bonuses )
				{
					_bonuses = value;
					NotifyPropertyChanged( "bonuses" );
				}
			}
		}
		public int health
		{
			get => _health;
			set
			{
				if ( value != _health )
				{
					_health = value;
					NotifyPropertyChanged( "health" );
				}
			}
		}
		public int shieldValue
		{
			get => _shieldValue;
			set
			{
				if ( value != _shieldValue )
				{
					_shieldValue = value;
					_shieldValue = Math.Min( _shieldValue, _health );
					NotifyPropertyChanged( "shieldValue" );
				}
			}
		}
		public int sorceryValue
		{
			get => _sorceryValue;
			set
			{
				if ( value != _sorceryValue )
				{
					_sorceryValue = value;
					_sorceryValue = Math.Min( _sorceryValue, _health );
					NotifyPropertyChanged( "sorceryValue" );
				}
			}
		}
		public int damage
		{
			get => _damage;
			set
			{
				if ( value != _damage )
				{
					_damage = value;
					NotifyPropertyChanged( "damage" );
				}
			}
		}
		public int fear
		{
			get => _fear;
			set
			{
				if ( value != _fear )
				{
					_fear = value;
					NotifyPropertyChanged( "fear" );
				}
			}
		}
		public bool isLarge
		{
			get => _isLarge;
			set
			{
				_isLarge = value;
				NotifyPropertyChanged( "isLarge" );
				bonuses = bonuses;
				if ( _isArmored || _isBloodThirsty || _isLarge )
					isElite = true;
				else
					isElite = false;
			}
		}
		public bool isBloodThirsty
		{
			get => _isBloodThirsty;
			set
			{
				_isBloodThirsty = value;
				NotifyPropertyChanged( "isBloodThirsty" );
				bonuses = bonuses;
				if ( _isArmored || _isBloodThirsty || _isLarge )
					isElite = true;
				else
					isElite = false;
			}
		}
		public bool isArmored
		{
			get => _isArmored;
			set
			{
				_isArmored = value;
				NotifyPropertyChanged( "isArmored" );
				bonuses = bonuses;
				if ( _isArmored || _isBloodThirsty || _isLarge )
					isElite = true;
				else
					isElite = false;
			}
		}
		public bool isElite
		{
			get => _isElite;
			set
			{
				if ( value != _isElite )
				{
					_isElite = value;
					NotifyPropertyChanged( "isElite" );
				}
			}
		}
		public int maxMovementValue
		{
			get => _maxMovementValue;
			set
			{
				_maxMovementValue = value;
				NotifyPropertyChanged( "maxMovementValue" );
			}
		}
		public int movementValue
		{
			get => _movementValue;
			set
			{
				_movementValue = value;
				NotifyPropertyChanged( "movementValue" );
			}
		}
		public int loreReward
		{
			get => _loreReward;
			set
			{
				_loreReward = value;
				NotifyPropertyChanged( "loreReward" );
			}
		}
		public bool defaultStats
		{
			get { return _defaultStats; }
			set
			{
				_defaultStats = value;
				NotifyPropertyChanged( "defaultStats" );
			}
		}
		public bool isEasy
		{
			get { return _isEasy; }
			set
			{
				_isEasy = value;
				NotifyPropertyChanged( "isEasy" );
			}
		}
		public bool isNormal
		{
			get { return _isNormal; }
			set
			{
				_isNormal = value;
				NotifyPropertyChanged( "isNormal" );
			}
		}
		public bool isHard
		{
			get { return _isHard; }
			set
			{
				_isHard = value;
				NotifyPropertyChanged( "isHard" );
			}
		}

		public string specialAbility { get; set; }
		public Ability negatedBy { get; set; }
		public MonsterType monsterType { get; set; }
		public int count
		{
			get => _count;
			set
			{
				if ( value != _count )
				{
					_count = value;
					NotifyPropertyChanged( "count" );
				}
			}
		}

		public static string[] monsterNames = { "Ruffian", "Goblin Scout", "Orc Hunter", "Orc Marauder", "Warg", "Hill Troll", "Wight" };

		public event PropertyChangedEventHandler PropertyChanged;

		public Monster( string name )
		{
			GUID = Guid.NewGuid();
			dataName = name;
			damage = 1;
			fear = 1;
			health = 5;
			triggerName = "None";
			negatedBy = Ability.Might;
			isLarge = false;
			count = 1;
			isElite = false;
			shieldValue = sorceryValue = 0;
			loreReward = 0;
			movementValue = 2;
			maxMovementValue = 4;
			defaultStats = true;
			specialAbility = "";
			isEasy = isNormal = isHard = true;
		}

		public void NotifyPropertyChanged( string propName )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propName ) );
		}
	}
}
