using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
public class depthclient : MonoBehaviour
{
    
    public Movimiento_autonomo mov_auto;
    private RenderTexture[] renderTextures = new RenderTexture[3];
    private Texture2D[] textures = new Texture2D[3];
    public Camera[] cameras = new Camera[3]; // Asigna tus cámaras en el inspector
    private float lastSendTime; // Para rastrear el tiempo de envío
    private int currentIndex = 0;
    private Queue<byte[]> imageQueue = new Queue<byte[]>();
    Texture2D[] screenShots = new Texture2D[2];
    private int i;
    private string angle;
    private TcpClient client; // Ajusta la dirección IP y el puerto
    private NetworkStream stream;
    public float captureinterval;
    public float clearinter;
    private int bytes;
    public string message;
    public bool binary;
    private float nextContTime;
    public int contant;
    public int cont;
    public rest reseting;
    void OnEnable()
    {
        GameObject objWithMovimientoAutonomo = GameObject.FindGameObjectWithTag("MovimientoAutonomo");
        mov_auto = objWithMovimientoAutonomo.GetComponent<Movimiento_autonomo>();
        cameras[0] = GameObject.FindWithTag("stereo1").GetComponent<Camera>();
        cameras[1] = GameObject.FindWithTag("stereo2").GetComponent<Camera>();
        cameras[2] = GameObject.FindWithTag("camprin").GetComponent<Camera>();
        client = new TcpClient("localhost", 1024);
        stream = client.GetStream();
        lastSendTime = Time.time; 
        contant =  cont;
        nextContTime = Time.time + 15f;
        clearinter = Time.time +100;
        binary = false;
    }

    void SetupTCP(byte[] data)
    {
        byte[] dataWithMarker = new byte[data.Length + 12]; // 12 bytes para "END_OF_IMAGE"
        data.CopyTo(dataWithMarker, 0);
        byte[] marker = System.Text.Encoding.ASCII.GetBytes("END_OF_IMAGE");
        marker.CopyTo(dataWithMarker, data.Length);
        stream.Write(dataWithMarker, 0, dataWithMarker.Length);
        
        if(i==3)
        {
            i=0;
            data = new byte[2048];
            bytes = stream.Read(data, 0, data.Length);
           
            message = Encoding.ASCII.GetString(data, 0, bytes);
            mov_auto.GirarHaciaAnguloAutonoma(float.Parse(message));
            data = null;
            
        }
        cont++;
    }

    void Update()
    {
        if(Time.time -lastSendTime >=captureinterval)
        {
            lastSendTime = Time.time;
            SendMessage();

        }
    }

    void SendMessage()
    {
        if(i<=2)
        {
            
            RenderTexture rt = new RenderTexture(960,540, 24);
            Texture2D screenShot = new Texture2D(960,540, TextureFormat.RGB24, false);
            cameras[i].targetTexture = rt;
            cameras[i].Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, 960,540), 0, 0);
            byte[] data = screenShot.EncodeToPNG();
            imageQueue.Enqueue(data);
            cameras[i].targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);
            i++;
            
            
        }
        // Comprueba si hay imágenes en la cola y envía una a la vez
        if (imageQueue.Count > 0)
        {
            byte[] imageData = imageQueue.Dequeue();
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback((state) =>
            {
                SetupTCP(imageData);
            }));
        }
        
    }
    private void OnDestroy()
    {
        Debug.Log("Limpiando memoria");
        GC.SuppressFinalize(this);
    }
}