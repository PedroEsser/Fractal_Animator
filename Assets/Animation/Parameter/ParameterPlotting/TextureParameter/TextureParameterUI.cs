using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureParameterUI : ParameterUI<TextureData>
{
    public TextureLoader loader;
    public VectorParameterUI Position, Size, Offset, Scale;
    public Button ShowMore;
    public Sprite TriangleRight, TriangleDown;

    public void HandleTextureSelect(string[] images)
    {
        if (Parameter == null)
        {
            TextureParameter par = new TextureParameter(NameText.text, "", Vector2.zero, new Vector2(1, 1), Vector2.zero, new Vector2(1, 1));
            SetParameter(par);
            ShowMore.gameObject.SetActive(true);
        }
        string name = FileLoader.GetFileName(images[0]);
        ((TextureParameter)Parameter).TextureName = name;
    }

    public override void SetParameter(Parameter<TextureData> parameter)
    {
        TextureParameter par = (TextureParameter)parameter;
        this.Parameter = par;

        Position.SetParameter(par.Position);
        Position.NameText.text = "Position";

        Size.SetParameter(par.Size);
        Size.NameText.text = "Size";

        Offset.SetParameter(par.Offset);
        Offset.NameText.text = "Offset";

        Scale.SetParameter(par.Scale);
        Scale.NameText.text = "Scale";

        NameText.text = parameter.Name;
    }

    public void ToggleShowMore()
    {
        bool showingMore = ShowMore.image.sprite == TriangleDown;
        showingMore = !showingMore;
        ShowMore.image.sprite = showingMore ? TriangleDown : TriangleRight;

        Position.gameObject.SetActive(showingMore);
        Size.gameObject.SetActive(showingMore);
        Offset.gameObject.SetActive(showingMore);
        Scale.gameObject.SetActive(showingMore);
    }

}
