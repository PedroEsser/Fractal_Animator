using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DraggableElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    public float ExitMargin = 0;
    public RectTransform RectTransform;
    public Image ui;
    public RectTransform WindowRectTransform;
    public UnityEvent<PointerEventData> onBeginDrag, onDrag, onEndDrag;
    public bool xLock, yLock, disabledWhenOutside = true;

    public bool BeingDragged { private set; get; }

    public Vector2 LocalPosition
    {
        get => RectTransform.anchoredPosition;
        set => RectTransform.anchoredPosition = value;
    }

    public void OnBeginDrag(PointerEventData @event) 
    {
        onBeginDrag.Invoke(@event);
        BeingDragged = true; 
    }

    public void OnDrag(PointerEventData @event) 
    {
        onDrag.Invoke(@event);
        Vector2 pos = LocalPosition;
        if (!xLock)
            pos.x += @event.delta.x;
        if (!yLock)
            pos.y += @event.delta.y;
        LocalPosition = pos;
    }

    public void OnEndDrag(PointerEventData @event) 
    {
        onEndDrag.Invoke(@event);
        BeingDragged = false; 
    }

    void Start()
    {
        BeingDragged = false;
    }

    public bool IsInsideWindow()
    {
        Vector3[] parentCorners = new Vector3[4];
        WindowRectTransform.GetWorldCorners(parentCorners);

        Vector3 childPosition = RectTransform.position;
        Rect rect = RectTransform.rect;

        return childPosition.x - rect.width / 2 + ExitMargin >= parentCorners[0].x &&
               childPosition.x + rect.width / 2 - ExitMargin <= parentCorners[2].x &&
               childPosition.y - rect.height / 2 + ExitMargin >= parentCorners[0].y &&
               childPosition.y + rect.height / 2 - ExitMargin <= parentCorners[2].y;
    }

    void Update()
    {
        ui.enabled = !disabledWhenOutside || IsInsideWindow();
    }
}
