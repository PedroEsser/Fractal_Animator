using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static ColorParameter;

[Serializable()]
public class TextureData
{

    public float PositionX, PositionY, SizeX, SizeY, OffsetX, OffsetY, ScaleX, ScaleY, R, G, B, A;

    public Vector2 Position { get => new Vector2(PositionX, PositionY); set { PositionX = value.x; PositionY = value.y; }  }
    public Vector2 Size { get => new Vector2(SizeX, SizeY); set { SizeX = value.x; SizeY = value.y; }  }
    public Vector2 Offset { get => new Vector2(OffsetX, OffsetY); set { OffsetX = value.x; OffsetY = value.y; }  }
    public Vector2 Scale { get => new Vector2(ScaleX, ScaleY); set { ScaleX = value.x; ScaleY = value.y; }  }
    public Color32 Color { get => new Color32(To255(R), To255(G), To255(B), To255(A));
        set { R = value.r; G = value.g; B = value.b; A = value.a; }
    }

    public TextureData(Vector2 position, Vector2 size, Vector2 offset, Vector2 scale, Color32 color)
    {
        Position = position;
        Size = size;
        Offset = offset;
        Scale = scale;
        Color = color;
    }

}
