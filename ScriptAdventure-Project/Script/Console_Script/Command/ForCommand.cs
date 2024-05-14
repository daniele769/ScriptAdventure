using Godot;
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

public partial class ForCommand : Node
{
    private CodeAnalizer codeAnalizer;

    public ForCommand(CodeAnalizer analizer){
        codeAnalizer = analizer;
    }

    public void For(string command, ref string body, StringBuilder sb)
	{
		int pos0 = command.IndexOf("(");
		int pos = command.IndexOf(")");
		if(pos0 == -1 || pos == -1){
			sb.AppendLine(Costanti.ErrorNotvalidParameters + " in 'for()' Command");
			return;
		}
		string argument = command.Substring(pos0, pos - pos0 + 1);
		argument = Regex.Replace(argument, " ", "");
		string var = argument.Substring(1, argument.IndexOf(";") - 1);
		argument = argument.Substring(argument.IndexOf(";") + 1);
		string limit = argument.Substring(0, argument.IndexOf(";"));
		argument = argument.Substring(argument.IndexOf(";") + 1);
		string step = argument.Substring(0, argument.IndexOf(")"));
		For_Control(var, limit, step, ref body, sb);
	}

    private void For_Control(string var, string limit, string step, ref string body, StringBuilder sb)
	{
		string nameVar = "";
		string typeVar = "";
		var initialValue = 0f;
		var limitValue = 0f;
		int caso = -1;
		if (var.StartsWith("int"))
		{
			nameVar = var.Substring(3);
			typeVar = "int";
		}
		else
		if (var.StartsWith("float"))
		{
			nameVar = var.Substring(5);
			typeVar = "float";
		}
		else
		{
			sb.AppendLine(Costanti.ErrorNoValidType + " in 'for()' Command");
			return;
		}
		string initial = "";
		if (nameVar.Contains("="))
		{
			initial = nameVar.Substring(nameVar.IndexOf("=") + 1);
			nameVar = nameVar.Substring(0, nameVar.IndexOf("=") );
		}
		else
		{
			sb.AppendLine(Costanti.ErrorNotvalidParameters + " in 'for()' Command");
			return;
		}
		if(nameVar.Equals("")){
			sb.AppendLine("***ERROR*** No counter variable name declared in 'for()' Command");
			return;
		}

		if (limit.StartsWith(nameVar))
		{
			limit = limit.Substring(nameVar.Length);
			GD.Print("NameVar = " + nameVar);
			GD.Print("limit = " + limit);			

			if (limit.StartsWith(">=") || limit.StartsWith("<=") || limit.StartsWith(">") || limit.StartsWith("<"))
			{
				if (limit.StartsWith(">="))
				{
					caso = 0;
					limit = limit.Substring(2);
				}
				else
				if (limit.StartsWith("<="))
				{
					caso = 1;
					limit = limit.Substring(2);
				}
				else
				if (limit.StartsWith(">"))
				{
					caso = 2;
					limit = limit.Substring(1);
				}
				else
				if (limit.StartsWith("<"))
				{
					caso = 3;
					limit = limit.Substring(1);
				}
				else
				{
					sb.AppendLine(Costanti.ErrorNotvalidParameters + " in 'for()' Command");
					return;
				}

				try
				{
					if (typeVar.Equals("int"))
					{
						GD.Print("type is int");
						initialValue = initial.ToInt();
						limitValue = limit.ToInt();
					}
					else
					if (typeVar.Equals("float"))
					{
						GD.Print("type is float (" + initial + ")");
						initialValue = float.Parse(initial, CultureInfo.InvariantCulture.NumberFormat);
						GD.Print("InitialValue = " + initialValue);
						limitValue = limit.ToFloat();
					}

				}
				catch (Exception exception)
				{
					Exception ex = new Exception("Exception Error: " + exception.Message);
					sb.AppendLine("Exception Error: " + ex.Message + " in 'for()' Command");
					return;
				} 
			} else{
				sb.AppendLine(Costanti.ErrorNotvalidParameters + " in 'for()' Command");
				return;
			}
		} else 
		{
			sb.AppendLine(Costanti.ErrorNotvalidParameters + " in 'for()' Command");
			return;
		}
		GD.Print("step = " + step);
		if(step.StartsWith(nameVar)){
			step = step.Substring(nameVar.Length);
			if(step.Equals("++") || step.Equals("--")){
				ForCalc(caso, initialValue, limitValue, step, nameVar , ref body, sb);
			} else{
				sb.AppendLine(Costanti.ErrorNotvalidParameters + " in 'for()' Command");
			}
				
			return;
		}
		sb.AppendLine(Costanti.ErrorNotvalidParameters + " in 'for()' Command");
	}

    private void ForCalc(int caso, float initialValue, float limitValue, string step, string nameVar, ref string body, StringBuilder sb){
		GD.Print("caso = " + caso);
		GD.Print("initialValue = " + initialValue);
		GD.Print("limitValue = " + limitValue);
		GD.Print("step = " + step); 
		bool isBlock = false;
		if(caso == 0 || caso == 2){
			if(step.Equals("++")){
				if(initialValue < limitValue)
					sb.AppendLine(Costanti.WarningCycleNotStart + " in 'for()' Command");
				else
					sb.AppendLine(Costanti.ErrorInfiniteCycle + " in 'for()' Command");
				return;
			}
			if(step.Equals("--")){
				if(caso == 0)
					limitValue--;
				for(float i = initialValue; i > limitValue; i--)
				{
					//sb.AppendLine(nameVar + " = " + i);
					string s = body;
					try{
						codeAnalizer.CheckCode(ref s, ref isBlock, sb, i, nameVar);
					}catch(Exception ex){
						sb.AppendLine(ex.Message + " in 'for()' Command");
						return;
					}

				}
				return;
			}
		}
		if(caso == 1 || caso == 3){
			if(step.Equals("--")){
				if(initialValue > limitValue)
					sb.AppendLine(Costanti.WarningCycleNotStart + " in 'for()' Command");
				else
					sb.AppendLine(Costanti.ErrorInfiniteCycle + " in 'for()' Command");
				return;
			}
			if(step.Equals("++")){
				if(caso == 1)
					limitValue++;
				for(float i = initialValue; i < limitValue; i++)
				{
					//sb.AppendLine(nameVar + " = " + i);
					string s = body;
					try{
						codeAnalizer.CheckCode(ref s, ref isBlock, sb, i, nameVar);
					}catch(Exception ex){
						sb.AppendLine(ex.Message + " in 'for()' Command");
						return;
					}
					
				}
				return;
			}
		}
		
	}
}
