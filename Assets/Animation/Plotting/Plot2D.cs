using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class Plot2D : MonoBehaviour
{

    public bool smooth;
    public AxisPlot xAxis, yAxis;
    public Image Plane;
    public float angle, smoothAngle;
    public UnityEvent<PointerEventData> onDrag;

    public Vector4 Window { get => new Vector4((float)xAxis.smoothOffset, (float)yAxis.smoothOffset, (float)xAxis.smoothWindow, (float)yAxis.smoothWindow); }
    public Vector2 Center { 
        get => new Vector2((float)xAxis.offset, (float)yAxis.offset);
        set 
        {
            xAxis.SetOffset(value.x);
            yAxis.SetOffset(value.y);
        }
    }
    public Vector2 WindowSize { 
        get => new Vector2((float)xAxis.smoothWindow, (float)yAxis.smoothWindow);
        set
        {
            xAxis.SetWindow(value.x);
            yAxis.SetWindow(value.y);
        }
    }
    
    public Rect PlaneRect { get => Plane.rectTransform.rect; }
    public Vector2 ScreenDimensions { get => new Vector2(PlaneRect.width, PlaneRect.height); }

    public float AspectRatio { get => xAxis.Length / yAxis.Length; }

    protected void Start()
    {
        SetSmooth(smooth);
    }

    public void SetSmooth(bool smooth)
    {
        this.smooth = smooth;
        xAxis.smooth = smooth;
        yAxis.smooth = smooth;
    }

    protected void Update()
    {
        smoothAngle = smooth ? Mathf.Lerp(smoothAngle, angle, 0.2f) : angle;
        if (Plane.material != null)
        {
            Plane.material.SetVector("_Window", Window);
            Plane.material.SetFloat("_Angle", smoothAngle);
        }
    }

    public int XSpaceToPixelSpace(float x)
    {
        return xAxis.getPixelAt(x);
        //return (int)((x - xAxis.offset) * PlaneRect.width / xAxis.smoothWindow);
    }
    public int YSpaceToPixelSpace(float y)
    {
        return (int)(yAxis.getPixelAt(y) - yAxis.Length);
        //return (int)((y - yAxis.offset) * PlaneRect.height / yAxis.smoothWindow);
    }

    public Vector2Int WorldSpaceToPixelSpace(Vector2 world)
    {
        return new Vector2Int(XSpaceToPixelSpace(world.x), YSpaceToPixelSpace(world.y));
    }

    public Vector2 PixelSpaceToWorldSpace(Vector2 pixel)
    {
        return new Vector2((float)xAxis.getRealAt((int)pixel.x), (float)(yAxis.getRealAt((int)pixel.y) + yAxis.smoothWindow));
        //pixel = pixel / ScreenDimensions - new Vector2(0.5f, 0.5f);
        //return Center + WindowSize * pixel;
    }

    public void OnScroll(BaseEventData eventData)
    {
        xAxis.OnScroll(eventData);
        yAxis.OnScroll(eventData);
    }

    public void OnDrag(BaseEventData eventData)
    {
        PointerEventData p = (PointerEventData)eventData;
        Vector2 drag = new Vector2((float)xAxis.DragForce(p.delta), (float)yAxis.DragForce(p.delta));
        drag = Complex.Polar(smoothAngle) * (Complex)drag;
        Center = new Vector2((float)xAxis.offset, (float)yAxis.offset) - drag;
        onDrag.Invoke(p);
    }

    public void ResetAxis()
    {
        if((xAxis.window / yAxis.window - AspectRatio) < 0)
            xAxis.SetWindow(yAxis.window * AspectRatio);
        else
            yAxis.SetWindow(xAxis.window / AspectRatio);
    }

}
