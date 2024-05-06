using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static OptionUI;
using SFB;


public class TextureSelector : OptionsWindow
{

    public UnityEvent<Texture2D> OnTextureSelect;
    public FileLoader FileLoader;

    private void Start()
    {
        FileLoader.filters = new ExtensionFilter[] { new ExtensionFilter("", "png", "jpg", "jpeg") };
        //FileLoader.OnFileSelect.AddListener(images => foreach (string s in images) LoadNewImage(s); );
    }

    public override void Appear(List<Option> options)
    {
        //if (gameObject.activeSelf)
        //    return;
        float height = options.Count * OptionPrefab.GetComponent<RectTransform>().rect.height;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, height + 10);
        foreach (Option o in options)
        {
            LoadOptionUI(o);
        }
    }

    public void Show()
    {
        List<Option> options = new List<Option>();
        foreach (string name in TextureHandler.HANDLER.DefaultTextures.Keys)
        {
            options.Add(new Option(name, () => OnTextureSelect.Invoke(TextureHandler.GetTexture(name))));
        }
        Appear(options);
    }

    private void LoadNewImage(string path)
    {
        string name = TextureHandler.HandleTextureLoad(path);
        Texture2D tex = TextureHandler.GetTexture(name);

    }

    private void LoadOptionUI(Option o)
    {
        OptionUI ui = Instantiate(OptionPrefab, Container.transform);
        ui.SetOption(o);
        optionUIs.Add(ui);
    }

}
