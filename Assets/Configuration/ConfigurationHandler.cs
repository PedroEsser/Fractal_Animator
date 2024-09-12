using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;


public static class ConfigurationHandler
{

    public static readonly string CONFIG_TXT_PATH = "Assets\\Configuration\\config.txt";
    public static readonly string CONFIG_FOLDER_PATH = "Assets\\Configuration\\configs\\";
    public static readonly string DEFAULT_CONFIG_NAME = "default_config";

    
    public static Configuration CurrentConfig;
    public static string CurrentConfigName;

    public static string CurrentConfigPath { get => GetConfigPath(CurrentConfigName); }
    public static string CurrentContentFolder { get => GetContentFolderPath(CurrentConfigName); }

    public static string GetConfigPath(string name) { return CONFIG_FOLDER_PATH + name + ".bin"; }
    public static string GetConfigName(string path) { return path.Substring(path.LastIndexOf("\\"), path.Length - path.LastIndexOf("\\") - 4); }

    public static string GetContentFolderPath(string name) { return CONFIG_FOLDER_PATH + name + "_files\\"; }

    public static UnityEvent<Configuration> OnLoad = new UnityEvent<Configuration>();

    public static void InitializeConfig()
    {
        try
        {
            SetConfig(GetLastConfigName());
        }
        catch (Exception)
        {
            SetNewConfig(DEFAULT_CONFIG_NAME);
        }
    }

    public static Configuration LoadConfig(string name)
    {
        Stream stream = File.Open(GetConfigPath(name), FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();
        Configuration config = (Configuration)formatter.Deserialize(stream);
        stream.Close();
        return config;
    }

    public static void SaveConfig(string name = null)
    {
        if(name == null)
            name = GetLastConfigName();
        Stream stream = File.Open(GetConfigPath(name), FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, CurrentConfig);
        stream.Close();
    }

    public static void SetConfig(string name)
    {
        CurrentConfig = LoadConfig(name);
        CurrentConfig.Init();
        SetLastConfigName(name);
        OnLoad.Invoke(CurrentConfig);
    }

    public static void SetNewConfig(string name)
    {
        CurrentConfig = new Configuration();

        Directory.CreateDirectory(GetContentFolderPath(name));
        SaveConfig(name);
        SetConfig(name);
    }

    public static void SetLastConfigName(string name)
    {
        Stream stream = File.Open(CONFIG_TXT_PATH, FileMode.Create);
        StreamWriter writer = new StreamWriter(stream);
        writer.WriteLine(name);
        writer.Close();
        CurrentConfigName = name;
    }
    public static string GetLastConfigName()
    {
        if (!File.Exists(CONFIG_TXT_PATH))
        {
            SetLastConfigName(DEFAULT_CONFIG_NAME);
            return DEFAULT_CONFIG_NAME;
        }
        Stream stream = File.Open(CONFIG_TXT_PATH, FileMode.Open);
        StreamReader reader = new StreamReader(stream);
        string last = reader.ReadLine();
        reader.Close();
        return last;
    }

}
