using Godot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public class ParameterControl
{
    public void SetParameter(string nameVar, string valVar, ConsoleInteraction selectedObj, StringBuilder sb, float? i = null, string nameForCounter = ""){
		if(selectedObj.IsParameterExist(nameVar)){
			try{
				SetValue(nameVar, valVar, selectedObj, sb, i, nameForCounter);
			} catch{
				throw;
			}
			
			return;
		}
		sb.AppendLine(Costanti.ErrorNotvalidParameters);
    }

	 public string GetParameter(string nameVar, ConsoleInteraction selectedObj, StringBuilder sb){
		string value = "";
		if(selectedObj.IsParameterExist(nameVar)){
			value = GetValue(nameVar, selectedObj);
			// if(value.Equals("")){
			// 	sb.AppendLine(Costanti.ErrorPropertyNotExist);	
			// }	
			return value;
		}
		sb.AppendLine(Costanti.ErrorPropertyNotExist);
		return value;
    }

	private void CheckOperation(string line, StringBuilder sb, float? i = null){

	}

	private void SetValue(string nameVar, string valVar, ConsoleInteraction selectedObj, StringBuilder sb, float? i = null, string nameForCounter = "")
	{
		GD.Print("ValVar = " + valVar + "| ForCounter = " + nameForCounter);
		if(valVar.Equals(nameForCounter))
		{
			valVar = i.ToString();
			//GD.Print("ValVar ="  + valVar);
		} 
		try{
			selectedObj.SetPropertyValue(nameVar, valVar, sb);
		} catch{
			throw;
		}
	}

	private string GetValue(string nameVar, ConsoleInteraction selectedObj)
	{ 
		return selectedObj.GetParameterValue(nameVar);
	}
}
