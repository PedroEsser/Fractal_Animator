using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ParameterPlotter<T> : MonoBehaviour
{

    public bool updatePlot = true;
    public Parameter<T> parameter;

    void Start()
    {
        
    }


    virtual protected void Update()
    {
        if (updatePlot && parameter != null)
            UpdatePlotter(parameter.GetValue());
    }

    public virtual void SetParameter(Parameter<T> par) 
    { 
        this.parameter = par;
    }
    public abstract void UpdatePlotter(T value);

}
