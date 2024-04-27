using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable()]
public class HermiteSplineInterpolation : Interpolation<float>
{

    public List<HermiteInterpolation> Interpolations { get => interpolations.Cast<HermiteInterpolation>().ToList(); }

    public override Vector4[] SplineAsVector()
    {
        Vector4[] vectors = new Vector4[256];
        int i = 0;
        foreach (HermiteInterpolation hi in Interpolations)
        {
            vectors[i++] = new Vector4(hi.startT, hi.endT, hi.startValue, hi.endValue);
            vectors[i++] = new Vector4(hi.startSlope, hi.endSlope);
        }

        return vectors;
     }
    

    public override BasicInterpolation<float> CreateInterpolation(int startT, int endT, float startValue, float endValue)
    {
        return HermiteInterpolation.CreateLinear(startT, endT, startValue, endValue);
        //return HermiteInterpolation.CreateEaseInEaseOut(startT, endT, startValue, endValue);
        //return HermiteInterpolation.CreateEaseIn(startT, endT, startValue, endValue);
    }
}
