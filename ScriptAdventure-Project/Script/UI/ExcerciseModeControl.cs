using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

public partial class ExcerciseModeControl : PanelContainer
{
	[Export] private SpinBox exerciseNumberSpinBox;
	[Export] private CheckBox checkBaseLevel;
	[Export] private CheckBox checkAdvancedLevel;
	[Export] private SpinBox tryForHintSpinBox;
	[Export] private CheckBox declarationSubject;
	[Export] private CheckBox ifSubject;
	[Export] private CheckBox forSubject;
	[Export] private CheckBox whileSubject;
	[Export] private CheckBox arraySubject;
	[Export] private CheckBox structSubject;
	[Export] private Button backToMenuButton;
	[Export] private Button startButton;
	[Export] private ExerciseModeInitializer exerciseModeInitializer;
	private PopupPanel popupPanel;
	private int exerciseNumber;
	private int tryForHint;
	private List<string> subjectList;
	private List<string> difficultyLevel;
	private bool isSubjectListOk;
	private bool isDifficultyListOk;

    public override void _Ready()
    {
        backToMenuButton.Pressed += BackToMenuAction;
    }

	private void BackToMenuAction()
	{
		GetTree().ChangeSceneToFile("res://Scene/Menu.tscn");
	}

    private void StartExercise()
	{
		InitializeProperties();
		if(ArePropertiesOk()){
			exerciseModeInitializer.Setup(exerciseNumber, tryForHint, subjectList, difficultyLevel, this);
			
		}	
	}

	private void InitializeProperties()
	{
		exerciseNumber = (int) exerciseNumberSpinBox.Value;
		tryForHint = (int) tryForHintSpinBox.Value;
		subjectList = new List<string>();
		difficultyLevel = new List<string>();

		if(declarationSubject.ButtonPressed)
			subjectList.Add(Costanti.SubjectDeclaration);
		if(ifSubject.ButtonPressed)
			subjectList.Add(Costanti.SubjectIf);
		if(forSubject.ButtonPressed)
			subjectList.Add(Costanti.SubjectFor);
		if(whileSubject.ButtonPressed)
			subjectList.Add(Costanti.SubjectWhile);
		if(arraySubject.ButtonPressed)
			subjectList.Add(Costanti.SubjectArray);
		if(structSubject.ButtonPressed)
			subjectList.Add(Costanti.SubjectStruct);

		if(checkBaseLevel.ButtonPressed)
			difficultyLevel.Add(Costanti.BaseLevel);
		if(checkAdvancedLevel.ButtonPressed)
			difficultyLevel.Add(Costanti.AdvancedLevel);
	}

	private bool ArePropertiesOk()
	{
		if(subjectList.Count == 0 || difficultyLevel.Count == 0 || exerciseNumber <= 0 || tryForHint <= 0 || !AreImportedListOk()){
			popupPanel = Static_PopupWindowSpawner.MakePopup();
			VBoxContainer vBoxContainer = (VBoxContainer) Static_NodeMethod.GetChildType(popupPanel, "VBoxContainer");

			if(exerciseNumber <= 0){
			 	Static_PopupWindowSpawner.AddLabel(popupPanel, "Errore. Il numero di esercizi non può essere minore o uguale a zero.");
			}
			if(tryForHint <= 0){
			 	Static_PopupWindowSpawner.AddLabel(popupPanel, "Errore. Il numero di tentativi non può essere minore o uguale a zero.");
			}
			if(difficultyLevel.Count == 0){
				Static_PopupWindowSpawner.AddLabel(popupPanel, "Errore. Non è stata selezionata nessuna difficoltà.");
			}
			if(subjectList.Count == 0){
				Static_PopupWindowSpawner.AddLabel(popupPanel, "Errore. Non è stata selezionato nessun argomento.");
			}
			if(!isSubjectListOk)
				Static_PopupWindowSpawner.AddLabel(popupPanel, "Errore. Argomento non riconosciuto");
			if(!isDifficultyListOk)
				Static_PopupWindowSpawner.AddLabel(popupPanel, "Errore. Difficoltà non riconosciuta");
			Static_PopupWindowSpawner.AddCloseButton(popupPanel);
			popupPanel.PopupExclusiveCentered(this);
			return false;
		}
		return true;
	}

	public void StartExerciseFromIni(int exNumber, int tryNumber, List<int> subjects, List<int> difficulties)
	{
		exerciseNumber = exNumber;
		tryForHint = tryNumber;
		subjectList = new List<string>();
		difficultyLevel = new List<string>();

		for(int i = 0; i < subjects.Count; i++)
		{
			subjectList.Add(GetSubjectFromInt(subjects[i]));
		}
		for(int i = 0; i < difficulties.Count; i++)
		{
			difficultyLevel.Add(GetDifficultyFromInt(difficulties[i]));
		}

		if(ArePropertiesOk())
		{
			exerciseModeInitializer.Setup(exerciseNumber, tryForHint, subjectList, difficultyLevel, this);
		}
	}

	private string GetSubjectFromInt(int n)
	{
		switch(n)
		{
			case 0: return Costanti.SubjectDeclaration;
					
			case 1: return Costanti.SubjectIf;
					
			case 2: return Costanti.SubjectFor;
					
			case 3: return Costanti.SubjectWhile;
					
			case 4: return Costanti.SubjectArray;
					
			case 5: return Costanti.SubjectStruct;
					
		}
		return "";
	}

	private string GetDifficultyFromInt(int n)
	{
		switch(n)
		{
			case 0: return Costanti.BaseLevel;
					
			case 1: return Costanti.AdvancedLevel;

		}
		return "";
	}

	private bool AreImportedListOk()
	{
		bool areOk = true;
		isDifficultyListOk = true;
		isSubjectListOk = true;

		foreach(string subject in subjectList)
		{
			if(subject.Equals(""))
			{
				areOk = false;
				isSubjectListOk = false;
			}	
		}

		foreach(string difficulty in difficultyLevel)
		{
			if(difficulty.Equals(""))
			{
				areOk = false;
				isDifficultyListOk = false;
			}	
		}
		return areOk;
	}
}
