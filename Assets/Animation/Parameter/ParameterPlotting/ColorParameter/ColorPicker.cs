using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{

    public DraggableElement HueButton, SatBriButton;
    public Material ColorPickerMaterial;
    public Color BackgroundColor;

    public Rect Rect { get => GetComponent<RectTransform>().rect; }
    public float Size { get => Mathf.Min(Rect.width, Rect.height); }

    public ColorParameter Parameter;

    public void SetParameter(ColorParameter parameter)
    {
        Parameter = parameter;
    }

    private void Update()
    {
        if(Parameter != null)
            ColorPickerMaterial.SetColor("HSVColor", Parameter.GetHSVValue().ToColor());
        ColorPickerMaterial.SetFloat("Size", Size);
    }

}
