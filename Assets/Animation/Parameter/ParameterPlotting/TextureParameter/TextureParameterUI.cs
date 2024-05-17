using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TextureParameterUI : ParameterUI<TextureData>
{

    public TextureParameter TextureParameter { get => (TextureParameter)Parameter; }
    public TextureLoader Loader;
    public GameObject ParameterContainer;
    public VectorParameterUI Position, Size, Offset, Scale;
    public ColorParameterUI Color;
    public Button ShowMore;
    public Sprite TriangleRight, TriangleDown;
    public UnityEvent OnDelete;

    public override void SetParameter(Parameter<TextureData> parameter)
    {
        base.SetParameter(parameter);
        TextureParameter par = (TextureParameter)parameter;

        Position.SetParameter(par.Position);
        Position.NameText.text = "Position";

        Size.SetParameter(par.Size);
        Size.NameText.text = "Size";

        Offset.SetParameter(par.Offset);
        Offset.NameText.text = "Offset";

        Scale.SetParameter(par.Scale);
        Scale.NameText.text = "Scale";

        Color.SetParameter(par.Color);
        Color.NameText.text = "Color";

        ShowMore.gameObject.SetActive(true);
        UpdateTextureIcon();
    }

    public void ToggleShowMore()
    {
        bool showingMore = ShowMore.image.sprite == TriangleDown;
        showingMore = !showingMore;
        ShowMore.image.sprite = showingMore ? TriangleDown : TriangleRight;

        Vector2 size = gameObject.GetComponent<RectTransform>().sizeDelta;
        size.y += ParameterContainer.GetComponent<RectTransform>().sizeDelta.y * (showingMore ? 1 : -1);
        gameObject.GetComponent<RectTransform>().sizeDelta = size;
        ParameterContainer.SetActive(showingMore);
    }

    public void UpdateTextureIcon()
    {
        if (TextureParameter.TextureName != null)
            Loader.SetTexture(TextureHandler.GetTexture(TextureParameter.TextureName));
    }

    public void OnDeleteButtonPress() { OnDelete.Invoke(); }

}
