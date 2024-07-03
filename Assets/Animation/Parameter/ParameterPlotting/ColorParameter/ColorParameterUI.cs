using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ColorParameter;

public class ColorParameterUI : ParameterUI<ColorData>
{


    public GameObject ParameterContainer;
    public NumberParameterUI rUI;
    public NumberParameterUI gUI; 
    public NumberParameterUI bUI;
    public NumberParameterUI aUI;
    public Image ColorDisplayer;
    public ColorPicker Picker;
    public RightClickable rightClickable;

    public bool InputView { get => ParameterContainer.activeSelf; }

    public bool IsHSV { get => ((ColorParameter)Parameter).HSV; set => ((ColorParameter)Parameter).HSV = value; }
    public RectTransform Rect { get => GetComponent<RectTransform>(); }

    public override void SetParameter(Parameter<ColorData> parameter)
    {
        base.SetParameter(parameter);
        ColorParameter par = (ColorParameter)parameter;
        rUI.SetParameter(par.R);
        gUI.SetParameter(par.G);
        bUI.SetParameter(par.B);
        aUI.SetParameter(par.A);
        Picker.SetParameter(par);
        UpdateColorChannelNames();
        UpdateRightClickOptions();
    }

    void UpdateRightClickOptions()
    {
        rightClickable.options.Clear();
        if (InputView)
        {
            rightClickable.AddOption("Color Picker", () => 
            {
                SetPickerView();
                ParameterHandlerV2 p = new ParameterHandlerV2();
                p.AddParameter(Parameter);
                p.CreateVectorParameter("test", Vector2.zero);
                p.BindTimeline(ConfigurationHandler.CurrentConfig.Timeline);
            });
        }
        else
        {
            rightClickable.AddOption("Input View", () =>
            {
                SetInputView();
            });
        }
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
        IsHSV = !IsHSV;
        UpdateColorChannelNames();
    }

    public void SetPickerView()
    {
        float newHeight = Rect.rect.height + Picker.GetComponent<RectTransform>().rect.height - ParameterContainer.GetComponent<RectTransform>().rect.height;
        Rect.sizeDelta = new Vector2(Rect.sizeDelta.x, newHeight);
        ParameterContainer.SetActive(false);
        Picker.gameObject.SetActive(true);
        RightClickHandler.HandleRightClickDisappear();
        UpdateRightClickOptions();
    }

    public void SetInputView()
    {
        float newHeight = Rect.sizeDelta.y - Picker.GetComponent<RectTransform>().rect.height + ParameterContainer.GetComponent<RectTransform>().rect.height;
        Rect.sizeDelta = new Vector2(Rect.sizeDelta.x, newHeight);
        Rect.gameObject.SetActive(false);
        ParameterContainer.gameObject.SetActive(true);
        RightClickHandler.HandleRightClickDisappear();
        UpdateRightClickOptions();
    }

}
