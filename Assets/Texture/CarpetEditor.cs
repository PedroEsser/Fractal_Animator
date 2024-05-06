using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarpetEditor : MonoBehaviour
{

    public Image Image;
    public Plot2D plot;
    public InfiniteCarpet Carpet;


    public void Start()
    {
        Carpet = ConfigurationHandler.CurrentConfig.Settings.TextureHandler.Carpet;

    }

    private void Update()
    {
        Carpet.UpdateShader(Image.material);
    }

}
