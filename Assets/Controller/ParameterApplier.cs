using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ParameterApplier
{

    public ParameterHandler GetParameters();
    public void BindTimeline(Timeline timeline);
    public void UpdateShader(Material mat);

}
