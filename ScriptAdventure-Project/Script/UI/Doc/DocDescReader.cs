using Godot;
using System;

public class DocDescReader
{
    public string GetDescProperty(string name)
    {
        string desc = "";
        if (name.Equals(Costanti.PropertyFrame))
        {
            desc = Costanti.PropertyFrameDesc;
        } else
        if (name.Equals(Costanti.PropertyRegisteredObj))
        {
            desc = Costanti.PropertyRegisteredObjDesc;
        } else
        if (name.Equals(Costanti.PropertyTestBool))
        {
            desc = Costanti.PropertyTestBoolDesc;
        } else
        if (name.Equals(Costanti.PropertyUniqueName))
        {
            desc = Costanti.PropertyUniqueNameDesc;
        } 

        return desc;
    }

    public string GetDescMethod(string name){
        string desc = "";
        if (name.Equals(Costanti.MethodAction))
        {
            desc = Costanti.MethodActionDesc;
        } else
        if (name.Equals(Costanti.MethodMoveUp))
        {
            desc = Costanti.MethodMoveUpDesc;
        } else
        if (name.Equals(Costanti.MethodMoveDown))
        {
            desc = Costanti.MethodMoveDownDesc;
        } else
        if (name.Equals(Costanti.MethodMoveLeft))
        {
            desc = Costanti.MethodMoveLeftDesc;
        } else
        if (name.Equals(Costanti.MethodMoveRight))
        {
            desc = Costanti.MethodMoveRightDesc;
        } 

        return desc;
    }
}
