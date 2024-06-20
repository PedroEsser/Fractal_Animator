using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OptionUI;
using SFB;

public class FileWindow : OptionsWindow
{

    private List<Option> options;
    public GameObject CreateImageWindow;
    public GameObject CreateVideoWindow;

    private void AddOptions()
    {
        options = new List<Option>();
        options.Add(new Option("Save", () => ConfigurationHandler.SaveConfig()));
        options.Add(new Option("Save as", () => SaveAs()));
        options.Add(new Option("New", () => New()));
        options.Add(new Option("Load", () => Load()));
        options.Add(new Option("Create Image", () => PopupWindowHandler.HandlePopup(CreateImageWindow, "Create Image", new Vector2(1200, 900))));
        options.Add(new Option("Create Video", () => PopupWindowHandler.HandlePopup(CreateVideoWindow, "Create Video", new Vector2(1200, 900))));
    }

    private void SaveAs()
    {
        //string[] folder = StandaloneFileBrowser.OpenFolderPanel("Save As", "Assets/Configuration/configs", false);
        string file = StandaloneFileBrowser.SaveFilePanel("Save Animation", "Assets/Configuration/configs", "config", "bin");
        if (file.Length == 0)
            return;
        ConfigurationHandler.SaveConfig(FileLoader.GetName(file));
    }

    private void New()
    {
        string path = StandaloneFileBrowser.SaveFilePanel("New Animation", ConfigurationHandler.CONFIG_FOLDER_PATH, "newConfig", "bin");
        if (path.Length == 0)
            return;

        ConfigurationHandler.SetNewConfig(ConfigurationHandler.GetConfigName(path));
    }
    
    private void Load()
    {
        string[] result = StandaloneFileBrowser.OpenFilePanel("Load Animation", ConfigurationHandler.CONFIG_FOLDER_PATH, "bin", false);
        if (result.Length == 0)
            return;

        ConfigurationHandler.SetNewConfig(ConfigurationHandler.GetConfigName(result[0]));
    }

    public void Appear()
    {
        if (options == null)
            AddOptions();
        base.Appear(options);
    }

}
