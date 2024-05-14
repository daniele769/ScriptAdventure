using Godot;
using MEC;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class DialogUiControl : Control
{
	[Export] TextureRect textureRect;
	[Export] Label nameLabel;
	[Export] Label textLabel;
	[Export] AudioStreamPlayer audioPlayer;
	[Export] AudioStream textSound;
	[Export] float time;
	private CharController charController;
	private bool haveToGoNewDialog;
	private bool areDialogFinished;
	private int maxLenghtText;
	private List<string> textList;
	private int currentTextPos;
	private CoroutineHandle coroutine;

	public bool AreDialogFinished { get => areDialogFinished; set => areDialogFinished = value; }
	public bool HaveToGoNewDialog { get => haveToGoNewDialog; set => haveToGoNewDialog = value; }

	public override void _Ready()
	{
		//maxLenghtText = 440;
		audioPlayer.Stream = textSound;
		haveToGoNewDialog = false;
		areDialogFinished = true;

		textList = new List<string>();
	}

	public void AddText(string text)
	{
		textList.Add(text);
	}

	public void DisplayText(CharController controller = null)
	{
		if(controller != null)
		{
			charController = controller;
			charController.IsMovementEnabled = false;
		}
		areDialogFinished = false;
		currentTextPos = 0;
		this.Visible = true;
		coroutine = Timing.RunCoroutine(RunText(1));
	}

	private IEnumerator<double> RunText(int lenght)
	{
		textLabel.Text = "" + textList[currentTextPos].Substring(0, lenght);
		audioPlayer.Play();
		yield return Timing.WaitForSeconds(time);
		if(lenght == textList[currentTextPos].Length)
		{
			currentTextPos++;
			GD.Print("current text pos = " + currentTextPos);
			
			Timing.KillCoroutines(coroutine);
			if(currentTextPos == textList.Count)
			{
				haveToGoNewDialog = false;
				areDialogFinished = true;
			}
			 else
			 	haveToGoNewDialog = true;
			GD.Print("HaveToGoToNewDialog = " + haveToGoNewDialog);
		}
			
		else
			coroutine = Timing.RunCoroutine(RunText(lenght + 1));
	}

	
	public override void _Process(double delta)
	{
		if(coroutine.IsValid && !haveToGoNewDialog && !areDialogFinished && Input.IsActionJustPressed(Costanti.Interact))
		{
			GD.Print("Entra");
			Timing.KillCoroutines(coroutine);
			textLabel.Text = textList[currentTextPos];
			currentTextPos++;
			if(textList.Count > currentTextPos)
				haveToGoNewDialog = true;
			else
			{
				haveToGoNewDialog = false;
				areDialogFinished = true;
			}
				
			return;
		}

		if(!areDialogFinished && haveToGoNewDialog && Input.IsActionJustPressed(Costanti.Interact))
		{
			haveToGoNewDialog = false;
			coroutine = Timing.RunCoroutine(RunText(1));
			return;
		}

		if(areDialogFinished && !haveToGoNewDialog && Input.IsActionJustPressed(Costanti.Interact))
		{
			if(charController != null)
				charController.IsMovementEnabled = true;
			textList.Clear();
			this.Visible = false;
			charController = null;
		}
	}
}
