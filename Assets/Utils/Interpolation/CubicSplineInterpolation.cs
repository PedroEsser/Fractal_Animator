using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CubicSplineInterpolation : Interpolation<Vector2>
{

    public List<CubicBezier> Interpolations { get => interpolations.Cast<CubicBezier>().ToList(); }

    public override BasicInterpolation<Vector2> CreateInterpolation(int startT, int endT, Vector2 startValue, Vector2 endValue)
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
