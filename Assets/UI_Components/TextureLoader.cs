using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SFB;

public class TextureLoader : MonoBehaviour
{

    public Button Button;
    public Texture2D Texture;
    public FileLoader FileLoader;
    public Sprite Empty;


    public void Start()
    {
        FileLoader.filters = new ExtensionFilter[] { new ExtensionFilter("", "png", "jpg", "jpeg") };
        FileLoader.OnFileSelect.AddListener(images => LoadTexture(images[0]));
    }

    public void LoadTexture(string path)
    {
        string name = FileLoader.GetFileName(path);
        try
        {
            File.Copy(path, ConfigurationHandler.GetContentFolderPath(ConfigurationHandler.CurrentConfigName) + name);
        }
        catch { }
        ConfigurationHandler.CurrentConfig.defaultTexturePath = path.Substring(path.LastIndexOf("\\"));
        Texture = GetImageFromPath(path);
        FileLoader.Icon.sprite = Empty;
        ColorBlock cb = Button.colors;
        cb.normalColor = Color.white;
        Button.colors = cb;
        Button.image.sprite = Sprite.Create(Texture, new Rect(0.0f, 0.0f, Texture.width, Texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        Controller.Singleton.PlotterMaterial.SetTexture("_Texture", Texture);
    }

    public static Texture2D GetImageFromPath(string path)
    {
        byte[] rawData = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(rawData);
        return tex;
    }

}
