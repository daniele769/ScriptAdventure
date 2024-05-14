using Godot;
using System;

public partial class CreditControl : CanvasLayer
{
	[Export] public Button backToMenu;

	public override void _Ready()
	{
		backToMenu.Pressed += BackToMenuAction;
	}

	private void BackToMenuAction()
	{
		GetTree().ChangeSceneToFile("res://Scene/Menu.tscn");
	}
}
