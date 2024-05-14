using Godot;
using System;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

public partial class CodeAnalizer : Node
{
    private ForCommand forCommand;
    private ParameterControl parameterControl;
    private CommandControl commandControl;
	private MethodControl methodControl;
	private ConsoleInteraction selectedObj;
	private ScriptConsole scriptConsole;

    public CodeAnalizer(ScriptConsole script)
	{
		scriptConsole = script;
        parameterControl = new ParameterControl();
		commandControl = new CommandControl(this, parameterControl); 
		methodControl = new MethodControl(this);

	}

	public ConsoleInteraction GetSelectedObject(){
		return scriptConsole.SelectedObj;
	}

    public void CheckCode(ref string code, ref bool isBlock, StringBuilder sb, float? i = null, string nameForCounter = "")
	{
		//GD.Print("Code " + code + " forCount = " + i);
		string line;
		try{
			line = FindLine(ref code, ref isBlock, sb);
		} catch{
			throw;
		}
		if(line.Length == 0){
			sb.AppendLine(Costanti.ErrorNotRecognizedCode);
			return;
		}
		//GD.Print("isBlock = " + isBlock);
		if (!isBlock)
		{
			try{
				CheckLine(line, ref code, sb, i, nameForCounter);
			} catch{
				throw;
			}

			//GD.Print("code to check again " + code);
			if (!code.Equals("")){
				try{
					CheckCode(ref code, ref isBlock, sb, i);
				} catch{
					throw;
				}
			}
				
			return;
		}
		CheckBlock(ref line, sb, i, nameForCounter);
		
		
		isBlock = false;
		if (!code.Equals("")){
			try{
				CheckCode(ref code, ref isBlock, sb, i);
			} catch{
				throw;
			}
		}
			
	}

    private string FindLine(ref string code, ref bool isBlock, StringBuilder sb)
	{
		int posDot = -1;
		int posBlock = -1;
		int posBracket = -1;
		string substring = "";
		//GD.Print("FindLine initial code " + code);
		if (code.Contains("{"))
		{
			posBlock = code.IndexOf("{");
			GD.Print("posBlock: " + posBlock);

		}
		if (code.Contains(";"))
		{
			posDot = code.IndexOf(";");
			GD.Print("posDot: " + posDot);
		}
		if (code.Contains("("))
		{
			posBracket = code.IndexOf("(");
			GD.Print("posBracket: " + posBracket);
		}
		try{
			if (posDot == posBlock)
			{
				Exception ex = new Exception(Costanti.ErrorNotRecognizedCode);
				throw ex;
			}
		}catch{
			throw;
		}
		//GD.Print("posDot = " + posDot + " PosBlock = " + posBlock);
		if (posDot > -1 && ((posBlock != -1 && posDot < posBlock) || (posBlock == -1)) && ((posBracket != -1 && posBracket > posDot) || (posBracket == -1)))
		{
			substring = code.Substring(0, posDot + 1);
			if (substring.Length != code.Length)
				code = code.Substring(posDot + 1);
			else
				code = "";
			GD.Print("; rilevato: " + substring);
			substring = substring.Remove(substring.Length - 1);

		}
		else
		if (posDot > -1 && posBracket > -1 && posBracket < posDot && posDot > code.IndexOf(")") && (posBlock == -1 || posBlock > posDot))
		{
			substring = code.Substring(0, posDot + 1);
			if (substring.Length != code.Length)
				code = code.Substring(posDot + 1);
			else
				code = "";
			GD.Print("() rilevato: " + substring);
		} 
		else
		if (posBlock > -1)
		{
			string codeBlock = this.CalcCodeBlock(code);
			//string bracketBlock = "";
			substring = code.Substring(0, code.IndexOf("{") + 1) + codeBlock;
			GD.Print("Substring after Bracket research: " + substring);
			if (substring.Length != code.Length)
				code = code.Substring(substring.Length);
			else
				code = "";
			isBlock = true;
			GD.Print("{} rilevato: " + substring);
		}

		GD.Print("Remaining Code: " + code);
		return substring;
	}

	private string CalcCodeBlock(string code){
		int count = -1;
		//GD.Print("Code in CalcBracket: " + code);
		string substring = code.Substring(0, code.LastIndexOf("}") + 1);
		//substring = Regex.Replace(substring, "\n","");
		substring = substring.Substring(code.IndexOf("{") + 1);
		GD.Print("calcBracketSubstring_Start: " + substring);
		//return substring;
		substring = _calcCodeBlock(substring, ref count);
		for(int i = 0; i < count; i++){
			substring = substring + "}";
		}
		return substring;
	}

	private string _calcCodeBlock(string substring, ref int count){
		if(substring.Contains("{") && substring.Contains("}") && substring.IndexOf("{") > substring.IndexOf("}") ){
			count++;
			substring = substring.Substring(0, substring.LastIndexOf("}"));
			substring = _calcCodeBlock(substring, ref count);
		}
		return substring;
	}

    private void CheckBlock(ref string block, StringBuilder sb, float? i = null, string nameForCounter = "")
	{
		FindCommandInBlock(ref block, sb, i, nameForCounter);
		
	}

	private void CheckLine(string line, ref string code, StringBuilder sb, float? i = null, string nameForCounter = ""){
		string nameParameter;
		string valParameter;

		selectedObj = scriptConsole.SelectedObj;
		if (line.StartsWith("\n")) 
			line = line.Substring(1);
		
		if(!line.Contains("=")){
			if(line.Contains("(") && line.Contains(")")){
				methodControl.CheckMethod(line, ref code, sb, i, nameForCounter);
				return;
			}
			sb.AppendLine(Costanti.ErrorNotRecognizedCode);
			return;
		} 
		
		nameParameter = line.Substring(0, line.IndexOf("=")).Trim();
		line = line.Substring(line.IndexOf("="));
		if(line.Length == 1){
			sb.AppendLine(Costanti.ErrorNotRecognizedCode);
			return;
		}
		valParameter = line.Substring(1).Trim();
		GD.Print("NameParameter = " + nameParameter + " | ValParameter = " + valParameter);
		try{
			parameterControl.SetParameter(nameParameter, valParameter, selectedObj, sb, i, nameForCounter);
		} catch{
			throw;
		}
		
	}
    private void FindCommandInBlock(ref string block, StringBuilder sb, float? i = null, string nameForCounter = "")
	{
		string body = "";
		string command = block.Substring(0, block.LastIndexOf("{")).Trim() + 1;
		int pos0 = block.IndexOf("{");
		int pos = block.LastIndexOf("}");
		if(pos0 != pos - 1)
			body = block.Substring(pos0 + 1, pos - 1 - pos0 ).Trim();
		commandControl.CheckCommand(command, ref body, sb, i, nameForCounter);
	
	}
}
