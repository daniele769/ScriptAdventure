using Godot;
using System;
using System.Linq;

public partial class Level0 : Node2D
{
	[Export] FadeInOut fadeInOut;
	[Export] DialogUiControl dialogUiControl;
	[Export] AudioStreamPlayer audioStreamPlayer;
	[Export] CharMovementControl charMovementControl;
	[Export] InputManager inputManager;
	private CharController charController;
	private bool isGameJustStarted;
	private bool isFadeOutStarted;
	private bool isMovementDialogFinished;
	private bool isMenuDialogFinished;
	
	
	public override void _Ready()
	{
		if(!PlayerPrefs.ListKeys.Contains(Costanti.level0))
		{
			PlayerPrefs.SetValue<bool>(Costanti.level0, true);
		}
		fadeInOut.FadeIn(0);
		charController = (CharController) Static_NodeMethod.GetChildType(charMovementControl, "Sprite2D");
		charController.IsMovementEnabled = false;
		charController.IsAnimationEnabled = false;
		dialogUiControl.AddText("Bene, la connessione è avvenuta con successo.");
		dialogUiControl.AddText("La simulazione full-dive sta per cominciare");
		dialogUiControl.AddText("...");
		dialogUiControl.AddText("Pare che ci siano degli errori, è necessario risolverli per avviare il gioco.");
		dialogUiControl.DisplayText();
		isGameJustStarted = true;
		isFadeOutStarted = false;
		isMovementDialogFinished = false;
		
	}

	public override void _Process(double delta)
	{
		if(isGameJustStarted && fadeInOut.GetOpacity() == 1 && !dialogUiControl.Visible)
		{
			//GD.Print("FadeOut");
			fadeInOut.FadeOut(2);
			isFadeOutStarted = true;
			isGameJustStarted = false;
			return;

		}
		if(isFadeOutStarted && fadeInOut.GetOpacity() == 0)
		{
			dialogUiControl.AddText("Risolvi gli errori degli script in modalità full-dive.");
			dialogUiControl.AddText("Per interagire con le console avvicinati finchè non si illuminano e premi INVIO.");
			dialogUiControl.AddText("Per muoverti usa i tasti wasd.");
			dialogUiControl.AddText("Usa ESC per aprire il menu.");
			dialogUiControl.DisplayText(charController);
			isFadeOutStarted = false;
			return;
			//charController.IsMovementEnabled = true;
		}

		if(!isMovementDialogFinished && charController.IsMovementEnabled == true && (Input.IsActionPressed(Costanti.MoveUp) || 
			Input.IsActionPressed(Costanti.MoveDown) || Input.IsActionPressed(Costanti.MoveLeft) || Input.IsActionPressed(Costanti.MoveRight)))
		{
			dialogUiControl.AddText("Pare che le animazioni del tuo avatar non funzionino.");
			dialogUiControl.AddText("Prova a risolvere gli errori alle console.");
			dialogUiControl.DisplayText(charController);
			isMovementDialogFinished = true;
			return;
		}

		if(!isMenuDialogFinished && inputManager.IsMenuEnabled == false && Input.IsActionPressed(Costanti.OpenMenu))
		{
			dialogUiControl.AddText("Pare che il menu non funzioni");
			dialogUiControl.AddText("Prova a risolvere gli errori alle console.");
			dialogUiControl.DisplayText(charController);
			isMenuDialogFinished = true;
			return;
		}

	}
}
