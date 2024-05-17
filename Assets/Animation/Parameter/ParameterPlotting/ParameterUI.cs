using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ParameterUI<T> : MonoBehaviour
{

    public Text NameText;
    public Parameter<T> Parameter;

    public virtual void SetParameter(Parameter<T> parameter)
    {
        this.Parameter = parameter;
        NameText.text = parameter.Name;
    }


}
