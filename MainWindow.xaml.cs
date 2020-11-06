using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using JiME.Views;

namespace JiME
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public Scenario scenario { get; set; }

		public MainWindow( Guid campaignGUID ) : this( null )
		{
			scenario.campaignGUID = campaignGUID;
		}

		public MainWindow( Scenario s = null )
		{
			InitializeComponent();

			System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
			System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
			System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

			scenario = s ?? new Scenario();
			scenario.TriggerTitleChange( false );
			DataContext = scenario;
			Debug.Log( scenario.scenarioGUID );

			appVersion.Text = Utils.appVersion;
			formatVersion.Text = Utils.formatVersion;

			interactionsUC.onAddEvent += OnAddEvent;
			interactionsUC.onRemoveEvent += OnRemoveEvent;
			triggersUC.onAddEvent += OnAddTrigger;
			triggersUC.onRemoveEvent += OnRemoveTrigger;
			interactionsUC.onSettingsEvent += OnSettingsInteraction;
			triggersUC.onSettingsEvent += OnSettingsTrigger;
			objectivesUC.onAddEvent += OnAddObjective;
			objectivesUC.onRemoveEvent += OnRemoveObjective;
			objectivesUC.onSettingsEvent += OnSettingsObjective;
			//Debug.Log( this.FindResource( "mylist" ).GetType() );

			//setup source of UI lists (scenario has to be created first!)
			interactionsUC.dataListView.ItemsSource = scenario.interactionObserver;
			triggersUC.dataListView.ItemsSource = scenario.triggersObserver;
			objectivesUC.dataListView.ItemsSource = scenario.objectiveObserver;

			//initialize utilities
			Utils.Init();

			//debug
			//debug();
		}

		void debug()
		{
			//scenario.threatObserver.Add( new Threat( "Threat 1", 10 ) { threshold = 10, triggerName = "Threat Trigger" } );
			//scenario.AddInteraction( new Interaction( "Dummy Event", false ) { interactionType = InteractionType.Text, triggerName = "Threat Trigger" } );

			//scenario.AddInteraction( new TextInteraction( "Dummy Text Interaction" ) );
		}

		#region TOOLBAR ACTIONS
		void OnAddEvent( object sender, EventArgs e )
		{
			ContextMenu cm = this.FindResource( "cmButton" ) as ContextMenu;
			cm.PlacementTarget = sender as Button;
			cm.IsOpen = true;
		}

		void OnRemoveEvent( object sender, EventArgs e )
		{
			//TODO check if USED by a THREAT
			int idx = interactionsUC.dataListView.SelectedIndex;
			if ( idx != -1 )
				scenario.RemoveData( interactionsUC.dataListView.SelectedItem );
			interactionsUC.dataListView.SelectedIndex = 0;
		}

		void OnAddTrigger( object sender, EventArgs e )
		{
			AddTrigger();
		}

		void OnRemoveTrigger( object sender, EventArgs e )
		{
			string selected = ( (Trigger)triggersUC.dataListView.SelectedItem ).dataName;

			Tuple<string, string> used = scenario.IsTriggerUsed( selected );

			if ( used != null )
			{
				MessageBox.Show( $"The selected Trigger [{selected}] is being used by [{used.Item2}] called [{used.Item1}].", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error );
				return;
			}

			int idx = triggersUC.dataListView.SelectedIndex;
			if ( idx != -1 )
				scenario.RemoveData( triggersUC.dataListView.SelectedItem );
			triggersUC.dataListView.SelectedIndex = 0;
		}

		void OnAddObjective( object sender, EventArgs e )
		{
			AddObjective();
		}

		void OnRemoveObjective( object sender, EventArgs e )
		{
			if ( scenario.objectiveObserver.Count > 1 )
			{
				int idx = objectivesUC.dataListView.SelectedIndex;
				if ( idx != -1 )
					scenario.RemoveData( objectivesUC.dataListView.Items[idx] );
				objectivesUC.dataListView.SelectedIndex = 0;
			}
			else
				MessageBox.Show( "There must be at least one Objective.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error );
		}

		void OnSettingsInteraction( object sender, EventArgs e )
		{
			if ( interactionsUC.dataListView.SelectedItem is TextInteraction )
			{
				TextInteractionWindow tw = new TextInteractionWindow( scenario, (TextInteraction)interactionsUC.dataListView.SelectedItem );
				tw.ShowDialog();
			}
			else if ( interactionsUC.dataListView.SelectedItem is BranchInteraction )
			{
				BranchInteractionWindow bw = new BranchInteractionWindow( scenario, (BranchInteraction)interactionsUC.dataListView.SelectedItem );
				bw.ShowDialog();
			}
			else if ( interactionsUC.dataListView.SelectedItem is TestInteraction )
			{
				TestInteractionWindow bw = new TestInteractionWindow( scenario, (TestInteraction)interactionsUC.dataListView.SelectedItem );
				bw.ShowDialog();
			}
			else if ( interactionsUC.dataListView.SelectedItem is DecisionInteraction )
			{
				DecisionInteractionWindow bw = new DecisionInteractionWindow( scenario, (DecisionInteraction)interactionsUC.dataListView.SelectedItem );
				bw.ShowDialog();
			}
			else if ( interactionsUC.dataListView.SelectedItem is ThreatInteraction )
			{
				ThreatInteractionWindow bw = new ThreatInteractionWindow( scenario, (ThreatInteraction)interactionsUC.dataListView.SelectedItem );
				bw.ShowDialog();
			}
			else if ( interactionsUC.dataListView.SelectedItem is MultiEventInteraction )
			{
				MultiEventWindow bw = new MultiEventWindow( scenario, (MultiEventInteraction)interactionsUC.dataListView.SelectedItem );
				bw.ShowDialog();
			}
			else if ( interactionsUC.dataListView.SelectedItem is PersistentTokenInteraction )
			{
				PersistentInteractionWindow bw = new PersistentInteractionWindow( scenario, (PersistentTokenInteraction)interactionsUC.dataListView.SelectedItem );
				bw.ShowDialog();
			}
			else if ( interactionsUC.dataListView.SelectedItem is ConditionalInteraction )
			{
				ConditionalInteractionWindow bw = new ConditionalInteractionWindow( scenario, (ConditionalInteraction)interactionsUC.dataListView.SelectedItem );
				bw.ShowDialog();
			}
			else if ( interactionsUC.dataListView.SelectedItem is DialogInteraction )
			{
				DialogInteractionWindow bw = new DialogInteractionWindow( scenario, (DialogInteraction)interactionsUC.dataListView.SelectedItem );
				bw.ShowDialog();
			}
			else if ( interactionsUC.dataListView.SelectedItem is ReplaceTokenInteraction )
			{
				ReplaceTokenInteractionWindow bw = new ReplaceTokenInteractionWindow( scenario, (ReplaceTokenInteraction)interactionsUC.dataListView.SelectedItem );
				bw.ShowDialog();
			}
		}

		void OnSettingsTrigger( object sender, EventArgs e )
		{
			string selected = ( (Trigger)triggersUC.dataListView.SelectedItem ).dataName;
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario, selected );
			tw.ShowDialog();
		}

		void OnSettingsObjective( object sender, EventArgs e )
		{
			ObjectiveEditorWindow ow = new ObjectiveEditorWindow( scenario, ( (Objective)objectivesUC.dataListView.SelectedItem ), false );
			ow.ShowDialog();
		}
		#endregion

		private void scenarioName_PreviewMouseDown( object sender, System.Windows.Input.MouseButtonEventArgs e )
		{
			if ( e.ButtonState == System.Windows.Input.MouseButtonState.Pressed )
			{
				if ( scenarioName.Visibility == Visibility.Visible )
				{
					scenarioName.Visibility = Visibility.Collapsed;
					scenarioNameEdit.Visibility = Visibility.Visible;
					scenarioNameEdit.Text = scenarioName.Text;
					scenarioNameEdit.Focus();
					scenarioNameEdit.SelectAll();
				}
				else
				{
					scenarioName.Visibility = Visibility.Visible;
					scenarioNameEdit.Visibility = Visibility.Collapsed;
				}
			}
		}

		private void ScenarioNameEdit_PreviewKeyDown( object sender, System.Windows.Input.KeyEventArgs e )
		{
			if ( e.Key == System.Windows.Input.Key.Enter )
			{
				onScenarioNameFocusLost();
			}
		}

		private void scenarioNameEdit_LostFocus( object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e )
		{
			onScenarioNameFocusLost();
		}

		void onScenarioNameFocusLost()
		{
			if ( scenarioNameEdit.Text.Trim() != string.Empty )
			{
				scenario.scenarioName = scenarioNameEdit.Text;
			}
			else
				MessageBox.Show( "The Scenario name cannot be an empty string.", "Invalid Scenario Name", MessageBoxButton.OK, MessageBoxImage.Information );
			scenarioName.Visibility = Visibility.Visible;
			scenarioNameEdit.Visibility = Visibility.Collapsed;
		}

		private void ScenarioSettingsButton_Click( object sender, RoutedEventArgs e )
		{
			ScenarioWindow sw = new ScenarioWindow( scenario );
			sw.Owner = this;
			if ( sw.ShowDialog() == true )
			{
				scenario.scenarioName = sw.scenarioName;
			}
		}

		private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
		{
			if ( scenario.isDirty )
			{
				if ( MessageBox.Show( "The Project has changes that haven't been saved.  Are you sure you want to exit without saving?", "Project Changes Not Saved", MessageBoxButton.YesNo, MessageBoxImage.Question ) == MessageBoxResult.No )
					e.Cancel = true;
			}
		}

		#region COMMANDS
		void AddObjective()
		{
			ObjectiveEditorWindow ow = new ObjectiveEditorWindow( scenario, new Objective( "Default Short Name - Change This" ) );
			if ( ow.ShowDialog() == true )
			{
				scenario.AddObjective( ow.objective );
			}
		}

		void AddTrigger()
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			tw.ShowDialog();
		}

		private void CommandExit_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			if ( scenario.isDirty )
			{
				if ( MessageBox.Show( "The Project has changes that haven't been saved.  Are you sure you want to exit without saving?", "Project Changes Not Saved", MessageBoxButton.YesNo, MessageBoxImage.Question ) == MessageBoxResult.No )
					return;
			}
			Application.Current.Shutdown();
		}

		private void CommandExit_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}

		private void CommandNewObjective_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			AddObjective();
		}

		private void CommandNewObjective_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}

		private void CommandNewTrigger_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			AddTrigger();
		}

		private void CommandNewTrigger_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}

		private void CommandNewEvent_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			ContextMenu cm = this.FindResource( "cmButton" ) as ContextMenu;
			cm.PlacementTarget = sender as Window;
			cm.IsOpen = true;
		}

		private void CommandNewEvent_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}

		private void CommandNewProject_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			if ( MessageBox.Show( "Are you sure you want to close this Project and start a new one?", "New Project", MessageBoxButton.YesNo, MessageBoxImage.Question ) == MessageBoxResult.Yes )
			{
				ProjectWindow mainWindow = new ProjectWindow();
				mainWindow.Show();
				Close();
			}
		}

		private void CommandNewProject_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}

		private void CommandOpenProject_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			if ( MessageBox.Show( "Are you sure you want to close this Project and open a different one?", "Open Project", MessageBoxButton.YesNo, MessageBoxImage.Question ) == MessageBoxResult.Yes )
			{
				ProjectWindow mainWindow = new ProjectWindow();
				mainWindow.Show();
				Close();
			}
		}

		private void CommandOpenProject_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}

		private void CommandSaveProject_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			FileManager fm = new FileManager( scenario );
			if ( fm.Save() )
			{
				scenario.fileName = fm.fileName;
				scenario.saveDate = fm.saveDate;
				scenario.TriggerTitleChange();
			}
		}

		private void CommandSaveProject_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}

		private void CommandScenarioSettings_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			ScenarioWindow sw = new ScenarioWindow( scenario );
			sw.Owner = this;
			if ( sw.ShowDialog() == true )
			{
				scenario.scenarioName = sw.scenarioName;
			}
		}

		private void CommandScenarioSettings_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}

		private void CommandSaveProjectAs_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			FileManager fm = new FileManager( scenario );
			if ( fm.SaveAs() )
			{
				scenario.fileName = fm.fileName;
				scenario.saveDate = fm.saveDate;
				scenario.TriggerTitleChange();
			}
		}

		private void CommandSaveProjectAs_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}

		private void CommandNewChapter_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			ChapterPropertiesWindow cw = new ChapterPropertiesWindow( scenario );
			if ( cw.ShowDialog() == true )
			{
				scenario.AddChapter( cw.chapter );
			}
		}

		private void CommandNewChapter_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}

		//Interaction popup commands
		private void CommandNewTextInteraction_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			TextInteractionWindow ew = new TextInteractionWindow( scenario );
			if ( ew.ShowDialog() == true )
			{
				scenario.AddInteraction( ew.interaction );
			}
		}
		private void CommandNewTextInteraction_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}
		private void CommandNewBranchInteraction_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			BranchInteractionWindow ew = new BranchInteractionWindow( scenario );
			if ( ew.ShowDialog() == true )
			{
				scenario.AddInteraction( ew.interaction );
			}
		}
		private void CommandNewBranchInteraction_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}
		private void CommandNewThreatInteraction_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			ThreatInteractionWindow ew = new ThreatInteractionWindow( scenario );
			if ( ew.ShowDialog() == true )
			{
				scenario.AddInteraction( ew.interaction );
			}
		}
		private void CommandNewThreatInteraction_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}
		private void CommandNewTestInteraction_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			TestInteractionWindow ew = new TestInteractionWindow( scenario );
			if ( ew.ShowDialog() == true )
			{
				scenario.AddInteraction( ew.interaction );
			}
		}
		private void CommandNewTestInteraction_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}
		private void CommandNewDecisionInteraction_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			DecisionInteractionWindow ew = new DecisionInteractionWindow( scenario );
			if ( ew.ShowDialog() == true )
			{
				scenario.AddInteraction( ew.interaction );
			}
		}
		private void CommandNewDecisionInteraction_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}
		private void CommandNewMultiInteraction_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			MultiEventWindow ew = new MultiEventWindow( scenario );
			if ( ew.ShowDialog() == true )
			{
				scenario.AddInteraction( ew.interaction );
			}
		}
		private void CommandNewMultiInteraction_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}
		private void CommandNewPersistentInteraction_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			PersistentInteractionWindow ew = new PersistentInteractionWindow( scenario );
			if ( ew.ShowDialog() == true )
			{
				scenario.AddInteraction( ew.interaction );
			}
		}
		private void CommandNewPersistentInteraction_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}
		private void CommandNewConditionalInteraction_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			ConditionalInteractionWindow ew = new ConditionalInteractionWindow( scenario );
			if ( ew.ShowDialog() == true )
			{
				scenario.AddInteraction( ew.interaction );
			}
		}
		private void CommandNewConditionalInteraction_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}
		private void CommandNewDialogInteraction_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			DialogInteractionWindow ew = new DialogInteractionWindow( scenario );
			if ( ew.ShowDialog() == true )
			{
				scenario.AddInteraction( ew.interaction );
			}
		}
		private void CommandNewDialogInteraction_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}
		private void CommandNewReplaceTokenInteraction_Executed( object sender, System.Windows.Input.ExecutedRoutedEventArgs e )
		{
			ReplaceTokenInteractionWindow ew = new ReplaceTokenInteractionWindow( scenario );
			if ( ew.ShowDialog() == true )
			{
				scenario.AddInteraction( ew.interaction );
			}
		}
		private void CommandNewReplaceTokenInteraction_CanExecute( object sender, System.Windows.Input.CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}
		#endregion

		private void RemoveChapterButton_Click( object sender, RoutedEventArgs e )
		{
			Chapter c = ( (Button)e.Source ).DataContext as Chapter;
			foreach ( var tile in c.tileObserver )
				scenario.globalTilePool.Add( tile.idNumber );
			TileSorter sorter = new TileSorter();
			List<int> foo = scenario.globalTilePool.ToList();
			foo.Sort( sorter );
			scenario.globalTilePool.Clear();
			foreach ( int s in foo )
				scenario.globalTilePool.Add( s );
			scenario.RemoveData( c );
		}

		private void ChapterPropsButton_Click( object sender, RoutedEventArgs e )
		{
			Chapter c = ( (Button)e.Source ).DataContext as Chapter;

			ChapterPropertiesWindow cw = new ChapterPropertiesWindow( scenario, c );
			cw.ShowDialog();
		}

		private void TileEditButton_Click( object sender, RoutedEventArgs e )
		{
			Chapter c = ( (Button)e.Source ).DataContext as Chapter;
			if ( c.isRandomTiles )
			{
				TilePoolEditorWindow tp = new TilePoolEditorWindow( scenario, c );
				tp.ShowDialog();
			}
			else
			{
				TileEditorWindow tw = new TileEditorWindow( scenario, c );
				tw.ShowDialog();
			}
		}
	}
}
