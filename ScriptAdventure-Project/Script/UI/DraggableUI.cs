using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class DraggableUI : Area2D
{
	private Vector2 mousePos;
	private bool dragging;
	private Vector2 pos;
	private Vector2 offsetPos;
	private PanelContainer parent;
	private Area2D area2DColliderParentDuplicated;
	private List<Area2D> areaList;


    public override void _EnterTree()
	{
		areaList = new List<Area2D>();
		this.CollisionLayer = 2;
		this.CollisionMask = 2;
		dragging = true;
		parent = this.GetParent<Label>().GetParent<PanelContainer>();
		GD.Print(parent.Name);
		pos = parent.Position;
		offsetPos = CalcOffset();
	}

	public override void _Process(double delta){
		if(dragging == false){
			parent.QueueFree();
		}
	}

	private Vector2 CalcOffset(){
		GD.Print("Calc offset");
		float x = parent.Size.X;
		float y = parent.Size.Y;
		return new Vector2(x/2, y/2);
	}

	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventMouseButton mouseEvent){
			if(mouseEvent.IsReleased()){
				GD.Print("Mouse release");
				dragging = false;
				if(this.HasOverlappingAreas()){
					areaList.Clear();
					foreach(Area2D area in GetOverlappingAreas()){
						areaList.Add(area);
					}
				}	
				if(areaList.Count != 0){
					foreach(Area2D area in areaList){
						SubstitudeOverlappingArea(area);
					}
				}
			}
		}
		if(dragging){
			if(@event is InputEventMouseMotion mouseMotion ){
				SetPos();
				//GD.Print("Mouse Pos = " + mousePos.ToString() + "  Rect Pos = " + parent.Position);
			}
		}
	}

	public void SetPos()
	{
		mousePos = this.GetViewport().GetMousePosition();
		parent.Position = mousePos - offsetPos;
	}

	private void SubstitudeOverlappingArea(Area2D area)
	{
		Label label = area.GetParent<Label>();
		if(label.GetParent<PanelContainer>().Visible == false)
				return;
		label.Text = parent.GetChild<Label>(0).Text;
		Area2D areaDuplicated = (Area2D) this.Duplicate();
		Area2D currentArea = label.GetChild<Area2D>(0);
		label.RemoveChild(currentArea);
		label.AddChild(areaDuplicated);
		areaDuplicated.SetScript("");
		currentArea.QueueFree(); 
	}
	
	
}
