using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable()]
public class TextureHandler : ParameterApplier
{

    public ParameterHandler TextureParameters;

    public Vector2 TextureOffset { 
        get => TextureParameters.FindVectorParameter("Texture Offset").GetValue(); 
        set => TextureParameters.FindVectorParameter("Texture Offset").SetValue(value); 
    }
    public Vector2 TextureScale { 
        get => TextureParameters.FindVectorParameter("Texture Scale").GetValue(); 
        set => TextureParameters.FindVectorParameter("Texture Offset").SetValue(value); 
    }

    public TextureHandler()
    {
        TextureParameters = new ParameterHandler();

        TextureParameters.CreateVectorParameter("Texture Offset", Vector2.zero);
        TextureParameters.CreateVectorParameter("Texture Scale", new Vector2(1, 1));
        TextureParameters.CreateNumberParameter("Texture Angle", 0f);
        TextureParameters.CreateNumberParameter("Light Angle", 0f);
        TextureParameters.CreateNumberParameter("Light Height", 2f);
    }
    public TextureHandler(TextureHandler seed)
    {
        TextureParameters = new ParameterHandler(seed.TextureParameters);
    }

    public ParameterHandler GetParameters()
    {
        return TextureParameters;
    }

    public void UpdateShader(Material mat)
    {
        Vector2 textureOffset = TextureOffset;
        Vector2 textureScale = TextureScale;
        mat.SetVector("_TextureTransformation", new Vector4(textureOffset.x, textureOffset.y, textureScale.x, textureScale.y));
        mat.SetFloat("_LightAngle", TextureParameters.FindParameter("Light Angle").GetValue());
        mat.SetFloat("_LightHeight", TextureParameters.FindParameter("Light Height").GetValue());
        mat.SetFloat("_LightTextureAngle", TextureParameters.FindParameter("Texture Angle").GetValue());
    }
}
