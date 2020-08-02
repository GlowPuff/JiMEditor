using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace JiME.Views
{
	/// <summary>
	/// Interaction logic for TilePoolEditorWindow.xaml
	/// </summary>
	public partial class TilePoolEditorWindow : Window
	{
		public Scenario scenario { get; set; }
		public Chapter chapter { get; set; }
		public ObservableCollection<string> randomInteractions { get; set; }
		bool closing = false;
		int numinters = 0, requestedInters = 0;

		public TilePoolEditorWindow( Scenario s, Chapter c )
		{
			InitializeComponent();
			DataContext = this;

			scenario = s;
			chapter = c;

			//fill interaction group combobox
			var ri = from inter in scenario.interactionObserver where inter.isTokenInteraction select inter.dataName;
			HashSet<string> hash = new HashSet<string>( new string[] { "None" } );
			Regex rx = new Regex( @"\sGRP\d+$" );
			foreach ( string item in ri )
			{
				MatchCollection matches = rx.Matches( item );
				if ( matches.Count > 0 )
					hash.Add( matches[0].Value.Trim() );
			}
			randomInteractions = new ObservableCollection<string>( hash );
			randInter.SelectedItem = chapter.randomInteractionGroup;

			UpdateTexts();
		}

		private void ToRandomButton_Click( object sender, RoutedEventArgs e )
		{
			int item = (int)global.SelectedItem;
			if ( chapter.randomTilePool.Count < 4 )
			{
				scenario.globalTilePool.Remove( item );
				chapter.randomTilePool.Add( item );
				global.UnselectAll();
				//random.UnselectAll();
			}

			UpdateTexts();
		}

		private void ToPoolButton_Click( object sender, RoutedEventArgs e )
		{
			int item = (int)random.SelectedItem;
			if ( item > 0 )
			{
				TileSorter sorter = new TileSorter();
				chapter.randomTilePool.Remove( item );
				scenario.globalTilePool.Add( item );
				List<int> foo = scenario.globalTilePool.ToList();
				foo.Sort( sorter );
				scenario.globalTilePool.Clear();
				foreach ( int s in foo )
					scenario.globalTilePool.Add( s );
				//global.UnselectAll();
				random.UnselectAll();
			}

			UpdateTexts();
		}

		bool TryClose()
		{
			if ( chapter.randomTilePool.Count >= 1 )
				return true;
			else
			{
				MessageBox.Show( "There must be at least 1 Tile in the Random Tile Pool.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error );
				return false;
			}
		}

		private void OkButton_Click( object sender, RoutedEventArgs e )
		{
			if ( TryClose() )
			{
				closing = true;
				DialogResult = true;
			}
		}

		private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
		{
			chapter.randomInteractionGroup = randInter.Text;
			if ( !closing && !TryClose() )
				e.Cancel = true;
		}

		private void Global_MouseDoubleClick( object sender, System.Windows.Input.MouseButtonEventArgs e )
		{
			int item = (int)global.SelectedItem;
			if ( chapter.randomTilePool.Count < 4 )
			{
				scenario.globalTilePool.Remove( item );
				chapter.randomTilePool.Add( item );
				global.UnselectAll();
			}

			UpdateTexts();
		}

		private void Random_MouseDoubleClick( object sender, System.Windows.Input.MouseButtonEventArgs e )
		{
			int item = (int)random.SelectedItem;
			if ( item > 0 )
			{
				TileSorter sorter = new TileSorter();
				chapter.randomTilePool.Remove( item );
				scenario.globalTilePool.Add( item );
				List<int> foo = scenario.globalTilePool.ToList();
				foo.Sort( sorter );
				scenario.globalTilePool.Clear();
				foreach ( int s in foo )
					scenario.globalTilePool.Add( s );
				random.UnselectAll();
			}

			UpdateTexts();
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

		void UpdateTexts()
		{
			selectedInfoText.Text = $"There are {numinters} Token Interactions in this group.";

			int numspaces = chapter.randomTilePool.Aggregate( 0, ( acc, cur ) =>
			{
				return acc + ( cur / 100 ) % 10;
			} );
			spaceInfoText2.Text = $"There are {numspaces} spaces available in this Random Tile Pool to place Tokens.";

			int max = Math.Min( numinters, numspaces );
			numIntersUsedText.Text = $"Randomly use how many of the Interactions from the selected Interaction Group, up to a maximum of {max}:";

			max = Math.Min( requestedInters, max );
			numIntersUsed.Text = max.ToString();
		}

		private void groupHelp_Click( object sender, RoutedEventArgs e )
		{
			HelpWindow hw = new HelpWindow( HelpType.Grouping, 1 );
			hw.ShowDialog();
		}

		private void Window_Loaded( object sender, RoutedEventArgs e )
		{
			UpdateTexts();
		}
	}
}
