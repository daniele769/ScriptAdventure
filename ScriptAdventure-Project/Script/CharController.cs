using Godot;
using System;
using MEC;
using System.Collections.Generic;

public partial class CharController : Sprite2D
{
	private CoroutineHandle myCoroutine;
	private string direction;
	private string currentDir;
	private bool isAnimPlaying;
	private bool isMovementEnabled;
	private bool isAnimationEnabled;

    public string Direction { get => direction; set => direction = value; }
    public bool IsMovementEnabled { get => isMovementEnabled; set => isMovementEnabled = value; }
    public bool IsAnimationEnabled { get => isAnimationEnabled; set => isAnimationEnabled = value; }


    [Signal] public delegate void DirectionChanged_EventHandler();


    public override void _Ready()
	{
		Direction = "";
		currentDir = "";
		isAnimPlaying = false;
		isMovementEnabled = true;
		isAnimationEnabled = true;
	}

	public override void _Process(double delta)
	{
		AnimationControl();
	}

	private void AnimationControl()
	{
		CheckInput();
		if (IsDirectionChanged() && isAnimationEnabled)
		{
			if (Direction == Costanti.DirectionLeft || Direction == Costanti.DirectionLeftUp || Direction == Costanti.DirectionLeftDown)
			{
				isAnimPlaying = true;
				myCoroutine = Timing.RunCoroutine(MakeAnimation(4, 7));
			}
			else
			if (Direction == Costanti.DirectionRight || Direction == Costanti.DirectionRightUp || Direction == Costanti.DirectionRightDown)
			{
				isAnimPlaying = true;
				myCoroutine = Timing.RunCoroutine(MakeAnimation(8, 11));
			}
			else
			if (Direction == Costanti.DirectionUp)
			{
				isAnimPlaying = true;
				myCoroutine = Timing.RunCoroutine(MakeAnimation(12, 15));
			}
			else
			if (Direction == Costanti.DirectionDown)
			{
				isAnimPlaying = true;
				myCoroutine = Timing.RunCoroutine(MakeAnimation(0, 3));
			}
			currentDir = Direction;

		}
	}

	private IEnumerator<double> MakeAnimation(int coefficienteStart, int coefficienteEnd)
	{
		float time = 0.12f;
		for (int i = coefficienteStart; i < coefficienteEnd; i++)
		{
			this.Frame = i;
			yield return Timing.WaitForSeconds(time);
		}
		myCoroutine = Timing.RunCoroutine(this.MakeAnimation(coefficienteStart, coefficienteEnd), Costanti.PlayerAnimationTag);
	}

	private void CheckInput()
	{
		if(!isMovementEnabled)
		{
			Direction = "";
			return;
		}
		if (Input.IsActionPressed(Costanti.MoveLeft) & Input.IsActionPressed(Costanti.MoveUp))
		{
			Direction = Costanti.DirectionLeftUp;
		}
		else
		if (Input.IsActionPressed(Costanti.MoveLeft) & Input.IsActionPressed(Costanti.MoveDown))
		{
			Direction = Costanti.DirectionLeftDown;
		}
		else
		if (Input.IsActionPressed(Costanti.MoveLeft))
		{
			Direction = Costanti.DirectionLeft;
		}
		else
		if (Input.IsActionPressed(Costanti.MoveRight) & Input.IsActionPressed(Costanti.MoveDown))
		{
			Direction = Costanti.DirectionRightDown;
		}
		else
		if (Input.IsActionPressed(Costanti.MoveRight) & Input.IsActionPressed(Costanti.MoveUp))
		{
			Direction = Costanti.DirectionRightUp;
		}
		else
		if (Input.IsActionPressed(Costanti.MoveRight))
		{
			Direction = Costanti.DirectionRight;
		}
		else
		if (Input.IsActionPressed(Costanti.MoveUp))
		{
			Direction = Costanti.DirectionUp;
		}
		else
		if (Input.IsActionPressed(Costanti.MoveDown))
		{
			Direction = Costanti.DirectionDown;
		}
		else
			Direction = "";
	}

	public void StopAnimation()
	{
		if (isAnimPlaying)
		{
			Timing.KillCoroutines(myCoroutine);
			SetIdleSPrite();
			isAnimPlaying = false;
		}
	}

	bool IsDirectionChanged()
	{
		if (Direction.Equals(currentDir))
		{
			return false;
		}
		StopAnimation();
		currentDir = Direction;
		EmitSignal(SignalName.DirectionChanged_);
		return true;
	}

	private void SetIdleSPrite()
	{
		if (currentDir == Costanti.DirectionLeft || Direction == Costanti.DirectionLeftUp || Direction == Costanti.DirectionLeftDown)
		{
			this.Frame = 18;
		}
		else
		if (currentDir == Costanti.DirectionRight || Direction == Costanti.DirectionRightUp || Direction == Costanti.DirectionRightDown)
		{
			this.Frame = 19;
		}
		else
		if (currentDir == Costanti.DirectionUp)
		{
			this.Frame = 17;
		}
		else
		if (currentDir == Costanti.DirectionDown)
		{
			this.Frame = 16;
		}
	}
}
