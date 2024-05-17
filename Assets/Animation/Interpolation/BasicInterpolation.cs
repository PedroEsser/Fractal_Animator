using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable()]
public abstract class BasicInterpolation<T>
{
    public int startT { get; }
    public int endT { get; }

    public T startValue;
    public T endValue;


    public BasicInterpolation(int startT, int endT, T startValue, T endValue)
    {
        this.startT = startT;
        this.endT = endT;
        this.startValue = startValue;
        this.endValue = endValue;
    }
    public abstract T ValueAt(float t);

}
