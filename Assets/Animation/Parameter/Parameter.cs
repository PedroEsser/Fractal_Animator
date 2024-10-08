using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable()]
public abstract class Parameter<T>
{

    public string Name { get; }

    public Interpolation<T> interpolation;

    protected T Value;

    public Parameter(string name, Interpolation<T> interpolation)
    {
        this.Name = name;
        this.Value = interpolation.ValueAt(0);
        this.interpolation = interpolation;
    }

    public Parameter(string name, T value)
    {
        this.Name = name;
        this.Value = value;
    }

    public Parameter(string name) : this(name, default(T)) { }

    public virtual T ValueAt(float t) 
    {
        if (interpolation == null || interpolation.keyframes.Count == 0)
            return Value;
        return interpolation.ValueAt(t);
    }

    public virtual T GetValue()
    {
        return this.Value;
    }

    public virtual void SetValue(T value)
    {
        this.Value = value;
    }

    private void SetT(float t)
    {
        Value = ValueAt(t);
    }

    public virtual void BindTimeline(Timeline timeline)
    {
        timeline.OnTimeChange.AddListener(t => SetT(t));
    }

    public abstract Parameter<T> Copy();

    //public static implicit operator Parameter<object>(Parameter<T> p) => p;
    //public static implicit operator Parameter<T>(Parameter<object> p) => p;


}