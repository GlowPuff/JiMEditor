using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace JiME
{
	public class Campaign : INotifyPropertyChanged
	{
		string _campaignName, _fileVersion;

		public Guid campaignGUID;
		public string campaignName
		{
			get => _campaignName;
			set
			{
				_campaignName = value;
				PropChanged( "campaignName" );
			}
		}
		public string fileVersion
		{
			get => _fileVersion;
			set
			{
				_fileVersion = value;
				PropChanged( "fileVersion" );
			}
		}
		public string storyText { get; set; }
		public string description { get; set; }

		public ObservableCollection<CampaignItem> scenarioCollection { get; set; }
		public ObservableCollection<Trigger> triggerCollection { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public Campaign()
		{
			campaignGUID = Guid.NewGuid();
			scenarioCollection = new ObservableCollection<CampaignItem>();
			triggerCollection = new ObservableCollection<Trigger>();
			campaignName = "";
			storyText = "";
			description = "";
			fileVersion = Utils.formatVersion;
		}

		void PropChanged( string name )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
		}
	}
}
