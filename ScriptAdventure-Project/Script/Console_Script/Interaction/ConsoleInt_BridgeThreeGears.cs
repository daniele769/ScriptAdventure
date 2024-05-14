using Godot;
using System;
using System.Text;

public partial class ConsoleInt_BridgeThreeGears : ConsoleInteraction, SwitchControlled
{
   private BridgeControlThreeGears bridgeControlThreeGears;

   public ConsoleInt_BridgeThreeGears(BridgeControlThreeGears control)
    {
        bridgeControlThreeGears = control;
        Initialize();
    }

    public override void SetPropertyValue(string name, string value, StringBuilder sb)
    {
        if (IsParameterExist(name))
        {
            
            switch (name)
            {
                case "UniqueName":  if(!IsUniqueNameUnique(value, bridgeControlThreeGears.GetTree(), sb))
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
        GD.Print("StartRotation");
        bridgeControlThreeGears.StartRotation(switchControl);
    }

    public void CloseAction(SwitchControl switchControl)
    {
        GD.Print("StartRollback");
        bridgeControlThreeGears.StartRollback(switchControl);
    }
}
