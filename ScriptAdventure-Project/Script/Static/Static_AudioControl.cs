using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Static_AudioControl : Node
{
    public static void UpdateAudioValues()
	{
        List<string> keyList = PlayerPrefs.ListKeys.ToList<string>();
        if(!keyList.Contains(Costanti.musicValue) && !keyList.Contains(Costanti.soundValue))
            return;
        
		float music = PlayerPrefs.GetFloat(Costanti.musicValue);
		float sound = PlayerPrefs.GetFloat(Costanti.soundValue);
        GD.Print("Music Value is " + music + "| Sound Value is " + sound);

		if(music == -30)
			AudioServer.SetBusMute(AudioServer.GetBusIndex(Costanti.AudioBusMusic), true);	
		else
		{
			AudioServer.SetBusMute(AudioServer.GetBusIndex(Costanti.AudioBusMusic), false);
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(Costanti.AudioBusMusic), music);
		}

		if(sound == -30)
			AudioServer.SetBusMute(AudioServer.GetBusIndex(Costanti.AudioBusSound), true);	
		else
		{
			AudioServer.SetBusMute(AudioServer.GetBusIndex(Costanti.AudioBusSound), false);
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(Costanti.AudioBusSound), sound);
		}
	}
}
