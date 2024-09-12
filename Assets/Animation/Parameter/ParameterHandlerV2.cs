using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterHandlerV2
{

    public Dictionary<Type, List<Parameter<object>>> Parameters;

    public ParameterHandlerV2()
    {
        Parameters = new Dictionary<Type, List<Parameter<object>>>();
    }

    public void AddParameter<T>(Parameter<T> p)
    {
        if (!Parameters.ContainsKey(p.GetType()))
            Parameters[p.GetType()] = new List<Parameter<object>>();

        Parameters[p.GetType()].Add(p as Parameter<object>);
    }

    public Parameter<T> GetParameter<T>(string name)
    {
        if (!Parameters.ContainsKey(typeof(T)))
            return null;

        foreach(Parameter<object> p in Parameters[typeof(T)])
        {
            if (p.Name == name)
                return p as Parameter<T>;
        }
        return null;
    }

    public NumberParameter CreateNumberParameter(string name, float value = 0, bool isInt = false)
    {
        NumberParameter p = new NumberParameter(name, value, isInt);
        AddParameter(p);
        return p;
    }

    public VectorParameter CreateVectorParameter(string name, Vector2 value)
    {
        VectorParameter p = new VectorParameter(name, value);
        AddParameter(p);
        return p;
    }

    public ColorParameter CreateColorParameter(string name, Color color)
    {
        ColorParameter p = new ColorParameter(name, color);
        AddParameter(p);
        return p;
    }

    public TextureParameter CreateTextureParameter(string name, string textureName)
    {
        TextureParameter p = new TextureParameter(name, textureName);
        AddParameter(p);
        return p;
    }

    public void BindTimeline(Timeline timeline)
    {
        foreach(KeyValuePair<Type,List<Parameter<object>>> kv in Parameters)
        {
            Type t = kv.Key;
            Debug.Log(t);
            Debug.Log(kv.Value.Count);
            Debug.Log(kv.Value[0].Name);
            foreach (Parameter<object> p in kv.Value)
            {
                Debug.Log(p.Name);
                p.BindTimeline(timeline);
            }
        }
    }

}
