using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public abstract class Fractal : ParameterApplier
{

    public readonly string ShaderPath;
    public ParameterHandler FractalParameters;
    public ColorParameter InsideColor, OutsideColor;

    public float Iterations
    {
        get => FractalParameters.FindParameter("Iterations").GetValue();
        set => FractalParameters.FindParameter("Iterations").SetValue(value);
    }
    public float EscapeRadius
    {
        get => FractalParameters.FindParameter("Escape Radius").GetValue();
        set => FractalParameters.FindParameter("Escape Radius").SetValue(value);
    }
    public Vector2 ZStart
    {
        get => FractalParameters.FindVectorParameter("Z Start").GetValue();
        set => FractalParameters.FindVectorParameter("Z Start").SetValue(value);
    }


    public Fractal(string ShaderPath)
    {
        this.ShaderPath = ShaderPath;
        FractalParameters = new ParameterHandler();
        FractalParameters.CreateNumberParameter("Iterations", 32, false);
        FractalParameters.CreateNumberParameter("Escape Radius", 10000);
        FractalParameters.CreateVectorParameter("Z Start", Vector2.zero);
        InsideColor = FractalParameters.CreateColorParameter("Inside Color", Color.black);
        OutsideColor = FractalParameters.CreateColorParameter("Outside Color", Color.white);
    }
    public Fractal(Fractal seed)
    {
        this.ShaderPath = seed.ShaderPath;
        FractalParameters = new ParameterHandler(seed.FractalParameters);
        InsideColor = seed.InsideColor;
        OutsideColor = seed.OutsideColor;
    }

    public virtual void UpdateShader(Material mat)
    {
        mat.SetFloat("_MaxIter", Iterations);
        mat.SetFloat("_EscapeRadius", EscapeRadius);
        mat.SetVector("_ZStart", ZStart);
        mat.SetColor("_InsideColor", InsideColor.GetValue().ToColor());
        mat.SetColor("_OutsideColor", OutsideColor.GetValue().ToColor());
    }

    public abstract IEnumerable<Complex> GetOrbitIterator(Complex seed);

    public ParameterHandler GetParameters()
    {
        return FractalParameters;
    }

    public void BindTimeline(Timeline timeline)
    {
        FractalParameters.BindTimeline(timeline);
    }
}
