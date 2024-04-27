using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable()]
public class HermiteInterpolation : BasicInterpolation<float>
{

    public float startSlope, endSlope;

    public HermiteInterpolation(int startT, int endT, float startValue = 0, float endValue = 1, float startSlope = 1, float endSlope = 1): base(startT, endT, startValue, endValue)
    {
        this.startValue = startValue;
        this.endValue = endValue;
        this.startSlope = startSlope;
        this.endSlope = endSlope;
    }

    public override float ValueAt(float t)
    {
        float transformedT = (t - startT) / (endT - startT);
        float transformedTSquared = transformedT * transformedT;
        float oneMinusTSquared = 1 - transformedT;
        oneMinusTSquared *= oneMinusTSquared;

        float value = startSlope * (transformedT * oneMinusTSquared) +
            (transformedTSquared * (3 - 2 * transformedT)) +
            endSlope * (transformedTSquared * (transformedT - 1));

        return value * (endValue - startValue) + startValue;
    }

    public static HermiteInterpolation CreateConstant(int startT, int endT, float value)
    {
        return new HermiteInterpolation(startT, endT, value, value, 0, 0);
    }

    public static HermiteInterpolation CreateLinear(int startT, int endT, float start, float end)
    {
        return new HermiteInterpolation(startT, endT, start, end, 1, 1);
    }

    public static HermiteInterpolation CreateEaseIn(int startT, int endT, float start, float end)
    {
        return new HermiteInterpolation(startT, endT, start, end, 0, 1);
    }

    public static HermiteInterpolation CreateEaseOut(int startT, int endT, float start, float end)
    {
        return new HermiteInterpolation(startT, endT, start, end, 1, 0);
    }

    public static HermiteInterpolation CreateEaseInEaseOut(int startT, int endT, float start, float end)
    {
        return new HermiteInterpolation(startT, endT, start, end, 0, 0);
    }

}
