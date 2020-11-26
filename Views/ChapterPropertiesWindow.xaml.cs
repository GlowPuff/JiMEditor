using System.Windows;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System;
using System.Windows.Controls;

namespace JiME.Views
{
	/// <summary>
	/// Interaction logic for ChapterPropertiesWindow.xaml
	/// </summary>
	public partial class ChapterPropertiesWindow : Window
	{
		public Chapter chapter { get; set; }
		public ObservableCollection<string> randomInteractions { get; set; }
		public Scenario scenario { get; set; }
		bool closing = false/*, currentRandomToggle*/;
		int numinters = 0, requestedInters = 0;

		public ChapterPropertiesWindow( Scenario s, Chapter c = null )
		{
			InitializeComponent();

			SourceInitialized += ( x, y ) =>
			{
				this.HideMinimizeAndMaximizeButtons();
			};

			DataContext = this;

			cancelButton.Visibility = c == null ? Visibility.Visible : Visibility.Collapsed;
			chapter = c ?? new Chapter( "New Block" );
			scenario = s;
			//currentRandomToggle = chapter.isRandomTiles;
			if ( chapter.dataName != "Start" )
			{
				preExCB.Visibility = Visibility.Collapsed;
				preExText.Visibility = Visibility.Collapsed;
			}
			else
			{//disable some settings for Start block
				useRandomCB.IsEnabled = false;
				randomBlock.Visibility = Visibility.Collapsed;
				//hintBlock.Visibility = Visibility.Collapsed;
				dynamicCB.Visibility = Visibility.Collapsed;
				dynText.Visibility = Visibility.Collapsed;
				flavorBox.Visibility = Visibility.Collapsed;
				exploreBox.Visibility = Visibility.Collapsed;
			}

			var ri = from inter in scenario.interactionObserver where inter.isTokenInteraction select inter.dataName;
			HashSet<string> hash = new HashSet<string>( new string[] { "None" } );
			Regex rx = new Regex( @"\sGRP\d+$" );
			foreach ( string item in ri )
			{
				MatchCollection matches = rx.Matches( item );
				if ( matches.Count > 0 )
				{
					//make sure all Events in the group are token interactions
					var isToken = scenario.interactionObserver.Where( x => x.dataName.Contains( matches[0].Value ) ).All( x => x.isTokenInteraction );
					if ( isToken )
						hash.Add( matches[0].Value.Trim() );
				}
			}
			randomInteractions = new ObservableCollection<string>( hash );
			randInter.SelectedItem = chapter.randomInteractionGroup;

			UpdateTexts();
		}

		private void OkButton_Click( object sender, RoutedEventArgs e )
		{
			if ( !TryClose() )
				return;
			closing = true;
			DialogResult = true;
		}

		private void CancelButton_Click( object sender, RoutedEventArgs e )
		{
			//return any tiles back into the global pool
			foreach ( var tile in chapter.tileObserver )
				scenario.globalTilePool.Add( tile.idNumber );
			List<int> sorted = scenario.globalTilePool.OrderBy( key => key ).ToList();
			for ( int i = 0; i < sorted.Count; i++ )
				scenario.globalTilePool[i] = sorted[i];


			closing = true;
			DialogResult = false;
		}

		bool TryClose()
		{
			if ( !useRandomCB.IsChecked.Value )
			{
				chapter.randomInteractionGroup = "None";
				randInter.SelectedItem = "None";
			}

			if ( chapter.dataName != "Start"
				&& ( chapter.triggeredBy == "None" /*|| chapter.triggeredBy == "Trigger Random Event" */) )
			{
				MessageBox.Show( "'Triggered By' cannot be set to None.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error );
				return false;
			}
			else if ( string.IsNullOrEmpty( chapter.dataName.Trim() ) )
			{
				MessageBox.Show( "'Chapter Name' cannot be empty.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error );
				return false;
			}

			chapter.randomInteractionGroup = randInter.SelectedItem as string;

			return true;
		}

		private void EditFlavorButton_Click( object sender, RoutedEventArgs e )
		{
			TextEditorWindow tw = new TextEditorWindow( scenario, EditMode.Flavor, chapter.flavorBookData );
			if ( tw.ShowDialog() == true )
				chapter.flavorBookData.pages = tw.textBookController.pages;
		}

		private void Window_ContentRendered( object sender, System.EventArgs e )
		{
			nameTB.Focus();
			nameTB.SelectAll();
		}

		private void AddExploreTriggerButton_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				chapter.exploreTrigger = tw.triggerName;
			}
		}

		private void AddTriggerByButton_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
			{
				chapter.triggeredBy = tw.triggerName;
			}
		}

		private void TileEditButton_Click( object sender, RoutedEventArgs e )
		{
			if ( chapter.isRandomTiles )
			{
				TilePoolEditorWindow tp = new TilePoolEditorWindow( scenario, chapter );
				tp.ShowDialog();
				UpdateTexts();
			}
			else
			{
				TileEditorWindow tw = new TileEditorWindow( scenario, chapter );
				tw.ShowDialog();
				UpdateTexts();
			}
		}

