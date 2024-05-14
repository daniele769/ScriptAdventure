using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

public partial class Block_ExerciseControl : VBoxContainer
{
	[Export] private VBoxContainer block;
	[Export] private Button removeButton;
	private List<BlockRow> rowList;
	private int childCount;

	public override void _Ready()
	{
		rowList = new List<BlockRow>();
		childCount = block.GetChildCount();
		removeButton.Pressed += Remove;
		
	}

	public override void _Process(double delta)
	{
		childCount = block.GetChildCount();
		Node row =  block.GetChild(childCount - 1);
		Label label = (Label) Static_NodeMethod.GetInternalChildName(row, "TextBox");
		if(!label.Text.Trim().Equals("")){
			PackedScene scene = GD.Load<PackedScene>("res://Prefab/UIElementExerciseMode/Block/row.tscn");
			BlockRow newRow = (BlockRow)scene.Instantiate();
			newRow.SetIfControl(this);
			rowList.Add(newRow);
			block.AddChild(newRow);
			newRow.Visible = true;
		} else {
			
		}
	}

	public void DeleteEmptyRow()
	{
		List<BlockRow> list = new List<BlockRow>(rowList);
		list.RemoveAt(list.Count - 1);
		foreach(BlockRow row in list){
			Label label = (Label) Static_NodeMethod.GetInternalChildName(row, "TextBox");
			if(label.Text.Trim().Equals("")){
				rowList.Remove(row);
				row.QueueFree();
				
			}
		}
	}

	private void Remove()
	{
		Node parent = this.GetParent();
		InteractiveBox interactiveBox = (InteractiveBox) GD.Load<PackedScene>("res://Prefab/UIElementExerciseMode/InteractiveBox.tscn").Instantiate();
		parent.AddChild(interactiveBox);
		int childPos = Static_NodeMethod.CalcPosInParentChildreen(parent, this);
		parent.MoveChild(interactiveBox, childPos);
		interactiveBox.Visible = true;
		this.QueueFree();
	}
}
