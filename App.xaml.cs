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

			Application.Current.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
		}

		//generic, app-wide error handler to catch any unhandled exceptions
		private void Dispatcher_UnhandledException( object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e )
		{
			var inner = e.Exception.InnerException.ToString().Split( new string[] { "at" }, 2, System.StringSplitOptions.None );


			MessageBox.Show(
				"An unhandled exception occurred: \r\n\r\n"
				+ inner[0]
				+ "\r\nStack Trace:\r\n"
				+ inner[1], "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
			Application.Current.Shutdown();
			e.Handled = true;
		}

		private void Application_DispatcherUnhandledException( object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e )
		{
			MessageBox.Show( "An unhandled exception occurred: \r\n" + e.Exception.Message, "App Exception", MessageBoxButton.OK, MessageBoxImage.Error );
			e.Handled = true;
		}
	}
}
