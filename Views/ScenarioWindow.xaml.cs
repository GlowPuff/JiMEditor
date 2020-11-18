using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System;

namespace JiME.Views
{
	/// <summary>
	/// Dummy class used for ComboBox Value binding
	/// </summary>
	public class ThreatItem
	{
		public IInteraction trigger { get; set; }
		public Threat threat { get; set; }
		public ThreatList parent;

		public ThreatItem( ThreatList p, IInteraction trigger, Threat threat )
		{
			parent = p;
			this.trigger = trigger;
			this.threat = threat;
		}
	}
	/// <summary>
	/// Dummy class used for ItemControl DataTemplate binding
	/// </summary>
	public class ThreatList : INotifyPropertyChanged
	{
		//combobox source
		public ObservableCollection<ThreatItem> threatItemCollection { get; set; }
		Threat _theThreat;
		public Threat theThreat
		{
			get => _theThreat;
			set
			{
				_theThreat = value;
				PropChanged( "theThreat" );
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		void PropChanged( string name )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
		}

		ThreatItem _selectedItem;
		public ThreatItem selectedItem
		{
			get => _selectedItem;
			set
			{
				_selectedItem = value;
				PropChanged( "selectedItem" );
			}
		}

		public void ReloadTriggers( IInteraction[] collection )
		{
			//this.collection = null;
			threatItemCollection = new ObservableCollection<ThreatItem>();
			foreach ( var inter in collection )
				threatItemCollection.Add( new ThreatItem( this, inter, theThreat ) );

			if ( threatItemCollection.Count > 0 )
				selectedItem = ( from foo in threatItemCollection where theThreat.triggerName == foo.trigger.dataName select foo ).FirstOrDefault();
			else
				selectedItem = null;

			if ( selectedItem == null && threatItemCollection.Count > 0 )
				selectedItem = ( from foo in threatItemCollection
												 where foo.trigger.dataName == "None"
												 select foo ).First();
		}

		public ThreatList( Threat threat, IInteraction[] collection )
		{
			theThreat = threat;
			ReloadTriggers( collection );
		}
	}

	/// <summary>
	/// Interaction logic for ScenarioWindow.xaml
	/// </summary>
	public partial class ScenarioWindow : Window
	{
		bool closing = false;

		public ObservableCollection<ThreatList> threatCollection { get; set; }

		public string scenarioName { get; set; }
		public Scenario scenario { get; set; }

		bool currentType;


		public ScenarioWindow( Scenario s )
		{
			InitializeComponent();
			scenario = s;
			scenarioName = s.scenarioName;
			currentType = scenario.scenarioTypeJourney;
			threatCollection = new ObservableCollection<ThreatList>();
			DataContext = this;

			List<Threat> sorted = scenario.threatObserver.OrderBy( key => key.threshold ).ToList();
			for ( int i = 0; i < sorted.Count; i++ )
				scenario.threatObserver[i] = sorted[i];

			//fill the dummy collection
			foreach ( Threat t in scenario.threatObserver )
				threatCollection.Add( new ThreatList( t, scenario.interactionObserver.Where( x => !x.isTokenInteraction ).ToArray() ) );

			resolutionCB.ItemsSource = scenario.resolutionObserver;
			threatList.ItemsSource = threatCollection;//bind dummy

			if ( scenario.campaignGUID != Guid.Empty )
			{
				campaignNotice.Visibility = Visibility.Visible;
				campaignGUID.Text = scenario.campaignGUID.ToString();
			}
		}

		void UpdateThreatPanel()
		{
			for ( int i = 0; i < threatCollection.Count; i++ )
			{
				scenario.threatObserver[i] = threatCollection[i].theThreat;
			}

			List<Threat> sorted = scenario.threatObserver.OrderBy( key => key.threshold ).ToList();
			for ( int i = 0; i < sorted.Count; i++ )
				scenario.threatObserver[i] = sorted[i];

			threatCollection?.Clear();
			foreach ( Threat t in scenario.threatObserver )
				threatCollection.Add( new ThreatList( t, scenario.interactionObserver.Where( x => !x.isTokenInteraction ).ToArray() ) );
		}

