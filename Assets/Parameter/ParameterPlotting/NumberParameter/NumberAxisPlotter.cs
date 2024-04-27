using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NumberAxisPlotter : ParameterPlotter<float>
{

    public AxisPlot axis;
    public DraggableElement button;

    public override void SetParameter(Parameter<float> par)
    {
        base.SetParameter(par);
        axis.SetOffset(par.GetValue()*0.9999f);
        axis.SetWindow(par.GetValue() == 0 ? 2.1 : Mathf.Abs(par.GetValue()*2));
    }

    public override void UpdatePlotter(float value)
    {
        if (button == null)
            return;

        Vector2 pos = button.LocalPosition;
        if (button.BeingDragged)
            parameter.SetValue((float)(axis.getRealAt((int)pos.x) + axis.smoothWindow / 2));
        else
            pos.x = axis.getPixelAt(parameter.GetValue()) - axis.Length / 2;

        pos.y = -axis.Axis.rectTransform.sizeDelta.y / 2;
        button.LocalPosition = pos;
    }
}
