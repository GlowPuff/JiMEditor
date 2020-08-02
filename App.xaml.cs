using System.Diagnostics;
using System.Windows;

namespace JiME
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void Application_Startup( object sender, StartupEventArgs e )
		{
			//PresentationTraceSources.Refresh();
			//PresentationTraceSources.DataBindingSource.Listeners.Add( new ConsoleTraceListener() );
			//PresentationTraceSources.DataBindingSource.Listeners.Add( new DebugTraceListener() );
			//PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning | SourceLevels.Error;
		}

		private void Application_DispatcherUnhandledException( object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e )
		{
			MessageBox.Show( "An unhandled exception occurred: \r\n" + e.Exception.Message, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
			e.Handled = true;
		}
	}
}
