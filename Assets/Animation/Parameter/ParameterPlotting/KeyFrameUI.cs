using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyFrameUI : MonoBehaviour
{

    public Button button;
    public Sprite knob;
    public Sprite rhombus;
    public NumberParameter parameter;

    public void SetParameter(NumberParameter parameter)
    {
        this.parameter = parameter;
        if (parameter.interpolation == null)
        {
            parameter.interpolation = new HermiteSplineInterpolation();
        }
            
    }

    public void ToggleKeyFrame()
    {
        int t = (int)ConfigurationHandler.CurrentConfig.Timeline.CurrentTime;
        if (IsKeyFramed())
        {
            UndoHandler.DoRemoveKeyFrameAction(parameter, t);
            //parameter.interpolation.RemoveKeyFrame();
        }
        else
        {
            UndoHandler.DoSetKeyFrameAction(parameter, t);
            //parameter.interpolation.AddKeyFrame((int)ConfigurationHandler.CURRENT_CONFIG.Timeline.CurrentTime, parameter.GetValue());
        }
    }

    void Update()
    {
        if (IsKeyFramed()) 
        {
            button.image.sprite = rhombus;
        }
        else
        {
            button.image.sprite = knob;
        }
    }

    public bool IsKeyFramed()
    {
        KeyFrame<float> k = parameter.interpolation.KeyFrameAt(Controller.CurrentTime);
        return k != null && k.value == parameter.GetValue();
    }

}
