using Godot;
using MEC;
using System;
using System.Collections.Generic;

public partial class DoorControl : RigidBody2D, Interactable, Animable
{
    private ConsoleInteraction consoleInteraction;
    private List<Sprite2D> spriteList;

    public override void _Ready()
    {
        List<Node> allNode = new List<Node>();
        List<Sprite2D> spriteList = new List<Sprite2D>();
        CollisionShape2D collision = (CollisionShape2D) Static_NodeMethod.GetChildType(this, "CollisionShape2D");
        AudioStreamPlayer audioStreamPlayer = (AudioStreamPlayer)Static_NodeMethod.GetChildType(this, "AudioStreamPlayer");

        allNode = (List<Node>) Static_NodeMethod.GetAllChild(allNode, this);
        spriteList = (List<Sprite2D>) Static_NodeMethod.GetAllChildWithTypeInList<Sprite2D>(allNode, "Sprite2D");
        consoleInteraction = new ConsoleInt_Door(spriteList, collision, audioStreamPlayer);
    }

    public void Action()
    {
        throw new NotImplementedException();
    }

    public ConsoleInteraction GetConsoleInteraction()
    {
        return consoleInteraction;
    }

    public bool Get_IsPlaying()
    {
        return consoleInteraction.isPlaying;
    }

    public void Set_IsPlaying(bool value)
    {
        consoleInteraction.isPlaying = value;
    }

    public Node GetNode()
    {
        return this;
    }

}
