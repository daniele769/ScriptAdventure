using Godot;
using System;

public partial class CheckBox_Style : CheckBox
{
	Color uncheckedColor;

	public override void _Ready()
	{
		if(this.ButtonPressed)
		{
			this.ButtonPressed = false;
			uncheckedColor = this.Modulate;
			this.ButtonPressed = true;
			return;
		}
		uncheckedColor = this.Modulate;
	}



    public override void _Process(double delta)
    {
		if(this.ButtonPressed)
			this.Modulate = new CheckBox().Modulate;
		else
			this.Modulate = uncheckedColor;
    }
}
