using System;
using Godot;

public interface Interactable 
{
    public void Action();
    public ConsoleInteraction GetConsoleInteraction();
    public Node GetNode();
    // public bool Get_IsPlaying();
    // public void Set_IsPlaying(bool value);
}
