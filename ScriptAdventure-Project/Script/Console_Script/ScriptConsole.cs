using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

public partial class ScriptConsole : BoxContainer
{
	private Label outputLabel;
	private TextEdit textEdit;
	private CodeAnalizer codeAnalizer;
	private ConsoleInteraction selectedObj;
	private ScrollContainer scrollContainer;
	private ScrollBar scrollBar; 
	private double maxScrollValue;
	private List<string> logCodeList;
	public int posLog;

    public ConsoleInteraction SelectedObj { get => selectedObj; set => selectedObj = value; }
    public List<string> LogCodeList { get => logCodeList; set => logCodeList = value; }
    public TextEdit TextEdit { get => textEdit; set => textEdit = value; }

    public override void _Ready()
	{
		outputLabel = (Label)Static_NodeMethod.GetInternalChildType(this, "Label");
		textEdit = (TextEdit)Static_NodeMethod.GetInternalChildType(this, "TextEdit");
		logCodeList = new List<string>();
		
		posLog = -1;
		codeAnalizer = new CodeAnalizer(this);
		scrollContainer = (ScrollContainer) Static_NodeMethod.GetInternalChildType(this, "ScrollContainer");
		scrollBar = scrollContainer.GetVScrollBar();
		scrollBar.Connect("changed", Callable.From(this.ScrollConsole));
		maxScrollValue = scrollBar.MaxValue;

		this.Visible = false;
		this.ProcessMode = Node.ProcessModeEnum.Disabled;
	}

	private string GetCode()
	{
		string code = textEdit.Text.Trim();
		return code;
	}

	private void ScrollConsole(){
		if(scrollBar.MaxValue != maxScrollValue){
			maxScrollValue = scrollBar.MaxValue;
			scrollContainer.ScrollVertical = (int) maxScrollValue;
		}
	}

	private void ClearConsole(){
		outputLabel.Text = "";
	}

	private void ValidateCode()
	{
		bool isBlock = false;
		string code = this.GetCode();
		logCodeList.Add(code);
		textEdit.Text = "";
		StringBuilder sb = new StringBuilder();
		try{
			codeAnalizer.CheckCode(ref code, ref isBlock, sb);
		} catch(Exception ex){
			sb.AppendLine(ex.Message);
		}
		sb.AppendLine("-------------------------------------------");
		outputLabel.Text = outputLabel.Text + sb.ToString();
		posLog = -1;

	}
}
