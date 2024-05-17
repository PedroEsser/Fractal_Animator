using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static VectorParameter;

public class CubicSplineInterpolation : Interpolation<Vector2Data>
{

    public List<CubicBezier> Interpolations { get => interpolations.Cast<CubicBezier>().ToList(); }

    public override BasicInterpolation<Vector2Data> CreateInterpolation(int startT, int endT, Vector2Data startValue, Vector2Data endValue)
    {
        return CubicBezier.CreateEaseInEaseOut(startT, endT, startValue, endValue);
    }

    public override Vector4[] SplineAsVector() {
            Vector4[] vectors = new Vector4[256];
            int i = 0;
            foreach (CubicBezier cb in Interpolations)
                foreach (Vector2 vec in cb.Points)
                    vectors[i++] = vec;

            return vectors;
     } 

    
}
