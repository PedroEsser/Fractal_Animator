using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VectorParameterUI : MonoBehaviour
{

    public RectTransform rect;
    public VectorParameter parameter;
    public GameObject InterpolationUIPrefab;

    public Text text;
    public RightClickable rightClickable;
    public VectorAxisPlotter axisPlotter;
    public InputVectorPlotter inputPlotter;

    private Vector2 beginDragValue;

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
            CubicSplineInterpolationUI interpolationUI = PopupWindowHandler.HandlePopup(InterpolationUIPrefab, parameter.Name + " Interpolation").GetComponent<CubicSplineInterpolationUI>();
            interpolationUI.SetInterpolation((CubicSplineInterpolation)parameter.interpolation);
        });
    }

    public void SetParameter(VectorParameter parameter)
    {
        this.parameter = parameter;
        axisPlotter.SetParameter(parameter);
        inputPlotter.SetParameter(parameter);
        text.text = parameter.Name;
    }

    public void SetAxisPlotter()
    {
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 300);
        inputPlotter.gameObject.SetActive(false);
        axisPlotter.gameObject.SetActive(true);
        RightClickHandler.HandleRightClickDisappear();
        UpdateOptions();
    }

    public void SetInputPlotter()
    {
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 90);
        axisPlotter.gameObject.SetActive(false);
        inputPlotter.gameObject.SetActive(true);
        RightClickHandler.HandleRightClickDisappear();
        UpdateOptions();
    }

    public void BeginNameDrag(BaseEventData eventData) { beginDragValue = parameter.GetValue(); }

    public void NameDrag(BaseEventData eventData)
    {
        PointerEventData ev = (PointerEventData)eventData;
        Vector2 delta = new Vector2(ev.delta.x * axisPlotter.plot2D.Window.z, ev.delta.y * axisPlotter.plot2D.Window.w);
        parameter.SetValue(parameter.GetValue() + delta / 1000f);
    }

    public void EndNameDrag(BaseEventData eventData)
    {
        UndoHandler.DoParameterSetAction(parameter, parameter.GetValue(), beginDragValue);
    }


}