		private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
		{
			if ( !closing )
				e.Cancel = !TryClose();
		}

		private void groupHelp_Click( object sender, RoutedEventArgs e )
		{
			HelpWindow hw = new HelpWindow( HelpType.Grouping, 1 );
			hw.ShowDialog();
		}

		private void randInter_SelectionChanged( object sender, SelectionChangedEventArgs e )
		{
			//get # of interactions in selected group, then update selectedInfoText
			ComboBox cb = e.Source as ComboBox;
			numinters = scenario.interactionObserver.Count( x => x.dataName != "None" && x.dataName.EndsWith( cb.SelectedItem.ToString() ) );
			UpdateTexts();
		}

		private void numIntersUsed_TextChanged( object sender, TextChangedEventArgs e )
		{
			Int32.TryParse( ( (TextBox)e.Source ).Text, out requestedInters );
			UpdateTexts();
		}

		private void useRandomCB_Click( object sender, RoutedEventArgs e )
		{
			//clear tokenlist if using a random group
			//if ( ( (CheckBox)sender ).IsChecked.Value )
			//{
			//	for ( int i = 0; i < chapter.tileObserver.Count; i++ )
			//	{
			//		( (HexTile)chapter.tileObserver[i] ).tokenList.Clear();
			//	}
			//}
			//else

			//clear random group name if NOT using random groups
			if ( !( (CheckBox)sender ).IsChecked.Value )
			{
				chapter.randomInteractionGroup = "None";
				randInter.SelectedItem = "None";
				chapter.randomInteractionGroupCount = 0;
			}
		}

		private void RandomToggleCB_Click( object sender, RoutedEventArgs e )
		{
			if ( !chapter.isRandomTiles )
			{
				MessageBox.Show( "Switching back to fixed tiles will automatically change any random tiles using a Random Side to Side A.\r\n\r\nReminder: Be sure to use the Tile Editor to properly place your tiles, now that they will be in fixed, user-defined positions.", "Switching to Fixed Tiles", MessageBoxButton.OK, MessageBoxImage.Information );
				foreach ( var tile in chapter.tileObserver )
				{
					if ( ( (HexTile)tile ).tileSide == "Random" )
						( (HexTile)tile ).tileSide = "A";
				}
			}
			//if ( chapter.isRandomTiles != currentRandomToggle )
			//{
			//	var ret = MessageBox.Show( "Are you sure you want to toggle between Random Tiles and Fixed Tiles?\n\nALL TILE DATA IN THIS CHAPTER WILL BE RESET IF YOU SWITCH.", "Switch Between Random and Fixed Tiles", MessageBoxButton.YesNo, MessageBoxImage.Question );
			//	if ( ret == MessageBoxResult.Yes )
			//	{
			//		currentRandomToggle = chapter.isRandomTiles;
			//		if ( chapter.isRandomTiles )
			//		{
			//			foreach ( HexTile tile in chapter.tileObserver )
			//				scenario.globalTilePool.Add( tile.idNumber );
			//			chapter.tileObserver.Clear();
			//		}
			//		else
			//		{
			//			foreach ( int tile in chapter.randomTilePool )
			//				scenario.globalTilePool.Add( tile );
			//			chapter.randomTilePool.Clear();
			//		}

			//		List<int> sorted = scenario.globalTilePool.OrderBy( key => key ).ToList();
			//		for ( int i = 0; i < sorted.Count; i++ )
			//			scenario.globalTilePool[i] = sorted[i];
			//	}
			//	else
			//		chapter.isRandomTiles = currentRandomToggle;
			//}
		}

		void UpdateTexts()
		{

			int fixedTokenCount = 0;
			if ( chapter.tileObserver.Count > 0 )
				fixedTokenCount = chapter.tileObserver.Select( x => (HexTile)x ).Select( x => x.tokenList.Count ).Aggregate( ( acc, cur ) => acc + cur );

			selectedInfoText.Text = $"There are {numinters} Events in the selected Group.";
			fixedCountText.Text = $"There are {fixedTokenCount} fixed Tokens in this Block.";

			int numspaces = chapter.tileObserver.Aggregate( 0, ( acc, cur ) =>
			{
				return acc + ( cur.idNumber / 100 ) % 10;
			} );
			spaceInfoText2.Text = $"There are {numspaces} total spaces available on this Block's tiles to place Tokens.";

			int max = Math.Min( numinters, numspaces - fixedTokenCount );
			numIntersUsedText.Text = $"Randomly use how many of the Events from the selected Interaction Group, up to a maximum of {max}:";

			max = Math.Min( requestedInters, max );
			numIntersUsed.Text = max.ToString();
		}
	}
}
