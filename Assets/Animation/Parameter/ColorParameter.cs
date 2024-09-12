using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static ColorParameter;

[Serializable()]
public class ColorParameter : Parameter<ColorData>
{

    public static readonly string R_SUFFIX = "(R)", G_SUFFIX = "(G)", B_SUFFIX = "(B)", A_SUFFIX = "(A)";
    public NumberParameter R { get; }
    public NumberParameter G { get; }
    public NumberParameter B { get; }
    public NumberParameter A { get; }

    public bool HSV;

    public ColorParameter(string name, NumberParameter r, NumberParameter g, NumberParameter b, NumberParameter a, bool hsv) : base(name)
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = a;
        this.HSV = hsv;
        base.SetValue(this.GetValue());
    }

    public ColorParameter(string name, float r, float g, float b, float a) : 
        this(name, new NumberParameter(name + R_SUFFIX, r), new NumberParameter(name + G_SUFFIX, g), 
            new NumberParameter(name + B_SUFFIX, b), new NumberParameter(name + A_SUFFIX, a), true) { }

    public ColorParameter(string name, Color32 c) : this(name, From255(c.r), From255(c.g), From255(c.b), From255(c.a)) { }

    public static byte To255(float v) { return (byte)(v * 255); }
    public static float From255(byte b) { return ((float)b / 255); }

    public override ColorData GetValue()
    {
        if (HSV)
        {
            Color rgb = Color.HSVToRGB(R.GetValue() - Mathf.Floor(R.GetValue()), G.GetValue(), B.GetValue());
            return new ColorData(rgb.r, rgb.g, rgb.b, A.GetValue());
        }
        return new ColorData(R.GetValue(), G.GetValue(), B.GetValue(), A.GetValue());
    }

    public ColorData GetHSVValue()
    {
        return new ColorData(R.GetValue(), G.GetValue(), B.GetValue(), A.GetValue());
    }

    public override void SetValue(ColorData value)
    {
        base.SetValue(value);
        if (HSV)
        {
            Color.RGBToHSV(value.ToColor(), out float h, out float s, out float v);
            R.SetValue(h);
            G.SetValue(s);
            B.SetValue(v);
            A.SetValue(value.a);
            return;
        }
        R.SetValue(value.r);
        G.SetValue(value.g);
        B.SetValue(value.b);
        A.SetValue(value.a);
    }

    public override ColorData ValueAt(float t)
    {
        if (HSV)
        {
            Color rgb = Color.HSVToRGB(R.ValueAt(t) - Mathf.Floor(R.ValueAt(t)), G.ValueAt(t), B.ValueAt(t));
            return new ColorData(To255(rgb.r), To255(rgb.g), To255(rgb.b), To255(A.ValueAt(t)));
        }
        return new ColorData(To255(R.ValueAt(t)), To255(G.ValueAt(t)), To255(B.ValueAt(t)), To255(A.ValueAt(t)));
    }

    public override void BindTimeline(Timeline timeline)
    {
        R.BindTimeline(timeline);
        G.BindTimeline(timeline);
        B.BindTimeline(timeline);
        A.BindTimeline(timeline);
    }

    public override Parameter<ColorData> Copy()
    {
        return new ColorParameter(Name, GetValue());
    }

    [Serializable()]
    public class ColorData
    {
        public float r, g, b, a;
        public ColorData(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
        public Color32 ToColor() { return new Color32(To255(r), To255(g), To255(b), To255(a)); }

        public static implicit operator ColorData(Color32 c) { return new ColorData(c.r, c.g, c.b, c.a); }
        public static implicit operator Color32(ColorData d) => new Color32(To255(d.r), To255(d.g), To255(d.b), To255(d.a));

    }

}
