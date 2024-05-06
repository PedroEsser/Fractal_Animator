using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable()]
public class TextureData
{

    public Vector2 Position, Size, Offset, Scale;

    public TextureData(Vector2 position, Vector2 size, Vector2 offset, Vector2 scale)
    {
        Position = position;
        Size = size;
        Offset = offset;
        Scale = scale;
    }

}
