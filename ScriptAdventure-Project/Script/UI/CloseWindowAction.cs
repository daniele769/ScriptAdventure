using Godot;
using System;

public partial class CloseWindowAction : Button
{
	[Export] Container window;
	public override void _Ready()
	{
		this.Pressed += Close;
	}

	private void Close()
	{
		window.Visible = false;
	}
}
