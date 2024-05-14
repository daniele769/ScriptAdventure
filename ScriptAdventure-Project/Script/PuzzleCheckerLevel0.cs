using Godot;
using MEC;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class PuzzleCheckerLevel0 : Node2D
{
	[Export] BoxScript_Spawner[] consoleArray;
	[Export] RigidBody2D elevator;
	[Export] AudioStream openDoorSound;
	[Export] AudioStreamPlayer soundPlayer;
	[Export] CharMovementControl charMovementControl;
	[Export] DialogUiControl dialogUiControl;
	[Export] AudioStreamPlayer audioStreamPlayer;
	[Export] InputManager inputManager;

	private CharController charController;
	private Sprite2D elevatorSprite;
	private bool isPuzzleSolved;
	private bool isConsole0DialogFinished;
	private bool isConsole1DialogFinished;
	private bool isConsole2DialogFinished;
	private bool isConsole3DialogFinished;
	private bool isAllConsoleSolvedDialogFinished;
	private CoroutineHandle coroutine;
	private CollisionShape2D collider;
	
	
	public override void _Ready()
	{
		inputManager.IsMenuEnabled = false;
		charController = (CharController) Static_NodeMethod.GetChildType(charMovementControl, "Sprite2D");
		elevatorSprite = (Sprite2D) Static_NodeMethod.GetChildType(elevator, "Sprite2D");
		collider = (CollisionShape2D) Static_NodeMethod.GetChildType(elevator, "CollisionShape2D");
		isPuzzleSolved = false;
		isConsole0DialogFinished = false;
		isConsole1DialogFinished = false;
		isConsole2DialogFinished = false;
		isAllConsoleSolvedDialogFinished = false;
	}

	public override void _Process(double delta)
	{
		if(!isConsole0DialogFinished && consoleArray[0].state == (int)BoxScript_Spawner.State.correct && 
			!IsInstanceValid(((ConsoleInt_BoxScriptSpawner)consoleArray[0].ConsoleInteraction).PanelContainer))
		{
			
			dialogUiControl.AddText("Bene, adesso le animazioni dovrebbero funzionare");
			dialogUiControl.DisplayText(charController);
			isConsole0DialogFinished = true;
			charController.IsAnimationEnabled = true;
		}
		else
		if(!isConsole1DialogFinished && consoleArray[1].state == (int)BoxScript_Spawner.State.correct && 
			!IsInstanceValid(((ConsoleInt_BoxScriptSpawner)consoleArray[1].ConsoleInteraction).PanelContainer))
		{
			dialogUiControl.AddText("Pare che ora funzioni anche la musica");
			dialogUiControl.DisplayText(charController);
			isConsole1DialogFinished = true;

			float volume = audioStreamPlayer.VolumeDb;
			audioStreamPlayer.VolumeDb = -30;
			Tween tween = audioStreamPlayer.CreateTween();
			tween.TweenProperty(audioStreamPlayer, "volume_db", volume, 2);
			tween.TweenCallback(Callable.From(() => tween.Dispose()));
			audioStreamPlayer.Play();
		}
		else
		if(!isConsole2DialogFinished && consoleArray[2].state == (int)BoxScript_Spawner.State.correct && 
			!IsInstanceValid(((ConsoleInt_BoxScriptSpawner)consoleArray[2].ConsoleInteraction).PanelContainer))
		{
			
			dialogUiControl.AddText("Ottimo, ora il menu dovrebbe funzionare");
			dialogUiControl.DisplayText(charController);
			isConsole2DialogFinished = true;
			inputManager.IsMenuEnabled = true;
		}
		else
		if(!isConsole3DialogFinished && consoleArray[3].state == (int)BoxScript_Spawner.State.correct && 
			!IsInstanceValid(((ConsoleInt_BoxScriptSpawner)consoleArray[3].ConsoleInteraction).PanelContainer))
		{
			
			dialogUiControl.AddText("Accidenti, pensavo di aver cancellato questo codice.");
			dialogUiControl.AddText("Inizialmente volevo inserire un minigioco con le battaglie navali ma poi ho cambiato idea.");
			dialogUiControl.AddText("Ahahah...colpa mia.");
			dialogUiControl.DisplayText(charController);
			isConsole3DialogFinished = true;
		}

		if(!isPuzzleSolved && CheckIfPuzzleSolved())
		{
			isPuzzleSolved = true;
			StartAnimation(0, 3);
			soundPlayer.Stream = openDoorSound;
			soundPlayer.Play();
			collider.Disabled = true;
			return;
		}

		if(isConsole3DialogFinished && isConsole0DialogFinished && isConsole1DialogFinished && 
			isConsole2DialogFinished && !isAllConsoleSolvedDialogFinished &&
			!IsInstanceValid(((ConsoleInt_BoxScriptSpawner)consoleArray[0].ConsoleInteraction).PanelContainer) &&
			!IsInstanceValid(((ConsoleInt_BoxScriptSpawner)consoleArray[1].ConsoleInteraction).PanelContainer) &&
			!IsInstanceValid(((ConsoleInt_BoxScriptSpawner)consoleArray[2].ConsoleInteraction).PanelContainer) &&
			!IsInstanceValid(((ConsoleInt_BoxScriptSpawner)consoleArray[3].ConsoleInteraction).PanelContainer) &&
			dialogUiControl.AreDialogFinished && !dialogUiControl.HaveToGoNewDialog && dialogUiControl.Visible == false)
		{
			
			dialogUiControl.AddText("Bene pare che tu abbia risolto tutti gli errori.");
			dialogUiControl.AddText("Adesso puoi usare l'ascensore per entrare nel gioco vero e proprio");
			dialogUiControl.DisplayText(charController);
			isAllConsoleSolvedDialogFinished = true;
		}
	}

	private void StartAnimation(int startIndex, int lastIndex)
	{
		GD.Print("StartAnimation");
		coroutine = Timing.RunCoroutine(Static_AnimationMethod.MakeAnimation(elevatorSprite, startIndex, lastIndex));
	}

	private bool CheckIfPuzzleSolved()
	{
		foreach(BoxScript_Spawner console in consoleArray)
		{
			if(console.state != (int)BoxScript_Spawner.State.correct)
			{
				return false;
			}
		}
		return true;
	}
}
