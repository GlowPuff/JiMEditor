using System.Windows.Input;

namespace JiME
{
	public static class MyCommands
	{
		public static readonly RoutedUICommand Exit = new RoutedUICommand
			(
				"Exit",
				"Exit",
				typeof( MyCommands ),
				new InputGestureCollection()
				{
					new KeyGesture(Key.X, ModifierKeys.Alt)
				}
			);

		public static readonly RoutedUICommand NewObjective = new RoutedUICommand
			(
				"NewObjective",
				"NewObjective",
				typeof( MyCommands ),
				new InputGestureCollection()
				{
					new KeyGesture(Key.O, ModifierKeys.Alt)
				}
			);

		public static readonly RoutedUICommand NewTrigger = new RoutedUICommand
			(
				"NewTrigger",
				"NewTrigger",
				typeof( MyCommands ),
				new InputGestureCollection()
				{
					new KeyGesture(Key.T, ModifierKeys.Alt)
				}
			);

		public static readonly RoutedUICommand NewEvent = new RoutedUICommand
			(
				"NewEvent",
				"NewEvent",
				typeof( MyCommands ),
				new InputGestureCollection()
				{
					new KeyGesture(Key.E, ModifierKeys.Alt)
				}
			);

		public static readonly RoutedUICommand NewProject = new RoutedUICommand
			(
				"NewProject",
				"NewProject",
				typeof( MyCommands ),
				new InputGestureCollection()
				{
					new KeyGesture(Key.N, ModifierKeys.Control)
				}
			);

		public static readonly RoutedUICommand OpenProject = new RoutedUICommand
			(
				"OpenProject",
				"OpenProject",
				typeof( MyCommands ),
				new InputGestureCollection()
				{
							new KeyGesture(Key.O, ModifierKeys.Control)
				}
			);

		public static readonly RoutedUICommand SaveProject = new RoutedUICommand
			(
				"SaveProject",
				"SaveProject",
				typeof( MyCommands ),
				new InputGestureCollection()
				{
							new KeyGesture(Key.S, ModifierKeys.Control)
				}
			);

		public static readonly RoutedUICommand SaveProjectAs = new RoutedUICommand
			(
				"SaveProjectAs",
				"SaveProjectAs",
				typeof( MyCommands ),
				new InputGestureCollection()
				{
							new KeyGesture(Key.S, ModifierKeys.Control|ModifierKeys.Alt)
				}
			);

		public static readonly RoutedUICommand ScenarioSettings = new RoutedUICommand
			(
				"ScenarioSettings",
				"ScenarioSettings",
				typeof( MyCommands ),
				new InputGestureCollection()
				{
							new KeyGesture(Key.S, ModifierKeys.Alt)
				}
			);

		public static readonly RoutedUICommand NewChapter = new RoutedUICommand
			(
				"NewChapter",
				"NewChapter",
				typeof( MyCommands ),
				new InputGestureCollection()
				{
							new KeyGesture(Key.C, ModifierKeys.Alt)
				}
			);

		//Interaction popup commands
		public static readonly RoutedUICommand NewTextInteraction = new RoutedUICommand
			(
				"New Text Dialog",
				"NewTextInteraction",
				typeof( MyCommands ),
				new InputGestureCollection()
				{
					//new KeyGesture(Key.C, ModifierKeys.Alt)
				}
			);
		public static readonly RoutedUICommand NewBranchInteraction = new RoutedUICommand
			(
				"New Story Branch",
				"NewBranchInteraction",
				typeof( MyCommands ),
				new InputGestureCollection()
				{
					//new KeyGesture(Key.C, ModifierKeys.Alt)
				}
			);
		public static readonly RoutedUICommand NewThreatInteraction = new RoutedUICommand
			(
				"New Monster Threat",
				"NewThreatInteraction",
				typeof( MyCommands ),
				new InputGestureCollection()
				{
					//new KeyGesture(Key.C, ModifierKeys.Alt)
				}
			);
		public static readonly RoutedUICommand NewTestInteraction = new RoutedUICommand
			(
				"New Stat Test",
				"NewTestInteraction",
				typeof( MyCommands ),
				new InputGestureCollection()
				{
					//new KeyGesture(Key.C, ModifierKeys.Alt)
				}
			);
		public static readonly RoutedUICommand NewDecisionInteraction = new RoutedUICommand
			(
				"New Decision Interaction",
				"NewDecisionInteraction",
				typeof( MyCommands ),
				new InputGestureCollection()
				{
					//new KeyGesture(Key.C, ModifierKeys.Alt)
				}
			);
		public static readonly RoutedUICommand NewMultiInteraction = new RoutedUICommand
			(
				"New Multi-Event",
				"NewMultiInteraction",
				typeof( MyCommands ),
				new InputGestureCollection()
				{
					//new KeyGesture(Key.C, ModifierKeys.Alt)
				}
			);
		public static readonly RoutedUICommand NewPersistentInteraction = new RoutedUICommand
			(
				"New Persistent Event",
				"NewPersistentInteraction",
				typeof( MyCommands ),
				new InputGestureCollection()
				{
					//new KeyGesture(Key.C, ModifierKeys.Alt)
				}
			);
		public static readonly RoutedUICommand NewConditionalInteraction = new RoutedUICommand
			(
				"New Conditional Event",
				"NewConditionalInteraction",
				typeof( MyCommands ),
				new InputGestureCollection()
				{
					//new KeyGesture(Key.C, ModifierKeys.Alt)
				}
			);
	}
}