		private void OkButton_Click( object sender, RoutedEventArgs e )
		{
			if ( !closing && TryClose() )
				DialogResult = closing = true;
		}

		bool TryClose()
		{
			UpdateThreatPanel();

			if ( nameTB.Text.Trim() == string.Empty )
			{
				MessageBox.Show( "The Scenario Title can't be empty.", "Invalid Scenario Title", MessageBoxButton.OK, MessageBoxImage.Error );
				return false;
			}

			return true;
		}

		//private void CancelButton_Click( object sender, RoutedEventArgs e )
		//{
		//	DialogResult = false;
		//}

		private void Window_ContentRendered( object sender, System.EventArgs e )
		{
			nameTB.Focus();
			nameTB.SelectAll();
		}

		private void EditIntroButton_Click( object sender, RoutedEventArgs e )
		{
			TextEditorWindow te = new TextEditorWindow( scenario, EditMode.Intro, scenario.introBookData );
			if ( te.ShowDialog() == true )
			{
				scenario.introBookData.dataName = te.shortName;
				scenario.introBookData.pages = te.textBookController.pages;
			}
		}

		private void ResolutionEditButton_Click( object sender, RoutedEventArgs e )
		{
			if ( resolutionCB.SelectedIndex < 0 )
				return;

			TextEditorWindow te = new TextEditorWindow( scenario, EditMode.Resolution, scenario.resolutionObserver[resolutionCB.SelectedIndex] );
			bool chk = true;//default to true
			if ( scenario.scenarioEndStatus.TryGetValue( scenario.resolutionObserver[resolutionCB.SelectedIndex].dataName, out chk ) )
			{
				te.successCB.IsChecked = chk;
				te.failCB.IsChecked = !chk;
				te.successChecked = chk;
			}
			else
			{
				te.successCB.IsChecked = true;
				te.failCB.IsChecked = false;
				te.successChecked = true;
			}

			if ( te.ShowDialog() == true )
			{
				scenario.resolutionObserver[resolutionCB.SelectedIndex].dataName = te.shortName;
				scenario.resolutionObserver[resolutionCB.SelectedIndex].pages = te.textBookController.pages;
				scenario.resolutionObserver[resolutionCB.SelectedIndex].triggerName = te.triggerLB.Text;
				//if a success/fail bool doesn't exist, add it now
				if ( !scenario.scenarioEndStatus.ContainsKey( te.shortName ) )
					scenario.scenarioEndStatus.Add( te.shortName, te.successChecked );
				//otherwise update value
				else
					scenario.scenarioEndStatus[te.shortName] = te.successChecked;

				scenario.PruneScenarioEnd();
			}
		}

		private void AddResolutionButton_Click( object sender, RoutedEventArgs e )
		{
			TextBookData data = new TextBookData();
			data.pages.Add( "Default Text" );
			data.triggerName = "None";
			TextEditorWindow te = new TextEditorWindow( scenario, EditMode.Resolution, data );
			te.successCB.IsChecked = true;
			te.failCB.IsChecked = false;
			te.successChecked = true;

			if ( te.ShowDialog() == true )
			{
				data.dataName = te.shortName;
				data.pages = te.textBookController.pages;
				data.triggerName = te.triggerLB.Text;
				scenario.AddResolution( data, te.successChecked );
				resolutionCB.SelectedIndex = scenario.resolutionObserver.Count - 1;
			}
		}

		private void RemoveResolutionButton_Click( object sender, RoutedEventArgs e )
		{
			if ( scenario.resolutionObserver.Count > 1 )
				scenario.RemoveData( resolutionCB.SelectedItem as TextBookData );
			if ( scenario.resolutionObserver.Count == 1 )
				resolutionCB.SelectedIndex = 0;
		}

