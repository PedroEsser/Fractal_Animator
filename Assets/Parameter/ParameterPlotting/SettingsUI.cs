using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{

    public ParameterWindow Window, Fractal, Texture; 

    public void SetSettings(Settings settings)
    {
        Window.SetParameters(settings.WindowHandler.GetParameters());
        Fractal.SetParameters(settings.Fractal.GetParameters());
        Texture.SetParameters(settings.TextureHandler.GetParameters());
    }

}
