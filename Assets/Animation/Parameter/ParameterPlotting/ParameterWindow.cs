using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterWindow : MonoBehaviour
{

    public NumberParameterUI NumberParameterUIPrefab;
    public VectorParameterUI VectorParameterUIPrefab;
    public ColorParameterUI ColorParameterUIPrefab;
    public TextureParameterUI TextureParameterUIPrefab;
    public Transform ParameterContainer;
    public ParameterHandler parameters;

    public void SetParameters(ParameterHandler parameters)
    {
        this.parameters = parameters;
        AddParametersUI();
    }

    public void AddParametersUI()
    {
        foreach (Transform t in ParameterContainer)
            Destroy(t.gameObject);

        foreach (KeyValuePair<string, NumberParameter> p in parameters.GetNumberParameters())
            Instantiate(NumberParameterUIPrefab, ParameterContainer).SetParameter(p.Value);

        foreach(KeyValuePair<string, VectorParameter> p in parameters.GetVectorParameters())
            Instantiate(VectorParameterUIPrefab, ParameterContainer).SetParameter(p.Value);
    }
}
