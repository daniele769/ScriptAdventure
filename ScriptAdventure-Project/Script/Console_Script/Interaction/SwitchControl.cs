using Godot;
using System;
using MEC;
using System.Runtime.Versioning;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class SwitchControl : RigidBody2D, Interactable, Animable
{
    private ConsoleInteraction consoleInteraction;
    
    public override void _Ready()
    {
        Sprite2D sprite = (Sprite2D) Static_NodeMethod.GetChildType(this, "Sprite2D");
        consoleInteraction = new ConsoleInt_Switch(sprite, this);
    }

    public void Action()
    {
		GD.Print("consoleInteraction selected for interaction " + consoleInteraction.ToString());
		consoleInteraction._Action();	
    }

    bool Animable.Get_IsPlaying()
    {
        return consoleInteraction.isPlaying;
    }

    void Animable.Set_IsPlaying(bool value)
    {
        consoleInteraction.isPlaying = value;
    }

    public ConsoleInteraction GetConsoleInteraction()
    {
        return consoleInteraction;
    }

    public Node GetNode()
    {
        return this;
    }
}
