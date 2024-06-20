using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable()]
public class WindowSettings : ParameterApplier
{
    public ParameterHandler WindowParameters;

    public Vector2 Center
    {
        get => WindowParameters.FindVectorParameter("Center").GetValue();
        set => WindowParameters.FindVectorParameter("Center").SetValue(value);
    }
    public float Zoom
    {
        get => WindowParameters.FindParameter("Zoom").GetValue();
        set => WindowParameters.FindParameter("Zoom").SetValue(value);
    }
    public float Angle
    {
        get => WindowParameters.FindParameter("Angle").GetValue();
        set => WindowParameters.FindParameter("Angle").SetValue(value);
    }

    public Vector4 Window
    {
        get {
            Vector4 window = WindowParameters.FindVectorParameter("Center").GetValue();
            float xWindow = Mathf.Pow(10, -Zoom + 1);
            window.z = xWindow;
            window.w = xWindow / 16 * 9;
            return window;
        }
    }

    public WindowSettings()
    {
        WindowParameters = new ParameterHandler();
        WindowParameters.CreateVectorParameter("Center", Vector2.zero);
        WindowParameters.CreateNumberParameter("Zoom", 0f);
        WindowParameters.CreateNumberParameter("Angle", 0f);
    }
    public WindowSettings(WindowSettings seed)
    {
        WindowParameters = new ParameterHandler(seed.WindowParameters);
    }

    public ParameterHandler GetParameters()
    {
        return WindowParameters;
    }

    public void UpdateShader(Material mat)
    {
        Vector4 window = WindowParameters.FindVectorParameter("Center").GetValue();

        float xWindow = Mathf.Pow(10, -Zoom + 1);
        window.z = xWindow;
        window.w = xWindow / 16 * 9;
        mat.SetVector("_Window", window);

        mat.SetFloat("_Angle", WindowParameters.FindParameter("Angle").GetValue() * Mathf.PI * 2);
    }

    public void BindTimeline(Timeline timeline)
    {
        WindowParameters.BindTimeline(timeline);
    }
}
