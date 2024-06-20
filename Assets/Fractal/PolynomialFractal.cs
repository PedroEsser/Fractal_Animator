using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]

public class PolynomialFractal : Fractal
{

    public static readonly int MAX_ROOTS = 16;
    public List<Complex> Roots;

    public PolynomialFractal() : base("PolynomialFractal")
    {
        Roots = new List<Complex>();
        Roots.Add(Complex.ZERO);
        Roots.Add(Complex.ZERO);
        //Roots.Add(Complex.ZERO);
        FractalParameters.CreateVectorParameter("Root#0", Vector2.zero);
        FractalParameters.CreateVectorParameter("Root#1", Vector2.zero);
        //FractalParameters.CreateVectorParameter("Root#2", Vector2.zero);
    }


    public PolynomialFractal(PolynomialFractal seed) : base(seed)
    {
        Roots = seed.Roots;
    }

    public override IEnumerable<Complex> GetOrbitIterator(Complex seed)
    {
        return null;
    }

    public static Vector4[] PolynomialToVectorArray(ClassicPolynomial p)
    {
        Vector4[] array = new Vector4[MAX_ROOTS];
        for(int i = 0; i < p.Coefficients.Length; i++)
        {
            array[i] = p.Coefficients[i];
        }
        return array;
    }

    public override void UpdateShader(Material mat)
    {
        base.UpdateShader(mat);
        Roots[0] = (Complex)(Vector2)FractalParameters.FindVectorParameter("Root#0").GetValue();
        Roots[1] = (Complex)(Vector2)FractalParameters.FindVectorParameter("Root#1").GetValue();
        //Roots[2] = (Complex)(Vector2)FractalParameters.FindVectorParameter("Root#2").GetValue();
        ClassicPolynomial polynomial = ClassicPolynomial.fromRoots(Roots);
        ClassicPolynomial derivative = polynomial.derivative();
        mat.SetInt("_CoefficientCount", polynomial.Coefficients.Length);
        mat.SetVectorArray("_Polynomial", PolynomialToVectorArray(polynomial));
        mat.SetVectorArray("_Derivative", PolynomialToVectorArray(derivative));
    }

}
