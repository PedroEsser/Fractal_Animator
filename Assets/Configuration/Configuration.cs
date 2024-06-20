using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[Serializable()]
public class Configuration
{

    public Timeline Timeline;
    public Settings Settings;

    public string defaultVideoPath;
    public string defaultTexturePath;
    public int defaultImageWidth, defaultImageHeight;

    public Configuration()
    {
        Settings = new Settings();
        Timeline = new Timeline();
        //Settings.GetAllParameters().BindTimeline(Timeline);
        defaultVideoPath = "";
        defaultTexturePath = "";
        defaultImageWidth = 1920;
        defaultImageHeight = 1080;
    }

    public Configuration(Configuration seed)
    {
        Settings = new Settings(seed.Settings);
        Timeline = new Timeline(seed.Timeline);
        Settings.BindTimeline(Timeline);
    }

    public void Init()
    {
        Timeline.OnTimeChange = new UnityEvent<float>();
        Settings.BindTimeline(Timeline);
    }

}
