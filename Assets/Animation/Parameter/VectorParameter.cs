using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorParameter : Parameter<Vector2>
{

    public static readonly string X_SUFFIX = "(X)", Y_SUFFIX = "(Y)";
    public NumberParameter X { get; }
    public NumberParameter Y { get; }

    public VectorParameter(string name, NumberParameter x, NumberParameter y) : base(name, new Vector2(x.GetValue(), y.GetValue()))
    {
        this.X = x;
        this.Y = y;
    }

    public VectorParameter(string name, float x, float y) : this(name, new NumberParameter(name + X_SUFFIX, x), new NumberParameter(name + Y_SUFFIX, y)) { }
    public VectorParameter(string name, Vector2 vector) : this(name, vector.x, vector.y) { }

    public override Vector2 GetValue()
    {
        return new Vector2(X.GetValue(), Y.GetValue());
    }

    public override void SetValue(Vector2 value)
    {
        base.SetValue(value);
        X.SetValue(value.x);
        Y.SetValue(value.y);
    }

    public override Vector2 ValueAt(float t)
    {
        return new Vector2(X.interpolation.ValueAt(t), Y.interpolation.ValueAt(t));
    }
}
