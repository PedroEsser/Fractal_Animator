using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using SFB;

public class FileLoader : MonoBehaviour
{

    public string title = "Load File", directory = "";
    public ExtensionFilter[] filters;
    public bool folderLoader = false, multiselect = false;
    public Text Name;
    public Image Icon;
    public UnityEvent<string[]> OnFileSelect;

    public virtual void Start()
    {
        OnFileSelect.AddListener(files => Name.text = GetName(files[0]));
    }

    public void LoadFile()
    {
        string[] files;
        if (folderLoader)
        {
            files = StandaloneFileBrowser.OpenFolderPanel(title, directory, multiselect);
        }
        else
        {
            files = StandaloneFileBrowser.OpenFilePanel(title, directory, filters, multiselect);
        }
        if (files.Length == 0)
            return;
        OnFileSelect.Invoke(files);
    }

    public static string GetFileName(string file) { return file.Substring(file.LastIndexOf("\\") + 1, file.Length - file.LastIndexOf("\\") - 1); }
    public static string GetName(string file) { return file.Substring(file.LastIndexOf("\\") + 1, file.LastIndexOf(".") - file.LastIndexOf("\\") - 1); }

}