		private void EventInteractionCB_SelectionChanged( object sender, SelectionChangedEventArgs e )
		{
			ThreatItem item = ( (ComboBox)e.OriginalSource ).SelectedValue as ThreatItem;
			if ( item != null )
			{
				item.threat.triggerName = item.trigger.dataName;
				item.parent.theThreat = item.threat;
				//reset vars on the interaction
				if ( scenario.threatObserver.Any( x => x.triggerName == item.trigger.dataName ) )
				{
					item.trigger.triggerName = "None";
					item.trigger.isTokenInteraction = false;
				}
			}
		}

		/*private void AddThreatTrigger_Click( object sender, RoutedEventArgs e )
		{
			TextInteraction DUMMY = new TextInteraction( "foo", false );

			ThreatList tlist = ( (Button)sender ).DataContext as ThreatList;

			//TODO
			//window shown based on POPUP selection

			EventEditorWindow ew = new EventEditorWindow( scenario, null, true );
			if ( ew.ShowDialog() == true )
			{
				scenario.AddInteraction( DUMMY );// ew.interaction );
				tlist.theThreat.triggerName = ew.interaction.dataName;
				ThreatItem ti = new ThreatItem( tlist, DUMMY, tlist.theThreat );
				foreach ( ThreatList tl in threatCollection )
					tl.threatItemCollection.Add( ti );
				tlist.selectedItem = ti;
			}

			//TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			//if ( tw.ShowDialog() == true )
			//{
			//	tlist.theThreat.triggerName = tw.triggerName;
			//	ThreatItem ti = new ThreatItem( tlist, tw.newTrigger, tlist.theThreat );
			//	foreach ( ThreatList tl in threatCollection )
			//		tl.collection.Add( ti );
			//	tlist.selectedItem = ti;
			//}
		}*/

		private void addThreatButton_Click( object sender, RoutedEventArgs e )
		{
			scenario.threatObserver.Add( new Threat() );
			UpdateThreatPanel();
		}

		private void RemoveThreat_Click( object sender, RoutedEventArgs e )
		{
			scenario.RemoveData( ( (ThreatList)( (Button)sender ).DataContext ).theThreat );
			threatCollection.Clear();

			//sort by threshold
			List<Threat> sorted = scenario.threatObserver.OrderBy( key => key.threshold ).ToList();
			for ( int i = 0; i < sorted.Count; i++ )
				scenario.threatObserver[i] = sorted[i];

			//recreate list of events
			foreach ( Threat t in scenario.threatObserver )
				threatCollection.Add( new ThreatList( t, scenario.interactionObserver.Where( x => !x.isTokenInteraction ).ToArray() ) );
		}

		private void RadioType_Checked( object sender, RoutedEventArgs e )
		{
			if ( scenario.scenarioTypeJourney != currentType )
			{
				var ret = MessageBox.Show( "Are you sure you want to change the Scenario Type?\n\nALL CHAPTER AND TILE DATA WILL BE RESET IF YOU SWITCH THE SCENARIO TYPE.\n\nObjectives, Events and Triggers will remain unchanged.", "Change Scenario Type", MessageBoxButton.YesNo, MessageBoxImage.Question );
				if ( ret == MessageBoxResult.Yes )
				{
					currentType = scenario.scenarioTypeJourney;
					if ( scenario.scenarioTypeJourney )
						scenario.chapterObserver[0].ToJourneyTile();
					else
					{
						scenario.chapterObserver[0].ToBattleTile();
						scenario.WipeChapters();
					}
				}
				else
					scenario.scenarioTypeJourney = currentType;
			}
		}

		private void Window_Closing( object sender, CancelEventArgs e )
		{
			if ( !closing && !TryClose() )
				e.Cancel = true;
		}

		private void specialBtn_Click( object sender, RoutedEventArgs e )
		{
			TextBookData tbd = new TextBookData( "Scenario Special Instructions" );
			tbd.pages.Add( scenario.specialInstructions );

			TextEditorWindow te = new TextEditorWindow( scenario, EditMode.Special, tbd );
			if ( te.ShowDialog() == true )
			{
				scenario.specialInstructions = te.textBookController.pages[0];
			}
		}
	}
}
