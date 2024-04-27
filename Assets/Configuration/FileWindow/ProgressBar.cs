using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    public RectTransform Progress;
    public RectTransform Background;
    public Text text;
    public float progress;
    public bool paused = false;
    public Button PlayButton;
    public Sprite Play, Pause;

    public void SetProgress(float progress, string text = null)
    {
        this.progress = progress;
        Progress.sizeDelta = new Vector2(Background.sizeDelta.x * progress, Background.sizeDelta.y);
        if (text != null)
            this.text.text = text;
    }

    public void SetPaused(bool paused)
    {
        this.paused = paused;
        UpdatePlayButton();
    }

    public void TogglePlay() { SetPaused(!paused); }

    private void UpdatePlayButton() { PlayButton.image.sprite = paused ? Pause : Play; }

}
