using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoParameterUI : ParameterUI<TextureData>
{

    public TextureParameterUI TextureParameter;
    public VideoPlayer Player;

    public override void SetParameter(Parameter<TextureData> parameter)
    {
        base.SetParameter(parameter);
        VideoParameter par = (VideoParameter)parameter;
        TextureParameter.SetParameter(par.TextureParameter);
    }

}
