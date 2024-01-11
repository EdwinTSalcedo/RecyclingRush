using UnityEngine;
using System;
using System.IO;

public class Capture_image : MonoBehaviour
{
    public Camera Camino;
    public RenderTexture renderTexture;
    private string assetsFolder = "Assets/Images";
    private string dateFolder;
    private int counter = 0;
    private int renderTextureWidth = 1920; // Nueva resolución de ancho
    private int renderTextureHeight = 1080; // Nueva resolución de alto
    private string currentDate;

    private void Awake()
    {
        renderTexture = new RenderTexture(renderTextureWidth, renderTextureHeight, 24);
        renderTexture.Create();

        // Asignar el RenderTexture a la cámara
        Camino.targetTexture = renderTexture;
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
    Camino.Render();

    string fileName = "capture" + counter.ToString() + ".png";

    // Obtén el directorio de Documentos del sistema operativo
    string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
    
    // Define la ruta de las carpetas
    string recyclingRushPath = Path.Combine(documentsPath, "!Recycling Rush");
    string validationPath = Path.Combine(recyclingRushPath, "Validation");
    
    // Verifica si las carpetas existen, si no, las crea
    if (!Directory.Exists(recyclingRushPath))
    {
        Directory.CreateDirectory(recyclingRushPath);
    }
    if (!Directory.Exists(validationPath))
    {
        Directory.CreateDirectory(validationPath);
    }
    
    // Crea la carpeta con el nombre de la variable 'time'
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

    Camino.Render();
}

}
