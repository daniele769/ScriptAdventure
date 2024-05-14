using Godot;
using System;
using System.Collections.Generic;

public partial class Focus_InteractiveObj : RayCast2D
{
	[Export] CharController charController;

	private RigidBody2D body;
	private RigidBody2D previousBody = null;
	
    public RigidBody2D Body { get => body; set => body = value; }

	private void UpdateRaycastDirection(){
		string Direction = charController.Direction;
		//GD.Print("Dir changed");
		if (Direction == Costanti.DirectionLeft || Direction == Costanti.DirectionLeftUp || Direction == Costanti.DirectionLeftDown)
			{
				this.TargetPosition = 100 * Vector2.Left;
			}
			else
			if (Direction == Costanti.DirectionRight || Direction == Costanti.DirectionRightUp || Direction == Costanti.DirectionRightDown)
			{
				this.TargetPosition = 100 * Vector2.Right;
			}
			else
			if (Direction == Costanti.DirectionUp)
			{
				this.TargetPosition = 100 * Vector2.Up;
			}
			else
			if (Direction == Costanti.DirectionDown)
			{
				this.TargetPosition = 100 * Vector2.Down;
			}
	}



	
	public override void _Process(double delta)
	{
		body = (RigidBody2D) GetCollider();
		if(body != previousBody)
		{
			if(body != null)
			{
				SetMaterialToAllSprite();
			}
			if(previousBody == null)
			{
				previousBody = body;
				return;
			}
			if(body == null)
			{
				ReleaseMaterialToPreviousBody();
				previousBody = body;
				return;
			}
		}
		
		//GD.Print(body.ToString());
	}
	private void SetMaterialToAllSprite()
	{
		List<Node> allNode = new List<Node>();
		allNode = (List<Node>) Static_NodeMethod.GetAllChild(allNode, body);

		List<Sprite2D> spriteList = Static_NodeMethod.GetAllChildWithTypeInList<Sprite2D>(allNode, "Sprite2D");
		foreach(Sprite2D sprite in spriteList)
		{
			Shader shader = GD.Load<Shader>("res://Shader/Outline.gdshader");
			ShaderMaterial shaderMaterial = new ShaderMaterial();
			shaderMaterial.Shader = shader;
			shaderMaterial.SetShaderParameter("width", 3f);
			shaderMaterial.SetShaderParameter("pattern", 2);
			shaderMaterial.SetShaderParameter("color", new Vector4(255, 255, 255, 199));
			shaderMaterial.SetShaderParameter("add_margins", true);
			sprite.Material = shaderMaterial;
		}
	}

	private void ReleaseMaterialToPreviousBody()
	{
		List<Node> allNode = new List<Node>();
		allNode = (List<Node>) Static_NodeMethod.GetAllChild(allNode, previousBody);

		List<Sprite2D> spriteList = Static_NodeMethod.GetAllChildWithTypeInList<Sprite2D>(allNode, "Sprite2D");
		foreach(Sprite2D previousSprite in spriteList)
		{
			previousSprite.Material = null;
		}
	}
}
