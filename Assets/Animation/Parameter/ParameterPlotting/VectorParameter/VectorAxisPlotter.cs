using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VectorParameter;

public class VectorAxisPlotter : ParameterPlotter<Vector2Data>
{

    public Plot2D plot2D;
    public DraggableElement button;
    public Vector2 Value;

    public override void SetParameter(Parameter<Vector2Data> par)
    {
        base.SetParameter(par);
        Vector2 val = par.GetValue();
        plot2D.Center = val*0.9999f;
        float max = Mathf.Max(Mathf.Abs(val.x), Mathf.Abs(val.y))*2;
        if (max == 0)
            max = 2.1f;
        plot2D.WindowSize = new Vector2(max, max);
    }

    public override void UpdatePlotter(Vector2Data value)
    {
        if (button == null)
            return;

        if (button.BeingDragged)
            parameter.SetValue(new Vector2Data(plot2D.PixelSpaceToWorldSpace(button.LocalPosition)));
        else
            button.LocalPosition = plot2D.WorldSpaceToPixelSpace(value);
    }

}
