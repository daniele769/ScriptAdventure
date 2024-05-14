using Godot;
using System;

public partial class KeybindingControl : CanvasLayer
{
	[Export] Button backToMenuButton;
	private MenuControl menuControl;

	public override void _Ready()
	{
		backToMenuButton.Pressed += BackToMenuAction;

	}

	private void BackToMenuAction()
	{
		if(menuControl == null){
			GetTree().ChangeSceneToFile("res://Scene/Menu.tscn");
			return;
		}
		menuControl.Visible = true;
		this.QueueFree();
		return;
		
	}

	public void SetMenuControl(MenuControl node)
	{
		menuControl = node;
	}
}
