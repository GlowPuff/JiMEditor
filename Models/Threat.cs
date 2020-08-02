using System;
using System.ComponentModel;

namespace JiME
{
	public class Threat : INotifyPropertyChanged, ICommonData
	{
		int _threshold;
		string _dataName;
		string _triggerName;

		public string dataName
		{
			get { return _dataName; }
			set
			{
				if ( value != _dataName )
				{
					_dataName = value;
					Prop( "dataName" );
				}
			}
		}
		public Guid GUID { get; set; }
		public bool isEmpty { get; set; }
		public int threshold
		{
			get { return _threshold; }
			set
			{
				if ( value != _threshold )
				{
					_threshold = value;
					Prop( "threshold" );
				}
			}
		}
		public string triggerName
		{
			get => _triggerName;
			set
			{
				if ( value != _triggerName )
				{
					_triggerName = value;
					Prop( "triggerName" );
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public Threat( string name = "Default Name", int thresholdValue = 0 )
		{
			GUID = Guid.NewGuid();
			dataName = name;
			threshold = thresholdValue;
			triggerName = "None";
		}

		void Prop( string name )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
		}
	}
}
