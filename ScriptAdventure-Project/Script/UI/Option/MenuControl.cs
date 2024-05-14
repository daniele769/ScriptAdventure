using Godot;
using NLog.LayoutRenderers;
using System;

public partial class MenuControl : CanvasLayer
{
	[Export] public Button newGameButton;
	[Export] public Button selectLevelButton;
	[Export] private Button loadSaveButton;
	[Export] public Button exerciseModeButton;
	[Export] private Button optionButton;
	[Export] private Button keybindingButton;
	[Export] private Button backToTitleButton;
	[Export] public Button creditButton;
	[Export] private Button closeButton;
	[Export] private Button exitButton;
	[Export] private PackedScene exerciseModeScene;
	[Export] private PackedScene newGameScene;
	[Export] private PackedScene selectLevelScene;
	[Export] private PackedScene optionScene;
	[Export] private PackedScene keybindingScene;
	[Export] private PackedScene creditScene;
	
	public override void _Ready()
	{
		exerciseModeButton.Pressed += ExerciseModeAction;
		newGameButton.Pressed += NewGameAction;
		selectLevelButton.Pressed += SelectLevelAction;
		optionButton.Pressed += OptionAction;
		backToTitleButton.Pressed += BackToTitleAction;
		exitButton.Pressed += ExitAction;
		keybindingButton.Pressed += KeybindingAction;
		creditButton.Pressed += CreditAction;

	}

	private void ExerciseModeAction()
	{
		GetTree().ChangeSceneToPacked(exerciseModeScene);
	}

	private void NewGameAction()
	{
		GetTree().ChangeSceneToPacked(newGameScene);
	}

	private void SelectLevelAction()
	{
		GetTree().ChangeSceneToPacked(selectLevelScene);
	}

	private void CreditAction()
	{
		GetTree().ChangeSceneToPacked(creditScene);
	}

	private void OptionAction()
	{
		if(GetTree().CurrentScene.Name.Equals("Menu")){
			GetTree().ChangeSceneToPacked(optionScene);
			return;
		} 
		OptionControl optionControl = (OptionControl) optionScene.Instantiate();
		this.GetParent().AddChild(optionControl);
		optionControl.SetMenuControl(this);
		this.Visible = false;
		
	}

	private void KeybindingAction()
	{
		if(GetTree().CurrentScene.Name.Equals("Menu")){
			GetTree().ChangeSceneToPacked(keybindingScene);
			return;
		} 
		KeybindingControl keybindingControl = (KeybindingControl) keybindingScene.Instantiate();
		this.GetParent().AddChild(keybindingControl);
		keybindingControl.SetMenuControl(this);
		this.Visible = false;
		
	}

	private void BackToTitleAction()
	{
		GetTree().ChangeSceneToFile("res://Scene/Menu.tscn");
	}

	public Button GetCloseButton()
	{
		return closeButton;
	}

	public Button GetBackToTitleButton()
	{
		return backToTitleButton;
	}

	private void ExitAction()
	{
		GetTree().Quit();
	}
}
