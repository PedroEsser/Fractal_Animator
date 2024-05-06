using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable()]
public class TextureSettings : ParameterApplier
{

    public ParameterHandler CarpetParameters;
    
    public InfiniteCarpet Carpet;

    public Vector2 CarpetOffset { 
        get => CarpetParameters.FindVectorParameter("Carpet Offset").GetValue(); 
        set => CarpetParameters.FindVectorParameter("Carpet Offset").SetValue(value); 
    }
    public Vector2 CarpetScale { 
        get => CarpetParameters.FindVectorParameter("Carpet Scale").GetValue(); 
        set => CarpetParameters.FindVectorParameter("Carpet Scale").SetValue(value); 
    }

    public TextureSettings()
    {
        CarpetParameters = new ParameterHandler();

        CarpetParameters.CreateVectorParameter("Carpet Offset", Vector2.zero);
        CarpetParameters.CreateVectorParameter("Carpet Scale", new Vector2(1, 1));
        CarpetParameters.CreateNumberParameter("Carpet Angle", 0f);
        CarpetParameters.CreateNumberParameter("Light Angle", 0f);
        CarpetParameters.CreateNumberParameter("Light Height", 2f);

        Carpet = new InfiniteCarpet();
    }
    public TextureSettings(TextureSettings seed)
    {
        CarpetParameters = new ParameterHandler(seed.CarpetParameters);
    }

    public ParameterHandler GetParameters()
    {
        return CarpetParameters;
    }

    public void UpdateShader(Material mat)
    {
        Vector2 textureOffset = CarpetOffset;
        Vector2 textureScale = CarpetScale;
        mat.SetVector("_CarpetTransformation", new Vector4(textureOffset.x, textureOffset.y, textureScale.x, textureScale.y));
        mat.SetFloat("_LightAngle", CarpetParameters.FindParameter("Light Angle").GetValue());
        mat.SetFloat("_LightHeight", CarpetParameters.FindParameter("Light Height").GetValue());
        mat.SetFloat("_LightTextureAngle", CarpetParameters.FindParameter("Carpet Angle").GetValue());
        Carpet.UpdateShader(mat);
    }
}
