using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class OptionControl : CanvasLayer
{
	[Export] Button backToMenuButton;
	[Export] Button saveButton;
	[Export] OptionSelectorUI resSelector;
	[Export] OptionSelectorUI displaySelector;
	[Export] LineEdit ipBroker;
	[Export] LineEdit portBroker;
	[Export] LineEdit usernameBroker;
	[Export] LineEdit passwordBroker;
	[Export] CheckBox isBrokerConnectionOn;
	[Export] HSlider musicSlider;
	[Export] HSlider soundSlider;
	private List<Vector2I> allResList;
	private List<Vector2I> compatibleResList;
	private List<string> resListString;
	private List<string> disListString;
	private MenuControl menuControl;

	public override void _Ready()
	{
		backToMenuButton.Pressed += BackToMenuAction;
		saveButton.Pressed += Save;
		InitializeResList();
		InitializeDisplayValue();
		InitializeAudioValue();
		InitializeBrokerValue();
	}

    public override void _Process(double delta)
    {
        if(displaySelector.GetValue().Equals(Costanti.fullscreen)){
			resSelector.Visible = false;
		} else {
			resSelector.Visible = true;
		}
    }

    private void InitializeDisplayValue()
	{
		disListString = new List<string>();
		disListString.Add(Costanti.windowed);
		disListString.Add(Costanti.windowedNoBorder);
		disListString.Add(Costanti.fullscreen);
		displaySelector.SetValueList(disListString);

		if(!PlayerPrefs.ListKeys.Contains(Costanti.keyDisplay)){
			displaySelector.SetValue(0);
			return;
		}
		if(PlayerPrefs.GetString(Costanti.keyDisplay).Equals(Costanti.windowed)){
			displaySelector.SetValue(0);
		}
		else if(PlayerPrefs.GetString(Costanti.keyDisplay).Equals(Costanti.windowedNoBorder)){
			displaySelector.SetValue(1);
		}
		else if(PlayerPrefs.GetString(Costanti.keyDisplay).Equals(Costanti.fullscreen)){
			displaySelector.SetValue(2);
		}
	}

	private void InitializeResList(){
		allResList = new List<Vector2I>();
		compatibleResList = new List<Vector2I>();
		resListString = new List<string>();
		allResList = Static_DisplayControl.GetResolutions();

		CalcCompatibleResList();
		CalcResListString();
		resSelector.SetValueList(resListString);

		if(!PlayerPrefs.ListKeys.Contains(Costanti.keyResolution)){
			Vector2I vector2I = GetTree().Root.GetWindow().Size;
			resSelector.SetValue(compatibleResList.IndexOf(vector2I));
			return;
		}
		string res = PlayerPrefs.GetString(Costanti.keyResolution);
		resSelector.SetValue(resListString.IndexOf(res));
	}

	private void InitializeAudioValue()
	{
		List<string> keyList = PlayerPrefs.ListKeys.ToList<string>();
        if(!keyList.Contains(Costanti.musicValue) && !keyList.Contains(Costanti.soundValue))
            return;
		
		musicSlider.Value = PlayerPrefs.GetFloat(Costanti.musicValue);
		soundSlider.Value = PlayerPrefs.GetFloat(Costanti.soundValue);
	}

	private void CalcResListString()
	{
		foreach(Vector2I res in compatibleResList){
			string resString = " (" + res.X + "x" + res.Y + ") ";
			resListString.Add(resString);
		}
	}

	private void CalcCompatibleResList()
	{
		Vector2I maxRes = DisplayServer.ScreenGetSize();
		GD.Print("Display size = " + maxRes);
		foreach(Vector2I res in allResList){
			if(res.Y <= maxRes.Y)
				compatibleResList.Add(res);
		}
	}

	private void BackToMenuAction()
	{
		if(menuControl == null){
			GetTree().ChangeSceneToFile("res://Scene/Menu.tscn");
			return;
		}
		menuControl.Visible = true;
		this.QueueFree();
		return;
		
	}

	private void Save()
	{
		PlayerPrefs.SetString(Costanti.keyDisplay, displaySelector.GetValue());
		if(!resSelector.Visible){
			PlayerPrefs.SetString(Costanti.keyResolution, resListString[resListString.Count - 1]);
		} else {
			PlayerPrefs.SetString(Costanti.keyResolution, resSelector.GetValue());
		}
		Static_DisplayControl.ApplyDisplaySetting(this);
		PlayerPrefs.SetString(Costanti.ipBroker, ipBroker.Text);
		PlayerPrefs.SetString(Costanti.portBroker, portBroker.Text);
		PlayerPrefs.SetString(Costanti.usernameBroker, usernameBroker.Text);
		PlayerPrefs.SetString(Costanti.passwordBroker, passwordBroker.Text);
		if(isBrokerConnectionOn.ButtonPressed)
			PlayerPrefs.SetBool(Costanti.isBrokerConnectionOn, true);
		else
			PlayerPrefs.SetBool(Costanti.isBrokerConnectionOn, false);
		PlayerPrefs.SetFloat(Costanti.musicValue, (float) musicSlider.Value);
		PlayerPrefs.SetFloat(Costanti.soundValue, (float) soundSlider.Value);
		Static_AudioControl.UpdateAudioValues();
	}

	

	private void InitializeBrokerValue()
	{
		List<string> keyList = PlayerPrefs.ListKeys.ToList<string>();

		if(keyList.Contains(Costanti.ipBroker) && !PlayerPrefs.GetString(Costanti.ipBroker).Equals(""))
			ipBroker.Text = PlayerPrefs.GetString(Costanti.ipBroker);
		if(keyList.Contains(Costanti.portBroker) && !PlayerPrefs.GetString(Costanti.portBroker).Equals(""))
			portBroker.Text = PlayerPrefs.GetString(Costanti.portBroker);
		if(keyList.Contains(Costanti.usernameBroker) && !PlayerPrefs.GetString(Costanti.usernameBroker).Equals(""))
			usernameBroker.Text = PlayerPrefs.GetString(Costanti.usernameBroker);
		if(keyList.Contains(Costanti.passwordBroker) && !PlayerPrefs.GetString(Costanti.passwordBroker).Equals(""))
			passwordBroker.Text = PlayerPrefs.GetString(Costanti.passwordBroker);
		if(keyList.Contains(Costanti.isBrokerConnectionOn) && PlayerPrefs.GetBool(Costanti.isBrokerConnectionOn) == true)
			isBrokerConnectionOn.ButtonPressed = true;
	}

	public void SetMenuControl(MenuControl node)
	{
		menuControl = node;
	}
	
}
