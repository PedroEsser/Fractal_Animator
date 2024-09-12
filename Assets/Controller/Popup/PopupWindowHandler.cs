using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupWindowHandler : MonoBehaviour
{

    public PopupWindow PopupWindowPrefab;
    private static PopupWindow PopupWindow;

    void Start()
    {
        PopupWindow = GameObject.Instantiate(PopupWindowPrefab, transform);
        PopupWindow.gameObject.SetActive(false);
    }

    public static GameObject HandlePopup(GameObject prefab, string title, Vector2 position, Vector2 dimensions)
    {
        PopupWindow.Clear();
        PopupWindow.HeaderText.text = title;
        PopupWindow.rect.sizeDelta = dimensions;
        PopupWindow.rect.position = position;
        GameObject obj = GameObject.Instantiate(prefab, PopupWindow.Body.transform);
        PopupWindow.gameObject.SetActive(true);

        return obj;
    }

    public static GameObject HandlePopup(GameObject prefab, string title, Vector2 dimensions)
    {
        return HandlePopup(prefab, title, new Vector2(960, 540), dimensions);
    }
    
    public static GameObject HandlePopup(GameObject prefab, string title)
    {
        return HandlePopup(prefab, title, new Vector2(600, 800));
    }

    public static void ClosePopup()
    {
        PopupWindow.gameObject.SetActive(false);
    }

}
