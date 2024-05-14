using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public abstract partial class ConsoleInteraction : Node
{
    protected Dictionary<string, string> propertiesList = new Dictionary<string, string>();
    //protected Dictionary<string, string> MethodsList = new Dictionary<string, string>();
    protected List<string> methodsList = new List<string>();
    protected string uniqueName = "";
    protected RigidBody2D registeredObj;
    public bool isPlaying = false;

    public string UniqueName { get => uniqueName; set => uniqueName = value; }
    public RigidBody2D RegisteredObj { get => registeredObj;}
    public abstract SwitchControlled GetSwitchControlled();

    protected abstract void Initialize();
    public abstract void SetPropertyValue(string name, string value, StringBuilder sb);
    public abstract void _Action(StringBuilder sb = null);

    public bool IsParameterExist(string nameVar)
    {
        foreach(string key in propertiesList.Keys){
            //GD.Print("Key = " + key + "; searched = " + nameVar + ";");
            if(key.Equals(nameVar.Trim())){
                return true;
            }
        }
        return false;
    }
    public string GetParameterValue(string nameVar){
        if(IsParameterExist(nameVar)){
            return propertiesList.GetValueOrDefault(nameVar);
        }
        return "";
    }

    public List<string> GetAllParametersList(){
        List<string> keyList = propertiesList.Keys.ToList();
        return keyList;
    }

    public List<string> GetAllMethodsList(){
        //List<string> keyList = MethodsList.Keys.ToList();
        List<string> keyList = methodsList;
        return keyList;
    }

    public string InvokeMethod(string methodName, List<object> args){
        methodName = methodName.Substring(0, methodName.IndexOf("("));
        if(args == null){
            GD.Print("args is null");
            
            return (string)GetType().GetMethod(methodName).Invoke(this, null);
        }
        GD.Print("args is not null");
        return (string) GetType().GetMethod(methodName).Invoke(this, args.ToArray());
    }

    protected void SetInt(ref int var, string val, string nameVar, StringBuilder sb, int? boundLimit = -1)
    {
        int value;
        try
        {
            value = val.ToInt();
            if (boundLimit != -1 && value >= boundLimit)
            {
                //sb.AppendLine(Costanti.ErrorOutOfBounds);
                Exception ex = new Exception(Costanti.ErrorOutOfBounds);
                throw ex;
            }
        }
        catch (Exception exception)
        {
            //sb.AppendLine(Costanti.ErrorException + ex.Message);
            Exception ex = new Exception(Costanti.ErrorException + exception.Message);
            throw ex;
        }
    
        var = val.ToInt();
        sb.AppendLine(nameVar + " = " + var);
    }

    protected void SetBool(ref bool var, string val, string nameVar, StringBuilder sb)
    {
        bool value;
        try
        {
            value = Convert.ToBoolean(val);
           
        }
        catch 
        {
            throw;
        }
    
        var = Convert.ToBoolean(val);
        sb.AppendLine(nameVar + " = " + var);
    }

    protected void SetString(ref string var, ref string val, string nameVar, StringBuilder sb){
        val = CalcValString(val, sb);
        if(val == null)
            return;
        if(val.Contains('"') && val.Trim().StartsWith('"'))
            SetString(ref var, ref val, nameVar, sb);
        if(val == null)
            return;
        var = val;
        sb.AppendLine(nameVar + " = " + var);
      
    }

    private string CalcValString(string val, StringBuilder sb){
        if(!val.Trim().StartsWith('"') || !val.Trim().EndsWith('"') || val.Trim().IndexOf('"').Equals(val.Trim().LastIndexOf('"') )){
            sb.AppendLine(Costanti.Error + "write the string inside the " + '"' + " " + '"' + " ");
            return null;
        }
        val = val.Substring(val.IndexOf('"') + 1);
        val = val.Substring(0, val.LastIndexOf('"'));
        return val;
    }

    protected void SetRegisteredObj(ref RigidBody2D registeredObj, string name, SceneTree sceneTree, StringBuilder sb){
        Node node = GetNodeInUniqueNameGroup(name, sceneTree);
        if(node == null){
            sb.AppendLine(Costanti.Error + "No object with " + name + " uniqueName find");
            return;
        }
        RigidBody2D rigidBody = (RigidBody2D) node;
        registeredObj = rigidBody;
        sb.AppendLine("Object " + name + " registered");
    }

    private Node GetNodeInUniqueNameGroup(string name, SceneTree sceneTree){
		List<Node> nodeList = sceneTree.GetNodesInGroup(Costanti.UniqueNameGroup).ToList();
		foreach(Node node in nodeList){
            GD.Print("Nodo in gruppo = " + node.Name);
            NodePath nodePath = node.GetPath();
            Interactable interactable = sceneTree.Root.GetNode<Interactable>(nodePath);
			ConsoleInteraction consoleInteraction = interactable.GetConsoleInteraction();
            if(name.Trim().Equals("")){
                GD.Print("uniqueName not written");
                continue;
            }
            GD.Print("UniqueName = " + consoleInteraction.UniqueName + " Searched Name = " + name);
			if(consoleInteraction.UniqueName.Equals(name)){
				return node;
			}
		}
		return null;
	}

    protected bool IsUniqueNameUnique(string name, SceneTree sceneTree, StringBuilder sb){
        name = name.Substring(name.IndexOf('"') + 1, name.LastIndexOf('"') - (name.IndexOf('"') + 1));
        Node node = GetNodeInUniqueNameGroup(name, sceneTree); 
        GD.Print("Unique name to check is " + name); 
        if(node == null){
            GD.Print("The uniqueName is unique");
            return true;
            
        }
        GD.Print("The uniqueName is not unique");
        sb.AppendLine(Costanti.Error + "This unique name already exists");
        return false;
    }
}
