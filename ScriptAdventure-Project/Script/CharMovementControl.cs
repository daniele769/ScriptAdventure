using Godot;
using System;

public partial class CharMovementControl : RigidBody2D
{
	private CharController charController;
	private float speed;
	public static Vector2 upLeft = Vector2.Up + Vector2.Left;
	public static Vector2 upRight = Vector2.Up + Vector2.Right;
	public static Vector2 downLeft = Vector2.Down + Vector2.Left;
	public static Vector2 downRight = Vector2.Down + Vector2.Right;

	public float Speed { get => speed; set => speed = value; }


	public override void _Ready()
	{
		Speed = 550f;
		charController = this.GetChild<CharController>(0);
	}

	public override void _Process(double delta)
	{
		Vector2 direction = GetDirectionVector();
		this.MoveAndCollide(direction * ((float) (550 * delta)));
	}

	private Vector2 GetDirectionVector(){
		string direction = charController.Direction;
		if (direction.Equals(Costanti.DirectionLeft))
		{
			return Vector2.Left;
		}
		if (direction.Equals(Costanti.DirectionRight))
		{
			return Vector2.Right;
		}
		if (direction.Equals(Costanti.DirectionUp))
		{
			return Vector2.Up;
		}
		if (direction.Equals(Costanti.DirectionDown))
		{
			return Vector2.Down;
		}
		if (direction.Equals(Costanti.DirectionLeftUp))
		{
			return upLeft.Normalized();
		}
		if (direction.Equals(Costanti.DirectionLeftDown))
		{
			return downLeft.Normalized();
		}
		if (direction.Equals(Costanti.DirectionRightUp))
		{
			return upRight.Normalized();
		}
		if (direction.Equals(Costanti.DirectionRightDown))
		{
			return downRight.Normalized();
		}

		return Vector2.Zero;
	}
}
