using UnityEngine;
using System;
using System.IO;

public class CaptureImage : MonoBehaviour
{
    public Camera camino;
    public RenderTexture renderTexture;
    private string assetsFolder = "Assets/Images";
    private string dateFolder;
    private int counter = 0;
    private int renderTextureWidth = 1920; // New width resolution
    private int renderTextureHeight = 1080; // New height resolution
    private string currentDate;

    private void Awake()
    {
        renderTexture = new RenderTexture(renderTextureWidth, renderTextureHeight, 24);
        renderTexture.Create();

        // Assign the RenderTexture to the camera
        camino.targetTexture = renderTexture;
        currentDate = DateTime.Now.ToString("yyyy_MM_dd_HH_mm");
        dateFolder = Path.Combine(assetsFolder, currentDate);

        if (!Directory.Exists(dateFolder))
        {
            Directory.CreateDirectory(dateFolder);
        }
    }

    public void CaptureAndSave()
    {
        // Configure the camera to use the current lighting conditions
        camino.Render();

        string fileName = $"capture{counter}.png";

        // Get the Documents directory path
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // Define folder paths
        string recyclingRushPath = Path.Combine(documentsPath, "!Recycling Rush");
        string validationPath = Path.Combine(recyclingRushPath, "Validation");

        // Create folders if they don't exist
        Directory.CreateDirectory(recyclingRushPath);
        Directory.CreateDirectory(validationPath);

        // Create a folder with the name of the 'currentDate' variable
        string timeFolderPath = Path.Combine(validationPath, currentDate);
        Directory.CreateDirectory(timeFolderPath);

        string fullPath = Path.Combine(timeFolderPath, fileName);
        Texture2D screenShot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        RenderTexture.active = null;
        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(fullPath, bytes);
        Destroy(screenShot);
        counter++;

        // Capture again for subsequent frames
        camino.Render();
    }
}
