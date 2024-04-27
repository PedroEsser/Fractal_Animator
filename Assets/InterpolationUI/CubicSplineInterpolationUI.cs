using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubicSplineInterpolationUI : Plot2D
{

    private CubicSplineInterpolation interpolation;
    public Vector2 start, end, end2, end3;

    new void Start()
    {
        base.Start();   
    }

    new void Update()
    {
        base.Update();
        interpolation = new CubicSplineInterpolation();
        interpolation.AddKeyFrame(0, start);
        interpolation.AddKeyFrame(10, end);
        interpolation.AddKeyFrame(20, end2);
        interpolation.AddKeyFrame(30, end3);
        Plane.material.SetVectorArray("_Points", interpolation.SplineAsVector());
        Plane.material.SetInt("_SplineCount", interpolation.interpolations.Count);
        
    }

    public void SetInterpolation(CubicSplineInterpolation interpolation)
    {
        this.interpolation = interpolation;
    }

}
