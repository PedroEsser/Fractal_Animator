using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static VectorParameter;

public class VectorParameterUI : ParameterUI<Vector2Data>
{

    public RectTransform rect;
    public GameObject InterpolationUIPrefab;
    public Texture2D CursorTexture;

    public RightClickable rightClickable;
    public VectorAxisPlotter axisPlotter;
    public InputVectorPlotter inputPlotter;

    private Vector2 beginDragValue;

    public bool InputView { get => inputPlotter.gameObject.activeSelf; }


    void Start()
    {
        UpdateOptions();
        axisPlotter.button.onBeginDrag.AddListener(p => beginDragValue = Parameter.GetValue());
        axisPlotter.button.onEndDrag.AddListener(p => UndoHandler.DoParameterSetAction(Parameter, Parameter.GetValue(), beginDragValue));
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
            CubicSplineInterpolationUI interpolationUI = PopupWindowHandler.HandlePopup(InterpolationUIPrefab, Parameter.Name + " Interpolation").GetComponent<CubicSplineInterpolationUI>();
            interpolationUI.SetInterpolation((CubicSplineInterpolation)Parameter.interpolation);
        });
    }

    public override void SetParameter(Parameter<Vector2Data> parameter)
    {
        base.SetParameter(parameter);
        axisPlotter.SetParameter(parameter);
        inputPlotter.SetParameter(parameter);
    }

    public void SetAxisPlotter()
    {
        float newHeight = rect.sizeDelta.y + axisPlotter.GetComponent<RectTransform>().rect.height - inputPlotter.GetComponent<RectTransform>().rect.height;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);
        inputPlotter.gameObject.SetActive(false);
        axisPlotter.gameObject.SetActive(true);
        RightClickHandler.HandleRightClickDisappear();
        UpdateOptions();
    }

    public void SetInputPlotter()
    {
        float newHeight = rect.sizeDelta.y - axisPlotter.GetComponent<RectTransform>().rect.height + inputPlotter.GetComponent<RectTransform>().rect.height;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);
        axisPlotter.gameObject.SetActive(false);
        inputPlotter.gameObject.SetActive(true);
        RightClickHandler.HandleRightClickDisappear();
        UpdateOptions();
    }

    public void BeginNameDrag(BaseEventData eventData) { beginDragValue = Parameter.GetValue(); }

    public void NameDrag(BaseEventData eventData)
    {
        PointerEventData ev = (PointerEventData)eventData;
        Vector2 delta = new Vector2(ev.delta.x * axisPlotter.plot2D.Window.z, ev.delta.y * axisPlotter.plot2D.Window.w);
        Parameter.SetValue(Parameter.GetValue() + delta / 1000f);
    }

    public void EndNameDrag(BaseEventData eventData)
    {
        UndoHandler.DoParameterSetAction(Parameter, Parameter.GetValue(), beginDragValue);
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
