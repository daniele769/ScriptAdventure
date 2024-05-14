using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DocControl : BoxContainer
{
	[Export] private ScriptConsole scriptConsole;
	private DocDescReader dockDescReader;
	private ConsoleInteraction selectedObj;
	private List<Node> nodeAddedList;

	public override void _Ready()
	{
		dockDescReader = new DocDescReader();
		selectedObj = null;

		nodeAddedList = new List<Node>();
	}

	public override void _Draw()
	{
		base._Draw();
		selectedObj = scriptConsole.SelectedObj;
		if (nodeAddedList.Count != 0)
			RemoveAddedNode();
		DrawSelectedObjElements();

	}

	private void DrawSelectedObjElements()
	{
		BoxContainer propertiesDock = (BoxContainer)Static_NodeMethod.GetInternalChildName(this, Costanti.PropertiesDoc);
		BoxContainer methodsDock = (BoxContainer)Static_NodeMethod.GetInternalChildName(this, Costanti.MethodsDoc);

		DrawProperties(propertiesDock);
		DrawMethods(methodsDock);

	}

	private void DrawProperties(BoxContainer propertiesDock)
	{
		Label labelProperty = (Label)Static_NodeMethod.GetInternalChildName(propertiesDock, Costanti.ExampleProperty);
		Label labelDesc = (Label)Static_NodeMethod.GetInternalChildName(propertiesDock, Costanti.ExamplePropertyDesc);
		Label labelValue = (Label)Static_NodeMethod.GetInternalChildName(propertiesDock, Costanti.ExamplePropertyValue);
		Node propertiesVBoxContainer = Static_NodeMethod.GetInternalChildName(propertiesDock, Costanti.PropertiesVBoxContainer);
		Node propertiesDescVBoxContainer = Static_NodeMethod.GetInternalChildName(propertiesDock, Costanti.PropertiesDescVBoxContainer);
		Node propertiesValueVBoxContainer = Static_NodeMethod.GetInternalChildName(propertiesDock, Costanti.PropertiesValueVBoxContainer);
		GD.Print("SelectedObj is " + selectedObj.ToString());
		List<string> propertiesList = selectedObj.GetAllParametersList();
		foreach (string property in propertiesList)
		{
			labelProperty.Visible = true;
			labelDesc.Visible = true;
			labelValue.Visible = true;
			Node newProperty = labelProperty.Duplicate();
			GD.Print("New Node property created " + newProperty.ToString());
			Label name = (Label)InstanceFromId(newProperty.GetInstanceId());
			name.Text = property;
			propertiesVBoxContainer.AddChild(newProperty);
			nodeAddedList.Add(newProperty);


			Node newPropertyValue = labelValue.Duplicate();
			Label value = (Label)InstanceFromId(newPropertyValue.GetInstanceId());
			value.Text = CalcPropertyValue(property);
			propertiesValueVBoxContainer.AddChild(newPropertyValue);
			nodeAddedList.Add(newPropertyValue);

			Node newPropertyDesc = labelDesc.Duplicate();
			Label desc = (Label)InstanceFromId(newPropertyDesc.GetInstanceId());
			desc.Text = dockDescReader.GetDescProperty(property);
			propertiesDescVBoxContainer.AddChild(newPropertyDesc);
			nodeAddedList.Add(newPropertyDesc);
		}
		labelProperty.Visible = false;
		labelDesc.Visible = false;
		labelValue.Visible = false;
	}

	private string CalcPropertyValue(string property)
	{
		string value = "";
		if (property.Equals(Costanti.PropertyRegisteredObj))
		{
			if(selectedObj.RegisteredObj == null)
				return "\t";
			RigidBody2D registeredObjBody = selectedObj.RegisteredObj;
			Interactable interactable = (Interactable) registeredObjBody;
			ConsoleInteraction registeredObj = interactable.GetConsoleInteraction();
			value = registeredObj.UniqueName;
		}
		else
			value = selectedObj.GetParameterValue(property);
		if (value.Equals(""))
			value = "\t";
		return value;
	}

	private void DrawMethods(BoxContainer methodsDock)
	{
		Label labelMethods = (Label)Static_NodeMethod.GetInternalChildName(methodsDock, Costanti.ExampleMethod);
		Label labelDesc = (Label)Static_NodeMethod.GetInternalChildName(methodsDock, Costanti.ExampleMethodDesc);
		Node methodsVBoxContainer = Static_NodeMethod.GetInternalChildName(methodsDock, Costanti.MethodsVBoxContainer);
		Node methodsDescVBoxContainer = Static_NodeMethod.GetInternalChildName(methodsDock, Costanti.MethodsDescVBoxContainer);
		GD.Print("SelectedObj is " + selectedObj.ToString());
		List<string> methodsList = selectedObj.GetAllMethodsList();
		foreach (string method in methodsList)
		{
			Node newMethods = labelMethods.Duplicate();
			GD.Print("New Node methods created " + newMethods.ToString());
			Label name = (Label)InstanceFromId(newMethods.GetInstanceId());
			name.Text = method;
			methodsVBoxContainer.AddChild(newMethods);
			nodeAddedList.Add(newMethods);

			Node newMethodDesc = labelDesc.Duplicate();
			Label desc = (Label)InstanceFromId(newMethodDesc.GetInstanceId());
			desc.Text = dockDescReader.GetDescMethod(method);
			methodsDescVBoxContainer.AddChild(newMethodDesc);
			nodeAddedList.Add(newMethodDesc);
		}
	}

	private void RemoveAddedNode()
	{
		if (nodeAddedList.Count == 0)
			return;
		foreach (Node node in nodeAddedList)
		{
			Node parent = node.GetParent();
			parent.RemoveChild(node);
			node.Dispose();
		}
		nodeAddedList.Clear();
	}

	public override void _Process(double delta)
	{
	}

	private void CloseDock()
	{
		this.Visible = false;
	}
}
