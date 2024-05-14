using Godot;
using System;
using System.Text;

public class CommandControl
{
    private CodeAnalizer codeAnalizer;
    private ForCommand forCommand;
    private IFCommand ifCommand;
	private ParameterControl parameterControl;

    public CommandControl(CodeAnalizer analizer, ParameterControl control){
        codeAnalizer = analizer;
		parameterControl = control;

        forCommand = new ForCommand(analizer);
		ifCommand = new IFCommand(analizer, parameterControl);
    }

    public void CheckCommand(string command, ref string body, StringBuilder sb, float? i = null, string nameForCounter = "")
	{
		if (command.StartsWith("for("))
		{
			//sb.AppendLine("for command Action!");
			forCommand.For(command, ref body, sb);
			
			
		}
		else
		if (command.StartsWith("if("))
		{
			sb.AppendLine("if command Action!");
			ifCommand.If(command, ref body, sb, i, nameForCounter);
		}
		else
		{
			sb.AppendLine(Costanti.ErrorNotRecognizedCode);
		}
	}
}
