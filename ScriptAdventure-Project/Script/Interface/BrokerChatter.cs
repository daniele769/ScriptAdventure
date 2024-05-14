using Godot;
using HiveMQtt.Client.Results;
using System;

public interface BrokerChatter 
{
    public void SetConnectionResult(bool result);
    public void SetTextError(string text);
}
