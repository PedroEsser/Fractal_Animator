using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermiteSplineInterpolationUI : MonoBehaviour
{

    public Plot2D plot;
    private HermiteSplineInterpolation interpolation;
    public float start, end, end2, end3;

    void Update()
    {
        
    }

    public void SetInterpolation(HermiteSplineInterpolation interpolation)
    {
        this.interpolation = interpolation;
        plot.Plane.material.SetVectorArray("_Points", interpolation.SplineAsVector());
        plot.Plane.material.SetInt("_SplineCount", interpolation.interpolations.Count);
    }

}
