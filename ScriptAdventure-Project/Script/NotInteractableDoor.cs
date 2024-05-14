using Godot;
using MEC;
using System;
using System.Collections.Generic;

public partial class NotInteractableDoor : StaticBody2D
{
	[Export] Node parentSprite;
	[Export] int frameCount;
	[Export] CollisionShape2D collision;
	[Export] AudioStreamPlayer audioStreamPlayer;
	[Export] AudioStream openSound;
	private List<Sprite2D> spriteList;
	//private List<CoroutineHandle> coroutineList;

	public override void _Ready()
	{
		List<Node> allNode = new List<Node>();
		allNode = Static_NodeMethod.GetAllChild(allNode, parentSprite);
		spriteList = Static_NodeMethod.GetAllChildWithTypeInList<Sprite2D>(allNode, "Sprite2D");

	}

	public void OpenAnimation()
	{
		StartAnimation(0, frameCount);
		collision.Disabled = true;
	}

	public void CloseAnimation()
	{
		StartAnimation(frameCount, 0);
		collision.Disabled = false;
	}

	public void StartAnimation(int startIndex, int lastIndex)
	{
		audioStreamPlayer.Stream = openSound;
		audioStreamPlayer.Play();
		foreach(Sprite2D sprite in spriteList)
		{
			Timing.RunCoroutine(Static_AnimationMethod.MakeAnimation(sprite, startIndex, lastIndex));
		}
	}

	
	public override void _Process(double delta)
	{
	}
}
