using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace JiME
{
	public class BoolInvertConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( value is bool )
			{
				return !(bool)value;
			}
			return value;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( value is bool )
			{
				return !(bool)value;
			}
			return value;
		}
	}

	public class BoolInvertVisibility : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( (bool)value )
				return Visibility.Collapsed;
			else
				return Visibility.Visible;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}

	public class TitleConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			try
			{
				//Scenario s = value as Scenario;
				Tuple<bool, string, Guid, ProjectType> token = value as Tuple<bool, string, Guid, ProjectType>;
				return $"Your Journey - {token.Item4} Scenario - {( string.IsNullOrEmpty( token.Item2 ) ? "Untitled" : token.Item2 )}{( token.Item1 ? "*" : "" )}";
			}
			catch { return "Error Converting Window Title"; }
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}

	public class ProjectTypeConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( (ProjectType)value == ProjectType.Campaign )
				return "/JiME;component/Assets/ring.png";
			else
				return "/JiME;component/Assets/ring-silver.png";
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return ProjectType.Campaign;
		}
	}

	public class BoolToScrollbarVisibilityConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( (bool)value )
				return ScrollBarVisibility.Auto;
			else
				return ScrollBarVisibility.Hidden;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}

	public class TabEnabledConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			switch ( (InteractionType)value )
			{
				case InteractionType.Text:
					return 0;
				case InteractionType.Threat:
					return 1;
				case InteractionType.StatTest:
					return 2;
				case InteractionType.Decision:
					return 3;
				case InteractionType.Darkness:
					return 4;
				default:
					return 0;
			}
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}

	public class RadioBoolToScenarioTypeConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			ScenarioType sType = (ScenarioType)value;
			if ( (string)parameter == "1" && sType == ScenarioType.Journey )
			{
				return true;
			}
			else if ( (string)parameter == "2" && sType == ScenarioType.Battle )
			{
				return true;
			}

			return false;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( (string)parameter == "1" )
			{
				if ( (bool)value )
					return ScenarioType.Journey;
				else
					return ScenarioType.Battle;
			}
			else
			{
				if ( (bool)value )
					return ScenarioType.Battle;
				else
					return ScenarioType.Journey;
			}
		}
	}

	public class NullToVisibility : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return value != null ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}

	public class TokenEventVisibility : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			int param = int.Parse( (string)parameter );
			int tokens = (int)value;
			if ( param <= tokens )
				return Visibility.Visible;
			else
				return Visibility.Collapsed;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}

	public class TokenEnabled : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return (int)value == 0 ? false : true;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}

	public class SettingsConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( value != null )
				return value.ToString() != "None" && value.ToString() != "Random Event Trigger";
			else
				return false;
			//return (int)value == 0 ? false : true;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}

	public class SideConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( (string)parameter == "1" && (string)value == "A" )
				return true;
			else if ( (string)parameter == "2" && (string)value == "B" )
				return true;

			return false;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( (string)parameter == "1" )
				if ( (bool)value )
					return "A";
				else
					return "B";
			else if ( (string)parameter == "2" )
				if ( (bool)value )
					return "B";
				else
					return "A";
			return false;
		}
	}

	public class SelectedToBoolConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return (int)value >= 0;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}

	public class TerrainTokenEnabledConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( value is int )
				return (int)value > 0;
			return value;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}

	public class BoolToColor : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( (bool)value )
				return new SolidColorBrush( Colors.MediumSeaGreen );
			else
				return new SolidColorBrush( System.Windows.Media.Color.FromRgb( 70, 70, 74 ) );
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}
