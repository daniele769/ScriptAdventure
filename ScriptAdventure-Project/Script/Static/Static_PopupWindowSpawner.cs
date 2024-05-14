using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static Godot.Control;

public static class Static_PopupWindowSpawner 
{
    public static PopupPanel MakePopup(bool isExclusive = true)
	{
		PopupPanel popupPanel = new PopupPanel();
        popupPanel.Exclusive = isExclusive;
		StyleBoxFlat styleBoxFlat = new StyleBoxFlat();
		styleBoxFlat.BgColor = new Color(0,0,0,1);
		popupPanel.Set("theme_override_styles/panel", styleBoxFlat);
		VBoxContainer vBoxContainer = new VBoxContainer();
        popupPanel.AddChild(vBoxContainer);
        vBoxContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        vBoxContainer.SizeFlagsVertical = SizeFlags.ExpandFill;
        return popupPanel;
	}

    public static void AddCloseButton(PopupPanel popupPanel, string text = "Continua")
    {
        popupPanel.Exclusive = true;
        Button closePopupButton = new Button();
		closePopupButton.Text = text;
        VBoxContainer vBoxContainer = (VBoxContainer) Static_NodeMethod.GetChildType(popupPanel, "VBoxContainer");
        vBoxContainer.AddChild(closePopupButton);
        closePopupButton.SizeFlagsVertical = SizeFlags.Expand | SizeFlags.ShrinkEnd;
        Callable  callable = Callable.From(() => ClosePopup(popupPanel));
        closePopupButton.Connect(Button.SignalName.Pressed, callable);
        popupPanel.PopupWindow = false;
    }

    public static void AddLabel(PopupPanel popupPanel, string text)
    {
        Label label = new Label();
		label.SizeFlagsVertical = SizeFlags.ShrinkCenter | SizeFlags.Expand;
		label.Text = text;
        VBoxContainer vBoxContainer = (VBoxContainer) Static_NodeMethod.GetChildType(popupPanel, "VBoxContainer");
		vBoxContainer.AddChild(label);
        
    }

    private static void ClosePopup(PopupPanel popupPanel)
	{
        GD.Print("PopupClose called!!");
		popupPanel.QueueFree();
	}
}
