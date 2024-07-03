using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable()]
public class TextureParameter : Parameter<TextureData>
{

    public static readonly string POSITION_SUFFIX = "(POSITION)", SIZE_SUFFIX = "(SIZE)", 
        OFFSET_SUFFIX = "(OFFSET)", SCALE_SUFFIX = "(SCALE)", COLOR_SUFFIX = "(COLOR)";

    public string TextureName;
    public VectorParameter Position { get; }
    public VectorParameter Size { get; }
    public VectorParameter Offset { get; }
    public VectorParameter Scale { get; }
    public ColorParameter Color { get; }

    public TextureParameter(string name, string textureName, VectorParameter position, VectorParameter size, VectorParameter offset, VectorParameter scale, ColorParameter color)
        : base(name, new TextureData(position.GetValue(), size.GetValue(), offset.GetValue(), scale.GetValue(), color.GetValue())) 
    {
        TextureName = textureName;
        Position = position;
        Size = size;
        Offset = offset;
        Scale = scale;
        Color = color;
    }

    public TextureParameter(string name, string textureName, Vector2 position, Vector2 size, Vector2 offset, Vector2 scale, Color32 color) : this(
              name, textureName,
              new VectorParameter(name + POSITION_SUFFIX, position), new VectorParameter(name + SIZE_SUFFIX, size),
              new VectorParameter(name + OFFSET_SUFFIX, offset), new VectorParameter(name + SCALE_SUFFIX, scale), new ColorParameter(name + COLOR_SUFFIX, color))
    { }

    public TextureParameter(string name, string textureName) : this(
              name, textureName,
              new Vector2(.5f, .5f), new Vector2(1, 1), Vector2.zero, new Vector2(1, 1), new Color32(0, 0, 255, 255))
    { }

    public override TextureData GetValue()
    {
        return new TextureData(Position.GetValue(), Size.GetValue(), Offset.GetValue(), Scale.GetValue(), Color.GetValue());
    }

    public override TextureData ValueAt(float t)
    {
        return new TextureData(Position.ValueAt(t), Size.ValueAt(t), Offset.ValueAt(t), Scale.ValueAt(t), Color.ValueAt(t));
    }

    public override void BindTimeline(Timeline timeline)
    {
        Position.BindTimeline(timeline);
        Size.BindTimeline(timeline);
        Offset.BindTimeline(timeline);
        Scale.BindTimeline(timeline);
        Color.BindTimeline(timeline);
    }


}
