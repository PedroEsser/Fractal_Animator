using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable()]
public class InfiniteCarpet : ParameterApplier
{

    public float Length;
    //public ColorParameter BackgroundColor;
    public List<TextureParameter> TextureParameters;
    public List<string> TexturesMapping;

    [NonSerialized()] 
    public Texture2DArray LoadedTextures;

    public InfiniteCarpet()
    {
        Length = 1;
        TextureParameters = new List<TextureParameter>();
        TexturesMapping = new List<string>();
    }

    public InfiniteCarpet(InfiniteCarpet seed)
    {
        Length = seed.Length;
        TextureParameters = seed.TextureParameters;
        TexturesMapping = seed.TexturesMapping;
        UpdateTexturesMapping();
    }

    public void BindTimeline(Timeline timeline)
    {
        foreach (TextureParameter par in TextureParameters)
            par.BindTimeline(timeline);
    }
    public ParameterHandler GetParameters()
    {
        ParameterHandler parameters = new ParameterHandler();
        foreach (TextureParameter par in TextureParameters)
            parameters.AddTextureParameter(par);
        return parameters;
    }

    public void UpdateShader(Material mat)
    {
        float[] textureIndices = new float[500];
        Vector4[] TextureTransformations = new Vector4[1000];
        Color[] TextureColors = new Color[500];
        for(int i = 0; i < TextureParameters.Count; i++)
        {
            TextureParameter tex = TextureParameters[i];
            textureIndices[i] = IndexForTexture(tex.TextureName);
            TextureColors[i] = tex.Color.GetValue().ToColor();
            TextureTransformations[i * 2 + 0] = new Vector4(tex.Position.X.GetValue(), tex.Position.Y.GetValue(), tex.Size.X.GetValue()/2, tex.Size.Y.GetValue()/2);
            TextureTransformations[i * 2 + 1] = new Vector4(tex.Offset.X.GetValue(), tex.Offset.Y.GetValue(), tex.Scale.X.GetValue()/2, tex.Scale.Y.GetValue()/2);
        }
        mat.SetFloat("_Length", Length);
        mat.SetFloatArray("_TextureIndices", textureIndices);
        mat.SetInt("_TextureCount", TextureParameters.Count);
        mat.SetVectorArray("_TextureTransformations", TextureTransformations);
        mat.SetColorArray("_TextureColors", TextureColors);
        if(LoadedTextures != null)
            mat.SetTexture("_Textures", LoadedTextures);
    }

    private void UpdateTexturesMapping()
    {
        List<string> newTexturesMapping = new List<string>();
        foreach(TextureParameter par in TextureParameters)
        {
            if (!newTexturesMapping.Contains(par.TextureName))
                newTexturesMapping.Add(par.TextureName);
        }
        TexturesMapping = newTexturesMapping;
        UpdateTextures();
    }

    public void UpdateTextures()
    {
        if (TextureParameters.Count == 0)
            return;
        LoadedTextures = new Texture2DArray(TextureHandler.TEXTURE_SIZE, TextureHandler.TEXTURE_SIZE, TexturesMapping.Count, TextureFormat.RGBA32, false);
        for(int i = 0; i < TexturesMapping.Count; i++)
        {
            Texture2D tex = TextureHandler.GetTexture(TexturesMapping[i]);
            LoadedTextures.SetPixels32(tex.GetPixels32(), i);
        }
        LoadedTextures.Apply();
    }

    public TextureParameter AddTexture(string name, string textureName = null)
    {
        TextureParameter tex = new TextureParameter(name, textureName);
        TextureParameters.Add(tex);
        if (textureName != null && !TexturesMapping.Contains(textureName))
        {
            TexturesMapping.Add(textureName);
            UpdateTextures();
        }
        tex.BindTimeline(ConfigurationHandler.CurrentConfig.Timeline);
        return tex;
    }

    public void HandleTextureChange(TextureParameter par, string newTextureName)
    {
        par.TextureName = newTextureName;
        UpdateTexturesMapping();
    }

    public void HandleTextureDelete(string name)
    {
        foreach(TextureParameter par in TextureParameters)
        {
            if(par.Name == name)
            {
                TextureParameters.Remove(par);
                break;
            }
        }
        UpdateTexturesMapping();
    }

    private int IndexForTexture(string name) { return TexturesMapping.IndexOf(name); }

}
