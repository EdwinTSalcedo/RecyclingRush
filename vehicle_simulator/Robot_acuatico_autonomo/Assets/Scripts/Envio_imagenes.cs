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
    public GameObject bote;
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
                    RenderTexture activeRenderTextureTwo = RenderTexture.active;
                    RenderTexture.active = Camino.targetTexture;
                    Camino.Render();
                    image = new Texture2D(Camino.targetTexture.width, Camino.targetTexture.height);
                    image.ReadPixels(new Rect(0, 0, Camino.targetTexture.width, Camino.targetTexture.height), 0, 0);
                    image.Apply();
                    RenderTexture.active = activeRenderTextureTwo;
                    bytes = image.EncodeToPNG();
                    Destroy(image);
                    
                    File.WriteAllBytes(Application.dataPath +"/Imagenes"+ "/"+time+"/" +"Image" +FileCounter +"Angle"+bote.transform.localEulerAngles.y + ".png", bytes);
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
            time = System.DateTime.UtcNow.ToLocalTime().ToString("dd_MM_yyyy   HH_mm_ss");
            AssetDatabase.CreateFolder("Assets/Imagenes",time);
            contador=true;
        }
        
    }
}