using Godot;
using System;

public interface SwitchControlled
{
    public void OpenAction(SwitchControl switchControl);
    public void CloseAction(SwitchControl switchControl);
}
