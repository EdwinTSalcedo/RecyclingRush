using UnityEngine;
using System;
using System.Net.Sockets;
using System.Collections.Generic;

public class deph : MonoBehaviour
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
    public float captureinterval =5f;
    private string angle;
    
    
    private void OnEnable()
    {
        lastSendTime = Time.time; // Inicializa el tiempo de envío
    }

    private void Update()
    {
        if(Time.time -lastSendTime >=captureinterval)
        {
            if(i<=2)
            {
                RenderTexture rt = new RenderTexture(1920,1080, 24);
                Texture2D screenShot = new Texture2D(1920, 1080, TextureFormat.RGB24, false);
                cameras[i].targetTexture = rt;
                cameras[i].Render();
                RenderTexture.active = rt;
                screenShot.ReadPixels(new Rect(0, 0, 1920, 1080), 0, 0);
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
                SendToServer(imageData);
            }
            if(i==3)
            {
                ReceiveFromServer();
                i=0;
            }
            lastSendTime = Time.time;
        }
    }

    private void SendToServer(byte[] data)
{
    try
    {
        // Establece una conexión con el servidor Python
        TcpClient client = new TcpClient("127.0.0.1", 12345); // Ajusta la dirección IP y el puerto

        NetworkStream stream = client.GetStream();

        // Agrega la marca "END_OF_IMAGE" al final de los datos
        byte[] dataWithMarker = new byte[data.Length + 12]; // 12 bytes para "END_OF_IMAGE"
        data.CopyTo(dataWithMarker, 0);
        byte[] marker = System.Text.Encoding.ASCII.GetBytes("END_OF_IMAGE");
        marker.CopyTo(dataWithMarker, data.Length);

        // Envía los datos
        stream.Write(dataWithMarker, 0, dataWithMarker.Length);

        // Cierra la conexión
        client.Close();
    }
    catch (Exception e)
    {
        Debug.LogError("Error al enviar datos: " + e.Message);
    }
}


private void ReceiveFromServer()
{
    try
    {
        // Establece una conexión con el servidor Python
        TcpClient client = new TcpClient("127.0.0.1", 12345); // Ajusta la dirección IP y el puerto

        NetworkStream stream = client.GetStream();

        // Crea un buffer para almacenar los datos recibidos
        byte[] data = new byte[300];

        // Lee los datos del stream
        int bytesRead = stream.Read(data, 0, data.Length);

        // Convierte los datos a string
        string receivedData = System.Text.Encoding.ASCII.GetString(data, 0, bytesRead);

        // Cierra la conexión
        client.Close();
        Debug.Log(receivedData);
        
    }
    catch (Exception e)
    {
        Debug.LogError("Error al recibir datos: " + e.Message);
    }
}

}