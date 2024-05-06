using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputVectorPlotter : ParameterPlotter<Vector2>
{

    public NumberParameterUI xUI;
    public NumberParameterUI yUI;

    public override void UpdatePlotter(Vector2 value)
    {
        
    }

    public override void SetParameter(Parameter<Vector2> par)
    {
        base.SetParameter(par);
        VectorParameter vec = (VectorParameter)par;

        xUI.SetParameter(vec.X);
        xUI.NameText.text = "X";

        yUI.SetParameter(vec.Y);
        yUI.NameText.text = "Y";
    }

}
