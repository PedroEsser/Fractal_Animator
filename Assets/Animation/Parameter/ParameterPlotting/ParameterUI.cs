using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ParameterUI<T> : MonoBehaviour
{

    public Text NameText;
    public Parameter<T> Parameter;

    public abstract void SetParameter(Parameter<T> parameter);


}
