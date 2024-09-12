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

    public NumberParameter CreateNumberParameter(string name, float value = 0, bool isInt = false)
    {
        if (HasParameter(name))
            throw new Exception("Parameter \"" + name + "\" already exists.");

        NumberParameter p = new NumberParameter(name, value, isInt);
        AddParameter(p);

        return p;
    }

    public VectorParameter CreateVectorParameter(string name, Vector2 value)
    {
        if (HasVectorParameter(name))
            throw new Exception("Parameter \"" + name + "\" already exists.");

        VectorParameter p = new VectorParameter(name, value);
        AddVectorParameter(p);

        return p;
    }

    public ColorParameter CreateColorParameter(string name, Color32 color)
    {
        if (HasColorParameter(name))
            throw new Exception("Parameter \"" + name + "\" already exists.");

        ColorParameter p = new ColorParameter(name, color);
        AddColorParameter(p);

        return p;
    }

    public TextureParameter CreateTextureParameter(string name, string textureName)
    {
        if (HasTextureParameter(name))
            throw new Exception("Parameter \"" + name + "\" already exists.");

        TextureParameter t = new TextureParameter(name, textureName);
        AddTextureParameter(t);

        return t;
    }

    public void AddParameters(ParameterHandler handler)
    {
        foreach (KeyValuePair<string, NumberParameter> t in handler.Parameters)
            Parameters.Add(t.Key, t.Value);
    }

    public void AddParameter(NumberParameter parameter) { Parameters.Add(parameter.Name, parameter); }
    public void AddVectorParameter(VectorParameter parameter) 
    {
        AddParameter(parameter.X);
        AddParameter(parameter.Y);
    }

    public void AddColorParameter(ColorParameter parameter)
    {
        AddParameter(parameter.R);
        AddParameter(parameter.G);
        AddParameter(parameter.B);
        AddParameter(parameter.A);

    }

    public void AddTextureParameter(TextureParameter parameter) 
    {
        AddVectorParameter(parameter.Position);
        AddVectorParameter(parameter.Size);
        AddVectorParameter(parameter.Tiling);
        AddVectorParameter(parameter.Scale);
    }

    public bool HasParameter(string name) { return Parameters.ContainsKey(name); }
    public bool HasVectorParameter(string name) { return HasParameter(name + VectorParameter.X_SUFFIX) || HasParameter(name + VectorParameter.Y_SUFFIX); }
    
    public bool HasColorParameter(string name) { 
        return HasParameter(name + ColorParameter.R_SUFFIX) || HasParameter(name + ColorParameter.G_SUFFIX)
            || HasParameter(name + ColorParameter.B_SUFFIX) || HasParameter(name + ColorParameter.A_SUFFIX); 
    }
    public bool HasTextureParameter(string name) 
    {
        return 
            HasVectorParameter(name + TextureParameter.POSITION_SUFFIX) ||
            HasVectorParameter(name + TextureParameter.SIZE_SUFFIX) ||
            HasVectorParameter(name + TextureParameter.TILING_SUFFIX) ||
            HasVectorParameter(name + TextureParameter.SCALE_SUFFIX);
    }


    public NumberParameter FindParameter(string name)
    {
        if (Parameters.ContainsKey(name))
            return Parameters[name];
        return null;
    }
    
    public VectorParameter FindVectorParameter(string name)
    {
        if (Parameters.ContainsKey(name + VectorParameter.X_SUFFIX) && Parameters.ContainsKey(name + VectorParameter.Y_SUFFIX))
            return new VectorParameter(name, Parameters[name + VectorParameter.X_SUFFIX], Parameters[name + VectorParameter.Y_SUFFIX]);
        return null;
    }

    public ColorParameter FindColorParameter(string name)
    {
        if(HasParameter(name + ColorParameter.R_SUFFIX) && HasParameter(name + ColorParameter.G_SUFFIX)
            && HasParameter(name + ColorParameter.B_SUFFIX) && HasParameter(name + ColorParameter.A_SUFFIX))
        {
            return new ColorParameter(name, 
                Parameters[name + ColorParameter.R_SUFFIX], 
                Parameters[name + ColorParameter.G_SUFFIX], 
                Parameters[name + ColorParameter.B_SUFFIX], 
                Parameters[name + ColorParameter.A_SUFFIX], false);
        }
        return null;
    }

    public VectorParameter FindTextureParameter(string name)
    {
        if (Parameters.ContainsKey(name + VectorParameter.X_SUFFIX) && Parameters.ContainsKey(name + VectorParameter.Y_SUFFIX))
            return new VectorParameter(name, Parameters[name + VectorParameter.X_SUFFIX], Parameters[name + VectorParameter.Y_SUFFIX]);
        return null;
    }

    public Dictionary<string, NumberParameter> GetNumberParameters()
    {
        Dictionary<string, NumberParameter> numbers = new Dictionary<string, NumberParameter>();

        foreach (KeyValuePair<string, NumberParameter> t in Parameters)
            if (!t.Key.EndsWith(")"))
                numbers.Add(t.Key, t.Value);

        return numbers;
    }
    public Dictionary<string, VectorParameter> GetVectorParameters()
    {
        Dictionary<string, VectorParameter> vectors = new Dictionary<string, VectorParameter>();

        foreach (KeyValuePair<string, NumberParameter> t in Parameters)
            if (t.Key.EndsWith(VectorParameter.X_SUFFIX) || t.Key.EndsWith(VectorParameter.Y_SUFFIX))
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
