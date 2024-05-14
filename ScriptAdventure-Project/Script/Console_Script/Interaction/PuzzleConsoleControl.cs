using Godot;
using System;
using System.Collections.Generic;

public partial class PuzzleConsoleControl : RigidBody2D, Interactable
{
    [Export] Area2D[] areaList;
    [Export] RigidBody2D[] bodyList;
    [Export] int xColumn;
    [Export] int yRow;
    [Export] Container windowSolution;
    [Signal] public delegate void IsSolvedEventHandler();
    [Signal] public delegate void NotSolvedEventHandler();
    private bool isSolved = false;
    private ConsoleInteraction consoleInteraction;

    public override void _Ready()
    {
        consoleInteraction = new ConsoleInt_PuzzleConsole(areaList, bodyList, xColumn, yRow, windowSolution);
    }

    public override void _Process(double delta)
    {
        if((int)bodyList[0].GetMeta("X") == 1 && (int)bodyList[0].GetMeta("Y") == 1 &&
            (int)bodyList[1].GetMeta("X") == 3 && (int)bodyList[1].GetMeta("Y") == 2 &&
            (int)bodyList[2].GetMeta("X") == 2 && (int)bodyList[2].GetMeta("Y") == 2 &&
            (int)bodyList[3].GetMeta("X") == 4 && (int)bodyList[3].GetMeta("Y") == 3 &&
            (int)bodyList[4].GetMeta("X") == 1 && (int)bodyList[4].GetMeta("Y") == 3 &&
            (int)bodyList[5].GetMeta("X") == 4 && (int)bodyList[5].GetMeta("Y") == 1 &&
            !isSolved)
        {
            EmitSignal(SignalName.IsSolved);
            GD.Print("Puzzle Solved!!!!");
            isSolved = true;
            return;
        }
        if( ((int)bodyList[0].GetMeta("X") != 1 || (int)bodyList[0].GetMeta("Y") != 1 ||
             (int)bodyList[1].GetMeta("X") != 3 || (int)bodyList[1].GetMeta("Y") != 2 ||
             (int)bodyList[2].GetMeta("X") != 2 || (int)bodyList[2].GetMeta("Y") != 2 ||
             (int)bodyList[3].GetMeta("X") != 4 || (int)bodyList[3].GetMeta("Y") != 3 ||
             (int)bodyList[4].GetMeta("X") != 1 || (int)bodyList[4].GetMeta("Y") != 3 ||
             (int)bodyList[5].GetMeta("X") != 4 || (int)bodyList[5].GetMeta("Y") != 1) &&
            isSolved)
        {

            isSolved = false;
            EmitSignal(SignalName.NotSolved);
            GD.Print("Puzzle Not Solved!!!!");
        }
    }

    public void Action()
    {
        consoleInteraction._Action();
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
