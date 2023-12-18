using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;
using System.Threading;
using System.Text;  // Añade esta línea


public class SocketClient : MonoBehaviour
{
    public Camera cam;
    public float captureInterval = 10.0f;  // Intervalo de captura de 10 segundos
    public int quality = 25;  // Calidad de la imagen, de 0 (peor) a 100 (mejor)

    private int screenWidth;
    private int screenHeight;

    private void Start()
    {
        // Almacenar las dimensiones de la pantalla en las variables
        screenWidth = cam.pixelWidth;
        screenHeight = cam.pixelHeight;

        // Comienza a capturar y enviar imágenes cada 10 segundos
        InvokeRepeating("CaptureAndSendImage", 0.0f, captureInterval);
    }

    void CaptureAndSendImage()
    {
        print("pip");
        // Captura la imagen en el hilo principal
        RenderTexture rt = new RenderTexture(480, 270, 24); // Usar las variables
        
        Texture2D screenShot = new Texture2D(480, 270, TextureFormat.RGB24, false); // Usar las variables
        cam.targetTexture = rt;
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0,480, 270), 0, 0); // Usar las variables
        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToJPG(100 - quality);  // Modificar la calidad aquí

        // Iniciar un hilo secundario para enviar datos a Python
        Thread sendThread = new Thread(() =>
        {
            SendDataToPython(bytes);
        });
        sendThread.Start();
    }

    void SendDataToPython(byte[] data)
    {
        try
        {
            // Enviar los datos de la imagen a Python
            TcpClient client = new TcpClient("localhost", 5000);
            NetworkStream nwStream = client.GetStream();
            nwStream.Write(data, 0, data.Length);

            // Recibir datos del servidor Python
            byte[] buffer = new byte[1024];
            int bytesRead = nwStream.Read(buffer, 0, buffer.Length);
            //string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            //Debug.Log("Respuesta del servidor: " + response);

            client.Close();
        }
        catch (Exception e)
        {
            Debug.LogError("Error al enviar datos a Python: " + e.Message);
        }
    }

}
