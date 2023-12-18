
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class clientmomentum : MonoBehaviour
{
    private Texture2D texture;
    public Camera cam;
    public float captureInterval = 0.4f;
    private int screenWidth;
    private int screenHeight;
    private TcpClient client;
    private NetworkStream stream;
    private byte[] data;
    private int cont;
    public rest reseting;
    private int contant;
    public string message;
    private int bytes;
    public Movimiento_autonomo mov_auto;
    private float nextCaptureTime;
    private int continit;
    private float nextContTime;

    void OnEnable()
    {
        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.red);
        texture.Apply();
        client = new TcpClient("localhost", 12345);
        stream = client.GetStream();
        nextCaptureTime = Time.time + captureInterval;
        nextContTime = Time.time + 15f;
        contant =  cont;
    }

    void SetupTCP(byte[] data)
    {
        stream.Write(data, 0, data.Length);
        data = new byte[2048];
        bytes = stream.Read(data, 0, data.Length);
        message = Encoding.ASCII.GetString(data, 0, bytes);
        mov_auto.GirarHaciaAnguloAutonoma(float.Parse(message));
        data = null;
        cont++;
    }

    void Update()
    {
        if (Time.time >= nextCaptureTime)
        {
            nextCaptureTime = Time.time + captureInterval;
            SendMessage();
        }
        if (Time.time >= nextContTime)
        {
            nextContTime = Time.time + 5f;
            if(contant+1<=cont)
            {
                Debug.Log("El servidor esta conectado");
                contant=cont;
            }
            else
            {
                Debug.Log("El servidor esta desconectado");
                reseting.point=true;
            }
        }
    }

    void SendMessage()
    {
        screenWidth = cam.pixelWidth;
        screenHeight = cam.pixelHeight;
        RenderTexture rt = new RenderTexture(screenWidth, screenHeight, 24);
        Texture2D screenShot = new Texture2D(screenWidth, screenHeight, TextureFormat.RGB24, false);
        cam.targetTexture = rt;
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, screenWidth, screenHeight), 0, 0);
        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        Destroy(screenShot);
        // Utiliza el ThreadPool para enviar los datos en un hilo de fondo
        System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback((state) =>
        {
            SetupTCP(bytes);
        }));
    }
}