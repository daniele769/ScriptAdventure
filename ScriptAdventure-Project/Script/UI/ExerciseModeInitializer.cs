using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public partial class ExerciseModeInitializer : PanelContainer
{
	[Export] VBoxContainer exerciseContainer;
	[Export] Label labelCurrentExNumber;
	[Export] Label labelMaxExNumber;
	[Export] Button backToMenuButton;
	[Export] Label timeLabel;
	private List<string> subjectList;
	private List<string> difficultyLevel;
	private List<PackedScene> sceneList;
	private int maxExerciseNumber;
	private int currentExerciseNumber;
	private int tryForHint;
	private Node exerciseScene;
	private int sceneCount;
	private List<Exercise> exerciseResultList;
	private bool isTimerCounting;
	private ulong timeAtStart; 
	private ulong timePassedFromStart;
	private DateTime dateTime;

	public override void _Ready()
	{
		backToMenuButton.Pressed += BackToMenuAction;
		exerciseResultList = new List<Exercise>();
		isTimerCounting = false;
	}
	public void Setup(int exerciseNumber, int hint, List<string> listOfSubject, List<string> listOfDifficulty, ExcerciseModeControl excerciseModeControl)
	{
		currentExerciseNumber = 1;
		sceneList = new List<PackedScene>();
		

		maxExerciseNumber = exerciseNumber;
		tryForHint = hint;
		subjectList = listOfSubject;
		difficultyLevel = listOfDifficulty;
		GD.Print("ExerciseNumber = " + maxExerciseNumber + " | subjectFirst = " + subjectList[0] + " | difficultyFirst = " + difficultyLevel[0]);
		try{
			PreloadExercise();
		}catch{
			PopupPanel popupPanel = Static_PopupWindowSpawner.MakePopup();
			Static_PopupWindowSpawner.AddLabel(popupPanel, "Errore. Il numero massimo di esercizi disponibili per le categorie selezionate è " + sceneCount + ". ");
			Static_PopupWindowSpawner.AddCloseButton(popupPanel);
			popupPanel.PopupExclusiveCentered(this);
			return;
		}
		LoadFirstExercise();
		labelMaxExNumber.Text = (maxExerciseNumber * listOfSubject.Count).ToString();
		labelCurrentExNumber.Text = currentExerciseNumber.ToString();

		excerciseModeControl.Visible = false;
		this.Visible = true;
		isTimerCounting = true;
		timeAtStart = Time.GetTicksMsec();
		
	}

	public override void _Process(double delta)
	{
		if(isTimerCounting)
		{
			timePassedFromStart = Time.GetTicksMsec() - timeAtStart;	
			dateTime = DateTime.UnixEpoch;
			dateTime = dateTime.AddMilliseconds(timePassedFromStart);
			timeLabel.Text = dateTime.ToString("HH:mm:ss");
		}
	}

	private void LoadFirstExercise()
	{
		exerciseScene = sceneList[0].Instantiate();
		exerciseContainer.AddChild(exerciseScene);
		InitializeExScene();
	}

	private void InitializeExScene(){
		Button nextButton = (Button) Static_NodeMethod.GetInternalChildName(exerciseScene, "NextButton");
		nextButton.Pressed += LoadNextExercise;
		Exercise_Control scriptBox_Control = (Exercise_Control) exerciseScene.GetNode(exerciseScene.GetPath());
		scriptBox_Control.SetTry(tryForHint);
	}

	private void PreloadExercise()
	{
		string basePath = "res://Prefab/Exercise/";
		string path;
		
		foreach(string subject in subjectList){
			path = basePath + subject;
			if(difficultyLevel.Contains(Costanti.BaseLevel)){
				AddExerciseSceneToList(maxExerciseNumber, Costanti.BaseLevel, path);	
			}
			if(difficultyLevel.Contains(Costanti.AdvancedLevel)){
				AddExerciseSceneToList(maxExerciseNumber, Costanti.AdvancedLevel, path);	
			}
		}
	}

	private void AddExerciseSceneToList(int n, string difficulty, string path)
	{
		List<PackedScene> randomExerciseList = GetRandomExercise(maxExerciseNumber, difficulty, path);
			foreach(PackedScene scene in randomExerciseList){
				sceneList.Add(scene);
			}
	}

	private List<PackedScene> GetRandomExercise(int n, string difficulty, string path)
	{
		List<PackedScene> allExerciseInPath = GetAllExercise(difficulty, path);
		List<PackedScene> randomExercise = new List<PackedScene>();
		var rand = new Random();
		for(int i = 0; i < n; i++){
			int pos = rand.Next(allExerciseInPath.Count);
			GD.Print("Random pos is " + pos);
			randomExercise.Add(allExerciseInPath[pos]);
			allExerciseInPath.RemoveAt(pos);
		}
		return randomExercise;
	}

	private List<PackedScene> GetAllExercise(string difficulty, string path)
	{
		sceneCount = 0;
		List<PackedScene> allExerciseInPath = new List<PackedScene>();
		path = path + "/" + difficulty;
		GD.Print("ResPath = " + path);
		List<string> allResPath = GetAllFileInPath(path);
		foreach(string resPath in allResPath){
			allExerciseInPath.Add(GD.Load<PackedScene>(resPath));
			sceneCount++;
		}
		
		return allExerciseInPath;
	}

	private List<string> GetAllFileInPath(string path)
	{
		List<string> paths = new List<string>();
		DirAccess dir = DirAccess.Open(path);
		dir.ListDirBegin();
		string fileName = dir.GetNext();
		LoopGetFileInPath(fileName, path, paths, dir);
		return paths;
		
	}

	private void LoopGetFileInPath(string fileName, string path, List<string> paths, DirAccess dir)
	{
		if(fileName != ""){
			string resPath = path + "/" + fileName;
			GD.Print("Path added = " + resPath);
			paths.Add(resPath);
			fileName = dir.GetNext();
			LoopGetFileInPath(fileName, path, paths, dir);
		}
	}

	private void LoadNextExercise(){
		if(currentExerciseNumber < sceneList.Count)
		{
			labelCurrentExNumber.Text = currentExerciseNumber.ToString();
			SaveExerciseResult();
			currentExerciseNumber++;
			exerciseScene.QueueFree();
			exerciseScene = sceneList[currentExerciseNumber - 1].Instantiate();
			exerciseContainer.AddChild(exerciseScene);
			InitializeExScene();
			return;
		}
		SaveExerciseResult();
		ResultExerciseControl resultExerciseControl = (ResultExerciseControl) GD.Load<PackedScene>("res://Scene/ResultExercise.tscn").Instantiate();
		this.Visible = false;
		this.GetParent().AddChild(resultExerciseControl);
		resultExerciseControl.Initialize(exerciseResultList);
		resultExerciseControl.Visible = true;
	}

	private void SaveExerciseResult()
	{
		int errors = ((Exercise_Control) exerciseScene).ErrorCount;
		string subject = subjectList[currentExerciseNumber - 1];
		Exercise exercise = new Exercise(subject, (ulong)errors, timePassedFromStart);
		GD.Print("Nuovo esercizio finito: " + exercise.Subject + " Errors: " + exercise.Errors + " Time: " + exercise.Time);
		exerciseResultList.Add(exercise);
	}

	private void BackToMenuAction()
	{
		GetTree().ChangeSceneToFile("res://Scene/Menu.tscn");
		this.QueueFree();
	}
}
