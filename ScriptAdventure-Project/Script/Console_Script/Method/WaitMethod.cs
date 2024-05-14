using Godot;
using MEC;
using System;
using System.Collections.Generic;
using System.Text;

public class WaitMethod 
{
    private CodeAnalizer codeAnalizer;
    private List<CoroutineHandle> myCoroutineList = new List<CoroutineHandle>(); 
    private CoroutineHandle myCoroutine;
    private bool isFirstCoroutine = true;
    private bool isOneCoroutineWaiting = false;

    public WaitMethod(CodeAnalizer analizer){
        codeAnalizer = analizer;
    }

    public void Wait(List<string> argumentList, string body, StringBuilder sb, float? i = null, string nameForCounter = ""){
        float seconds;
        if(argumentList.Count != 1){
            sb.AppendLine(Costanti.ErrorNotvalidParameters + " in Wait method");
        }
        try{
            //seconds = argumentList[0].ToFloat();
            seconds = (float) argumentList[0].ToFloat();
        } catch(Exception ex){
            Exception exception = new Exception(ex.Message + " in Wait method");
            throw exception;
        }
        GD.Print("seconds to wait_ " + seconds);
        if(isFirstCoroutine && i != null)
        {
            myCoroutine = Timing.RunCoroutine(WaitCoroutine(seconds, body, sb, i, nameForCounter));
        }
        else if (i != null)
        {
            Timing.RunCoroutine(WaitCoroutineFinished(seconds, body, sb, i, nameForCounter));
        }
        else
            Timing.RunCoroutine(WaitCoroutine(seconds, body, sb, i, nameForCounter));
    }

    private IEnumerator<Double> WaitCoroutineFinished(float seconds, string body, StringBuilder sb, float? i = null, string nameForCounter = "", bool isBlock = false){
        if(!isOneCoroutineWaiting)
        {
            isOneCoroutineWaiting = true;
            yield return Timing.WaitUntilDone(myCoroutine);
            isOneCoroutineWaiting = false;
            myCoroutine = Timing.RunCoroutine(WaitCoroutine(seconds, body, sb, i, nameForCounter));
        } 
        else
        {
            yield return Timing.WaitForOneFrame;
            Timing.RunCoroutine(WaitCoroutineFinished(seconds, body, sb, i, nameForCounter));
        }     
    }

    private IEnumerator<Double> WaitCoroutine(float seconds, string body, StringBuilder sb, float? i = null, string nameForCounter = ""){
        bool isBlock = false;
        //bool isBlock;
        // if(i != null)   
        //     isBlock = true;
        // else
        //     isBlock = false;
        // GD.Print("*****************IsBlock = " + isBlock);
        isFirstCoroutine = false;
        GD.Print("***********Start coroutine n." + i);
        yield return Timing.WaitForSeconds(seconds);
        //GD.Print("Waiting finished");
        GD.Print("**********Finished coroutine n." + i);
        if(!body.Equals(""))
            codeAnalizer.CheckCode(ref body, ref isBlock, sb, i, nameForCounter);
    }
}
