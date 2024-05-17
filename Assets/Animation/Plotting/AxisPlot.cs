using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AxisPlot : MonoBehaviour
{

    public bool isVertical, smooth, isActive, isInt;
    public double offset;
    public double window;
    public double smoothOffset, smoothWindow;
    public Image Axis;
    public GameObject labelPrefab;
    public List<GameObject> labels;
    public RectTransform labelsContainer;
    public Vector2 interval;
    public string labelSuffix;
    public Color background, foreground;

    private Rect AxisRect { get => Axis.rectTransform.rect; }

    public float Length { get => (isVertical ? AxisRect.height : AxisRect.width); }
    public float Height { get => (isVertical ? AxisRect.width : AxisRect.height); }

    public double PixelSpaceToAxisSpace { get => window / Length; }
    public double AxisSpaceToPixelSpace { get => Length / window; }

    protected void Start()
    {
        Axis.material = Instantiate(Axis.material);         // little hack so different ojects with the smae material can have different properties
        Axis.material.SetColor("_Background", background);
        Axis.material.SetColor("_Foreground", foreground);
        if (!isVertical)
            gameObject.AddComponent<VerticalLayoutGroup>();
        else
        {
            gameObject.AddComponent<HorizontalLayoutGroup>();
            labelsContainer.transform.SetSiblingIndex(1);
        }

        labels = new List<GameObject>();
    }

    protected void Update()
    {
        smoothWindow = Mathf.Lerp((float)smoothWindow, (float)window, 0.2f);
        smoothOffset = Mathf.Lerp((float)smoothOffset, (float)offset, 0.2f);
        

        double log = Mathf.Log10((float)(30d  * smoothWindow / Length));
        double a = Mathf.Pow(10, (float)(log - Mathf.Floor((float)log)));
        double delta;

        if (a < 2)
            delta = 1;
        else if (a < 5)
            delta = 2;
        else
            delta = 5;

        delta *= Mathf.Pow(10, Mathf.Floor((float)log));
        if (isInt)
        {
            delta = Mathf.Max(1, Mathf.Floor((float)delta));
        }

        Material axisMaterial = Axis.material;
        axisMaterial.SetFloat("_CurrentT", (float)smoothOffset);
        axisMaterial.SetFloat("_Zoom", (float)smoothWindow);
        axisMaterial.SetFloat("_Delta", (float)delta);
        axisMaterial.SetInt("_VerticalFlag", isVertical ? 1 : 0);
        axisMaterial.SetVector("_Interval", interval);
        
        UpdateLabels(delta*5);
    }

    private void UpdateLabels(double delta)
    {
        double low = smoothOffset - smoothWindow / 2;
        double high = smoothOffset + smoothWindow / 2;

        low = Mathf.Floor((float)(low / delta)) + 1;
        high = Mathf.Floor((float)(high / delta));
        int count = (int)(high - low) + 1;

        if(labels.Count < count)
            for (int i = labels.Count; i < count; i++)
                labels.Add(Instantiate(labelPrefab, labelsContainer.transform));
        
        for (int i = 0; i < labels.Count; i++)
        {
            bool active = i < count;
            labels[i].SetActive(active);
            if (active)
            {
                double x = (low + i) * delta;
                labels[i].GetComponent<Text>().text = string.Format("{0:0.######}", x) + labelSuffix;
                x = getPixelAt(x);// + labels[i].GetComponent<RectTransform>().rect.width/2;
                if(isVertical)
                    labels[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(labelsContainer.sizeDelta.x / 2, (float)x - AxisRect.height);
                else
                    labels[i].GetComponent<RectTransform>().anchoredPosition = new Vector3((float)x, -labelsContainer.sizeDelta.y / 2);
            }
        }
    }

    public double getRealAt(float x)
    {
        return smoothOffset + (x - 0.5f) * smoothWindow;
    }

    public double getRealAt(int x)
    {
        return smoothOffset + (x - Length / 2) * PixelSpaceToAxisSpace;
    }

    public int getPixelAt(double real)
    {
        return (int)((real - smoothOffset) * AxisSpaceToPixelSpace + Length / 2);
    }

    public void OnScroll(BaseEventData eventData)
    {
        if (!isActive)
            return;
        SetWindow(window * Mathf.Pow(0.95f, ((PointerEventData)eventData).scrollDelta.y));
    }
    
    public void OnDrag(BaseEventData eventData)
    {
        PointerEventData p = (PointerEventData)eventData;
        if (!isActive)
            return;
        SetOffset(offset - DragForce(p.delta));
    }

    public void SetOffset(double t)
    {
        this.offset = t;
        if (!smooth)
            smoothOffset = offset;
    }
    
    public void SetWindow(double window)
    {
        this.window = window;
        if (!smooth)
            smoothWindow = window;
    }

    public double DragForce(Vector2 delta) { return (isVertical ? delta.y : delta.x) * PixelSpaceToAxisSpace; }
}
