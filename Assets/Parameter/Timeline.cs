using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[Serializable()]
public class Timeline
{
    public int duration;
    public int fps;

    private float currentTime;
    public float CurrentTime
    {
        get { return currentTime; }
        set
        {
            this.currentTime = value;
            OnTimeChange.Invoke(value);
        }
    }

    public float CurrentTimeSeconds { get => CurrentTime / fps; }

    [NonSerialized()]
    public UnityEvent<float> OnTimeChange = new UnityEvent<float>();

    public Timeline(int duration = 1000)
    {
        this.currentTime = 0;
        this.fps = 60;
        this.duration = duration;
    }
    public Timeline(Timeline seed)
    {
        this.currentTime = seed.currentTime;
        this.fps = seed.fps;
        this.duration = seed.duration;
    }

    public int GetCurrentFrame() { return Mathf.FloorToInt(currentTime); }
    public int SetCurrentFrame(float frame) 
    { 
        currentTime = Mathf.Max(0, Mathf.Min(duration, Mathf.RoundToInt(frame)));
        return GetCurrentFrame();
    }



}
