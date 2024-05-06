using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public class Settings
{

    private readonly Dictionary<string, ParameterApplier> settings;

    public WindowSettings WindowHandler { get => (WindowSettings)GetParameters("Window"); private set => AddParameters("Window", value); }
    public Fractal Fractal { get => (Fractal)GetParameters("Fractal"); private set => AddParameters("Fractal", value); }
    public TextureSettings TextureHandler { get => (TextureSettings)GetParameters("Texture"); private set => AddParameters("Texture", value); }

    public Settings()
    {
        settings = new Dictionary<string, ParameterApplier>();
        WindowHandler = new WindowSettings();
        Fractal = new Mandelbrot();
        TextureHandler = new TextureSettings();
    }
    public Settings(Settings seed)
    {
        settings = new Dictionary<string, ParameterApplier>();
        WindowHandler = new WindowSettings(seed.WindowHandler);
        Fractal = new Mandelbrot((Mandelbrot)seed.Fractal);
        TextureHandler = new TextureSettings(seed.TextureHandler);
    }

    public void AddParameters(string name, ParameterApplier parameters) { settings.Add(name, parameters); }
    public ParameterApplier GetParameters(string name) { return settings[name]; }

    public NumberParameter GetParameter(string name)
    {
        foreach(ParameterApplier pa in settings.Values)
        {
            ParameterHandler p = pa.GetParameters();
            if (p.HasParameter(name))
                return p.FindParameter(name);
        }
        return null;
    }
    public VectorParameter GetVectorParameter(string name)
    {
        foreach(ParameterApplier pa in settings.Values)
        {
            ParameterHandler p = pa.GetParameters();
            if (p.HasVectorParameter(name))
                return p.FindVectorParameter(name);
        }
        return null;
    }

    public ParameterHandler GetAllParameters()
    {
        ParameterHandler all = new ParameterHandler();

        foreach (ParameterApplier p in settings.Values)
        {
            all.AddParameters(p.GetParameters());
        }

        return all;
    }

    public void UpdateShader(Material mat)
    {
        foreach (ParameterApplier p in settings.Values)
        {
            p.UpdateShader(mat);
        }
    }

}
