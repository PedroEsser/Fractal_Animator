using System.IO;
using System.Collections.Generic;
using UnityEngine;
using SFB;

public class TextureHandler : MonoBehaviour
{

    public static readonly int TEXTURE_SIZE = 2048;
    public static readonly string DEFAULT_TEXTURES_PATH = "Assets\\Texture\\DefaultTextures\\";
    public static TextureHandler HANDLER { get; private set; }
    public Dictionary<string, Texture2D> DefaultTextures;
    public Dictionary<string, Texture2D> UserTextures;

    void Start()
    {
        HANDLER = this;
        LoadDefaultTextures();
        LoadUserTextures();
        ConfigurationHandler.CurrentConfig.Settings.TextureSettings.Carpet.UpdateTextures();
    }

    public static string HandleTextureLoad(string path)
    {
        string name = GetNextAvailableName(FileLoader.GetFileName(path));
        Texture2D tex = GetImageFromPath(path);
        try
        {
            File.Copy(path, ConfigurationHandler.CurrentContentFolder + name);
            HANDLER.UserTextures.Add(name, tex);
        }
        catch { print("File copy failed"); }
        ConfigurationHandler.CurrentConfig.defaultTexturePath = path.Substring(path.LastIndexOf("\\"));
        return name;
    }

    private static string GetNextAvailableName(string name)
    {
        if (!File.Exists(ConfigurationHandler.CurrentContentFolder + name))
            return name;
        int index = 0;
        string extension = name.Substring(name.LastIndexOf("."));
        name = name.Substring(0, name.LastIndexOf(".") - 1);
        while (File.Exists(ConfigurationHandler.CurrentContentFolder + name + "(" + index + ")" + extension)) index++;
        return name + "(" + index + ")" + extension;
    }
    
    public static void LoadDefaultTextures()
    {
        HANDLER.DefaultTextures = new Dictionary<string, Texture2D>();
        string[] texturePaths = Directory.GetFiles(DEFAULT_TEXTURES_PATH);
        foreach (string path in texturePaths)
        {
            if (!IsImage(path))
                continue;
            Texture2D tex = GetImageFromPath(path);
            HANDLER.DefaultTextures.Add(FileLoader.GetName(path), tex);
        }
    }
    public static void LoadUserTextures()
    {
        HANDLER.UserTextures = new Dictionary<string, Texture2D>();
        string[] texturePaths = Directory.GetFiles(ConfigurationHandler.CurrentContentFolder);
        foreach(string path in texturePaths)
        {
            if (!IsImage(path))
                continue;
            Texture2D tex = GetImageFromPath(path);
            HANDLER.DefaultTextures.Add(FileLoader.GetFileName(path), tex);
        }
    }

    public static Texture2D GetTexture(string name)
    {
        if (HANDLER.UserTextures.ContainsKey(name))
            return HANDLER.UserTextures[name];
        return HANDLER.DefaultTextures[name];
    }

    public static Texture2D GetImageFromPath(string path)
    {
        byte[] rawData = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(rawData);
        //tex.Apply();

        /*Texture2D newTex = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);
        newTex.SetPixels32(tex.GetPixels32());
        newTex.Apply();*/
        return Resize(tex, TEXTURE_SIZE, TEXTURE_SIZE);
    }

    public static Texture2D Resize(Texture2D texture2D, int width, int height)
    {
        RenderTexture rt = new RenderTexture(width, height, 32);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D, rt);
        Texture2D result = new Texture2D(width, height);
        result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        result.Apply();
        RenderTexture.active = null;
        return result;
    }

    public static bool IsImage(string path) { return path.EndsWith(".png") || path.EndsWith(".jpg") || path.EndsWith(".jpeg"); }

}
