using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static OptionUI;

public class RightClickable : MonoBehaviour
{

    public List<Option> options = new List<Option>();

    void Start()
    {
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>() ?? gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => RightClickHandler.HandleRightClick((PointerEventData)data, this));
        trigger.triggers.Add(entry);
    }

    public void AddOption(string name, UnityAction action)
    {
        options.Add(new Option(name, action));
    }

}
