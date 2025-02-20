using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureSettingsUI : MonoBehaviour
{

    public TextureParameterUI TextureParameterPrefab;
    public VideoParameterUI VideoParameterPrefab;
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

    public TextureParameterUI CreateTextureParameterUI(TextureParameter par)
    {
        TextureParameterUI ui = Instantiate(TextureParameterPrefab, Container.transform);
        ui.SetParameter(par);
        ui.UpdateTextureIcon();
        ui.Loader.OnMediaSelect.AddListener(path => {
            string tex = TextureHandler.HandleTextureLoad(path);
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

    public VideoParameterUI CreateVideoParameterUI(string path)
    {
        string videoName = FileLoader.GetName(path);
        TextureParameter par = Carpet.AddVideoTexture(videoName);
        VideoParameterUI ui = Instantiate(VideoParameterPrefab, Container.transform);
        ui.Player = Instantiate(ui.Player);
        ui.Player.url = path;
        VideoParameter videoPar = new VideoParameter(par, ui.Player);
        ui.SetParameter(videoPar);


        videoPar.BindTimeline(ConfigurationHandler.CurrentConfig.Timeline);
        //ui.UpdateTextureIcon();
        /*ui.TextureParameter.Loader.OnMediaSelect.AddListener(path => {
            string tex = TextureHandler.HandleTextureLoad(path);
            Carpet.HandleTextureChange(par, tex);
            ui.TextureParameter.UpdateTextureIcon();
        });*/

        ui.TextureParameter.OnDelete.AddListener(() =>
        {
            if (ui.Parameter != null)
                Carpet.HandleTextureDelete(ui.Parameter.Name);
            Destroy(ui.gameObject);
        });
        /*ui.TextureParameter.OnCopy.AddListener(() =>
        {
            TextureParameter copy = Carpet.AddTextureCopy((TextureParameter)ui.Parameter);
            AddTextureParameterUI(copy);
        });*/
        UIs.Add(ui.TextureParameter);
        return ui;
    }

    public void AddMedia(string path)
    {
        if (MediaLoader.IsTexture(path))
        {
            string textureName = TextureHandler.HandleTextureLoad(path);
            TextureParameter par = Carpet.AddTexture(textureName, textureName);
            CreateTextureParameterUI(par);
        }
        else if (MediaLoader.IsVideo(path))
        {
            CreateVideoParameterUI(path);
        }
        else
        {
            throw new System.Exception("Media Expected");
        }
    }

    public void AddTextureParameterUI(TextureParameter par = null)
    {
        TextureParameterUI ui = CreateTextureParameterUI(par);
    }


}
