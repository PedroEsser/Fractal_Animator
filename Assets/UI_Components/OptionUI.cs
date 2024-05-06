using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OptionUI : MonoBehaviour
{

    public UnityEvent OnClick;
    public Image Background;
    public Color IdleColor, HoverColor;

    private void Start()
    {
        Background.color = IdleColor;
        SetupTriggers();
    }

    private void SetupTriggers()
    {
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(_ => Clicked());
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener(_ => PointerEnter());
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener(_ => PointerExit());
        trigger.triggers.Add(entry);
    }

    public void Clicked() { OnClick.Invoke(); }
    public void PointerEnter() { Background.color = HoverColor; }
    public void PointerExit() { Background.color = IdleColor; }

    public virtual void SetOption(Option option)
    {
        OnClick.RemoveAllListeners();
        OnClick.AddListener(option.action);
    }

    public class Option
    {
        public string name;
        public UnityAction action;
        public Option(string name, UnityAction action)
        {
            this.name = name;
            this.action = action;
        }
    }

}
