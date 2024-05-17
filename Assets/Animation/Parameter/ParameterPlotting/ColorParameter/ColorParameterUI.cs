using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ColorParameter;

public class ColorParameterUI : ParameterUI<ColorData>
{

    public NumberParameterUI rUI;
    public NumberParameterUI gUI; 
    public NumberParameterUI bUI;
    public NumberParameterUI aUI;
    public Image ColorDisplayer;
    public Text ColorSpaceButtonText;
    public bool IsHSV { get => ColorSpaceButtonText.text == "HSV"; }

    public override void SetParameter(Parameter<ColorData> parameter)
    {
        base.SetParameter(parameter);
        ColorParameter par = (ColorParameter)parameter;
        rUI.SetParameter(par.R);
        gUI.SetParameter(par.G);
        bUI.SetParameter(par.B);
        aUI.SetParameter(par.A);
        UpdateColorChannelNames();
    }

    private void Update()
    {
        ColorDisplayer.color = Parameter.GetValue().ToColor();
    }

    private void UpdateColorChannelNames()
    {
        if (!IsHSV)
        {
            rUI.NameText.text = "R";
            gUI.NameText.text = "G";
            bUI.NameText.text = "B";
        }
        else
        {
            rUI.NameText.text = "H";
            gUI.NameText.text = "S";
            bUI.NameText.text = "V";
        }
        aUI.NameText.text = "A";
    }

    public void ToggleColorSpace()
    {
        ColorSpaceButtonText.text = IsHSV ? "RGB" : "HSV";
        UpdateColorChannelNames();
        ((ColorParameter)Parameter).HSV = IsHSV;
    }

}
