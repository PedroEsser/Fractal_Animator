using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using SFB;


public class TextureLoader : MonoBehaviour
{

    public TextureSelector Selector;
    public Button Button;
    public Texture2D Texture;
    public FileLoader FileLoader;
    public Sprite Empty;
    public UnityEvent<string> OnTextureSelect;

    public void Start()
    {
        FileLoader.filters = new ExtensionFilter[] { new ExtensionFilter("", "png", "jpg", "jpeg") };
        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(() => SelectTexture());
    }
    
    public void SelectTexture()
    {
        TextureSelector selector = PopupWindowHandler.HandlePopup(Selector.gameObject, "Select Texture").GetComponent<TextureSelector>();
        selector.Show();
        selector.OnTextureSelect.AddListener(tex => {
            OnTextureSelect.Invoke(tex);
            PopupWindowHandler.ClosePopup();
        });
    }

    public void SetTexture(Texture2D tex)
    {
        Texture = tex;
        FileLoader.Icon.sprite = Empty;
        ColorBlock cb = Button.colors;
        cb.normalColor = Color.white;
        Button.colors = cb;
        Button.image.sprite = Sprite.Create(Texture, new Rect(0.0f, 0.0f, Texture.width, Texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        Controller.Singleton.PlotterMaterial.SetTexture("_Texture", Texture);
    }

}
