using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static VectorParameter;

[Serializable()]
public class CubicBezier : BasicInterpolation<Vector2Data>
{
    public Vector2[] Points { get; }
    public Vector2 P0 { get => Points[0]; set { Points[0] = value; } }
    public Vector2 P1 { get => Points[1]; set { Points[1] = value; } }
    public Vector2 P2 { get => Points[2]; set { Points[2] = value; } }
    public Vector2 P3 { get => Points[3]; set { Points[3] = value; } }

    public CubicBezier(int startT, int endT, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3) : base(startT, endT, p0, p3)
    {
        Points = new Vector2[4];
        P0 = p0;
        P1 = p1;
        P2 = p2;
        P3 = p3;
    }

    public override Vector2Data ValueAt(float t)
    {
        return (1 - t) * (1 - t) * (1 - t) * P0
            + 3 * (1 - t) * (1 - t) * t * P1
            + 3 * (1 - t) * t * t * P2
            + t * t * t * P3;
    }

    public static CubicBezier CreateConstant(int startT, int endT, Vector2 value)
    {
        return CreateLinear(startT, endT, value, new Vector2(value.x, value.y));
    }

    public static CubicBezier CreateLinear(int startT, int endT, Vector2 start, Vector2 end)
    {
        return new CubicBezier(startT, endT, start, start, end, end);
    }

    public static CubicBezier CreateEaseIn(int startT, int endT, Vector2 start, Vector2 end)
    {
        return new CubicBezier(startT, endT, start, new Vector2(start.x + (end.x - start.x) * 0.4f, start.y), new Vector2(end.x, end.y), end);
    }

    public static CubicBezier CreateEaseOut(int startT, int endT, Vector2 start, Vector2 end)
    {
        return new CubicBezier(startT, endT, start, new Vector2(start.x, start.y), new Vector2(end.x - (end.x - start.x) * 0.4f, end.y), end);
    }

    public static CubicBezier CreateEaseInEaseOut(int startT, int endT, Vector2 start, Vector2 end)
    {
        return new CubicBezier(startT, endT, start, new Vector2(start.x + (end.x - start.x) * 0.4f, start.y), new Vector2(end.x - (end.x - start.x) * 0.4f, end.y), end);
    }

}
