using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public partial class SceneLoader : Node2D
{
	private string configDirectoryPath;
	private string configIniPath;
	private bool forceExercise;
	private int exerciseNumber;
	private List<int> difficultyLevel;
	private int tryNumber;
	private List<int> subjectList;
	private PopupPanel popupPanel;
	private string error;

	public override void _Ready()
	{
		forceExercise = false;
		difficultyLevel = new List<int>();
		subjectList = new List<int>();
		error = "";

		string documentsPath =  System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
		configDirectoryPath = documentsPath + @"\ScriptAdventure";
		configIniPath = configDirectoryPath + @"\Config.ini";
		InitializeVideoOption();
		InitializeAudioOption();
		CheckConfigIni();
	}

	public override void _Process(double delta)
	{	
		if(!error.Equals("") && !error.Equals("verified"))
		{
			popupPanel = Static_PopupWindowSpawner.MakePopup();
			Static_PopupWindowSpawner.AddLabel(popupPanel, error + "\nImpossibile Avviare modalita esercizio");
			Static_PopupWindowSpawner.AddCloseButton(popupPanel);
			popupPanel.PopupExclusiveCentered(this);
			error = "verified";
		}
		else
		if(error.Equals("") && forceExercise)
		{
			Node node = GD.Load<PackedScene>("res://Scene/ModalitàEsercizio.tscn").Instantiate();
			GetTree().Root.AddChild(node);
			ExcerciseModeControl excerciseModeControl =  node.GetChild<ExcerciseModeControl>(1);
			excerciseModeControl.StartExerciseFromIni(exerciseNumber, tryNumber, subjectList, difficultyLevel);
			GetTree().Root.RemoveChild(this);
			this.QueueFree();
		}
		else
		if(!IsInstanceValid(popupPanel) && !forceExercise)
		{
			GetTree().ChangeSceneToFile("res://Scene/Menu.tscn");
		}
	}

	private void InitializeVideoOption()
	{
		Static_DisplayControl.ApplyDisplaySetting(this);
	}

	private void InitializeAudioOption()
	{
		Static_AudioControl.UpdateAudioValues();
	}

	private void CheckConfigIni()
	{
		if(!File.Exists(configIniPath))
		{
			GD.Print("config.ini not found");
			MakeConfigIniFile();
			GD.Print("config.ini created");
			GetTree().ChangeSceneToFile("res://Scene/Menu.tscn");
			return;
		}
		ReadConfigIni();
	}


	private void MakeConfigIniFile()
	{
		DirectoryInfo di = new DirectoryInfo(configDirectoryPath);
		if(!di.Exists)
		{
			di.Create();
		}
		string text = GetConfigTextDefault();
		File.WriteAllText(configIniPath, text);
	}

	private void ReadConfigIni()
	{
		StringBuilder sb = new StringBuilder();
		string line = "";
		StreamReader sr = new StreamReader(configIniPath);
		ReadLinesFromStream(sr, line, sb);
		error = sb.ToString();
		GD.Print("error = " + error);
	}

	private void ReadLinesFromStream(StreamReader sr, string line, StringBuilder sb)
	{
		line = sr.ReadLine();
		if(line != null)
		{
			CheckLine(line, sb);
			ReadLinesFromStream(sr, line, sb);
		}
	}

	private void CheckLine(string line, StringBuilder sb)
	{
		line = Regex.Replace(line, " ", "");
		if(line.StartsWith("ForceExerciseMode="))
		{
			GD.Print("Enter in ForceExerciseValue");
			line = line.Substring(line.IndexOf("=") + 1);
			if(line.Equals("false"))
				forceExercise = false;
			else 
			if(line.Equals("true"))
				forceExercise = true;
			else	
				sb.AppendLine("Valore 'ForceExerciseMode' non riconosciuto");
		} 
		else
		if(line.StartsWith("ExercisesNumber="))
		{
			GD.Print("Enter in ExerciseNumberValue");
			line = line.Substring(line.IndexOf("=") + 1);
			try
			{
				exerciseNumber = CheckIntValue(line, "Valore 'ExerciseNumber' non riconosciuto");
			}
			catch(Exception ex)
			{
				sb.AppendLine(ex.Message);
			}
		} 
		else
		if(line.StartsWith("Difficulty="))
		{
			GD.Print("Enter in Difficulty");
			line = line.Substring(line.IndexOf("=") + 1);
			try
			{
				if(!line.Contains(","))
					difficultyLevel.Add(CheckIntValue(line, "Valore 'Difficulty' non riconosciuto"));
				else
				{
					string[] values = line.Split(",");
					foreach(string value in values)
					{
					  	difficultyLevel.Add(CheckIntValue(value, "Valore 'Difficulty' non riconosciuto"));
					}
				}

			}
			catch(Exception ex)
			{
				sb.AppendLine(ex.Message);
			}
		}
		else
		if(line.StartsWith("TryNumber="))
		{
			GD.Print("Enter in TryNumber");
			line = line.Substring(line.IndexOf("=") + 1);
			try
			{
				tryNumber = CheckIntValue(line, "Valore 'TryNumber' non riconosciuto");
			}
			catch(Exception ex)
			{
				sb.AppendLine(ex.Message);
			}
		}
		else
		if(line.StartsWith("Subject="))
		{
			GD.Print("Enter in Subject");
			line = line.Substring(line.IndexOf("=") + 1);
			try
			{
				string[] values = line.Split(",");
				foreach(string value in values)
				{
					int intValue = CheckIntValue(value, "Valore 'Subject' non riconosciuto");
					subjectList.Add(intValue);
				}
			}
			catch(Exception ex)
			{
				sb.AppendLine(ex.Message);
			}
		}
	}

	private int CheckIntValue(string line, string exceptionMessage)
	{
		int n;
		try
		{
			n = line.ToInt();
		} 
		catch
		{
			throw new Exception(exceptionMessage);
		}
		return n;

	}

	private string GetConfigTextDefault()
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine("ForceExerciseMode = false");
		sb.AppendLine("ExercisesNumber = 1");
		sb.AppendLine("//0 - Base; 1 - Avanzato");
		sb.AppendLine("Difficulty = 0");
		sb.AppendLine("TryNumber = 3");
		sb.AppendLine("//0 - DichiarazioneVariabili; 1 - If; 2 - CicloFor; 3 - CicloWhile; 4 - Array; 5 - Struct");
		sb.AppendLine("Subject = 0, 1, 2");
		return sb.ToString();
	}

}
