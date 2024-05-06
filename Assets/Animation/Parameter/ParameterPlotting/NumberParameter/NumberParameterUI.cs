using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NumberParameterUI : ParameterUI<float>
{

    public NumberParameter parameter;
    public GameObject InterpolationUIPrefab;
    public Texture2D CursorTexture;

    public RightClickable rightClickable;
    public NumberAxisPlotter axisPlotter;
    public InputNumberPlotter inputPlotter;
    public KeyFrameUI keyFrameUI;

    private float beginDragValue;

    public bool InputView { get => inputPlotter.gameObject.activeSelf; }

    void Start()
    {
        UpdateOptions();
        axisPlotter.button.onBeginDrag.AddListener(p => beginDragValue = parameter.GetValue());
        axisPlotter.button.onEndDrag.AddListener(p => UndoHandler.DoParameterSetAction(parameter, parameter.GetValue(), beginDragValue));
    }

    void UpdateOptions()
    {
        rightClickable.options.Clear();
        if (InputView)
        {
            rightClickable.AddOption("Axis View", () => SetAxisPlotter());
        }
        else
        {
            rightClickable.AddOption("Input View", () => SetInputPlotter());
        }
        rightClickable.AddOption("Edit Interpolation", () =>
        {
            HermiteSplineInterpolationUI interpolationUI = PopupWindowHandler.HandlePopup(InterpolationUIPrefab, parameter.Name + " Interpolation").GetComponent< HermiteSplineInterpolationUI>();
            interpolationUI.SetInterpolation((HermiteSplineInterpolation)parameter.interpolation);
        });
    }

    public override void SetParameter(Parameter<float> parameter)
    {
        NumberParameter par = (NumberParameter)parameter;
        this.parameter = par;
        axisPlotter.SetParameter(par);
        inputPlotter.SetParameter(par);
        keyFrameUI.SetParameter(par);
        NameText.text = par.Name;
    }

    public void SetAxisPlotter()
    {
        inputPlotter.gameObject.SetActive(false);
        axisPlotter.gameObject.SetActive(true);
        RightClickHandler.HandleRightClickDisappear();
        UpdateOptions();
    }

    public void SetInputPlotter()
    {
        axisPlotter.gameObject.SetActive(false);
        inputPlotter.gameObject.SetActive(true);
        RightClickHandler.HandleRightClickDisappear();
        UpdateOptions();
    }

    public void BeginNameDrag(BaseEventData eventData) { beginDragValue = parameter.GetValue(); }

    public void NameDrag(BaseEventData eventData)
    {
        PointerEventData ev = (PointerEventData)eventData;
        float delta = (float)(ev.delta.x * axisPlotter.axis.window);
        parameter.SetValue(parameter.GetValue() + delta / 1000f);
    }

    public void EndNameDrag(BaseEventData eventData)
    {
        if (!Input.GetMouseButton(0))
        {
            UndoHandler.DoParameterSetAction(parameter, parameter.GetValue(), beginDragValue);
        }
    }

    public void NamePointerEnter(BaseEventData _)
    {
        Cursor.SetCursor(CursorTexture, new Vector2(CursorTexture.width, CursorTexture.height) / 2, CursorMode.Auto);
    }

    public void NamePointerExit(BaseEventData _)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

    }

}
