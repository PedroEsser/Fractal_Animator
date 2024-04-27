using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;

public class CreateImageWindow : MonoBehaviour
{
    public Image PreviewWindow;
    public RawImage Preview;
    public InputField FileName, Width, Height;
    public Dropdown FileType;
    private bool first = true;

    public Vector2Int Dimensions { get => new Vector2Int(int.Parse(Width.text), int.Parse(Height.text)); }

    private void Start()
    {
        Width.onEndEdit.AddListener(str => UpdatePreview());
        Height.onEndEdit.AddListener(str => UpdatePreview());
    }

    private void Update()
    {
        if (first)
        {
            UpdatePreview();
            first = false;
        }
    }

    public void UpdatePreview()
    {
        Vector2 windowDim = PreviewWindow.rectTransform.sizeDelta;
        if (windowDim.x == Dimensions.x && windowDim.y == Dimensions.y)
            return;

        float windowAR = windowDim.x / windowDim.y;
        float ar = (float)Dimensions.x / Dimensions.y;
        Vector2 dim = Vector2.zero;
        if(ar > windowAR)
        {
            dim.x = windowDim.x;
            dim.y = dim.x / ar;
        }
        else
        {
            dim.y = windowDim.y;
            dim.x = dim.y * ar;
        }
        if (dim.x == 0 || dim.y == 0)
            return;
        Preview.rectTransform.sizeDelta = dim;
        Preview.texture = CreateImage(new Vector2Int((int)dim.x, (int)dim.y));
    }

    public void Save()
    {
        string file = StandaloneFileBrowser.SaveFilePanel("Save As", ConfigurationHandler.CurrentConfig.defaultTexturePath, FileName.text, FileType.captionText.text);
        if (file.Length != 0) 
        {
            ConfigurationHandler.CurrentConfig.defaultTexturePath = file;
            SaveImage(CreateImage(Dimensions), file, FileType.captionText.text);
        }
    }

    public Texture2D CreateImage(Vector2Int dimensions)
    {
        RenderTexture renderTexture = RenderTexture.GetTemporary(dimensions.x, dimensions.y, 0, RenderTextureFormat.ARGB32);

        Vector4 window = ConfigurationHandler.CurrentConfig.Settings.WindowHandler.Window;
        window.w = window.z * dimensions.y / dimensions.x;
        Material mat = new Material(Controller.Singleton.PlotterMaterial);
        mat.SetVector("_Window", window);

        // Set the created Render Texture as the active render target
        RenderTexture.active = renderTexture;

        // Render with the shader
        Graphics.Blit(null, renderTexture, mat);

        // Create a Texture2D to read the pixels from the Render Texture
        Texture2D tex = new Texture2D(dimensions.x, dimensions.y, TextureFormat.RGB24, false);

        // Read pixels from the Render Texture to the Texture2D
        tex.ReadPixels(new Rect(0, 0, dimensions.x, dimensions.y), 0, 0);
        tex.Apply();

        // Reset the active Render Texture
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(renderTexture);


        return tex;
    }

    public void SaveImage(Texture2D tex, string path = "D:\\MandelbrotStuff\\fractal art\\Shop\\output.png", string type = "png")
    {
        byte[] bytes;
        if (type == "png")
            bytes = tex.EncodeToPNG();
        else
            bytes = tex.EncodeToJPG(85);

        System.IO.File.WriteAllBytes(path, bytes);
    }

}
