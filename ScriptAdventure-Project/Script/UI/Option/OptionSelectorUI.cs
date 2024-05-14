using Godot;
using System;
using System.Collections.Generic;

public partial class OptionSelectorUI : HBoxContainer
{
	[Export] private Button leftButton; 
	[Export] private Button rightButton; 
	[Export] private Label label;
	[Export] private Container descriptionRow;

	private List<string> valueList;

	public override void _Ready()
	{
		leftButton.Pressed += LeftAction;
		rightButton.Pressed += RightAction;

	}

    public override void _Process(double delta)
    {
        if(this.Visible == false){
			label.Visible = false;
			leftButton.Visible = false;
			rightButton.Visible = false;
			descriptionRow.Visible = false;
		} else {
			label.Visible = true;
			leftButton.Visible = true;
			rightButton.Visible = true;
			descriptionRow.Visible = true;
		}
    }

    private void LeftAction(){
		label.Text = valueList[GetNextPosValue(-1)];
	}

	private void RightAction(){
		label.Text = valueList[GetNextPosValue(+1)];
	}

	private int GetNextPosValue(int dir)
	{
		int currentPos = valueList.IndexOf(label.Text);
		int newPos = currentPos + dir;
		if(newPos < 0){
			newPos = valueList.Count - 1;
		}
		if(newPos == valueList.Count){
			newPos = 0;
		}
		return newPos;
	}

	public void SetValueList(List<string> values)
	{
		valueList = values;
	}

	public List<string> GetValueList()
	{
		return valueList;
	}

	public void SetValue(int pos)
	{
		label.Text = valueList[pos];
	}

	public string GetValue()
	{
		return label.Text;
	}

}
