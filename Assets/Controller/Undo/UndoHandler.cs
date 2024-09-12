using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoHandler : MonoBehaviour
{

    public static List<Action> UndoStack;
    public static int CurrentAction = 0;

    public void Start()
    {
        UndoStack = new List<Action>();
    }

    public void Update()
    {
        /**/
        if (Input.GetKeyDown(KeyCode.T))
            Test();
        if (Input.GetKeyDown(KeyCode.Z))
            Undo();
        if (Input.GetKeyDown(KeyCode.Y))
            ReDo();
        Event ev = Event.current;
        if (ev != null && ev.type == EventType.KeyDown && ev.modifiers == EventModifiers.Control)
        {
            if (ev.keyCode == KeyCode.Z)
                Undo();
            if (ev.keyCode == KeyCode.Y)
                ReDo();
        }
    }

    public void Test()
    {
        ParameterHandlerV2 p = new ParameterHandlerV2();
        p.CreateVectorParameter("test", Vector2.zero);
        p.BindTimeline(ConfigurationHandler.CurrentConfig.Timeline);

        print(p.GetParameter<VectorParameter>("test"));
    }
    public static void DoAction(Action action)
    {
        UndoStack.RemoveRange(CurrentAction, UndoStack.Count - CurrentAction);
        UndoStack.Add(action);
        action.Do();
        CurrentAction = UndoStack.Count;
    }

    public static void ReDo()
    {
        if(CurrentAction < UndoStack.Count)
            UndoStack[CurrentAction++].ReDo();
    }

    public static void Undo()
    {
        if (CurrentAction > 0)
            UndoStack[--CurrentAction].Undo();
    }

    public static Action GetLastAction() { return CurrentAction == 0 ? null : UndoStack[CurrentAction-1]; }

    public static void DoParameterSetAction<T>(Parameter<T> parameter, T setValue, T originalValue)
    {
        DoAction(new ParameterAction<T>.ParameterSet(parameter, setValue, originalValue));
    }
    public static void DoParameterSetAction<T>(Parameter<T> parameter, T setValue)
    {
        DoAction(new ParameterAction<T>.ParameterSet(parameter, setValue, parameter.GetValue()));
    }

    public static void DoSetKeyFrameAction<T>(Parameter<T> parameter, int t)
    {
        DoAction(new ParameterAction<T>.ParameterKeyFrameSet(parameter, true, t, parameter.GetValue()));
    }
    public static void DoRemoveKeyFrameAction<T>(Parameter<T> parameter, int t)
    {
        DoAction(new ParameterAction<T>.ParameterKeyFrameSet(parameter, false, t, parameter.GetValue()));
    }

}
