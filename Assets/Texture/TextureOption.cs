using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureOption : OptionUI
{

    public Text Text;
    public RawImage Image;

    public override void SetOption(Option option)
    {
        base.SetOption(option);
        Text.text = option.name;
        Image.texture = TextureHandler.GetTexture(option.name);
    }

}
