using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputNumberPlotter : ParameterPlotter<float>
{

    public InputField InputField;

    void Start()
    {
        InputField.onEndEdit.AddListener(val =>
        {
            if (float.TryParse(val, out float result))
            {
                UndoHandler.DoParameterSetAction(parameter, result);
                //parameter.SetValue(result);
            }
        });

        InputField.onValueChanged.AddListener(val =>
        {
            InputField.textComponent.color = float.TryParse(val, out _) ? Color.white : Color.red;
        });
    }

    public override void UpdatePlotter(float value)
    {
        if (InputField != null && EventSystem.current.currentSelectedGameObject != InputField.gameObject)
            InputField.text = value + "";
    }

    
}
