using Godot;
using System;

public partial class PuzzleSolved : Node2D
{
	[Export] PuzzleConsoleControl puzzleConsoleControl;
	[Export] NotInteractableDoor notInteractableDoor;
	[Export] AudioStreamPlayer audioStreamPlayer;
	[Export] AudioStream correctSound;

	public override void _Ready()
	{
		ConnectSignal();
	}

	private void ConnectSignal()
	{
		puzzleConsoleControl.Connect(PuzzleConsoleControl.SignalName.IsSolved, Callable.From(() => Solved()));
		puzzleConsoleControl.Connect(PuzzleConsoleControl.SignalName.NotSolved, Callable.From(() => NotSolved()));
	}

	private void Solved()
	{
		audioStreamPlayer.Stream = correctSound;
		audioStreamPlayer.Play();
		notInteractableDoor.OpenAnimation();
	}

	private void NotSolved()
	{
		notInteractableDoor.CloseAnimation();
	}

	
	public override void _Process(double delta)
	{
	}
}
