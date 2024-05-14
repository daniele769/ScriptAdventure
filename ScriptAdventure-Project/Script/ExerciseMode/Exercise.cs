using Godot;
using System;

public partial class Exercise : Node
{
    string subject;
    ulong errors;
    ulong time;

    public Exercise(string s, ulong e, ulong t)
    {
        subject = s;
        errors = e;
        time = t;
    }

    public string Subject { get => subject; set => subject = value; }
    public ulong Errors { get => errors; set => errors = value; }
    public ulong Time { get => time; set => time = value; }
}
