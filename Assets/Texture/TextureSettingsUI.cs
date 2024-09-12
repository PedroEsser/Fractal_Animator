using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureSettingsUI : MonoBehaviour
{
    public TextureParameterUI TextureParameterPrefab;
    public GameObject Container;
    public InfiniteCarpet Carpet;
    private List<TextureParameterUI> UIs;

    void Start()
    {
        UIs = new List<TextureParameterUI>();
        OnConfigLoad(ConfigurationHandler.CurrentConfig);
        ConfigurationHandler.OnLoad.AddListener(OnConfigLoad);
    }

    public void OnConfigLoad(Configuration config)
    {
        foreach (TextureParameterUI ui in UIs)
            Destroy(ui.gameObject);

        UIs.Clear();

        Carpet = config.Settings.TextureSettings.Carpet;
        foreach (TextureParameter par in Carpet.TextureParameters)
            AddTextureParameterUI(par);
    }

    public TextureParameterUI CreateTextureParameterUI()
    {
        TextureParameterUI ui = Instantiate(TextureParameterPrefab, Container.transform);
        ui.Loader.OnTextureSelect.AddListener(tex => {
            TextureParameter par;
            if (ui.Parameter == null)
            {
                par = Carpet.AddTexture("Texture " + Carpet.TextureParameters.Count, tex);
                ui.SetParameter(par);
            }
            else
            {
                par = (TextureParameter)ui.Parameter;
            }
            Carpet.HandleTextureChange(par, tex);
            ui.UpdateTextureIcon();
        });

        ui.OnDelete.AddListener(() =>
        {
            if (ui.Parameter != null)
                Carpet.HandleTextureDelete(ui.Parameter.Name);
            Destroy(ui.gameObject);
        });
        ui.OnCopy.AddListener(() =>
        {
            TextureParameter copy = Carpet.AddTextureCopy((TextureParameter)ui.Parameter);
            AddTextureParameterUI(copy);
        });
        UIs.Add(ui);
        return ui;
    }

    public void AddEmptyTextureParameterUI()
    {
        CreateTextureParameterUI().Loader.SelectTexture();
    }

    public void AddTextureParameterUI(TextureParameter par = null)
    {
        TextureParameterUI ui = CreateTextureParameterUI();
        ui.SetParameter(par);
    }


}
