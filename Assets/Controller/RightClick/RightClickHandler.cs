using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class RightClickHandler : MonoBehaviour
{

    public OptionsWindow RightClickWindowPrefab;
    private static OptionsWindow RightClickWindow;

    void Start()
    {
        RightClickWindow = GameObject.Instantiate(RightClickWindowPrefab, transform);
        RightClickWindow.gameObject.SetActive(false);
    }

    public static void HandleRightClick(PointerEventData evt, RightClickable r)
    {
        if (evt.button != PointerEventData.InputButton.Right)
            return;
        Vector2 offset = evt.position;
        offset.x += RightClickWindow.rect.sizeDelta.x/2 - 10;
        offset.y -= RightClickWindow.rect.sizeDelta.y/2 - 10;
        RightClickWindow.transform.position = offset;
        RightClickWindow.Appear(r.options);
    }

    public static void HandleRightClickDisappear()
    {
        RightClickWindow.Disappear();
    }

}