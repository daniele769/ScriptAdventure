using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

public partial class ConsoleInt_BoxScriptSpawner : ConsoleInteraction
{
    private CanvasLayer canvasLayer;
    private PanelContainer panelContainer;
    private PackedScene exerciseScene;
    private BoxScript_Spawner parent;
    public List<Signal> signalList;

    public PanelContainer PanelContainer { get => panelContainer; set => panelContainer = value; }


    public ConsoleInt_BoxScriptSpawner(BoxScript_Spawner boxScript_Spawner, CanvasLayer canvas, PackedScene scene){
        parent = boxScript_Spawner;
        canvasLayer = canvas;
        exerciseScene = scene;
        Initialize();
    }

    public override void SetPropertyValue(string name, string value, StringBuilder sb)
    {
        throw new NotImplementedException();
    }

    public override void _Action(StringBuilder sb = null)
    {
		VBoxContainer instance = (VBoxContainer) exerciseScene.Instantiate();
        Exercise_Control exercise_Control = (Exercise_Control) instance;
        parent.exercise_Control = exercise_Control;
        StyleBoxFlat styleBox = new StyleBoxFlat();
        styleBox.BgColor = new Color(0,0,0,1);
        InstantiatePanelContainer(instance, styleBox);
    }

    private void InstantiatePanelContainer(VBoxContainer instance, StyleBoxFlat styleBox){
        panelContainer = MakePanelContainer(styleBox);
        canvasLayer.AddChild(panelContainer);
        panelContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        panelContainer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        VBoxContainer vBoxContainer = new VBoxContainer();
        panelContainer.AddChild(vBoxContainer);
        vBoxContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        vBoxContainer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        AddMenuPanelContainer(vBoxContainer, styleBox);
        vBoxContainer.AddChild(instance);
        instance.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        instance.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        panelContainer.AnchorsPreset = (int)Control.LayoutPreset.FullRect;
        Button nextButton = (Button) Static_NodeMethod.GetInternalChildName(instance, "NextButton");
        Button checkButton = (Button) Static_NodeMethod.GetInternalChildName(instance, "CheckButton");
        nextButton.Visible = false;
        checkButton.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;
    }

    private void AddMenuPanelContainer(VBoxContainer vBoxContainer, StyleBoxFlat styleBox)
    {
        PanelContainer menuPanel = new PanelContainer();
        menuPanel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        menuPanel.Set("theme_override_styles/panel", styleBox);
        var button = GD.Load<PackedScene>("res://Prefab/button_WhiteRounded.tscn");
        Button closeButton = (Button) button.Instantiate();
        closeButton.Text = " Chiudi";
        menuPanel.AddChild(closeButton);
        closeButton.Pressed += CloseBox;
        vBoxContainer.AddChild(menuPanel);
    }

    private void CloseBox()
    {
        panelContainer.QueueFree();
    }

    private PanelContainer MakePanelContainer(StyleBoxFlat styleBox)
    {
        PanelContainer panelContainer = new PanelContainer();
        panelContainer.Set("theme_override_styles/panel", styleBox);

        return panelContainer;
    }

    public void Action(){
        _Action();
    }

    protected override void Initialize()
    {
        methodsList.Add(Costanti.MethodAction);
    }

    public override SwitchControlled GetSwitchControlled()
    {
        GD.Print("Not a SwitchControlled object");
        return null;
    }
}
