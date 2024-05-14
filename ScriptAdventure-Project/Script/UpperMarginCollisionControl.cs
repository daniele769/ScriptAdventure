using Godot;
using System;

public partial class UpperMarginCollisionControl : Area2D
{
	[Export] int layerToOverride;
	private RigidBody2D body;
	
	public override void _Ready()
	{
	}

	
	public override void _Process(double delta)
	{
		if(this.HasOverlappingBodies())
		{
			//GD.Print("Collision");
			body = (RigidBody2D) GetOverlappingBodies()[0];
			body.SetCollisionMaskValue(layerToOverride, false);
			
		}
		else if(body != null)
		{
			body.SetCollisionMaskValue(layerToOverride, true);
			body = null;
		}
	}
}
