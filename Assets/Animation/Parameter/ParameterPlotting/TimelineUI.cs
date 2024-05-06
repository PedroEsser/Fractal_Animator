using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TimelineUI : MonoBehaviour
{

    public NumberAxisPlotter timePlotter;
    public Timeline timeline;
    public InputField timeInput, fps, end;
    public Button PlayButton, PlayBackwardsButton;
    public bool play = false, backwards = false;
    public Sprite Play, PlayBackwards, Pause;

    bool Playing { get => play && !backwards; }
    bool PlayingBackwards { get => play && backwards; }

    private void Start()
    {
        timeline = ConfigurationHandler.CurrentConfig.Timeline;
        timePlotter.SetParameter(new NumberParameter("t", timeline.CurrentTime));
        timePlotter.axis.SetOffset(95);
        timePlotter.axis.SetWindow(200);
        
        timePlotter.button.onEndDrag.AddListener(ev => {
            int t = timeline.SetCurrentFrame(timePlotter.parameter.GetValue());
            timePlotter.parameter.SetValue(t);
            Controller.CURRENT_AUDIO.time = t/60f;
        });
        timeInput.onEndEdit.AddListener(str =>
        {
            int t = timeline.SetCurrentFrame(float.Parse(str));
            timePlotter.parameter.SetValue(t);
        });
        
        fps.text = timeline.fps + "";
        fps.onEndEdit.AddListener(str => timeline.fps = int.Parse(str));

        end.text = timeline.duration + "";
        end.onEndEdit.AddListener(str => timeline.duration = int.Parse(str));
    }

    public void Update()
    {
        timePlotter.axis.interval = new Vector2(0, timeline.duration);
        if (play)
        {
            timeline.CurrentTime += (backwards ? -1 : 1) * Time.deltaTime * timeline.fps;
            timeline.CurrentTime = (timeline.CurrentTime + timeline.duration) % timeline.duration;
            timePlotter.parameter.SetValue(timeline.CurrentTime);
        }

        if (timePlotter.button.BeingDragged)
            timeline.CurrentTime = timePlotter.parameter.GetValue();

        if (EventSystem.current.currentSelectedGameObject != timeInput.gameObject)
            timeInput.text = timePlotter.parameter.GetValue().ToString("0.###");
    }

    public void SetDuration(int duration)
    {
        timePlotter.axis.interval = new Vector2(0, duration);
        timeline.duration = duration;
    }

    public void TogglePlay()
    {
        if (!PlayingBackwards)
            play = !play;
        backwards = false;
        UpdateIcons();
    }

    public void TogglePlayBackwards()
    {
        if (!Playing)
            play = !play;
        backwards = true;
        UpdateIcons();
    }

    public void GoToStart()
    {
        play = false;
        timeline.CurrentTime = 0;
        UpdateIcons();
    }
    public void GoToEnd()
    {
        play = false;
        timeline.CurrentTime = timeline.duration;
        UpdateIcons();
    }

    private void UpdateIcons()
    {
        PlayButton.image.sprite = Playing ? Pause : Play;
        PlayBackwardsButton.image.sprite = PlayingBackwards ? Pause : PlayBackwards;
        timePlotter.parameter.SetValue(timeline.SetCurrentFrame(timeline.CurrentTime));
    }

    public void EndDrag(BaseEventData evt)
    {
        PointerEventData p = (PointerEventData)evt;
        timeline.duration += (int)(p.delta.x/2);
        timeline.duration = timeline.duration < 0 ? 0 : timeline.duration;
        end.text = timeline.duration + "";
    }

}
