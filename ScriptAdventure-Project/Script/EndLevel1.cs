using Godot;
using MEC;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class EndLevel1 : Area2D
{
	[Export] Sprite2D elevatorDoor;
	[Export] CollisionShape2D elevatorCollision;
	[Export] AudioStreamPlayer audioStreamPlayer;
	[Export] DialogUiControl dialogUiControl;
	[Export] CharMovementControl charMovementControl;
	private CharController charController;
	private RigidBody2D body;
	private CoroutineHandle coroutine;
	private bool isFirstEnter;
	private bool isTextStarted;
	
	public override void _Ready()
	{
		charController = (CharController) Static_NodeMethod.GetChildType(charMovementControl, "Sprite2D");
		isFirstEnter = true;
		isTextStarted = false;
	}

	public override void _Process(double delta)
	{
		if(this.HasOverlappingBodies() && body == null)
		{
			body = (RigidBody2D) GetOverlappingBodies()[0];
			Timing.RunCoroutine(StartEnding());
		}
		else if(!this.HasOverlappingBodies() && body != null)
		{
			body = null;
		}
	}

	private IEnumerator<double> StartEnding()
	{
		if(isFirstEnter)
		{
			charController.IsMovementEnabled = false;
			audioStreamPlayer.Play();
			coroutine = Timing.RunCoroutine(Static_AnimationMethod.MakeAnimation(elevatorDoor, 0, 3));
			yield return Timing.WaitUntilDone(coroutine);
			elevatorDoor.ZIndex = 0;
			elevatorCollision.Disabled = true;
			dialogUiControl.AddText("L'ascensore si è aperto. Sembra che qualcuno voglia che prosegui.");
			dialogUiControl.DisplayText();
			isFirstEnter = false;
			charController.IsMovementEnabled = true;
		}
	}
}
