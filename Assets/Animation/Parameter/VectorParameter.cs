using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static VectorParameter;

[Serializable()]
public class VectorParameter : Parameter<Vector2Data>
{

    public static readonly string X_SUFFIX = "(X)", Y_SUFFIX = "(Y)";
    public NumberParameter X { get; }
    public NumberParameter Y { get; }

    public VectorParameter(string name, NumberParameter x, NumberParameter y) : base(name, new Vector2Data(x.GetValue(), y.GetValue()))
    {
        this.X = x;
        this.Y = y;
    }

    public VectorParameter(string name, float x, float y) : this(name, new NumberParameter(name + X_SUFFIX, x), new NumberParameter(name + Y_SUFFIX, y)) { }
    public VectorParameter(string name, Vector2 vector) : this(name, vector.x, vector.y) { }

    public override Vector2Data GetValue()
    {
        return new Vector2Data(X.GetValue(), Y.GetValue());
    }

    public override void SetValue(Vector2Data value)
    {
        base.SetValue(value);
        X.SetValue(value.x);
        Y.SetValue(value.y);
    }

    public override Vector2Data ValueAt(float t)
    {
        return new Vector2Data(X.ValueAt(t), Y.ValueAt(t));
    }

    public override void BindTimeline(Timeline timeline)
    {
        X.BindTimeline(timeline);
        Y.BindTimeline(timeline);
    }

    public override Parameter<Vector2Data> Copy()
    {
        return new VectorParameter(Name, GetValue());
    }

    [Serializable()]
    public class Vector2Data
    {
        public float x, y;

        public Vector2Data(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2Data(Vector2 v) : this(v.x, v.y) { }

        public static implicit operator Vector2Data(Vector2 v) { return new Vector2Data(v.x, v.y); }
        public static implicit operator Vector2(Vector2Data d) => new Vector2(d.x, d.y);
        public static implicit operator Vector4(Vector2Data d) => new Vector4(d.x, d.y, 0, 0);

    }

}
