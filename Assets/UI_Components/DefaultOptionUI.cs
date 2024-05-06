using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DefaultOptionUI : OptionUI
{

    public Text text;

    public override void SetOption(Option option)
    {
        base.SetOption(option);
        text.text = option.name;
    }

}
