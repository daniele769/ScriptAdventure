using Godot;
using System;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;

public partial class IFCommand : Node
{
    private CodeAnalizer codeAnalizer;
	private ParameterControl parameterControl;

    public IFCommand(CodeAnalizer analizer, ParameterControl control){
        codeAnalizer = analizer;
		parameterControl = control;
    }

    public void If(string command, ref string body, StringBuilder sb, float? i = null, string nameForCounter = "")
	{
		string varName;
		string comparator = "";
		string argument;
		string value;
		int pos0 = command.IndexOf("(");
		int pos = command.IndexOf(")");
		if(pos0 == -1 || pos == -1){
			sb.AppendLine(Costanti.ErrorNotvalidParameters + " in 'if()' Command");
			return;
		}
		argument = command.Substring(pos0, pos - pos0 + 1);
		argument = Regex.Replace(argument, " ", "");
		//GD.Print("argument: " + argument);
		argument = argument.Substring(1, argument.Length - 2);
        sb.AppendLine("Nome var if: " + argument);
		if(!(argument.Contains("==") || argument.Contains("!="))){
			if(IsParameterTrue(argument, sb) == true){
				If_Control(ref body, sb, i, nameForCounter);
			}
			return;
		}
		if(argument.Contains("=="))
			comparator = "==";
		else if (argument.Contains("!="))
			comparator = "!=";	
		varName = argument.Substring(0, argument.IndexOf(comparator));
		GD.Print("VarIf Name: " + varName);
		argument = argument.Substring(argument.IndexOf(comparator) + 2);
		GD.Print("argument: " + argument);
		GD.Print("SelectedObject: " + codeAnalizer.GetSelectedObject());
		value = parameterControl.GetParameter(varName, codeAnalizer.GetSelectedObject(), sb);
		if(value.Equals("")){
			//GD.Print("entra");
			return;
		}
		GD.Print("PropertyValue: " + value + " | Argument: " + argument + " | comparator" + comparator);
		if(IsComparationTrue(value, argument, comparator)){
			GD.Print("Comparation true");
			If_Control(ref body, sb, i, nameForCounter);
			return;
		}
		GD.Print("Comparation false");
	}
	private bool IsComparationTrue(string value, string argument, string comparator){
		bool val = Convert.ToBoolean(value);
		bool argumentVal = Convert.ToBoolean(argument);
		if(comparator.Equals("==")){
			if(val == argumentVal)
				return true;
			return false;
		}
		if(comparator.Equals("!=")){
			if(val != argumentVal)
				return true;
			return false;
		}
		return false;
	}

	private bool? IsParameterTrue(string argument, StringBuilder sb){
		bool isNegative = false;
		bool val;
		if(argument.StartsWith('!')){
			isNegative = true;
			argument = argument.Substring(1);
		}
		if(argument.Trim().Equals("true")){
			return true;
		}
		if(argument.Trim().Equals("False")){
			return false;
		}
		string value = parameterControl.GetParameter(argument, codeAnalizer.GetSelectedObject(), sb);
		if(value == ""){
			//sb.AppendLine(Costanti.ErrorPropertyNotExist);
			return null;
		}
		val = Convert.ToBoolean(value);
		if(isNegative){
			GD.Print("isNegative");
			return !val;
		}
		return val;
	}

    private void If_Control(ref string body, StringBuilder sb, float? i = null, string nameForCounter = ""){
        string s = body;
        bool isBlock = false;
		GD.Print("Val i in IFcontrol = " + i);
		try{
			codeAnalizer.CheckCode(ref s, ref isBlock, sb, i, nameForCounter);
		} catch(Exception ex){
			sb.AppendLine(ex.Message + " in 'if()' Command");
			body = "";
		}
    }
}
