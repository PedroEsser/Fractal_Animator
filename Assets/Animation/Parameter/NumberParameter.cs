using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable()]
public class NumberParameter : Parameter<float>
{

    [NonSerialized()]
    public GameObject AxisNumberPlotterPrefab;
    public bool isInt { get; }

    public NumberParameter(string name, float value = 0, bool isInt = false) : base(name, value)
    {
        this.isInt = isInt;
    }
    public NumberParameter(NumberParameter seed) : this(seed.Name, seed.Value, seed.isInt)
    {
        this.interpolation = seed.interpolation;
    }

    override public float ValueAt(float t)
    {
        return isInt ? Mathf.RoundToInt(base.ValueAt(t)) : base.ValueAt(t);
    }

    public GameObject CreateAxisNumberPlotter(Transform parent = null)
    {
        return UnityEngine.Object.Instantiate(AxisNumberPlotterPrefab, parent);
    }

    public override Parameter<float> Copy()
    {
        return new NumberParameter(Name, GetValue(), isInt);
    }
}
