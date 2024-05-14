using Godot;
using MEC;
using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Text;

public partial class ConsoleInt_Switch : ConsoleInteraction
{
    private Sprite2D sprite;
    private SwitchControl switchControl;
    private int frame;
    private bool testBool;
    //private RigidBody2D registeredObj; 

    public ConsoleInt_Switch(Sprite2D sprite2D, SwitchControl control)
    {
        sprite = sprite2D;
        switchControl = control;
        Initialize();
    }

    protected override void Initialize()
    {
        GD.Print("Initialize");
        bool testBool = true;
        
        this.propertiesList.Add(Costanti.PropertyFrame, sprite.Frame.ToString());
        this.propertiesList.Add(Costanti.PropertyTestBool, testBool.ToString());
        this.propertiesList.Add(Costanti.PropertyRegisteredObj, registeredObj?.ToString());
        this.propertiesList.Add(Costanti.PropertyUniqueName, uniqueName);
        frame = sprite.Frame;

        this.methodsList.Add(Costanti.MethodAction);
    }

    public override void _Action(StringBuilder sb = null){
        if(registeredObj == null){
            if(sb != null)
                sb.AppendLine(Costanti.Error + "No object registered in Event manager");
            
            return;
        }
		if(isPlaying)
			return;
        SwitchControlled switchControlled = ((Interactable)registeredObj).GetConsoleInteraction().GetSwitchControlled();
        if(switchControlled == null)
        {
            if(sb == null)
                return;
            sb.AppendLine(Costanti.Error + "The selected object is not valid");
            return;
        }
        if(sprite.Frame == 0){
            if(sb != null) 
                sb.AppendLine("Open Action!");
            switchControlled.OpenAction(switchControl);
			OpenAction(sprite);
			return;
		}
        switchControlled.CloseAction(switchControl);
		CloseAction(sprite);
        if(sb != null)
            sb.AppendLine("Close Action!");
    }

    public string Action(){
        StringBuilder sb = new StringBuilder();
        this._Action(sb);
        return sb.ToString();
    }

    private void OpenAction(Sprite2D sprite){
        AudioStreamPlayer audioStreamPlayer = (AudioStreamPlayer)Static_NodeMethod.GetChildType(switchControl, "AudioStreamPlayer");
        audioStreamPlayer.Play();
		Timing.RunCoroutine(Static_AnimationMethod.MakeAnimation(sprite, 0, 3));
	}

    private void CloseAction(Sprite2D sprite){	
        AudioStreamPlayer audioStreamPlayer = (AudioStreamPlayer)Static_NodeMethod.GetChildType(switchControl, "AudioStreamPlayer");
        audioStreamPlayer.Play();	
		Timing.RunCoroutine(Static_AnimationMethod.MakeAnimation(sprite, 3, 0));
	}

    public override void SetPropertyValue(string name, string value, StringBuilder sb)
    {
        if (IsParameterExist(name))
        {
            switch (name)
            {
                case "Frame":   try{
                                        SetInt(ref frame, value, "Frame", sb, (sprite.Hframes * sprite.Vframes));
                                    } catch{
                                        throw;
                                    }
                   
                                    sprite.Frame = frame;
                                    propertiesList["Frame"] = frame.ToString();
                                    break;
                
                case "TestBool": try{
                                        SetBool(ref testBool, value, "TestBool", sb);
                                    } catch{
                                        throw;
                                    }
                                    propertiesList["TestBool"] = testBool.ToString();
                                    break;

                case "UniqueName":  if(!IsUniqueNameUnique(value, sprite.GetTree(), sb))
                                        break;
                                    SetString(ref uniqueName, ref value, "UniqueName", sb);
                                    propertiesList["UniqueName"] = uniqueName;
                                    GD.Print("Unique name = " + propertiesList["UniqueName"].ToString());
                                    break;

                case "RegisteredObj":   SetRegisteredObj(ref registeredObj, value, sprite.GetTree(), sb);
                                        if(registeredObj == null)
                                            break;
                                        propertiesList["RegisteredObj"] = registeredObj.ToString();
                                        GD.Print("RegisteredObject = " + registeredObj.ToString());
                                        break;
            }   
            return;
        }
        sb.AppendLine(Costanti.WarningParameterNotFound);

    }

    public override SwitchControlled GetSwitchControlled()
    {
        GD.Print("This is not a SwitchControlled Object");
        return null;
    }

}
