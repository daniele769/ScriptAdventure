using Godot;
using NLog.Fluent;
using System;
using System.Diagnostics;

public partial class DraggableSpawner : Area2D
{
	private Vector2 mousePos;
	private bool mouse_in;
	private bool dragging;
	private Node parent;
	private Node instance;
	
	public override void _Ready()
	{
		if(Static_NodeMethod.GetParentWithType(this, "CanvasLayer") == null){
			parent = GetTree().Root;
			return;
		}
		parent = (Node) Static_NodeMethod.GetParentWithType(this, "CanvasLayer");
	}

	public override void _MouseEnter(){
		mouse_in = true;
	}

	public override void _MouseExit(){
		//GD.Print("Mouse exit");
		mouse_in = false;
	}
	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventMouseButton mouseEvent && mouse_in){
			if(mouseEvent.IsPressed()){
				SpawnDraggableUI();
			} 
		}
	}

	private void SpawnDraggableUI()
	{
		PanelContainer container = this.GetParent().GetParent<PanelContainer>();
		PanelContainer spawned = (PanelContainer) Static_NodeMethod.DuplicateNodeInternal(container);
		//GD.Print(spawned);
		Area2D spawnedInteractive = spawned.GetChild(0).GetChild<Area2D>(0);
		Resource script = ResourceLoader.Load("res://Script/UI/DraggableUI.cs");
		spawnedInteractive.SetScript(script);
		DraggableUI draggableUI = spawned.GetChild(0).GetChild<DraggableUI>(0);
		parent.AddChild(spawned);
		draggableUI.SetPos();
	}
	
}
