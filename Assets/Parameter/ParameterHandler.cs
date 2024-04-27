using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public class ParameterHandler
{

    public Dictionary<string, NumberParameter> Parameters;

    public ParameterHandler()
    {
        Parameters = new Dictionary<string, NumberParameter>();
    }
    public ParameterHandler(ParameterHandler seed)
    {
        Parameters = new Dictionary<string, NumberParameter>();
        foreach (KeyValuePair<string, NumberParameter> entry in seed.Parameters)
            Parameters.Add(entry.Value.Name, new NumberParameter(entry.Value));
    }


    public VectorParameter CreateVectorParameter(string name, Vector2 value, string xName = null, string yName = null)
    {
        if (value == null)
            value = Vector2.zero;

        NumberParameter x = CreateNumberParameter(xName == null ? name + "(X)" : xName, value.x);
        NumberParameter y = CreateNumberParameter(yName == null ? name + "(Y)" : yName, value.y);
        VectorParameter p = new VectorParameter(name, x, y);
        return p;
    }

    public NumberParameter CreateNumberParameter(string name, float value = 0, bool isInt = false)
    {
        if (Parameters.ContainsKey(name))
            throw new System.Exception("Parameter \"" + name + "\" already exists.");
        NumberParameter p = new NumberParameter(name, value, isInt);
        AddParameter(p);
        return p;
    }

    public void AddParameter(NumberParameter parameter) { Parameters.Add(parameter.Name, parameter); }
    public void AddVectorParameter(VectorParameter parameter) 
    {
        AddParameter(parameter.X);
        AddParameter(parameter.Y);
    }

    public bool HasParameter(string name) { return Parameters.ContainsKey(name); }
    public bool HasVectorParameter(string name) { return Parameters.ContainsKey(name + "(X)") || Parameters.ContainsKey(name + "(Y)"); }

    public NumberParameter FindParameter(string name)
    {
        if (Parameters.ContainsKey(name))
            return Parameters[name];
        return null;
    }
    
    public VectorParameter FindVectorParameter(string name)
    {
        if (Parameters.ContainsKey(name + "(X)") && Parameters.ContainsKey(name + "(Y)"))
            return new VectorParameter(name, Parameters[name + "(X)"], Parameters[name + "(Y)"]);
        return null;
    }

    public void AddParameters(ParameterHandler handler)
    {
        foreach(KeyValuePair<string, NumberParameter> t in handler.Parameters)
            Parameters.Add(t.Key, t.Value);
    }

    public Dictionary<string, NumberParameter> GetNumberParameters()
    {
        Dictionary<string, NumberParameter> numbers = new Dictionary<string, NumberParameter>();

        foreach (KeyValuePair<string, NumberParameter> t in Parameters)
            if (!t.Key.EndsWith("(X)") && !t.Key.EndsWith("(Y)"))
                numbers.Add(t.Key, t.Value);

        return numbers;
    }
    public Dictionary<string, VectorParameter> GetVectorParameters()
    {
        Dictionary<string, VectorParameter> vectors = new Dictionary<string, VectorParameter>();

        foreach (KeyValuePair<string, NumberParameter> t in Parameters)
            if (t.Key.EndsWith("(X)") || t.Key.EndsWith("(Y)"))
            {
                string vectorName = t.Key.Substring(0, t.Key.Length - 3);
                if(!vectors.ContainsKey(vectorName))
                    vectors.Add(vectorName, FindVectorParameter(vectorName));
            }
        return vectors;
    }

    public void BindTimeline(Timeline timeline)
    {
        foreach (NumberParameter p in Parameters.Values)
            p.BindTimeline(timeline);
    }

}
