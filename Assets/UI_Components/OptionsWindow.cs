using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static OptionUI;

public class OptionsWindow : MonoBehaviour
{

    public OptionUI OptionPrefab;
    public RectTransform rect;
    public GameObject Container;
    protected List<OptionUI> optionUIs = new List<OptionUI>();
    public Color IdleColor, HoverColor;

    public virtual void Appear(List<Option> options)
    {
        float height = options.Count * OptionPrefab.GetComponent<RectTransform>().rect.height;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, height + 10);
        foreach (Option o in options)
        {
            OptionUI ui = Instantiate(OptionPrefab, Container.transform);
            ui.IdleColor = IdleColor;
            ui.HoverColor = HoverColor;
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
