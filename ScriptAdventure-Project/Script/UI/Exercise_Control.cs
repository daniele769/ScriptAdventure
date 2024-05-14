using Godot;
using HiveMQtt.Client;
using HiveMQtt.Client.Options;
using HiveMQtt.Client.Results;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;

public partial class Exercise_Control : VBoxContainer, BrokerChatter
{
	[Signal] public delegate void IsExerciseCorrectEventHandler();
    [Signal] public delegate void IsExerciseWrongEventHandler();
    [Signal] public delegate void IsExerciseBadSolutionEventHandler();
	private int Try = 3;
	private int errorCount;
	private int errorPos;
	private string[] solution;
	private bool isConnectionOk;
	private string connectionError;
	private List<string[]> badSolutionList;
    public int ErrorCount { get => errorCount; set => errorCount = value; }

    public override void _Ready()
    {
		badSolutionList = new List<string[]>();
		errorCount = 0;
		errorPos = -1;
		isConnectionOk = true;

		SetSolution();
		SetBadSolution();
    }

    public override void _Process(double delta)
    {
        if(!isConnectionOk)
		{
			PopupPanel popupPanel = Static_PopupWindowSpawner.MakePopup(false);
			Static_PopupWindowSpawner.AddLabel(popupPanel, "Errore durante la connessione al Broker\n");
			Static_PopupWindowSpawner.AddLabel(popupPanel, connectionError);
			popupPanel.PopupExclusive(this);
			isConnectionOk = true;
		}
    }

    private void SetSolution(){
		solution = (string[]) this.GetMeta("Solution");
	}

	private void SetBadSolution(){
		int n = (int) this.GetMeta("BadSolutionCount");
		if(n != 0){
			for(int i = 0; i < n; i++){
				badSolutionList.Add((string[]) this.GetMeta("BadSolution" + i));
			}
		}
	}

    private void CheckAction(){
		List<Label> lista = GetInteractiveTypeLabel();
		List<int> listaRisultati = new List<int>();
		int globalResult = 1;
		int i = 0;
		if(lista.Count > solution.Length){
			globalResult = -2;
			errorCount++;
			PrintResult(globalResult, -1);
			foreach(Label label in lista){
				UpdateLabelPanelStyle(label, -1);
			}
			return;
		}
		foreach(Label label in lista){
			listaRisultati.Add(ValidateExercise(label, i));
			i++;
		}
		i = 0;
		foreach(int result in listaRisultati){
			if(result == 0 && globalResult == 1){
				globalResult = 0;
				errorPos = i;
			} else
			if(result == -1 && (globalResult == 1 || globalResult == 0)){
				globalResult = -1;
				errorPos = i;
			}
			UpdateLabelPanelStyle(lista[i], result);
			i++;
		}
		if(globalResult == 0 || globalResult == -1)
			errorCount++;
		if(i == GetInteractiveTypeLabel().Count && GetInteractiveTypeLabel().Count < solution.Length)
		{
			globalResult = -3;

			foreach(Label label in lista)
			{
				UpdateLabelPanelStyle(label, -1);
			}
			PrintResult(globalResult, -1);
			errorCount++;
		}
		PrintResult(globalResult, errorPos);
	}

	private void UpdateLabelPanelStyle(Label label, int result){
		PanelContainer panelContainer = (PanelContainer) Static_NodeMethod.GetParentWithType(label, "PanelContainer");
		if(result == 0)
			panelContainer.SelfModulate = new Color(122, 122, 0);

		if(result == -1)
			panelContainer.SelfModulate = new Color(255, 0, 0);

		if(result == 1)
			panelContainer.SelfModulate = new Color(0, 255, 0);
	}

	private Label GetOutput_TextBox(){
		Node parent = Static_NodeMethod.GetInternalChildName(this, "Panel_Output");
		Label label = (Label) Static_NodeMethod.GetChildWhitName(parent, "OutputLabel");
		return label;
	}

