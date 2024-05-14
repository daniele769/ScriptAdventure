using Godot;
using System;

public partial class BlockRow : HBoxContainer
{
	[Export] Button clearButton;
	private Block_ExerciseControl if_ExerciseControl;

    public override void _Ready()
    {
        clearButton.Pressed += AddLabel;
    }

	public void SetIfControl(Block_ExerciseControl control)
	{
		if_ExerciseControl = control;
	}

	private void AddLabel()
	{
		clearButton.Pressed -= AddLabel;
		Container interactive = (Container) this.GetChild(1);
		interactive.Visible = true;
		if(interactive.GetChild(0).GetClass().Equals("Label")){
			interactive.GetChild(0).GetChild<Area2D>(0).Visible = true;
		}
		clearButton.Text = " - ";
		clearButton.Pressed += ClearLabel;
	}

    private void ClearLabel()
	{
		clearButton.Pressed -= ClearLabel;
		Container interactive = (Container) this.GetChild(1);
		interactive.Visible = false;
		if(interactive.GetChild(0).GetClass().Equals("Label")){
			interactive.GetChild<Label>(0).Text = "          ";
			interactive.GetChild(0).GetChild<Area2D>(0).Visible = false;
		} else {
			InteractiveBox interactiveBox = (InteractiveBox)GD.Load<PackedScene>("res://Prefab/UIElementExerciseMode/InteractiveBox.tscn").Instantiate();
			int pos = Static_NodeMethod.CalcPosInParentChildreen(this, interactive);
			this.AddChild(interactiveBox);
			this.MoveChild(interactiveBox, pos);
			interactive.QueueFree();
		}
		clearButton.Text = " + ";
		if_ExerciseControl.DeleteEmptyRow();
		clearButton.Pressed += AddLabel;
		
	}

}
