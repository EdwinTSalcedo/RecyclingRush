using System.Collections;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
public class Envio_imagenes : MonoBehaviour
{
    public float enfriamiento=5000;
    public float tiempoenfriamiento=0;
    public Camera Camino;
    Texture2D image;
    private bool activado;
    private bool contador=false;
    public int FileCounter;
    private int screenWidth;
    private int screenHeight;
    public GameObject bote;
    private string timeFolderPath;
    byte[] bytes;

    string time;

    public void FixedUpdate()
    {
        if(contador)
        {
            if(bote.GetComponent<Movimiento>().movHorizontal!=0 || bote.GetComponent<Movimiento>().movVertical!=0)
            {
                tiempoenfriamiento=tiempoenfriamiento+1000f*Time.deltaTime;
                if(tiempoenfriamiento>=enfriamiento)
                {
                    tiempoenfriamiento=0f;
                    //asdasdasdas
                    screenWidth = Camino.pixelWidth;
                    screenHeight = Camino.pixelHeight;
                    RenderTexture rt = new RenderTexture(screenWidth, screenHeight, 24); // Usar las variables
                    Texture2D image  = new Texture2D(screenWidth, screenHeight, TextureFormat.RGB24, false); // Usar las variables
                    Camino.targetTexture = rt;
                    Camino.Render();
                    RenderTexture.active = rt;
                    //asdasdas
                    image.ReadPixels(new Rect(0, 0, Camino.targetTexture.width, Camino.targetTexture.height), 0, 0);
                    image.Apply();
                    RenderTexture.active = rt;
                    bytes = image.EncodeToPNG();
                    Destroy(image);
                    string imagePath = Path.Combine(timeFolderPath, "Image" + FileCounter + "angle" + bote.GetComponent<Movimiento>().anguloenvio + "velocity" + bote.GetComponent<Movimiento>().velocidadenvio + ".png");
                    File.WriteAllBytes(imagePath, bytes);
                    FileCounter++;
                }
            }
        } 
    }    
    public void Captura()
    {
        if(contador==true)
        {
            contador=false;
            FileCounter=0;
        }
        else
        {
            time = System.DateTime.UtcNow.ToLocalTime().ToString("dd_MM_yyyy_HH_mm_ss");
            
            // Obtén el directorio de Documentos del sistema operativo
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            
            // Define la ruta de las carpetas
            string recyclingRushPath = Path.Combine(documentsPath, "!Recycling Rush");
            string selfDrivingPath = Path.Combine(recyclingRushPath, "Self-Driving");
            
            // Verifica si las carpetas existen, si no, las crea
            if (!Directory.Exists(recyclingRushPath))
            {
                Directory.CreateDirectory(recyclingRushPath);
            }
            if (!Directory.Exists(selfDrivingPath))
            {
                Directory.CreateDirectory(selfDrivingPath);
            }
            
            // Crea la carpeta con el nombre de la variable 'time'
            timeFolderPath = Path.Combine(selfDrivingPath, time);
            Directory.CreateDirectory(timeFolderPath);
            
            contador=true;
        }
    }
}