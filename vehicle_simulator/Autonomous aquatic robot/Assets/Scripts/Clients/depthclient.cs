using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

public class depthclient : MonoBehaviour
{
    public AutonomousMovement autonomousMovement;
    private RenderTexture[] renderTextures = new RenderTexture[3];
    private Texture2D[] textures = new Texture2D[3];
    public Camera[] cameras = new Camera[3]; // Assign your cameras in the Inspector
    private float lastSendTime; // To track the sending time
    private int currentIndex = 0;
    private Queue<byte[]> imageQueue = new Queue<byte[]>();
    private TcpClient client; // Adjust the IP address and port
    private NetworkStream stream;
    public float captureInterval;
    public float clearInterval;
    private int bytes;
    public string message;
    public bool binary;
    private float nextContTime;
    public int constant;
    public int cont;
    public rest reseting;

    void OnEnable()
    {
        // Find and assign the AutonomousMovement script
        GameObject objWithAutonomousMovement = GameObject.FindGameObjectWithTag("MovimientoAutonomo");
        autonomousMovement = objWithAutonomousMovement.GetComponent<AutonomousMovement>();

        // Find and assign cameras based on tags
        cameras[0] = GameObject.FindWithTag("stereo1").GetComponent<Camera>();
        cameras[1] = GameObject.FindWithTag("stereo2").GetComponent<Camera>();
        cameras[2] = GameObject.FindWithTag("camprin").GetComponent<Camera>();

        // Initialize TCP client and stream
        client = new TcpClient("localhost", 1024);
        stream = client.GetStream();

        // Initialize variables
        lastSendTime = Time.time;
        constant = cont;
        nextContTime = Time.time + 15f;
        clearInterval = Time.time + 100;
        binary = false;
    }

    void SetupTCP(byte[] data)
    {
        // Add marker "END_OF_IMAGE" to the data
        byte[] dataWithMarker = new byte[data.Length + 12]; // 12 bytes for "END_OF_IMAGE"
        data.CopyTo(dataWithMarker, 0);
        byte[] marker = System.Text.Encoding.ASCII.GetBytes("END_OF_IMAGE");
        marker.CopyTo(dataWithMarker, data.Length);

        // Send data to the server
        stream.Write(dataWithMarker, 0, dataWithMarker.Length);

        // If the index reaches 3, reset it and read angle data from the server
        if (currentIndex == 3)
        {
            currentIndex = 0;
            data = new byte[2048];
            bytes = stream.Read(data, 0, data.Length);

            // Convert received message to string and update the angle in the AutonomousMovement script
            message = Encoding.ASCII.GetString(data, 0, bytes);
            autonomousMovement.RotateToAutonomousAngle(float.Parse(message));
            data = null;
        }

        // Increment the counter
        cont++;
    }

    void Update()
    {
        // Check if it's time to capture and send an image
        if (Time.time - lastSendTime >= captureInterval)
        {
            lastSendTime = Time.time;
            SendMessage();
        }
    }

    void SendMessage()
    {
        // If the index is less than or equal to 2, capture and send an image
        if (currentIndex <= 2)
        {
            // Create a RenderTexture and a Texture2D to capture the screen
            RenderTexture rt = new RenderTexture(960, 540, 24);
            Texture2D screenShot = new Texture2D(960, 540, TextureFormat.RGB24, false);
            
            // Set the camera's target texture to render the scene
            cameras[currentIndex].targetTexture = rt;
            cameras[currentIndex].Render();

            // Set the active RenderTexture and read the pixels into the Texture2D
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, 960, 540), 0, 0);
            
            // Encode the Texture2D to PNG format and enqueue the data
            byte[] data = screenShot.EncodeToPNG();
            imageQueue.Enqueue(data);

            // Reset camera properties
            cameras[currentIndex].targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);

            // Increment the index
            currentIndex++;
        }

        // Check if there are images in the queue and send one at a time
        if (imageQueue.Count > 0)
        {
            byte[] imageData = imageQueue.Dequeue();

            // Use a thread pool to send data asynchronously
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback((state) =>
            {
                SetupTCP(imageData);
            }));
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Cleaning up memory");
        GC.SuppressFinalize(this);
    }
}
