using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static OptionUI;

public class OptionsWindow : MonoBehaviour
{

    public OptionUI OptionPrefab;
    public RectTransform rect;
    public Image Container;
    private List<OptionUI> optionUIs = new List<OptionUI>();

    public void Appear(List<Option> options)
    {
        if (gameObject.activeSelf)
            return;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, options.Count * 60 + 10);
        foreach (Option o in options)
        {
            OptionUI ui = GameObject.Instantiate<OptionUI>(OptionPrefab, Container.transform);
            ui.SetOption(o);
            optionUIs.Add(ui);
        }
        gameObject.SetActive(true);
    }

    public void Disappear()
    {
        foreach(OptionUI o in optionUIs)
        {
            Destroy(o.gameObject);
        }
        gameObject.SetActive(false);
        optionUIs.Clear();
    }

}
