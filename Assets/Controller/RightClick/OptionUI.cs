using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class OptionUI : MonoBehaviour
{

    public Text text;
    public Button button;

    public void SetOption(Option option)
    {
        text.text = option.name;
        button.onClick.AddListener(option.action);
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
