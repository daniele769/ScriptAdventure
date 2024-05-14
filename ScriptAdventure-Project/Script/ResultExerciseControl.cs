using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ResultExerciseControl : Control
{
	[Export] PanelContainer subjectRowProxy;
	[Export] PanelContainer errorRowProxy;
	[Export] PanelContainer timeRowProxy;
	[Export] PanelContainer totalErrorRow;
	[Export] PanelContainer totalTimeRow;
	[Export] Button backToMenuButton;
	VBoxContainer subjectContainer;
	VBoxContainer errorContainer;
	VBoxContainer timeContainer;
	
	private List<Exercise> exerciseList;
	private Dictionary<string, ulong[]> gridList;
	private ulong totalTime;
	private int totalError;

	public void Initialize(List<Exercise> list)
	{
		subjectContainer = subjectRowProxy.GetParent<VBoxContainer>();
		errorContainer = errorRowProxy.GetParent<VBoxContainer>();
		timeContainer = timeRowProxy.GetParent<VBoxContainer>();
		backToMenuButton.Pressed += BacKToMenu;

		exerciseList = list;
		gridList = CalcDictionary();
		InitializeFirstRow();
		InitializeOtherRow();
		InitializeTotalRow();
	}

	private Dictionary<string, ulong[]> CalcDictionary()
	{
		Dictionary<string, ulong[]> dictionary = new Dictionary<string, ulong[]>();
		foreach(Exercise ex in exerciseList)
		{
			GD.Print("Esercizio categoria " + ex.Subject + " trovato");
			if(!dictionary.ContainsKey(ex.Subject))
			{
				GD.Print("****Nuova Key!");
				ulong[] values = new ulong[2];
				values[0] = ex.Errors;
				values[1] = ex.Time;
				dictionary.Add(ex.Subject, values);
				GD.Print("Aggiunto esercizio " + ex.Subject + " errori: " + dictionary[ex.Subject][0] + " tempo: " + dictionary[ex.Subject][1]);
				continue;
			}
			GD.Print("****Categoria gia esistente");
			ulong[] oldValues = dictionary[ex.Subject];
			dictionary[ex.Subject] = CalcNewValues(oldValues, ex);
			GD.Print("Aggiornato esercizi " + ex.Subject + " errori: " + dictionary[ex.Subject][0] + " tempo: " + dictionary[ex.Subject][1]);
		}

		return dictionary;
	}

	private ulong[] CalcNewValues(ulong[] values, Exercise ex)
	{
		values[0] = values[0] + ex.Errors;
		values[1] = values[1] + ex.Time;
		return values;
	}

	private void InitializeFirstRow()
	{
		DateTime dateTime = DateTime.UnixEpoch;
		string key = gridList.Keys.ToArray()[0];
		dateTime = dateTime.AddMilliseconds(gridList[key][1]);

		UpdateLabel(subjectRowProxy, key);
		UpdateLabel(errorRowProxy, gridList[key][0].ToString());
		UpdateLabel(timeRowProxy, dateTime.ToString("HH:mm:ss"));
		totalTime = gridList[key][1];
		totalError = (int) gridList[key][0];
		gridList.Remove(key);

	}

	private void InitializeOtherRow()
	{
		foreach(string key in gridList.Keys.ToArray())
		{
			DateTime  dateTime = DateTime.UnixEpoch;
			
			PanelContainer subjectRow = (PanelContainer) subjectRowProxy.Duplicate(15);
			PanelContainer errorRow = (PanelContainer) errorRowProxy.Duplicate(15);
			PanelContainer timeRow = (PanelContainer) timeRowProxy.Duplicate(15);

			ulong time = gridList[key][1] - totalTime;
			dateTime = dateTime.AddMilliseconds(time);
			totalTime += time;
			totalError += (int) gridList[key][0];

			UpdateLabel(subjectRow, key);
			UpdateLabel(errorRow, gridList[key][0].ToString());
			UpdateLabel(timeRow, dateTime.ToString("HH:mm:ss"));

			subjectContainer.AddChild(subjectRow);
			errorContainer.AddChild(errorRow);
			timeContainer.AddChild(timeRow);
		}
	}

	private void InitializeTotalRow()
	{
		DateTime  dateTime = DateTime.UnixEpoch;
		dateTime = dateTime.AddMilliseconds(totalTime);

		UpdateLabel(totalTimeRow, dateTime.ToString("HH:mm:ss"));
		UpdateLabel(totalErrorRow, totalError.ToString());
	}

	private void UpdateLabel(PanelContainer panelContainer, string text)
	{
		Label label = panelContainer.GetChild<Label>(0);
		label.Text = text;
	}

	private void BacKToMenu()
	{
		this.GetTree().ChangeSceneToFile("res://Scene/Menu.tscn");
	}
}
