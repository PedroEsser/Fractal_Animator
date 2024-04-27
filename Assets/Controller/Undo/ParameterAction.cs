using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParameterAction<T> : Action
{

    public Parameter<T> parameter;

    public ParameterAction(Parameter<T> parameter)
    {
        this.parameter = parameter;
    }

    public abstract void Do();
    public abstract void Undo();


    public void ReDo()
    {
        Do();
    }

    public class ParameterSet : ParameterAction<T>
    {

        public T originalValue, setValue;

        public ParameterSet(Parameter<T> parameter, T setValue, T originalValue) : base(parameter)
        {
            this.originalValue = originalValue;
            this.setValue = setValue;
        }

        public override void Do()
        {
            parameter.SetValue(setValue);
        }

        public override void Undo()
        {
            parameter.SetValue(originalValue);
        }
    }

    public class ParameterKeyFrameSet : ParameterAction<T>
    {

        private bool keyFrame;
        private int t;
        private T value;
        
        public ParameterKeyFrameSet(Parameter<T> parameter, bool keyFrame, int t, T value) : base(parameter) 
        {
            this.keyFrame = keyFrame;
            this.t = t;
            this.value = value;
        }

        public override void Do()
        {
            if (keyFrame)
                parameter.interpolation.AddKeyFrame(t, value);
            else
                parameter.interpolation.RemoveKeyFrame(t);
        }

        public override void Undo()
        {
            if (!keyFrame)
                parameter.interpolation.AddKeyFrame(t, value);
            else
                parameter.interpolation.RemoveKeyFrame(t);
        }

    }


}
