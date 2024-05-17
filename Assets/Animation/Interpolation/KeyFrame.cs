using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable()]
public class KeyFrame<T>
{

    public int t;
    public T value;

    public KeyFrame(int t, T value)
    {
        this.t = t;
        this.value = value;
    }

}