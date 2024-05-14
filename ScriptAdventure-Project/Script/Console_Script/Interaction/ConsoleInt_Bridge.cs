using Godot;
using System;
using System.Text;

public partial class ConsoleInt_Bridge : ConsoleInteraction, SwitchControlled
{  
	private BridgeRotationControl bridgeRotationControl;

    public ConsoleInt_Bridge(BridgeRotationControl control)
    {
        bridgeRotationControl = control;
        Initialize();
    }

    public override void SetPropertyValue(string name, string value, StringBuilder sb)
    {
        if (IsParameterExist(name))
        {
            
            switch (name)
            {
                case "UniqueName":  if(!IsUniqueNameUnique(value, bridgeRotationControl.GetTree(), sb))
                                        break;
                                    SetString(ref uniqueName, ref value, "UniqueName", sb);
                                    propertiesList["UniqueName"] = uniqueName;
                                    GD.Print("Unique name = " + propertiesList["UniqueName"].ToString());
                                    break;
            }
        }
    }

    public override void _Action(StringBuilder sb = null)
    {
        
    }

    public string Action(){
        StringBuilder sb = new StringBuilder();
        this._Action(sb);
        return sb.ToString();
    }

    protected override void Initialize()
    {
        this.propertiesList.Add(Costanti.PropertyUniqueName, uniqueName);

    }

    public override SwitchControlled GetSwitchControlled()
    {
        return this;
    }

    public void OpenAction(SwitchControl switchControl)
    {
        bridgeRotationControl.StartRotation(switchControl);
    }

    public void CloseAction(SwitchControl switchControl)
    {
        bridgeRotationControl.StartRollback(switchControl);
    }
}
