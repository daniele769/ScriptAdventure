using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static partial class Static_DisplayControl 
{
    private static Vector2I res0 = new Vector2I(854, 480);
	private static Vector2I res1 = new Vector2I(1280, 720);
	private static Vector2I res2 = new Vector2I(1366, 768);
	private static Vector2I res3 = new Vector2I(1600, 900);
	private static Vector2I res4 = new Vector2I(1920, 1080);
	private static Vector2I res5 = new Vector2I(2560, 1440);
	private static Vector2I res6 = new Vector2I(3840, 2160);
    private static List<Vector2I> allResList = new List<Vector2I>();

    public static List<Vector2I> GetResolutions()
    {
        
		allResList.Add(res0);
		allResList.Add(res1);
		allResList.Add(res2);
		allResList.Add(res3);
		allResList.Add(res4);
		allResList.Add(res5);
		allResList.Add(res6);
        return allResList;

    }

    private static Vector2I GetResVectorFromString(string res)
    {
		//string res = PlayerPrefs.GetString(Costanti.keyResolution);
		string x = res.Substring(2, res.IndexOf("x") - 2);
		GD.Print("X = " + x);
		res = res.Substring(res.IndexOf("x") + 1);
		string y = res.Substring(0, res.IndexOf(")"));
		GD.Print("Y = " + y);

		Vector2I vector2I = new Vector2I(x.ToInt(), y.ToInt());
        GD.Print("Resolution founded = " + vector2I);
        return vector2I;
    }

    public static void ApplyDisplaySetting(Node node)
	{
		DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		if(PlayerPrefs.ListKeys.Contains(Costanti.keyResolution))
		{
			string res = PlayerPrefs.GetString(Costanti.keyResolution);
			node.GetTree().Root.GetWindow().Size = GetResolutions()[GetResolutions().IndexOf(GetResVectorFromString(res))];
			GD.Print("New Resolution is " + node.GetTree().Root.GetWindow().Size);
		}	
		if(PlayerPrefs.ListKeys.Contains(Costanti.keyDisplay) && PlayerPrefs.GetString(Costanti.keyDisplay).Equals(Costanti.windowed)){
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		}
		else if(PlayerPrefs.ListKeys.Contains(Costanti.keyDisplay) && PlayerPrefs.GetString(Costanti.keyDisplay).Equals(Costanti.windowedNoBorder)){
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		}
		else if(PlayerPrefs.ListKeys.Contains(Costanti.keyDisplay) && PlayerPrefs.GetString(Costanti.keyDisplay).Equals(Costanti.fullscreen)){
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
		}
		DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Enabled);
	
	}
}
