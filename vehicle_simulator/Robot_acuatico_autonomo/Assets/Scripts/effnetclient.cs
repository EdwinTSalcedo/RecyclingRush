using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
public class effnetclient : MonoBehaviour
{
    
    public Movimiento_autonomo mov_auto;
    private RenderTexture renderTextures;
    private Texture2D textures;
    public Camera cameras; // Asigna tus cámaras en el inspector
    private float lastSendTime; // Para rastrear el tiempo de envío
    private int currentIndex = 0;
    private Queue<byte[]> imageQueue = new Queue<byte[]>();
    Texture2D screenShots;
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
        cameras = GameObject.FindWithTag("camprin").GetComponent<Camera>();
        client = new TcpClient("localhost", 12345);
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
        data = new byte[2048];
        cont++;
        bytes = stream.Read(data, 0, data.Length);
        message = Encoding.ASCII.GetString(data, 0, bytes);
        mov_auto.GirarHaciaAnguloAutonoma(float.Parse(message));
        data = null;
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
        RenderTexture rt = new RenderTexture(960,540, 24);
        Texture2D screenShot = new Texture2D(960,540, TextureFormat.RGB24, false);
        cameras.targetTexture = rt;
        cameras.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, 960,540), 0, 0);
        byte[] data = screenShot.EncodeToPNG();
        cameras.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback((state) =>
        {
                SetupTCP(data);
        }));
    }
    private void OnDestroy()
    {
        Debug.Log("Limpiando memoria");
        GC.SuppressFinalize(this);
    }
}