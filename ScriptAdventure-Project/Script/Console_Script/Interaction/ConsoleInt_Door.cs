using Godot;
using MEC;
using System;
using System.Collections.Generic;
using System.Text;

public partial class ConsoleInt_Door : ConsoleInteraction, SwitchControlled
{
    private List<Sprite2D> spriteList;
    private CollisionShape2D collision;
    private AudioStreamPlayer audioStreamPlayer;
    private int frame;

    public ConsoleInt_Door(List<Sprite2D> list, CollisionShape2D collider, AudioStreamPlayer audio)
    {
        spriteList = list;
        collision = collider;
        audioStreamPlayer = audio;
        Initialize();
    }

    protected override void Initialize()
    {
        string Frame = "Frame";
        
        this.propertiesList.Add(Frame, spriteList[0].Frame.ToString());
        this.propertiesList.Add("UniqueName", uniqueName);
        frame = spriteList[0].Frame;
    }

    public void OpenAction(SwitchControl switchControl){
        audioStreamPlayer.Play();
        foreach(Sprite2D spriteBody in spriteList)
        {
            Timing.RunCoroutine(Static_AnimationMethod.MakeAnimation(spriteBody, 0, spriteBody.Hframes));
        }
		collision.Disabled = true;
	}

    public void CloseAction(SwitchControl switchControl){	
        audioStreamPlayer.Play();	
        foreach(Sprite2D spriteBody in spriteList)
        {
            Timing.RunCoroutine(Static_AnimationMethod.MakeAnimation(spriteBody, spriteBody.Hframes, 0, switchControl));
        }
		collision.Disabled = false;
	}

    private void SetFrame(int frame)
    {
        foreach(Sprite2D sprite in spriteList)
        {
            sprite.Frame = frame;
        }
    }

    public override void _Action(StringBuilder sb = null)
    {
        
    }

    public override void SetPropertyValue(string name, string value, StringBuilder sb)
    {
        if (IsParameterExist(name))
        {
            switch (name)
            {
                case "Frame":   try{
                                        SetInt(ref frame, value, "Frame", sb, (spriteList[0].Hframes * spriteList[0].Vframes));
                                    } catch{
                                        throw;
                                    }
                   
                                    //sprite.Frame = frame;
                                    SetFrame(frame);
                                    propertiesList["Frame"] = frame.ToString();
                                    break;

                case "UniqueName":  if(!IsUniqueNameUnique(value, spriteList[0].GetTree(), sb))
                                        break;
                                    SetString(ref uniqueName, ref value, "UniqueName", sb);
                                    propertiesList["UniqueName"] = uniqueName;
                                    GD.Print("Unique name = " + propertiesList["UniqueName"].ToString());
                                    break;
            }   
            return;
        }
        sb.AppendLine(Costanti.WarningParameterNotFound);

    }

    public override SwitchControlled GetSwitchControlled()
    {
        return this;
    }
}
