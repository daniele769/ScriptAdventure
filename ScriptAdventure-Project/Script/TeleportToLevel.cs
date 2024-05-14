using Godot;
using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class TeleportToLevel : Area2D
{
	[Export] string nextLevelPath;
	[Export] Sprite2D elevatorSprite;
	[Export] CharMovementControl character;
	[Export] AudioStream elevatorSound;
	[Export] AudioStream openDoorSound;
	[Export] AudioStreamPlayer soundPlayer;
	[Export] FadeInOut fadeBlack;

	private CoroutineHandle coroutine;
	private CharController charController;
	private PackedScene nextLevel;

	public override void _Ready()
	{
		charController = (CharController) Static_NodeMethod.GetChildType(character, "Sprite2D");
		nextLevel = GD.Load<PackedScene>(nextLevelPath);
	}

	private void StartAnimation()
	{
		elevatorSprite.ZIndex = 1;
		coroutine = Timing.RunCoroutine(Static_AnimationMethod.MakeAnimation(elevatorSprite, 3, 0));
		soundPlayer.Stream = openDoorSound;
		soundPlayer.Play();
	}

	private IEnumerator<double> ChangeLevel()
	{
		yield return Timing.WaitUntilDone(coroutine);
		while(soundPlayer.Playing)
		{
			yield return Timing.WaitForOneFrame;
		}
		fadeBlack.FadeIn(1f);
		soundPlayer.Stream = elevatorSound;
		soundPlayer.Play();
		yield return Timing.WaitForSeconds(1);	
		while(soundPlayer.Playing)
		{
			yield return Timing.WaitForOneFrame;
		}
		GetTree().ChangeSceneToPacked(nextLevel);
	}

	public void OnBodyEntered(Node2D body)
	{
		GD.Print("Teleport");
		charController.IsMovementEnabled = false;
		Timing.KillCoroutines();
		StartAnimation();
		Timing.RunCoroutine(ChangeLevel());
	}
	
}
