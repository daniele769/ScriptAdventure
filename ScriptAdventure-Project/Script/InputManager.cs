using Godot;
using System;
using MEC;

public partial class InputManager : Node2D
{
	[Export] Node2D characterNode;
	[Export] ScriptConsole scriptConsole;
	[Export] DocControl dockControl;
	[Export] CanvasLayer canvasLayer;
	private CharController charController;
	public ConsoleInteraction selectedObject;
	private MenuControl menuControl;
	private bool isMenuOpened = false;
	private bool isMenuEnabled = true;

    public bool IsMenuEnabled { get => isMenuEnabled; set => isMenuEnabled = value; }


    public override void _Ready()
    {
		//isMenuEnabled = true;
        charController = (CharController) Static_NodeMethod.GetInternalChildType(characterNode, "Sprite2D");
		if(charController == null)
			GD.PrintErr("CharController is null");
    }

    public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed(Costanti.Interact))
		{
			Interact();
		}

		if (@event.IsActionPressed(Costanti.OpenConsole))
		{
			OpenConsole();

		}

		if (@event.IsActionPressed(Costanti.LogCodeUp))
		{
			LogCodeUp();
		}

		if (@event.IsActionPressed(Costanti.LogCodeDown))
		{
			LogCodeDown();
		}

		if (@event.IsActionPressed(Costanti.OpenDock))
		{
			OpenDoc();
		}

		if (isMenuEnabled && @event.IsActionPressed(Costanti.OpenMenu))
		{
			OpenMenu();
		}
	}

	private void Interact()
	{
		if (scriptConsole != null && dockControl != null && scriptConsole.Visible && dockControl.Visible == false)
		{
			return;
		}
		Focus_InteractiveObj focus = (Focus_InteractiveObj)Static_NodeMethod.GetChildType(characterNode, "RayCast2D");
		if (focus.Body == null)
		{
			GD.Print("raycast null");
			return;
		}
		if (focus.Body != null)
		{
			GD.Print("Interact Action");
			Interactable obj = (Interactable)focus.Body;
			GD.Print("Seleceted obj for interaction " + obj.ToString());
			
			obj.Action();
		}
	}

	private void OpenConsole()
	{
		if(scriptConsole == null)
			return;
		if (scriptConsole.Visible)
		{
			if (dockControl.Visible)
				return;
			scriptConsole.Visible = false;
			charController.IsMovementEnabled = true;
			return;
		}

		Focus_InteractiveObj focus = (Focus_InteractiveObj)Static_NodeMethod.GetChildType(characterNode, "RayCast2D");
		if (focus.Body == null)
		{
			GD.Print("raycast null");
			return;
		}
		if (focus.Body != null)
		{
			Interactable switchControl = (Interactable)focus.Body;
			selectedObject = switchControl.GetConsoleInteraction();
			scriptConsole.SelectedObj = selectedObject;
			GD.Print("ObjectConsole selected " + scriptConsole.SelectedObj.ToString());
			scriptConsole.Visible = true;
			scriptConsole.ProcessMode = Node.ProcessModeEnum.Inherit;
			charController.IsMovementEnabled = false;
			charController.StopAnimation();

		}
	}

	private void LogCodeUp()
	{
		if (scriptConsole.Visible && scriptConsole.LogCodeList.Count != 0 && scriptConsole.posLog < scriptConsole.LogCodeList.Count - 1)
		{
			if (!scriptConsole.TextEdit.Text.Equals("") && !scriptConsole.TextEdit.Text.Equals(scriptConsole.LogCodeList[scriptConsole.LogCodeList.Count - 1 - scriptConsole.posLog]))
				return;
			scriptConsole.posLog++;
			scriptConsole.TextEdit.Text = scriptConsole.LogCodeList[scriptConsole.LogCodeList.Count - 1 - scriptConsole.posLog];
		}
	}

	private void LogCodeDown()
	{
		if (scriptConsole.Visible && scriptConsole.LogCodeList.Count != 0 && scriptConsole.posLog > 0)
		{
			if (!scriptConsole.TextEdit.Text.Equals("") && !scriptConsole.TextEdit.Text.Equals(scriptConsole.LogCodeList[scriptConsole.LogCodeList.Count - 1 - scriptConsole.posLog]))
				return;
			scriptConsole.posLog--;
			scriptConsole.TextEdit.Text = scriptConsole.LogCodeList[scriptConsole.LogCodeList.Count - 1 - scriptConsole.posLog];
		}
	}

	private void OpenDoc()
	{
		if (scriptConsole.Visible)
		{
			if (dockControl.Visible)
				dockControl.Visible = false;
			else
				dockControl.Visible = true;
		}
	}

	private void OpenMenu()
	{
		if(!isMenuOpened){
			menuControl = (MenuControl) GD.Load<PackedScene>("res://Scene/Menu.tscn").Instantiate().GetChild(0);
			menuControl.newGameButton.Visible = false;
			menuControl.exerciseModeButton.Visible = false;
			menuControl.selectLevelButton.Visible = false;
			menuControl.creditButton.Visible = false;
			Button closeButton = menuControl.GetCloseButton();
			Button backToTitleButton = menuControl.GetBackToTitleButton();
			closeButton.Pressed += OpenMenu;
			closeButton.Visible = true;
			backToTitleButton.Visible = true;
			menuControl.Reparent(canvasLayer);
			isMenuOpened = true;
			return;
		}

		if(isMenuOpened && menuControl.Visible){
			menuControl.QueueFree();
			isMenuOpened = false;
			return;
		}
	}
}
