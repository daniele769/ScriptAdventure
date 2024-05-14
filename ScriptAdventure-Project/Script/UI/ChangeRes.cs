using Godot;
using System;

public partial class ChangeRes : Button
{
	Vector2I defaultRes;
	Vector2I customRes;
	Vector2I currentRes;
	
	public override void _Ready()
	{
		defaultRes = new Vector2I(1280, 720);
		currentRes = defaultRes;
		customRes = new Vector2I(1920, 1080);
	}


	public override void _Process(double delta)
	{
	}

	public void ActionButton(){
		SetRes();
		GD.Print(GetTree().Root.GetWindow().Size.ToString());
	}

	private void SetRes(){
		
		if(currentRes == defaultRes){
			currentRes = customRes;
			
		}else
		if(currentRes == customRes){
			currentRes = defaultRes;		

		}
		GetTree().Root.GetWindow().Size = currentRes;
		//DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		
	}
}
