using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace JiME
{
	/// <summary>
	/// JSON serialization/deserialization
	/// </summary>
	public class FileManager
	{
		public string fileVersion { get; set; }
		public string fileName { get; set; }
		public string saveDate { get; set; }

		[JsonConverter( typeof( InteractionConverter ) )]
		public List<IInteraction> interactions { get; set; }
		public List<Trigger> triggers { get; set; }
		public List<Objective> objectives { get; set; }
		public List<TextBookData> resolutions { get; set; }
		public List<Threat> threats { get; set; }
		public List<Chapter> chapters { get; set; }
		public List<int> globalTiles { get; set; }
		public TextBookData introBookData { get; set; }
		public ProjectType projectType { get; set; }
		public string scenarioName { get; set; }
		public string objectiveName { get; set; }
		public int threatMax { get; set; }
		public bool threatNotUsed { get; set; }
		public bool scenarioTypeJourney { get; set; }

		public FileManager()
		{

		}

		public FileManager( Scenario source )
		{
			fileName = source.fileName;
			fileVersion = "1.0";
			saveDate = source.saveDate;

			interactions = source.interactionObserver.ToList();
			triggers = source.triggersObserver.ToList();
			objectives = source.objectiveObserver.ToList();
			resolutions = source.resolutionObserver.ToList();
			threats = source.threatObserver.ToList();
			chapters = source.chapterObserver.ToList();
			globalTiles = source.globalTilePool.ToList();

			introBookData = source.introBookData;
			projectType = source.projectType;
			scenarioName = source.scenarioName;
			objectiveName = source.objectiveName;
			threatMax = source.threatMax;
			threatNotUsed = source.threatNotUsed;
			scenarioTypeJourney = source.scenarioTypeJourney;
		}

		public bool Save( bool saveAs = false )
		{
			//string basePath = Path.Combine( System.AppDomain.CurrentDomain.BaseDirectory, "Projects" );
			string basePath = Path.Combine( Environment.ExpandEnvironmentVariables( "%userprofile%" ), "Documents", "Your Journey" );

			if ( saveAs || string.IsNullOrEmpty( fileName ) )
			{
				if ( !Directory.Exists( basePath ) )
					Directory.CreateDirectory( basePath );

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
			using ( var stream = File.CreateText( outpath ) )
			{
				stream.Write( output );
			}
			return true;
		}

		/// <summary>
		/// Supply the FULL PATH with the filename
		/// </summary>
		static Scenario Load( string filename )
		{
			string json = "";
			using ( StreamReader sr = new StreamReader( filename ) )
			{
				json = sr.ReadToEnd();
			}
			//ObservableCollection
			var fm = JsonConvert.DeserializeObject<FileManager>( json );

			return Scenario.CreateInstance( fm );
		}

		/// <summary>
		/// Supply the filename ONLY, not the full path
		/// </summary>
		public static Scenario LoadProject( string filename )
		{
			string basePath = Path.Combine( Environment.ExpandEnvironmentVariables( "%userprofile%" ), "Documents", "Your Journey" );

			//string basePath = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "Projects" );
			return Load( Path.Combine( basePath, filename ) );
		}

		/// <summary>
		/// Return ProjectItem info for files in Project folder
		/// </summary>
		public static IEnumerable<ProjectItem> GetProjects()
		{
			//string basePath = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "Projects" );

			string basePath = Path.Combine( Environment.ExpandEnvironmentVariables( "%userprofile%" ), "Documents", "Your Journey" );
			List<ProjectItem> items = new List<ProjectItem>();
			DirectoryInfo di = new DirectoryInfo( basePath );
			FileInfo[] files = di.GetFiles().Where( file => file.Extension == ".jime" ).ToArray();
			foreach ( FileInfo fi in files )
			{
				//Debug.Log( fi.FullName );
				Scenario s = Load( fi.FullName );
				if ( s != null )
					items.Add( new ProjectItem() { Title = s.scenarioName, projectType = s.projectType, Date = s.saveDate, fileName = s.fileName } );
			}
			return items;
		}
	}
}
