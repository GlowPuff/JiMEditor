using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace JiME
{
	/// <summary>
	/// JSON serialization/deserialization
	/// </summary>
	public class FileManager
	{
		/// <summary>
		/// This number is updated every time the file format changes with new features
		/// </summary>
		public Guid scenarioGUID { get; set; }
		public Guid campaignGUID { get; set; }
		public string specialInstructions { get; set; }
		public string fileVersion { get; set; }
		public string fileName { get; set; }
		public string saveDate { get; set; }
		public ProjectType projectType { get; set; }
		public string scenarioName { get; set; }
		public string objectiveName { get; set; }
		public int threatMax { get; set; }
		public bool threatNotUsed { get; set; }
		public bool scenarioTypeJourney { get; set; }
		public int loreReward { get; set; }
		public int loreStartValue { get; set; }
		public int xpReward { get; set; }
		public int shadowFear { get; set; }
		public bool useTileGraphics { get; set; }

		[JsonConverter( typeof( InteractionConverter ) )]
		public List<IInteraction> interactions { get; set; }
		public List<Trigger> triggers { get; set; }
		public List<Objective> objectives { get; set; }
		public List<TextBookData> resolutions { get; set; }
		public List<Threat> threats { get; set; }
		public List<Chapter> chapters { get; set; }
		public List<int> globalTiles { get; set; }
		public Dictionary<string, bool> scenarioEndStatus { get; set; } = new Dictionary<string, bool>();
		public TextBookData introBookData { get; set; }

		public FileManager()
		{

		}

		public FileManager( Scenario source )
		{
			scenarioGUID = source.scenarioGUID;
			campaignGUID = source.campaignGUID;
			specialInstructions = source.specialInstructions;
			fileName = source.fileName;
			fileVersion = Utils.formatVersion;
			saveDate = source.saveDate;
			loreReward = source.loreReward;
			loreStartValue = source.loreStartValue;
			xpReward = source.xpReward;
			shadowFear = source.shadowFear;
			useTileGraphics = source.useTileGraphics;

			interactions = source.interactionObserver.ToList();
			triggers = source.triggersObserver.Where( x => !x.isCampaignTrigger ).ToList();//source.triggersObserver.ToList();
			objectives = source.objectiveObserver.ToList();
			resolutions = source.resolutionObserver.ToList();
			threats = source.threatObserver.ToList();
			chapters = source.chapterObserver.ToList();
			globalTiles = source.globalTilePool.ToList();
			scenarioEndStatus = source.scenarioEndStatus;

			introBookData = source.introBookData;
			projectType = source.projectType;
			scenarioName = source.scenarioName;
			objectiveName = source.objectiveName;
			threatMax = source.threatMax;
			threatNotUsed = source.threatNotUsed;
			scenarioTypeJourney = source.scenarioTypeJourney;
		}

		/// <summary>
		/// saves Scenario. Detects if scenario is part of campaign, saves to proper folder
		/// </summary>
		public bool Save()
		{
			if ( campaignGUID == Guid.Empty )
				return Save( false, Path.Combine( Environment.ExpandEnvironmentVariables( "%userprofile%" ), "Documents", "Your Journey" ) );
			else
				return Save( false, Path.Combine( Environment.ExpandEnvironmentVariables( "%userprofile%" ), "Documents", "Your Journey", campaignGUID.ToString() ) );
		}

		/// <summary>
		/// saves a NEW standalone scenario to base project folder
		/// </summary>
		public bool SaveAs()
		{
			return Save( true, Path.Combine( Environment.ExpandEnvironmentVariables( "%userprofile%" ), "Documents", "Your Journey" ) );
		}

		/// <summary>
		/// saves a NEW scenario to campaign folder
		/// </summary>
		public bool SaveAs( string campaignFolder )
		{
			return Save( true, campaignFolder );
		}

		/// <summary>
		/// outFolder is the full path, excluding filename. Creates folder if it doesn't exist
		/// </summary>
		private bool Save( bool saveAs, string outFolder )
		{
			string basePath = outFolder;//Path.Combine( Environment.ExpandEnvironmentVariables( "%userprofile%" ), "Documents", "Your Journey" );

			if ( saveAs || string.IsNullOrEmpty( fileName ) )
			{
				if ( !Directory.Exists( basePath ) )
				{
					var di = Directory.CreateDirectory( basePath );
					if ( di == null )
					{
						MessageBox.Show( "Could not create the Scenario project folder.\r\nTried to create: " + basePath, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
						return false;
					}
				}

				SaveFileDialog saveFileDialog = new SaveFileDialog();
				saveFileDialog.DefaultExt = ".jime";
				saveFileDialog.Title = saveAs ? "Save Project As" : "Save Project";
				saveFileDialog.Filter = "Journey File (*.jime)|*.jime";
				saveFileDialog.InitialDirectory = basePath;
				if ( saveFileDialog.ShowDialog() == true )
					fileName = saveFileDialog.FileName;
				else
					return false;
			}

			//just use the filename, not the whole path
			FileInfo fi = new FileInfo( fileName );
			fileName = fi.Name;
			saveDate = DateTime.Now.ToString( "M/d/yyyy" );

			string output = JsonConvert.SerializeObject( this, Formatting.Indented );
			string outpath = Path.Combine( basePath, fileName );
			//Debug.Log( outpath );
			try
			{
				using ( var stream = File.CreateText( outpath ) )
				{
					stream.Write( output );
				}
			}
			catch ( Exception e )
			{
				MessageBox.Show( "Could not save the project file.\r\n\r\nException:\r\n" + e.Message, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
				return false;
			}
			return true;
		}

		/// <summary>
		/// Supply the FULL PATH with the filename
		/// </summary>
		static Scenario Load( string filename )
		{
			string json = "";
			try
			{
				using ( StreamReader sr = new StreamReader( filename ) )
				{
					json = sr.ReadToEnd();
				}

				var fm = JsonConvert.DeserializeObject<FileManager>( json );
				fm.fileName = new FileInfo( filename ).Name;
				return Scenario.CreateInstance( fm );
			}
			catch ( Exception e )
			{
				MessageBox.Show( "Could not load the project file.\r\n\r\nException:\r\n" + e.Message, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
				return null;
			}
		}

		/// <summary>
		/// Supply the filename ONLY, not the full path
		/// </summary>
		public static Scenario LoadProject( string filename )
		{
			string basePath = Path.Combine( Environment.ExpandEnvironmentVariables( "%userprofile%" ), "Documents", "Your Journey" );

			//make sure the project folder exists
			if ( !Directory.Exists( basePath ) )
			{
				MessageBox.Show( "Could not find the scenario project folder.\r\nTried to find: " + basePath, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
				return null;
			}

			return Load( Path.Combine( basePath, filename ) );
		}

		/// <summary>
		/// supply the campaign base path and scenario filename
		/// </summary>
		public static Scenario LoadProjectFromPath( string basePath, string filename )
		{
			//make sure the folder exists
			if ( !Directory.Exists( basePath ) )
			{
				MessageBox.Show( "Could not find the campaign project folder.\r\nTried to find: " + basePath, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
				return null;
			}

			return Load( Path.Combine( basePath, filename ) );
		}

		/// <summary>
		/// Return ProjectItem info for scenarios and campaigns in Project folder
		/// </summary>
		public static IEnumerable<ProjectItem> GetProjects()
		{
			string basePath = Path.Combine( Environment.ExpandEnvironmentVariables( "%userprofile%" ), "Documents", "Your Journey" );

			//make sure the project folder exists
			if ( !Directory.Exists( basePath ) )
			{
				var dinfo = Directory.CreateDirectory( basePath );
				if ( dinfo == null )
				{
					MessageBox.Show( "Could not create the scenario project folder.\r\nTried to create: " + basePath, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
					return null;
				}
			}

			List<ProjectItem> items = new List<ProjectItem>();
			DirectoryInfo di = new DirectoryInfo( basePath );
			FileInfo[] files = di.GetFiles().Where( file => file.Extension == ".jime" ).ToArray();
			//find campaigns
			foreach ( DirectoryInfo dInfo in di.GetDirectories() )
			{
				Campaign c = LoadCampaign( dInfo.Name );
				if ( c != null )
				{
					FileInfo fi = new FileInfo( Path.Combine( basePath, dInfo.Name, dInfo.Name + ".json" ) );
					ProjectItem pi = new ProjectItem();
					pi.projectType = ProjectType.Campaign;
					pi.Date = fi.LastWriteTime.ToString( "M/d/yyyy" );
					pi.Title = c.campaignName;
					pi.fileName = dInfo.Name;
					pi.fileVersion = c.fileVersion;
					items.Add( pi );
				}
			}

			//find scenario files
			foreach ( FileInfo fi in files )
			{
				//Debug.Log( fi.FullName );
				Scenario s = Load( fi.FullName );
				if ( s != null )
					items.Add( new ProjectItem() { Title = s.scenarioName, projectType = s.projectType, Date = s.saveDate, fileName = fi.Name, fileVersion = s.fileVersion } );
			}
			return items;
		}

		public static Campaign LoadCampaign( string campaignGUID )
		{
			if ( campaignGUID == "Saves" )
				return null;

			string basePath = Path.Combine( Environment.ExpandEnvironmentVariables( "%userprofile%" ), "Documents", "Your Journey", campaignGUID );
			string json = "";
			try
			{
				using ( StreamReader sr = new StreamReader( Path.Combine( basePath, campaignGUID + ".json" ) ) )
				{
					json = sr.ReadToEnd();
				}

				var c = JsonConvert.DeserializeObject<Campaign>( json, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Populate } );
				return c;
			}
			catch ( Exception e )
			{
				MessageBox.Show( "Could not load the Campaign data.\r\n\r\nException:\r\n" + e.Message, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
				return null;
			}
		}
	}
}
