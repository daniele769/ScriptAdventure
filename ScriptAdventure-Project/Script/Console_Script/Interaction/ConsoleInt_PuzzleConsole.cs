using Godot;
using System;
using System.Collections;
using System.Text;
using MEC;
using System.Collections.Generic;

public partial class ConsoleInt_PuzzleConsole : ConsoleInteraction
{
    private Area2D[] area2dList;
    private RigidBody2D[] bodyList;
    private int xColumn;
    private int yRow;
    private Container puzzleSolution;
    private bool isCoroutineRunning;
    private CoroutineHandle? myCoroutine;

    public ConsoleInt_PuzzleConsole(Area2D[] area2ds, RigidBody2D[] bodies, int x, int y, Container window)
    {
        area2dList = area2ds;
        bodyList = bodies;
        xColumn = x;
        yRow = y;
        puzzleSolution = window;

        Initialize();
    }
     protected override void Initialize()
    {
        methodsList.Add(Costanti.MethodMoveUp);
        methodsList.Add(Costanti.MethodMoveDown);
        methodsList.Add(Costanti.MethodMoveRight);
        methodsList.Add(Costanti.MethodMoveLeft);

        isCoroutineRunning = false;
    }

    public override void SetPropertyValue(string name, string value, StringBuilder sb)
    {
        throw new NotImplementedException();
    }

    public override void _Action(StringBuilder sb = null)
    {
        puzzleSolution.Visible = true;
    }

    public string Move(int id, int x, int y){
        GD.Print("Move Method!!");
        StringBuilder sb = new StringBuilder();
        _Move(id, x, y, sb);
        return sb.ToString();

    }

    public string MoveUp(int id){
        StringBuilder sb = new StringBuilder();
        _Move(id, 0, -1, sb);
        return sb.ToString();
    }

     public string MoveDown(int id){
        StringBuilder sb = new StringBuilder();
        _Move(id, 0, 1, sb);
        return sb.ToString();
    }

     public string MoveLeft(int id){
        StringBuilder sb = new StringBuilder();
        _Move(id, -1, 0, sb);
        return sb.ToString();
    }

     public string MoveRight(int id){
        StringBuilder sb = new StringBuilder();
        _Move(id, 1, 0, sb);
        return sb.ToString();
    }
    public void _Move(int id, int x, int y, StringBuilder sb){
        //RigidBody2D currentBody = (RigidBody2D) Static_NodeMethod.GetChildType(bodyList[id - 1], "RigidBody2D");
        GD.Print("Searched for obj id " + (id - 1));
        RigidBody2D currentBody = bodyList[id - 1];
        GD.Print("Path of currentBody" + currentBody.GetPath());
        int xCurrent = (int) currentBody.GetMeta("X");
        int yCurrent = (int) currentBody.GetMeta("Y");
        GD.Print("Selected body in pos (" + xCurrent + "; " + yCurrent + ")");
        int xFinal = CalcXFinal(xCurrent, x, sb);
        int yFinal = CalcYFinal(yCurrent, y, sb);
        GD.Print("pos to go = (" + xFinal + "; " + yFinal + ")");
        Area2D area = GetAreaAtPos(xFinal, yFinal, sb);
        if(area == null){
            return;
        }
        GD.Print("Area to go = (" + area.GetMeta("X") + "; " + area.GetMeta("Y") + ")");
         if(IsAreaFree(xFinal, yFinal, sb)){
            currentBody.SetMeta("X", xFinal);
            currentBody.SetMeta("Y", yFinal);
            if(isCoroutineRunning){
                GD.Print("********************Waiting Coroutine to finish");
                Timing.RunCoroutine(WaitCoroutineToFinish(currentBody, area, 0.0f));
                return;
            }
            isCoroutineRunning = true;
            GD.Print("***************Coroutine started");
            myCoroutine = Timing.RunCoroutine(MoveAtPos(currentBody, area, 0.0f));
        }
    }

    private IEnumerator<Double> WaitCoroutineToFinish(RigidBody2D body, Area2D area, float t){
        //GD.Print("********************Waiting Coroutine to finish");
        if(isCoroutineRunning){
            yield return Timing.WaitForOneFrame;
            Timing.RunCoroutine(WaitCoroutineToFinish(body, area, 0.0f));
        }
        else{
            GD.Print("********************Waiting finished");
            isCoroutineRunning = true;
            GD.Print("********************Coroutine started in waitingMethod");
            myCoroutine = Timing.RunCoroutine(MoveAtPos(body, area, t));
        }
    }

    private IEnumerator<Double> MoveAtPos(RigidBody2D body, Area2D area, float t){
        //GD.Print("Move meethod MoveAtPos");
        //GD.Print("t weight is " + t);
        body.GlobalTransform = body.GlobalTransform.InterpolateWith(area.GlobalTransform, t);
        if(!body.GlobalTransform.IsEqualApprox(area.GlobalTransform)){
            //GD.Print("MoveAtPos not terminated");
            yield return Timing.WaitForSeconds(0.01f);
            //GD.Print("t weight is " + t);
            t += 0.01f;
            myCoroutine = Timing.RunCoroutine(MoveAtPos(body, area, t));
        } else{
            isCoroutineRunning = false;
            myCoroutine = null;
            GD.Print("********************Coroutine finished");
        }
    }

    private bool IsAreaFree(int xFinal, int yFinal, StringBuilder sb){
        //GD.Print("Move meethod IsAreaFree");
        foreach(RigidBody2D body2D in bodyList){
            if((int)body2D.GetMeta("X") == xFinal && (int)body2D.GetMeta("Y") == yFinal){
                sb.AppendLine(Costanti.Error + "Selected Area(" + xFinal + ", " + yFinal + ")" + " is not Free");
                return false;
            }
        }
        return true;
    }

    private Area2D GetAreaAtPos(int x, int y, StringBuilder sb){
        //GD.Print("Move meethod GetAreaAtPos");
        foreach(Area2D area in area2dList){
            int xMeta = (int) area.GetMeta("X");
            int yMeta = (int) area.GetMeta("Y");
            if(xMeta == x && yMeta == y){
                //GD.Print("Area returned");
                return area;
            }
        }
        sb.AppendLine(" No area find at position (" + x + ", " + y + ").");
        return null;
    }

    private int CalcXFinal(int xCurrent, int x, StringBuilder sb){
        //GD.Print("Move meethod CalcX");
        GD.Print("XColumn are " + xColumn);
        int xFinal = xCurrent + x;
        GD.Print("XFinal = " + xFinal);
        if(xFinal > xColumn || xFinal < 1){
            sb.Append(Costanti.Error + " 'x' Out of Bounds in 'Move()' method.");
            //return -1;
        }
        return xFinal;    
    }

    private int CalcYFinal(int yCurrent, int y, StringBuilder sb){
        //GD.Print("Move meethod CalcX");
        GD.Print("YRow are " + yRow);
        int yFinal = yCurrent + y;
        GD.Print("yFinal = " + yFinal);
        if(yFinal > yRow || yFinal < 1){
            sb.Append(Costanti.Error + " 'y' Out of Bounds in 'Move()' method.");
            //return -1;
        }
        return yFinal;

    }

    public override SwitchControlled GetSwitchControlled()
    {
        GD.Print("Not a switchControlled object");
        return null;
    }

}
