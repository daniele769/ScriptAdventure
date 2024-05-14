using Godot;
using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class Level1 : Node
{
	[Export] FadeInOut fadeInOut;
	[Export] Sprite2D elevatorDoor;
	[Export] AudioStreamPlayer elevatorDoorSound;
	[Export] AudioStreamPlayer loopMusicPlayer;
	[Export] DialogUiControl dialogUiControl;
	[Export] CharMovementControl charMovementControl;
	private CharController charController;
	private CoroutineHandle coroutine;
	private bool isGameJustStarted;
	private bool isTextStarted;

	public override void _Ready()
	{
		if(!PlayerPrefs.ListKeys.Contains(Costanti.level1))
		{
			PlayerPrefs.SetValue<bool>(Costanti.level1, true);
		}
		float volume = loopMusicPlayer.VolumeDb;
		loopMusicPlayer.VolumeDb = -30;
		Tween tween = loopMusicPlayer.CreateTween();
		tween.TweenProperty(loopMusicPlayer, "volume_db", volume, 2);
		tween.TweenCallback(Callable.From(() => tween.Dispose()));
		loopMusicPlayer.Play();

		fadeInOut.FadeIn(0);
		charController = (CharController) Static_NodeMethod.GetChildType(charMovementControl, "Sprite2D");
		charController.IsMovementEnabled = false;
		isGameJustStarted = true;
		isTextStarted = false;
		charController.Frame = 16;
	}

	public override void _Process(double delta)
	{
		if(isGameJustStarted)
		{
			Timing.RunCoroutine(StartScene());
			isGameJustStarted = false;
		}
		if(dialogUiControl.AreDialogFinished && !charController.IsMovementEnabled && isTextStarted && dialogUiControl.Visible == false)
		{
			charController.IsMovementEnabled = true;
			isTextStarted = false;
		}
	}

	private IEnumerator<double> StartScene()
	{
		fadeInOut.FadeOut(2);
		yield return Timing.WaitForSeconds(2);
		OpenDoor();
		yield return Timing.WaitUntilDone(coroutine);
		elevatorDoor.ZIndex = 0;
		yield return Timing.WaitForSeconds(1);
		dialogUiControl.AddText("Bene, adesso sei finalmente dentro il gioco.");
		dialogUiControl.AddText("Esci dall'ascensore.");
		dialogUiControl.DisplayText();
		isTextStarted = true;
	}

    private void OpenDoor()
	{
		elevatorDoorSound.Play();
		coroutine = Timing.RunCoroutine(Static_AnimationMethod.MakeAnimation(elevatorDoor, 0, 3));
	}

}
