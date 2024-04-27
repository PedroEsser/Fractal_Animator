using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable()]
public abstract class Interpolation<T>
{

    public List<BasicInterpolation<T>> interpolations;
    public List<KeyFrame<T>> keyframes;

    public Interpolation()
    {
        interpolations = new List<BasicInterpolation<T>>();
        keyframes = new List<KeyFrame<T>>();
    }

    private void AddKeyFrame(KeyFrame<T> frame)
    {
        if(keyframes.Count == 0)
        {
            keyframes.Add(frame);
            return;
        }
        int i = GetInterpolationAt(frame.t);
        if (i == -1)
        {
            interpolations.Insert(0, CreateInterpolation(frame.t, keyframes[0].t, frame.value, keyframes[0].value));
            keyframes.Insert(0, frame);
            return;
        }

        if (keyframes[i].t == frame.t)
        {
            keyframes[i] = frame;
            if (i > 0)
                interpolations[i - 1].endValue = frame.value;
            if (i < keyframes.Count - 1)
                interpolations[i].startValue = frame.value;
            return;
        }

        if (i == keyframes.Count - 1)
        {
            keyframes.Add(frame);
            interpolations.Add(CreateInterpolation(keyframes[i].t, frame.t, keyframes[i].value, frame.value));
            return;
        }

        interpolations.RemoveAt(i);
        interpolations.Insert(i, CreateInterpolation(keyframes[i].t, frame.t, keyframes[i].value, frame.value));
        interpolations.Insert(i+1, CreateInterpolation(frame.t, keyframes[i+1].t, frame.value, keyframes[i+1].value));
        keyframes.Insert(i + 1, frame);
    }

    public Interpolation<T> AddKeyFrame(int t, T value)
    {
        AddKeyFrame(new KeyFrame<T>(t, value));
        return this;
    }

    public void RemoveKeyFrame(int t)
    {
        KeyFrame<T> f = KeyFrameAt(t);
        Debug.Assert(f != null);
        if (keyframes.Count <= 2)
        {
            interpolations.Clear();
            keyframes.Remove(f);
            return;
        }
        int index = keyframes.IndexOf(f);

        if (index < keyframes.Count - 1)
            interpolations.RemoveAt(index);

        if (index > 0)
            interpolations.RemoveAt(index-1);
        
        if(index > 0 && index < keyframes.Count - 1)
        {
            BasicInterpolation<T> inter = CreateInterpolation(keyframes[index - 1].t, keyframes[index + 1].t, keyframes[index - 1].value, keyframes[index + 1].value);
            interpolations.Insert(index - 1, inter);
        }

        keyframes.Remove(f);
    }

    public int GetInterpolationAt(float t)
    {
        if (t < keyframes[0].t)
            return -1;
        int i;
        for (i = 0; i < keyframes.Count - 1; i++)
        {
            if (t >= keyframes[i].t && t < keyframes[i + 1].t)
                break;
        }
        return i;
    }

    public KeyFrame<T> KeyFrameAt(int t)
    {
        foreach (KeyFrame<T> f in keyframes)
            if (f.t == t)
                return f;
        return null;
    }

    /*public Interpolation<T> AddInterpolation(int endT, T endValue)
    {
        int startT;
        T startValue;
        if(interpolations.Count == 0)
        {
            startT = 0;
            startValue = default;
        }
        else
        {
            startT = interpolations[interpolations.Count - 1].endT;
            startValue = interpolations[interpolations.Count - 1].ValueAt(startT);
        }
        if (endT < startT)
            throw new System.Exception("endT must be larger than last interpolation's end time");

        return AddInterpolation(startT, endT, startValue, endValue);
    }*/

    /*public Interpolation<T> AddInterpolation(int startT, int endT, T startValue, T endValue)
    {
        interpolations.Add(CreateInterpolation(startT, endT, startValue, endValue));
        return this;
    }*/
    public abstract BasicInterpolation<T> CreateInterpolation(int startT, int endT, T startValue, T endValue);
    public abstract Vector4[] SplineAsVector();


    public T ValueAt(float t)
    {
        if (keyframes.Count == 0)
            return default(T);

        if (t < keyframes[0].t)
            return keyframes[0].value;

        if (t >= keyframes[keyframes.Count-1].t)
            return keyframes[keyframes.Count - 1].value;

        return interpolations[GetInterpolationAt(t)].ValueAt(t);
    }

}
