using Godot;
using System;

public partial class DialogEvent : Area2D
{
	[Export] CharMovementControl charMovementControl;
	[Export] DialogUiControl dialogUiControl;
	private CharController charController;
	private bool isTextDisplayed;
	private bool isBodyEntered;
	
	public override void _Ready()
	{
		charController = (CharController) Static_NodeMethod.GetChildType(charMovementControl, "Sprite2D");
		isTextDisplayed = false;
		isBodyEntered = false;
	}

	private void OnBodyEntered(Node2D body)
	{
		if(isBodyEntered)
			return;
		charController.IsMovementEnabled = false;
		dialogUiControl.AddText("Che strano...");
		dialogUiControl.AddText("Pare che il mondo di gioco sia diverso in qualche modo.");
		dialogUiControl.AddText("In lontananza si vede un altro ascensore anche se non dovrebbe esistere. Prova a raggiungerlo.");
		dialogUiControl.AddText("In questo momento è attiva la modalità debug, andando vicino agli oggetti interagibili dovrebbero illuminarsi.");
		dialogUiControl.AddText("Una volta selezionato un oggetto puoi aprire la console di comando con il pulsante 'backslash'.");
		dialogUiControl.AddText("A quel punto puoi scrivere codice per interagire con gli oggetti nella scena.");
		dialogUiControl.AddText("Se non sai cosa fare puoi sempre aprire la documentazione di quell'oggetto con il pulsante apposito sopra la console.");
		dialogUiControl.AddText("In questo modo puoi controllare metodi e proprietà di un oggetto in modo da capire cosa fare");
		dialogUiControl.AddText("Bene! Detto questo prova ora a raggiungere quell'ascensore.");
		dialogUiControl.DisplayText();
		isTextDisplayed = true;
		isBodyEntered = true;
	}

	public override void _Process(double delta)
	{
		if(isTextDisplayed && dialogUiControl.AreDialogFinished)
		{
			isTextDisplayed = false;
			charController.IsMovementEnabled = true;
		}
	}
}
