using Godot;
using System;

public partial class InteractiveBox : PanelContainer
{
	[Export] private Label label;
	private Node parent;

    public override void _EnterTree()
    {
        parent = this.GetParent();
    }

	public override void _Process(double delta)
	{
		if(label.Text.Equals(Costanti.If1ArgSpawnerDef)){
			SwitchInteractiveBox("res://Prefab/UIElementExerciseMode/Block/if_1arg.tscn");	
		}
		else if(label.Text.Equals(Costanti.If3ArgsSpawnerDef)){
			SwitchInteractiveBox("res://Prefab/UIElementExerciseMode/Block/if_3args.tscn");
		}
		else if(label.Text.Equals(Costanti.ForIntiSpawnerDef)){
			SwitchInteractiveBox("res://Prefab/UIElementExerciseMode/Block/for_int i.tscn");
		}
		else if(label.Text.Equals(Costanti.ForIntjSpawnerDef)){
			SwitchInteractiveBox("res://Prefab/UIElementExerciseMode/Block/for_int j.tscn");
		}
		else if(label.Text.Equals(Costanti.ElseSpawnerDef)){
			SwitchInteractiveBox("res://Prefab/UIElementExerciseMode/Block/else.tscn");
		}
		else if(label.Text.Equals(Costanti.While1ArgSpawnerDef)){
			SwitchInteractiveBox("res://Prefab/UIElementExerciseMode/Block/while_1arg.tscn");
		}
		else if(label.Text.Equals(Costanti.While3ArgsSpawnerDef)){
			SwitchInteractiveBox("res://Prefab/UIElementExerciseMode/Block/while_3args.tscn");
		}
	}

	private void SwitchInteractiveBox(string resourcePath)
	{
		var scene = GD.Load<PackedScene>(resourcePath);
		Container container = (Container) scene.Instantiate();
		parent.AddChild(container);

		int childPos = Static_NodeMethod.CalcPosInParentChildreen(parent, this);
		parent.MoveChild(container, childPos);
		container.Visible = true;
		this.QueueFree();
	}

	
}
