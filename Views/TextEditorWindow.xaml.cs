using System;
using System.Windows;

namespace JiME.Views
{
	/// <summary>
	/// Interaction logic for TextEditorWindow.xaml
	/// </summary>
	public partial class TextEditorWindow : Window
	{
		const int maxChars = 800;
		EditMode editMode;
		//int resolutionIndex;

		public TextBookData bookData { get; set; }
		public Scenario scenario { get; set; }
		public string shortName { get; set; }
		public TextBookController textBookController;
		public bool successChecked;

		public TextEditorWindow( Scenario s, EditMode mode, TextBookData textBookData )
		{
			InitializeComponent();
			DataContext = this;

			SourceInitialized += ( x, y ) =>
			{
				this.HideMinimizeAndMaximizeButtons();
			};

			scenario = s;
			editMode = mode;

			Title = $"Edit {mode} Text";

			bookData = textBookData;

			//populate existing data
			if ( mode == EditMode.Resolution )
			{
				descriptionBlock.Visibility = Visibility.Visible;
				triggerGroup.Visibility = Visibility.Visible;
			}
			else
			{
				descriptionBlock.Visibility = Visibility.Collapsed;
				triggerGroup.Visibility = Visibility.Collapsed;
				resultbox.Visibility = Visibility.Collapsed;
			}

			shortName = bookData.dataName;

			textBookController = new TextBookController();
			textBookController.ImportPages( bookData.pages );
			pageText.Text = textBookController.GetPage( 0 );
			UpdateInfo();

			triggerLB.IsEnabled = mode == EditMode.Resolution;
		}

		private void AddButton_Click( object sender, RoutedEventArgs e )
		{
			textBookController.SetContent( pageText.Text );
			textBookController.AddPage();
			pageText.Text = string.Empty;
			UpdateInfo();
		}

		private void RemoveButton_Click( object sender, RoutedEventArgs e )
		{
			pageText.Text = textBookController.RemovePage();
			UpdateInfo();
		}

		private void TextBox_TextChanged( object sender, System.Windows.Controls.TextChangedEventArgs e )
		{
			int idx = pageText.CaretIndex;
			//pageText.Text = pageText.Text.Substring( 0, Math.Min( maxChars, pageText.Text.Length ) );
			pageText.CaretIndex = idx;
			UpdateInfo( false );
		}

		void UpdateInfo( bool focus = true )
		{
			//pageInfo.Text = $"Page {textBookController.index + 1} of {textBookController.pageCount}";
			//charCount.Text = $"Characters: {pageText.Text.Length}/{maxChars}";
			if ( focus )
			{
				pageText.Focus();
				pageText.CaretIndex = pageText.Text.Length;
			}
		}

		private void RightButton_Click( object sender, RoutedEventArgs e )
		{
			textBookController.SetContent( pageText.Text );
			pageText.Text = textBookController.Next();
			UpdateInfo();
		}

		private void LeftButton_Click( object sender, RoutedEventArgs e )
		{
			textBookController.SetContent( pageText.Text );
			pageText.Text = textBookController.Previous();
			UpdateInfo();
		}

		private void OkButton_Click( object sender, RoutedEventArgs e )
		{
			textBookController.SetContent( pageText.Text );

			if ( editMode == EditMode.Resolution
				&& ( (Trigger)triggerLB.SelectedItem == null || ( (Trigger)triggerLB.SelectedItem ).dataName == "None" ) )
			{
				MessageBox.Show( "The Trigger cannot be \"None\".", "Invalid Trigger", MessageBoxButton.OK, MessageBoxImage.Error );
				return;
			}

			//bool accept = true;
			//foreach ( string s in textBookController.pages )
			//	if ( string.IsNullOrEmpty( s.Trim() ) )
			//		accept = false;
			//if ( accept )
			//	DialogResult = true;
			//else
			//	MessageBox.Show( "Pages cannot be blank.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error );

			successChecked = successCB.IsChecked.Value;

			for ( int i = 0; i < textBookController.pages.Count; i++ )
				textBookController.pages[i] = textBookController.pages[i].Trim();
			DialogResult = true;
		}
		private void CancelButton_Click( object sender, RoutedEventArgs e )
		{
			DialogResult = false;
		}

		private void AddTriggerButton_Click( object sender, RoutedEventArgs e )
		{
			TriggerEditorWindow tw = new TriggerEditorWindow( scenario );
			if ( tw.ShowDialog() == true )
				bookData.triggerName = tw.triggerName;
		}
	}
}
