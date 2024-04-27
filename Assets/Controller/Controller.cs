using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{

    //public static FractalAnimation CURRENT_ANIMATION;
    public static Controller Singleton;
    public static AudioSource CURRENT_AUDIO;
    public static AudioSpectrum CURRENT_AUDIO_SPECTRUM;
    public static bool isPlaying;
    public Image Plotter;
    public AudioSource AudioSource;
    public AudioSpectrum Spectrum;
    public SettingsUI SettingsUI;
    public Material PlotterMaterial { get => Plotter.material; }

    public static int CurrentTime { get => (int)ConfigurationHandler.CurrentConfig.Timeline.CurrentTime; }


    void Awake()
    {
        Singleton = this;
        ConfigurationHandler.InitializeConfig();
        CURRENT_AUDIO = AudioSource;
        CURRENT_AUDIO_SPECTRUM = Spectrum;
    }

    void Update()
    {
        ConfigurationHandler.CurrentConfig.Settings.UpdateShader(PlotterMaterial);
        /*if (isPlaying)
        {
            float value = 0;
            for(int i = 0 ; i < CURRENT_AUDIO_SPECTRUM.MeanLevels.Length; i++)
            {
                value += CURRENT_AUDIO_SPECTRUM.MeanLevels[i];
            }
            value /= CURRENT_AUDIO_SPECTRUM.MeanLevels.Length;
            CURRENT_ANIMATION.Settings.GetParameter("Iterations").SetValue(30 - value*500);
        }
        /*currentUpdateTime += Time.deltaTime;
        if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
            audioSource.clip.GetData(clipSampleData, audioSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
            clipLoudness = 0f;
            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
            }
            clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for
        }*/
    }

    public static void SetConfig(Configuration config)
    {
        Singleton.Plotter.material.shader = Resources.Load<Shader>(config.Settings.Fractal.ShaderPath);
        Singleton.SettingsUI.SetSettings(config.Settings);
        string[] files = Directory.GetFiles(ConfigurationHandler.CurrentContentFolder);
        if (files.Length == 0)
            return;
        Singleton.Plotter.material.SetTexture("_Texture", TextureLoader.GetImageFromPath(files[0]));
    }

    public static void TogglePlay(bool play)
    {
        isPlaying = play;
        if (!play)
        {
            CURRENT_AUDIO.Stop();
            return;
        }
        float time = ConfigurationHandler.CurrentConfig.Timeline.CurrentTimeSeconds;
        CURRENT_AUDIO.time = time;
        CURRENT_AUDIO.Play();
    }

    public void Exit()
    {
        ConfigurationHandler.SaveConfig();
        Application.Quit();
    }


}
