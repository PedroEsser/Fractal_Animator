using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable()]
public class TextureParameter : Parameter<TextureData>
{

    public static readonly string POSITION_SUFFIX = "(POSITION)", SIZE_SUFFIX = "(SIZE)", OFFSET_SUFFIX = "(OFFSET)", SCALE_SUFFIX = "(SCALE)";

    public string TextureName;
    public VectorParameter Position { get; }
    public VectorParameter Size { get; }
    public VectorParameter Offset { get; }
    public VectorParameter Scale { get; }

    public TextureParameter(string name, string textureName, VectorParameter position, VectorParameter size, VectorParameter offset, VectorParameter scale)
        : base(name, new TextureData(position.GetValue(), size.GetValue(), offset.GetValue(), scale.GetValue())) 
    {
        TextureName = textureName;
        Position = position;
        Size = size;
        Offset = offset;
        Scale = scale;
    }

    public TextureParameter(string name, string textureName, Vector2 position, Vector2 size, Vector2 offset, Vector2 scale) : this(
              name, textureName,
              new VectorParameter(name + POSITION_SUFFIX, position), new VectorParameter(name + SIZE_SUFFIX, size),
              new VectorParameter(name + OFFSET_SUFFIX, offset), new VectorParameter(name + SCALE_SUFFIX, scale))
    { }

    public TextureParameter(string name, string textureName) : this(
              name, textureName,
              Vector2.zero, new Vector2(1, 1), Vector2.zero, new Vector2(1, 1))
    { }

    public override TextureData GetValue()
    {
        return new TextureData(Position.GetValue(), Size.GetValue(), Offset.GetValue(), Scale.GetValue());
    }

    public override TextureData ValueAt(float t)
    {
        return new TextureData(Position.ValueAt(t), Size.ValueAt(t), Offset.ValueAt(t), Scale.ValueAt(t));
    }



}
