using System.Collections;
using UnityEngine;
/*
Estas librerias son las utilizadas para la comunicacion entre Unity y Python. 
Tomando en cuenta el puerto y la IP asignada, la cual tiene que ser la misma en ambos archivos
*/
//Este namespace se encarga de habilitar los protocolos para la red
using System.Net;
//Esta clase del namespace Net se encarga de el envio y recibo de datos
using System.Net.Sockets;
//Esta clase se encarga de la creacion de subprocesos, los cuales permitiran el envio y recibo de datos en mas de un subproceso.
using System.Threading;
using System.Text;
public class Imagenes_detector : MonoBehaviour
{
        /*
    Este script se trata de un script que mandara una imagen como informacion 
    */
    //Esta variable es el subproceso en el cual se realizara el envio y recibo de datos
    Thread mthreaddetector;
    public int connectionportdetectordetector=2530;
    int confirmacion;
    byte[] buffer;
    IPAddress localadddetector;
    TcpClient clientdetector;
    TcpListener listenerdetector;
    ThreadStart tsdetector;
    public bool envio;
    public Camera Detector;
    string dataconfirmacion;
    Texture2D image;
    private bool activado=true;
    byte[] bytes;

    
    private void Start()
    {
        //Inicio de creacion de un Detector para comunicacion para las dos camaras
        tsdetector = new ThreadStart(GetInfodetector);
        mthreaddetector=new Thread(tsdetector);
        mthreaddetector.Start();   
        activado=true;
    }
    void GetInfodetector()
    {
        listenerdetector= new TcpListener(IPAddress.Any, connectionportdetectordetector);
        listenerdetector.Start();        
        clientdetector=listenerdetector.AcceptTcpClient();
        envio=true;
        while(envio)
        {
            Enviodedatos();
        }
        listenerdetector.Stop();
    }
    void Enviodedatos()
    {   try
        {
            NetworkStream nwStream= clientdetector.GetStream();
            nwStream.Write(bytes,0,bytes.Length); 
            buffer=new byte[clientdetector.ReceiveBufferSize];
            confirmacion=nwStream.Read(buffer,0,clientdetector.ReceiveBufferSize);
        }
        catch
        {
            Debug.Log("El detector se ha desconectado");
            envio=false;
            activado=false;
        }
        dataconfirmacion=Encoding.UTF8.GetString(buffer,0,confirmacion);
    }  
    public void FixedUpdate()
    {
        
        RenderTexture activeRenderTextureTwo = RenderTexture.active;
        RenderTexture.active = Detector.targetTexture;
        Detector.Render();
        image = new Texture2D(Detector.targetTexture.width, Detector.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, Detector.targetTexture.width, Detector.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = activeRenderTextureTwo;
        bytes = image.EncodeToPNG();
        Destroy(image);
        if(!activado)
        {
            mthreaddetector.Abort();
            tsdetector = new ThreadStart(GetInfodetector);
            mthreaddetector=new Thread(tsdetector);
            mthreaddetector.Start();
            activado=true;
        }
    }
    //Las funciones encargadas de enviar los datos de puerto e IP para la comunicacion de los dos Detectors Thread
   
}
