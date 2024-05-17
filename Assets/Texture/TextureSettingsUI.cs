using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureSettingsUI : MonoBehaviour
{
    public TextureParameterUI TextureParameterPrefab;
    public GameObject Container;
    public InfiniteCarpet Carpet { get => ConfigurationHandler.CurrentConfig.Settings.TextureSettings.Carpet; }
    public List<TextureParameter> TextureParameters { get => Carpet.TextureParameters; }

    void Start()
    {

        foreach (TextureParameter par in TextureParameters)
            AddTextureParameterUI(par);
    }

    public void AddEmptyTextureParameterUI()
    {
        TextureParameterUI ui = Instantiate(TextureParameterPrefab, Container.transform);
        ui.Loader.OnTextureSelect.AddListener(tex => {
            TextureParameter par;
            if (ui.Parameter == null)
            {
                par = Carpet.AddTexture("Texture " + TextureParameters.Count, tex);
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
            if(ui.Parameter != null)
                Carpet.HandleTextureDelete(ui.Parameter.Name);
            Destroy(ui.gameObject);
        });
    }

    public void AddTextureParameterUI(TextureParameter par)
    {
        TextureParameterUI ui = Instantiate(TextureParameterPrefab, Container.transform);
        ui.SetParameter(par);
    }


}
