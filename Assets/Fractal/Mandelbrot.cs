using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public class Mandelbrot : Fractal
{

    public Complex Power
    {
        get => (Complex)FractalParameters.FindVectorParameter("Power").GetValue();
        set => FractalParameters.FindVectorParameter("Power").SetValue(value);
    }

    public Mandelbrot(): base("Mandelbrot")
    {
        FractalParameters.CreateVectorParameter("Power", new Vector2(2, 0));
    }
    public Mandelbrot(Mandelbrot seed) : base(seed)
    {
    }

    public override IEnumerable<Complex> GetOrbitIterator(Complex seed)
    {
        for (Complex z = (Complex)ZStart; z.squaredNorm() < EscapeRadius; z = (z ^ Power) + seed) yield return z;
    }

    public override void UpdateShader(Material mat)
    {
        base.UpdateShader(mat);
        mat.SetVector("_Power", FractalParameters.FindVectorParameter("Power").GetValue());
    }
}
