using System;
using System.ComponentModel;

namespace JiME
{
	public class Trigger : INotifyPropertyChanged, ICommonData
	{
		string _dataName;
		bool _triggerValue;
		public string dataName
		{
			get { return _dataName; }
			set
			{
				if ( _dataName != value )
				{
					_dataName = value;
					NotifyChange( "dataName" );
				}
			}
		}
		public Guid GUID { get; set; }
		public bool isEmpty { get; set; }
		public bool triggerValue
		{
			get { return _triggerValue; }
			set
			{
				if ( _triggerValue != value )
				{
					_triggerValue = value;
					NotifyChange( "triggerValue" );
				}
			}
		}
		public string triggerName { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Trigger values always default to False
		/// </summary>
		public Trigger( string name )
		{
			dataName = name;
			isEmpty = false;
			GUID = Guid.NewGuid();
			triggerValue = false;
		}

		void NotifyChange( string prop )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( prop ) );
		}

		public static Trigger EmptyTrigger()
		{
			return new Trigger( "None" ) { isEmpty = true };
		}

		public static Trigger RandomTrigger()
		{
			return new Trigger( "Trigger Random Event" ) { isEmpty = true };
		}
	}
}
