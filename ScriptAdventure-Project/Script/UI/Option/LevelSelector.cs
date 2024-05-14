using Godot;
using System;
using System.Linq;

public partial class LevelSelector : Node
{
	[Export] Button level0_Button;
	[Export] Button level1_Button;
	[Export] Button backToMenu;
	[Export] string backToMenuScenePath;
	[Export] string level0ScenePath;
	[Export] string level1ScenePath;

	public override void _Ready()
	{
		backToMenu.Pressed += BackToMenuAction;
		if(PlayerPrefs.ListKeys.Contains(Costanti.level0))
		{
			if(PlayerPrefs.GetBool(Costanti.level0))
			{
				level0_Button.Disabled = false;
				level0_Button.Pressed += Level0Action;
			}
		}

		if(PlayerPrefs.ListKeys.Contains(Costanti.level1))
		{
			if(PlayerPrefs.GetBool(Costanti.level1))
			{
				level1_Button.Disabled = false;
				level1_Button.Pressed += Level1Action;
			}
		}
	}

	private void BackToMenuAction()
	{
		GetTree().ChangeSceneToFile(backToMenuScenePath);
	}

	private void Level0Action()
	{
		GetTree().ChangeSceneToFile(level0ScenePath);
	}

	private void Level1Action()
	{
		GetTree().ChangeSceneToFile(level1ScenePath);
	}

	public override void _Process(double delta)
	{
	}
}
