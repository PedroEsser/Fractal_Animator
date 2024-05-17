using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VectorParameter;

public class InputVectorPlotter : ParameterPlotter<Vector2Data>
{

    public NumberParameterUI xUI;
    public NumberParameterUI yUI;

    public override void UpdatePlotter(Vector2Data value)
    {
        
    }

    public override void SetParameter(Parameter<Vector2Data> par)
    {
        base.SetParameter(par);
        VectorParameter vec = (VectorParameter)par;

        xUI.SetParameter(vec.X);
        xUI.NameText.text = "X";

        yUI.SetParameter(vec.Y);
        yUI.NameText.text = "Y";
    }

}
