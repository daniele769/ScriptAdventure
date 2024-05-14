using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class MethodControl 
{
    private CodeAnalizer codeAnalizer;
    private WaitMethod waitMethod;
    public MethodControl(CodeAnalizer analizer){
        codeAnalizer = analizer;

        waitMethod = new WaitMethod(codeAnalizer);
    }

    public void CheckMethod(string line, ref string code, StringBuilder sb, float? i = null, string nameForCounter = ""){
        string methodName;
        string argumentStr;
        line = Regex.Replace(line, "\n", "");
        line = Regex.Replace(line, "\t", "");
        line = Regex.Replace(line, " ", "");
        GD.Print("Line in method: " + line);
        List<object> argumentList;
        List<string> list = new List<string>();
        methodName = line.Substring(0, line.IndexOf("("));
        GD.Print("MethodName: " + methodName);
        if(line.Contains(')') && line.Contains(')') && line.IndexOf(')') != line.IndexOf('(') + 1){
            line = line.Substring(line.IndexOf("(") + 1);
            argumentStr = line.Substring(0 , line.IndexOf(")"));
            GD.Print("Argumets: " + argumentStr + ";");
            list = argumentStr.Split(',').ToList<string>();
            GD.Print("ArgumetList: " + list + ";");
            GD.Print("ArgumetList count is " + list.Count);
            argumentList = CalcArgumentList(list, methodName);
            foreach(object obj in argumentList){
                GD.Print(obj);
            }
        } else
            argumentList = null;
        GD.Print("Code in CheckMethod " + code);
        //Metodi Universali
        if(methodName.Equals("Wait")){
            waitMethod.Wait(list, code, sb, i, nameForCounter);
            code = "";
            return;
        }
        //Metodi locali
        CheckMethodInSelectedObj(methodName, argumentList, sb);
    }

    private List<Object> CalcArgumentList(List<string> list, string methodName){
        List<Object> argumentList = new List<object>();
        if( methodName.Equals(Costanti.MethodMoveUp.Substring(0, Costanti.MethodMoveUp.IndexOf("("))) ||
            methodName.Equals(Costanti.MethodMoveDown.Substring(0, Costanti.MethodMoveDown.IndexOf("("))) ||
            methodName.Equals(Costanti.MethodMoveLeft.Substring(0, Costanti.MethodMoveLeft.IndexOf("("))) ||
            methodName.Equals(Costanti.MethodMoveRight.Substring(0, Costanti.MethodMoveRight.IndexOf("("))))
        {
            argumentList.Add(list[0].ToInt());
            
        }
        return argumentList;
    }

    private void CheckMethodInSelectedObj(string methodName, List<object> argumentList, StringBuilder sb){
        GD.Print("Cerco metodi nel selectedObj");
        ConsoleInteraction selectedObj = codeAnalizer.GetSelectedObject();
        List<string> methodsList = selectedObj.GetAllMethodsList();
        foreach(string s in methodsList){
            //methodName += "();";
            string str = s.Substring(0, s.IndexOf("("));
            GD.Print("method searched: " + methodName + " | method in class: " + str);
            if(methodName.Equals(str)){
                GD.Print("Find method " + methodName);
                GD.Print("Method to execute " + s);
                sb.AppendLine(selectedObj.InvokeMethod(s, argumentList));
            }
        }
    }

}
