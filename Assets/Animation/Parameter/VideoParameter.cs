using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoParameter : Parameter<TextureData>
{

    public TextureParameter TextureParameter;
    private RenderTexture renderTex;
    private VideoPlayer Player;
    private float StartT, PlaybackSpeed, EndT;
    public VideoParameter(TextureParameter textureParameter, VideoPlayer player, float startT = 0, float playbackSpeed = .5f)
        : base(textureParameter.Name, textureParameter.GetValue())
    {
        TextureParameter = textureParameter;
        Player = player;
        Player.Prepare();
        Player.sendFrameReadyEvents = true;
        renderTex = new RenderTexture(TextureHandler.TEXTURE_SIZE, TextureHandler.TEXTURE_SIZE, 3);
        Player.targetTexture = renderTex;
        Player.frameReady += UpdateTexture;
        StartT = startT;
        PlaybackSpeed = playbackSpeed;
        EndT = (float)Player.length;
    }

    public override TextureData ValueAt(float t)
    {
        /*if (!Player.isPlaying)
        {
            Player.Play();
        }*/
        Player.frame = (int)((t - StartT) * PlaybackSpeed) % (int)Player.frameCount;
        Player.Pause();
        //Debug.Log(Player.frame);
        return TextureParameter.ValueAt(t);
    }

    public void UpdateTexture(VideoPlayer source, long idx)
    {
        //Controller.Singleton.PlotterMaterial.SetTexture(TextureParameter.TextureName, Player.texture);
        ConfigurationHandler.CurrentConfig.Settings.TextureSettings.Carpet.UpdateVideoTexture(TextureParameter.TextureName, Player.texture);
    }

    public override void BindTimeline(Timeline timeline)
    {
        TextureParameter.BindTimeline(timeline);
        base.BindTimeline(timeline);
    }

    public override Parameter<TextureData> Copy()
    {
        throw new System.NotImplementedException();
    }

}
