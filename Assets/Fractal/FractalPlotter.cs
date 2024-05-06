using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FractalPlotter : MonoBehaviour
{

    public Plot2D plot;
    public RightClickable rightClick;

    private Vector2 beginDragCenter;

    public WindowSettings WindowHandler { get => ConfigurationHandler.CurrentConfig.Settings.WindowHandler; }

    void Start()
    {
        SetupListeners();
    }

    private void SetupListeners()
    {
        EventTrigger trigger = plot.Plane.GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Scroll;
        entry.callback.RemoveAllListeners();
        entry.callback.AddListener((data) =>
        {
            HandleZoom(((PointerEventData)data).scrollDelta.y);
        });
        trigger.triggers.Add(entry);

        VectorParameter center = WindowHandler.GetParameters().FindVectorParameter("Center");

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.BeginDrag;
        entry.callback.AddListener((data) => beginDragCenter = center.GetValue());
        trigger.triggers.Add(entry);
        
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.EndDrag;
        entry.callback.AddListener((data) => UndoHandler.DoParameterSetAction(center, center.GetValue(), beginDragCenter));
        trigger.triggers.Add(entry);

        plot.onDrag.AddListener((data) => WindowHandler.Center = plot.Center);
    }

    public void Update()
    {

        plot.Center = WindowHandler.Center;
        //ConfigurationHandler.SaveConfig();
        float xWindow = Mathf.Pow(10, -WindowHandler.Zoom + 1);
        plot.xAxis.SetWindow(xWindow);
        plot.yAxis.SetWindow(xWindow / plot.AspectRatio);

        plot.angle = WindowHandler.Angle * Mathf.PI * 2;
    }

    private void HandleZoom(float scrollDelta)
    {
        Action last = UndoHandler.GetLastAction();
        if(last != null && last is ParameterAction<float>.ParameterSet l && l.parameter.Name == "Zoom")
        {
            WindowHandler.Zoom = WindowHandler.Zoom + scrollDelta * 1f / 32;
            l.setValue = WindowHandler.Zoom;
        }
        else
        {
            UndoHandler.DoParameterSetAction(WindowHandler.GetParameters().FindParameter("Zoom"), WindowHandler.Zoom + scrollDelta * 1f / 32, WindowHandler.Zoom);
        }
    }

}
