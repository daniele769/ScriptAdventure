using Godot;
using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class BoxScript_Spawner : RigidBody2D, Interactable
{
	[Export] CanvasLayer canvasLayer;
    [Export] PackedScene exerciseScene;
    [Export] AudioStreamPlayer audioStreamPlayer;
    [Export] AudioStream wrongSound;
    [Export] AudioStream correctSound;
    public enum State
    {
        wrong,
        badSolution,
        correct,

    }
    public Exercise_Control exercise_Control;
	private ConsoleInteraction consoleInteraction;
    private Sprite2D sprite;
    private CoroutineHandle coroutine;
    [Export] public int state;

    public ConsoleInteraction ConsoleInteraction { get => consoleInteraction; set => consoleInteraction = value; }


    public override void _Ready()
    {
        state = (int) State.wrong; 
        sprite = (Sprite2D) Static_NodeMethod.GetChildType(this, "Sprite2D");
        StartAnimation(0, 2, 0.25f);
        consoleInteraction = new ConsoleInt_BoxScriptSpawner(this, canvasLayer, exerciseScene);
    }

    private IEnumerator<double> LoopAnimation(int startIndex, int lastIndex, float time)
    {
        coroutine = Timing.RunCoroutine(Static_AnimationMethod.MakeAnimation(sprite, startIndex, lastIndex, time));
        yield return Timing.WaitUntilDone(coroutine);
        coroutine = Timing.RunCoroutine(LoopAnimation(startIndex, lastIndex, time));
    }

    private void StartAnimation(int startIndex, int lastIndex, float time)
    {
        GD.Print("************Called start animation| start index = " + startIndex + " ; last index = " + lastIndex);
        Timing.KillCoroutines(coroutine);
        SetState(startIndex);
        coroutine = Timing.RunCoroutine(LoopAnimation(startIndex, lastIndex, time));
    }

    private void ConnectSignal()
    {
        if(exercise_Control == null)
            GD.PrintErr("Exercise control is null");
        exercise_Control.Connect(Exercise_Control.SignalName.IsExerciseBadSolution, Callable.From(() => StartAnimation(4, 6, 0.25f)));
        exercise_Control.Connect(Exercise_Control.SignalName.IsExerciseWrong, Callable.From(() => StartAnimation(0, 2, 0.25f)));
        exercise_Control.Connect(Exercise_Control.SignalName.IsExerciseCorrect, Callable.From(() => StartAnimation(2, 4, 0.25f)));
        
    }

    private void SetState(int startIndex)
    {
        switch (startIndex)
        {
            case 0: state = (int) State.wrong;
                    audioStreamPlayer.Stream = wrongSound;
                    audioStreamPlayer.Play();
                    return;
            case 2: state = (int) State.correct;
                    audioStreamPlayer.Stream = correctSound;
                    audioStreamPlayer.Play();
                    return;
            case 4: state = (int) State.badSolution;
                    audioStreamPlayer.Stream = wrongSound;
                    audioStreamPlayer.Play();
                    return;
        }
    }

    public void Action(){
        if(state != (int)State.correct)
        {
            consoleInteraction._Action();
            ConnectSignal();
        }
		
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
