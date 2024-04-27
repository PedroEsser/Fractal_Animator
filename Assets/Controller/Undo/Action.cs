using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Action
{

    public void Do();
    public void Undo();
    public void ReDo();

}