	private void PrintResult(int result, int i){
		string text = "";
		Label output_TextBox = GetOutput_TextBox();
		if(result == 0){
			Static_BrokerConnection.SendMessageToBroker(this, Costanti.brokerTopicExerciseMode, Costanti.brokerMessageBadSuccess);
			output_TextBox.AddThemeColorOverride("font_color", new Color(0.74f, 0.74f, 0));
			text = " Funziona, ma potrebbe essere più efficiente.";
			EmitSignal(SignalName.IsExerciseBadSolution);
			GD.Print("Risultato esercizio" + text);
			output_TextBox.Text = text;
			PrintHint(i, output_TextBox);
		} 
		if(result == -1){
			Static_BrokerConnection.SendMessageToBroker(this, Costanti.brokerTopicExerciseMode, Costanti.brokerMessageError);
			output_TextBox.AddThemeColorOverride("font_color", new Color(255, 0, 0));
			text = " Errore.";
			EmitSignal(SignalName.IsExerciseWrong);
			GD.Print("Risultato esercizio" + text);
			output_TextBox.Text = text;
			PrintHint(i, output_TextBox);
			return;
		} 
		if(result == 1){
			Static_BrokerConnection.SendMessageToBroker(this, Costanti.brokerTopicExerciseMode, Costanti.brokerMessageSuccess);
			output_TextBox.AddThemeColorOverride("font_color", new Color(0.50f, 0.75f, 0));
			text = " Corretto.";
			EmitSignal(SignalName.IsExerciseCorrect);
			GD.Print("Risultato esercizio" + text);
			output_TextBox.Text = text;
			Button nextButton = (Button) Static_NodeMethod.GetInternalChildName(this, "NextButton");
			if(nextButton.Visible)
				nextButton.Disabled = false;
			return;
		}
		if(result == -2){
			Static_BrokerConnection.SendMessageToBroker(this, Costanti.brokerTopicExerciseMode, Costanti.brokerMessageError);
			output_TextBox.AddThemeColorOverride("font_color", new Color(255, 0, 0));
			text = " Errore.";
			EmitSignal(SignalName.IsExerciseWrong);
			GD.Print("Risultato esercizio" + text);
			output_TextBox.Text = text;
			PrintHint(-1, output_TextBox);
			return;
		} 
		if(result == -3){
			Static_BrokerConnection.SendMessageToBroker(this, Costanti.brokerTopicExerciseMode, Costanti.brokerMessageError);
			output_TextBox.AddThemeColorOverride("font_color", new Color(255, 0, 0));
			text = " Errore.";
			EmitSignal(SignalName.IsExerciseWrong);
			GD.Print("Risultato esercizio" + text);
			output_TextBox.Text = text;
			PrintHint(-2, output_TextBox);
			return;
		} 
	}

	private void PrintHint(int i, Label label){
		if(i == -1){
			label.Text = label.Text + "\n " + "Sono presenti più linee di codice del dovuto, cerca di usare meno codice.";
			return;
		}
		if(i == -2){
			label.Text = label.Text + "\n " + "Sono presenti meno linee di codice del dovuto, devi usare più codice.";
			return;
		}
		if(errorCount > Try){
			string[] hint = (string[]) this.GetMeta("Hint");
			label.Text = label.Text + "\n " + hint[i];
		}
	}

	private int ValidateExercise(Label label, int i){
		GD.Print("label = " + label.Text + " | Solution = " + solution[i]);
		if(label.Text.Trim().Equals("")){
			return -1;
		}
		if(label.Text.Trim().Equals(solution[i])){
			return 1;
		}
		foreach(string[] badSolution in badSolutionList){
			if(label.Text.Trim().Equals(badSolution[i])){
				return 0;
			}
		}
		return -1;
		
	}

	

	public List<Label> GetInteractiveTypeLabel(){
		List<Label> lista = new List<Label>();
		List<Node> listaNodi = new List<Node>();
		Node node = Static_NodeMethod.GetInternalChildName(this, "VBox_RowContainer");
		listaNodi = Static_NodeMethod.GetAllChild(listaNodi, node);
		if(listaNodi.Count == 0){
			//GD.Print("Nessun nodo figlio");
			return lista;
		}
		//GD.Print("ci sono " + listaNodi.Count + " nodi figli");

		//int j = 0;
		foreach (Node child in listaNodi){
			if(child.GetClass().Equals("Area2D")){
				if(((Area2D)child).Visible == false)
					continue;
				// j++;
				// GD.Print("Ne ho trovati " + j);
				Label label = child.GetParent<Label>();
				//label.Text = "Trovato";
				lista.Add(label);
			}
		}
		return lista;
	}

	public void SetTry(int n)
	{
		Try = n;	
	}

	private void CloseBox(){
		this.QueueFree();
	}
    public void SetConnectionResult(bool result)
    {
        isConnectionOk = result;
    }

    public void SetTextError(string text)
    {
        connectionError = text;
    }

}
