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
    [NonSerialized()] public Texture2DArray LoadedTextures;

    public InfiniteCarpet()
    {
        Length = 10;
        TextureParameters = new List<TextureParameter>();
        TexturesMapping = new List<string>();
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
        for(int i = 0; i < TextureParameters.Count; i++)
        {
            TextureParameter tex = TextureParameters[i];
            textureIndices[i] = IndexForTexture(tex.TextureName);
            TextureTransformations[i * 2 + 0] = new Vector4(tex.Position.X.GetValue(), tex.Position.Y.GetValue(), tex.Size.X.GetValue(), tex.Size.Y.GetValue());
            TextureTransformations[i * 2 + 1] = new Vector4(tex.Offset.X.GetValue(), tex.Offset.Y.GetValue(), tex.Scale.X.GetValue(), tex.Scale.Y.GetValue());
        }
        mat.SetFloat("_Length", Length);
        mat.SetFloatArray("_TexturesIndices", textureIndices);
        mat.SetInt("_TexturesCount", TextureParameters.Count);
        mat.SetVectorArray("_TexturesTransformations", TextureTransformations);
        mat.SetTexture("_Textures", LoadedTextures);
    }

    public void LoadTextures()
    {
        LoadedTextures = new Texture2DArray(TextureHandler.TEXTURE_SIZE, TextureHandler.TEXTURE_SIZE, TexturesMapping.Count, UnityEngine.Experimental.Rendering.DefaultFormat.HDR, UnityEngine.Experimental.Rendering.TextureCreationFlags.None);
        for(int i = 0; i < TexturesMapping.Count; i++)
        {
            LoadedTextures.SetPixels32(TextureHandler.GetTexture(TexturesMapping[i]).GetPixels32(), i);
        }
        LoadedTextures.Apply();
    }

    public TextureParameter AddTexture(string name, string textureName)
    {
        TextureParameter tex = new TextureParameter(name, textureName);
        TextureParameters.Add(tex);
        if (!TexturesMapping.Contains(textureName))
        {
            TexturesMapping.Add(textureName);
            LoadTextures();
        }

        return tex;
    }

    private int IndexForTexture(string name) { return TexturesMapping.IndexOf(name); }

}
