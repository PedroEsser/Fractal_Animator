using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;

public class CreateVideoWindow : MonoBehaviour
{
    public Image PreviewWindow;
    public RawImage Preview;
    public InputField FileName, Width, Height, FPS, Duration;
    public ProgressBar ProgressBar;
    public int updateRate = 10;

    public Vector2Int Dimensions { get => new Vector2Int(int.Parse(Width.text), int.Parse(Height.text)); }

    private void Start()
    {
        //Width.onValueChanged.AddListener(str => UpdatePreview());
        //Height.onValueChanged.AddListener(str => UpdatePreview());
        FPS.text = ConfigurationHandler.CurrentConfig.Timeline.fps + "";
        Duration.text = ConfigurationHandler.CurrentConfig.Timeline.duration + "";
    }

    private void Update()
    {
        if(ProgressBar.progress <= 0 && Time.frameCount % updateRate == 0)
            UpdatePreview();
    }

    public void UpdatePreview()
    {
        Vector2 dim = Dimensions;
        Vector2 windowDim = PreviewWindow.rectTransform.sizeDelta;
        if (windowDim.x == dim.x && windowDim.y == dim.y)
            return;

        float windowAR = windowDim.x / windowDim.y;
        float ar = (float)dim.x / dim.y;
        if (ar > windowAR)
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
        string file = StandaloneFileBrowser.SaveFilePanel("Save As", ConfigurationHandler.CurrentConfig.defaultVideoPath, FileName.text, "mp4");
        if (file.Length != 0) 
        {
            ConfigurationHandler.CurrentConfig.defaultVideoPath = file;
            IEnumerator coroutine = CreateVideo(Dimensions, int.Parse(FPS.text), int.Parse(Duration.text), file);
            StartCoroutine(coroutine);
        }
    }

    public IEnumerator CreateVideo(Vector2Int dimensions, int fps = 60, int duration = -1, string path = "D:\\MandelbrotStuff\\fractal art\\gifs")
    {
        string name = path.Substring(path.LastIndexOf('\\') + 1, path.Length - path.LastIndexOf('\\') - 5);
        string folder = path.Substring(0, path.LastIndexOf('\\')) + "\\" + name + "_Frames";
        System.IO.Directory.CreateDirectory(folder);
        

        Configuration config = ConfigurationHandler.CurrentConfig;

        if (duration == -1)
            duration = config.Timeline.duration;
        Vector4 window = config.Settings.WindowSettings.Window;
        window.w = window.z * dimensions.y / dimensions.x * 2;

        Material mat = new Material(Controller.Singleton.PlotterMaterial);
        //Material mat = Controller.Singleton.PlotterMaterial;
        mat.SetVector("_Window", window);

        Texture2D tex = new Texture2D(dimensions.x, dimensions.y, TextureFormat.RGBA32, false);
        float accT = 0;
        float t = 0;

        for (int i = 0; i < duration; i++)
        {
            while(ProgressBar.paused)
                yield return null;

            //t = System.DateTime.Now.Second;
            float percent = (float)(i+1) / duration;
            ProgressBar.SetProgress(percent, "Rendering frame " + (i+1) + " of " + duration + " (" + (100*percent).ToString("0.00") + "%)");
            config.Timeline.CurrentTime = i;
            config.Settings.UpdateShader(mat);

            RenderTexture renderTexture = RenderTexture.GetTemporary(dimensions.x, dimensions.y, 0, RenderTextureFormat.ARGB32);
            RenderTexture.active = renderTexture;
            Graphics.Blit(null, renderTexture, mat);

            tex.ReadPixels(new Rect(0, 0, dimensions.x, dimensions.y), 0, 0);
            tex.Apply();

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(renderTexture);
            Preview.texture = tex;
            byte[] bytes = tex.EncodeToPNG();
            System.IO.File.WriteAllBytes(folder + "/Frame" + string.Format("{0:00000}", i) + ".png", bytes);
            yield return null;
        }
        ProgressBar.SetProgress(1, "Finished");
        //ImagesToVideo videoCommand = new ImagesToVideo(folder + "/Frame%05d.jpg", null, path, fps, duration, 20);
        //FFmpeg.Execute(videoCommand.ToString());
    }


    public Texture2D CreateImage(Vector2Int dimensions)
    {
        RenderTexture renderTexture = RenderTexture.GetTemporary(dimensions.x, dimensions.y, 0, RenderTextureFormat.ARGB32);

        Vector4 window = ConfigurationHandler.CurrentConfig.Settings.WindowSettings.Window;
        window.w = window.z * dimensions.y / dimensions.x;
        Material mat = new Material(Controller.Singleton.PlotterMaterial);
        mat.SetVector("_Window", window);

        // Set the created Render Texture as the active render target
        RenderTexture.active = renderTexture;

        // Render with the shader
        Graphics.Blit(null, renderTexture, mat);

        // Create a Texture2D to read the pixels from the Render Texture
        Texture2D tex = new Texture2D(dimensions.x, dimensions.y, TextureFormat.RGBA32, false);

        // Read pixels from the Render Texture to the Texture2D
        tex.ReadPixels(new Rect(0, 0, dimensions.x, dimensions.y), 0, 0);
        tex.Apply();

        // Reset the active Render Texture
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(renderTexture);

        return tex;
    }

}
