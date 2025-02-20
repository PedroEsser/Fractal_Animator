using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using SFB;
using System.Collections.Generic;

public class MediaLoader : MonoBehaviour
{

    public TextureSelector Selector;
    public Button Button;
    public Texture2D Texture;
    public FileLoader FileLoader;
    public Sprite Empty;
    public bool texture, video;
    public UnityEvent<string> OnMediaSelect;
    public static List<string> VIDEO_EXTENSIONS = new List<string> { "mp4", "webm", "mov" };
    public static List<string> IMAGE_EXTENSIONS = new List<string> { "png", "jpg", "jpeg" };

    public void Start()
    {

        List<string> filters = new List<string>();
        if (video)
        {
            filters.AddRange(VIDEO_EXTENSIONS);
        }
        if (texture)
        {
            filters.AddRange(IMAGE_EXTENSIONS);
        }
        /*else
        {
            FileLoader.filters = new ExtensionFilter[] { new ExtensionFilter("", "png", "jpg", "jpeg") };
            //Button.onClick.RemoveAllListeners();
            //Button.onClick.AddListener(() => SelectTexture());
        }*/
        FileLoader.filters = new ExtensionFilter[] { new ExtensionFilter("", filters.ToArray()) };

        FileLoader.OnFileSelect.AddListener(media =>
        {
            if (media.Length > 0)
                OnMediaSelect.Invoke(media[0]);
        });


        FileLoader.directory = ConfigurationHandler.CurrentConfig.defaultTexturePath;
    }
    
    /*public void SelectTexture()
    {
        TextureSelector selector = PopupWindowHandler.HandlePopup(Selector.gameObject, "Select Texture").GetComponent<TextureSelector>();
        selector.Show();
        selector.OnTextureSelect.AddListener(tex => {
            OnMediaSelect.Invoke(tex);
            PopupWindowHandler.ClosePopup();
        });
    }*/

    public void SetTexture(Texture2D tex)
    {
        Texture = tex;
        FileLoader.Icon.sprite = Empty;
        ColorBlock cb = Button.colors;
        cb.normalColor = Color.white;
        Button.colors = cb;
        Button.image.sprite = Sprite.Create(Texture, new Rect(0.0f, 0.0f, Texture.width, Texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    public static bool IsVideo(string path) { return VIDEO_EXTENSIONS.Contains(FileLoader.GetExtension(path)); }
    public static bool IsTexture(string path) { return IMAGE_EXTENSIONS.Contains(FileLoader.GetExtension(path)); }

}
