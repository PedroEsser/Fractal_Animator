using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable()]
public class ColorParameter : Parameter<Color32>
{

    public ColorParameter(string name, Color value) : base(name, value)
    {

    }

}
