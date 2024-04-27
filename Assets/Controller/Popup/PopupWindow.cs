using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PopupWindow : MonoBehaviour
{

    public RectTransform rect;
    public GameObject Body;
    public Text HeaderText;

    public void Clear()
    {
        foreach (Transform child in Body.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnHeaderMouseDrag(BaseEventData ev)
    {
        PointerEventData p = (PointerEventData)ev;
        transform.position += (Vector3)p.delta;
    }

}
