using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Linq;

namespace JiME
{
	public class ErrorChecker : INotifyPropertyChanged
	{
		string _Errors;
		string _errorCount;

		public string Errors
		{
			get => _Errors;
			set
			{
				if ( value != _Errors )
				{
					_Errors = value;
					PropChanged( "Errors" );
				}
			}
		}

		public string errorCount
		{
			get => _errorCount;
			set
			{
				if ( value != _errorCount )
				{
					_errorCount = value;
					PropChanged( "errorCount" );
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		void PropChanged( string name )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
		}

		public ErrorChecker()
		{
			Errors = "None Found";
			errorCount = "Warnings (0):";
		}

		public void AddWarning( string s )
		{
			Errors += "\r\n" + s;
		}

		public void ClearErrors()
		{
			Errors = "None Found";
		}

		public void CheckErrors( Scenario scenario )
		{
			int found = 0;
			//duplicate names across observables
			Dictionary<string, string> dupes = CheckDupeNames( scenario );
			found += dupes.Count;
			//missing trigger reference

			//"Next Objective" missing

			//no tiles in chapter

			//orphan event names (from renaming)

			Errors = "None Found";
			errorCount = $"Warnings ({found}):";
		}

		void CheckMissingTriggers()
		{

		}

		Dictionary<string, string> CheckDupeNames( Scenario s )
		{
			//int triggerDupes = 0;
			//foreach ( var trigger in s.triggersObserver )
			//{
			//	int dupes = s.triggersObserver.Count( t => t.dataName == trigger.dataName );
			//	if ( dupes > 1 )
			//		triggerDupes++;
			//}
			Dictionary<string, string> dupes = new Dictionary<string, string>();
			Dictionary<string, string> names = new Dictionary<string, string>();
			//names: key=Name value=Type
			//dupes: key=Type value=dupe_name|dupe_type

			foreach ( var item in s.interactionObserver )
			{
				if ( !names.ContainsKey( item.dataName ) )
					names.Add( item.dataName, "Interaction" );
				else
					dupes.Add( "Interaction", item.dataName + "|" + names[item.dataName] );
			}
			foreach ( var item in s.triggersObserver )
			{
				if ( !names.ContainsKey( item.dataName ) )
					names.Add( item.dataName, "Trigger" );
				else
					dupes.Add( "Trigger", item.dataName + "|" + names[item.dataName] );
			}
			foreach ( var item in s.objectiveObserver )
			{
				if ( !names.ContainsKey( item.dataName ) )
					names.Add( item.dataName, "Objective" );
				else
					dupes.Add( "Objective", item.dataName + "|" + names[item.dataName] );
			}
			foreach ( var item in s.resolutionObserver )
			{
				if ( !names.ContainsKey( item.dataName ) )
					names.Add( item.dataName, "TextBookData" );
				else
					dupes.Add( "TextBookData", item.dataName + "|" + names[item.dataName] );
			}
			foreach ( var item in s.threatObserver )
			{
				if ( !names.ContainsKey( item.dataName ) )
					names.Add( item.dataName, "Threat" );
				else
					dupes.Add( "Threat", item.dataName + "|" + names[item.dataName] );
			}
			foreach ( var item in s.chapterObserver )
			{
				if ( !names.ContainsKey( item.dataName ) )
					names.Add( item.dataName, "Chapter" );
				else
					dupes.Add( "Chapter", item.dataName + "|" + names[item.dataName] );
			}
			return dupes;
		}

		/// <summary>
		/// Returns pass/fail(bool), msgbox text(string), msgbox title(string)
		/// </summary>
		public static Tuple<bool, string, string> CheckInteraction( IInteraction interaction )
		{
			//check Decision values
			//if ( interaction.interactionType == InteractionType.Decision )
			//{
			//	if ( string.IsNullOrEmpty( interaction.choice1Trigger.Trim() )
			//		|| string.IsNullOrEmpty( interaction.choice2Trigger.Trim() )
			//		|| ( interaction.isThreeChoices && string.IsNullOrEmpty( interaction.choice3Trigger.Trim() ) ) )
			//	{
			//		return new Tuple<bool, string, string>( false, "Choice Labels cannot be empty.", "Data Error (Decision Tab)" );
			//	}

			//	if ( interaction.choice1Trigger == "None"
			//		|| interaction.choice1Trigger == "None"
			//		|| ( interaction.isThreeChoices && interaction.choice1Trigger == "None" ) )
			//	{
			//		return new Tuple<bool, string, string>( false, "Triggers are required for each choice, and cannot be set to None.", "Data Error (Decision Tab)" );
			//	}
			//}
			//else if ( interaction.interactionType == InteractionType.Threat )
			//{
			//	//make sure monsters not empty
			//	if ( interaction.monsterCollection.Count == 0 )
			//	{
			//		return new Tuple<bool, string, string>( false, "At least one Monster Threat is required.", "Data Error (Monster Threat Tab)" );
			//	}
			//}
			//else if ( interaction.interactionType == InteractionType.StatTest )
			//{
			//	//check success value
			//	if ( interaction.successValue == 0 )
			//	{
			//		return new Tuple<bool, string, string>( false, "The Success Value must be greater than 0.", "Data Error (Attribute Test Tab)" );
			//	}
			//	if ( interaction.successTrigger == "None"
			//		|| interaction.failTrigger == "None" )
			//	{
			//		return new Tuple<bool, string, string>( false, "The 'Trigger On Pass' and 'Trigger On Fail' Triggers cannot be set to None.", "Data Error (Attribute Test Tab)" );
			//	}
			//}

			return new Tuple<bool, string, string>( true, "", "" );
		}
	}
}
