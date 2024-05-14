using Godot;
using MEC;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class BridgeControlThreeGears : RigidBody2D, Interactable
{
	[Export] Sprite2D gear1;
	[Export] Sprite2D gear2;
	[Export] Sprite2D gear3;
	[Export] Sprite2D bridge1;
	[Export] Sprite2D bridge2;
	[Export] CollisionShape2D collider1Start;
	[Export] CollisionShape2D collider1End;
	[Export] CollisionShape2D collider2Start;
	[Export] CollisionShape2D collider2End;
	[Export] private AudioStreamPlayer gearSoundPlayer;
	[Export] private AudioStreamPlayer clockTimeSoundPlayer;
	[Export] float angleRotation = 90;
	[Export] float bridgeRotationSpeed = 137.5f;
	[Export] float gearRotationSpeed = 50;
	[Export] float timeToRollback = 5;
	private CoroutineHandle coroutine;
	private float counter = 0;
	private float ingranaggio1DefRot;
	private float ingranaggio2DefRot;
	private float ingranaggio3DefRot;
	private float bridge1DefRot;
	private float bridge2DefRot;
	private bool isWayOpen = false;
	private bool isStopped = true;
	private bool startRotation = false;
	private bool rollbackRotation = false;
	private SwitchControl switchControl;
	
	private ConsoleInteraction consoleInteraction;

	public override void _Ready()
	{
		bridge1DefRot = bridge1.RotationDegrees;
		bridge2DefRot = bridge2.RotationDegrees;
		consoleInteraction = new ConsoleInt_BridgeThreeGears(this);
	}

	public void StartRotation(SwitchControl control)
	{
		Timing.KillCoroutines(coroutine);
		clockTimeSoundPlayer.Stop();
		if (!isWayOpen && isStopped)
		{
			coroutine = Timing.RunCoroutine(CountRollback(control));
			switchControl = control;
			switchControl.GetConsoleInteraction().isPlaying = true;
			isStopped = false;
			startRotation = true;
			gearSoundPlayer.Play();
			return;
		}
		
	}

	public void StartRollback(SwitchControl control)
	{
		Timing.KillCoroutines(coroutine);
		clockTimeSoundPlayer.Stop();
		if(isStopped)
		{
			switchControl = control;
			switchControl.GetConsoleInteraction().isPlaying = true;
			rollbackRotation = true;
			isStopped = false;
			gearSoundPlayer.Play();
		}
	}

	
	public override void _Process(double delta)
	{
		if (startRotation)
		{
			if (counter < angleRotation)
			{
				GD.Print("Start Rotation");
				isStopped = false;
				counter += (float)delta * bridgeRotationSpeed;
				gear1.RotationDegrees += (float)delta * bridgeRotationSpeed;
				gear2.RotationDegrees += (float)delta * bridgeRotationSpeed;
				gear3.RotationDegrees -= (float)delta * gearRotationSpeed;
				bridge1.RotationDegrees += (float)delta * bridgeRotationSpeed;
				bridge2.RotationDegrees += (float)delta * bridgeRotationSpeed;
			}
			else if (!isStopped)
			{
				GD.Print("Stop Rotation");
				gearSoundPlayer.Stop();
				bridge1.RotationDegrees = bridge1DefRot + angleRotation;
				bridge2.RotationDegrees = bridge2DefRot + angleRotation;
				isStopped = true;
				DisableBridgeMapCollision();
				startRotation = false;	
				counter = 0;	
				switchControl.GetConsoleInteraction().isPlaying = false;	
			}
			return;
		}

		if(rollbackRotation)
		{
			if(counter < angleRotation)
			{
				GD.Print("Start Rollback");
				isStopped = false;
				counter += (float)delta * bridgeRotationSpeed;
				gear1.RotationDegrees -= (float)delta * bridgeRotationSpeed;
				gear2.RotationDegrees -= (float)delta * bridgeRotationSpeed;
				gear3.RotationDegrees += (float)delta * gearRotationSpeed;
				bridge1.RotationDegrees -= (float)delta * bridgeRotationSpeed;
				bridge2.RotationDegrees -= (float)delta * bridgeRotationSpeed;
			}
			else if (!isStopped)
			{
				GD.Print("Stop Rollback");
				gearSoundPlayer.Stop();
				bridge1.RotationDegrees = bridge1DefRot;
				bridge2.RotationDegrees = bridge2DefRot;
				isStopped = true;
				ActivateBridgeMapCollision();
				rollbackRotation = false;	
				counter = 0;	
				switchControl.GetConsoleInteraction().isPlaying = false;	
			}
			return;
		}
	}

	private IEnumerator<double> CountRollback(SwitchControl switchControl)
	{
		if(!clockTimeSoundPlayer.Playing)
			clockTimeSoundPlayer.Play();
		yield return Timing.WaitForSeconds(timeToRollback);
		clockTimeSoundPlayer.Stop();
		((ConsoleInt_Switch)switchControl.GetConsoleInteraction())._Action();
	}

	private void DisableBridgeMapCollision()
	{
		isWayOpen = true;
		collider2End.Disabled = true;
		collider2Start.Disabled = true;
		collider1End.Disabled = false;
		collider1Start.Disabled = false;
	}

	private void ActivateBridgeMapCollision()
	{
		isWayOpen = false;
		collider2End.Disabled = false;
		collider2Start.Disabled = false;
		collider1End.Disabled = true;
		collider1Start.Disabled = true;
	}

    public void Action()
    {
        throw new NotImplementedException();
    }

    public ConsoleInteraction GetConsoleInteraction()
    {
        return consoleInteraction;
    }

    public Node GetNode()
    {
        return this;
    }

}
