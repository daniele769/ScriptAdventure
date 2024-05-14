using Godot;
using System;

public partial class BridgeRotationControl : RigidBody2D, Interactable
{
	[Export] Sprite2D gear;
	[Export] Sprite2D bridge;
	[Export] CollisionShape2D colliderStart;
	[Export] CollisionShape2D colliderEnd;
	[Export] float angleRotation = -90;
	[Export] float rotationSpeed = 137.5f;
	private float counter;
	private bool isWayOpen = false;
	private bool isStopped = true;
	private bool startRotation = false;
	private bool rollbackRotation = false;
	private float bridgeDefRot;
	private SwitchControl switchControl;
	private AudioStreamPlayer audioStreamPlayer;
	private ConsoleInteraction consoleInteraction;


	public override void _Ready()
	{
		counter = 0;
		bridgeDefRot = bridge.RotationDegrees;
		audioStreamPlayer = (AudioStreamPlayer) Static_NodeMethod.GetChildType(this, "AudioStreamPlayer");
		consoleInteraction = new ConsoleInt_Bridge(this);
	}

	public void StartRotation(SwitchControl control)
	{
		if (!isWayOpen && isStopped)
		{
			switchControl = control;
			switchControl.GetConsoleInteraction().isPlaying = true;
			isStopped = false;
			startRotation = true;
			audioStreamPlayer.Play();
			return;
		}
		
	}

	public void StartRollback(SwitchControl control)
	{
		if(isStopped)
		{
			switchControl = control;
			switchControl.GetConsoleInteraction().isPlaying = true;
			rollbackRotation = true;
			isStopped = false;
			audioStreamPlayer.Play();
		}
	}

	public override void _Process(double delta)
	{
		if (startRotation)
		{
			if (counter < Math.Abs(angleRotation))
			{
				isStopped = false;
				counter += (float)delta * rotationSpeed;
				gear.RotationDegrees -= (float)delta * rotationSpeed;
				bridge.RotationDegrees -= (float)delta * rotationSpeed;
			}
			else if (!isStopped)
			{
				audioStreamPlayer.Stop();
				bridge.RotationDegrees = bridgeDefRot + angleRotation;
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
			if(counter < Math.Abs(angleRotation))
			{
				isStopped = false;
				counter += (float)delta * rotationSpeed;
				gear.RotationDegrees += (float)delta * rotationSpeed;
				bridge.RotationDegrees += (float)delta * rotationSpeed;
			}
			else if (!isStopped)
			{
				audioStreamPlayer.Stop();
				bridge.RotationDegrees = bridgeDefRot;
				isStopped = true;
				ActivateBridgeMapCollision();
				rollbackRotation = false;	
				counter = 0;	
				switchControl.GetConsoleInteraction().isPlaying = false;	
			}
			return;
		}
	}

	private void DisableBridgeMapCollision()
	{
		isWayOpen = true;
		colliderEnd.Disabled = true;
		colliderStart.Disabled = true;
	}

	private void ActivateBridgeMapCollision()
	{
		isWayOpen = false;
		colliderEnd.Disabled = false;
		colliderStart.Disabled = false;
	}

	public void Action()
	{
		consoleInteraction._Action();
	}

	public ConsoleInteraction GetConsoleInteraction()
	{
		GD.Print("Get console Bridge iteraction");
		return consoleInteraction;
	}

	public Node GetNode()
	{
		return this;
	}

}
