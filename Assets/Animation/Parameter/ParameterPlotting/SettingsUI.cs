using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{

    public ParameterWindow Window, Fractal, Texture;
    public ColorParameterUI InsideColorUI, OutsideColorUI;

    public void SetSettings(Settings settings)
    {
        Window.SetParameters(settings.WindowSettings.GetParameters());
        Fractal.SetParameters(settings.Fractal.GetParameters());
        Texture.SetParameters(settings.TextureSettings.GetParameters());
        InsideColorUI.SetParameter(settings.Fractal.InsideColor);
        OutsideColorUI.SetParameter(settings.Fractal.OutsideColor);
    }

}
