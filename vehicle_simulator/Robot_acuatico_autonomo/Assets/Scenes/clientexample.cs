 using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class clientexample : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private string serverIp = "127.0.0.1"; // Cambia esto por la dirección IP del servidor
    private int port = 12345; // El mismo puerto que configuraste en el servidor

    private void Start()
    {
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        try
        {
            client = new TcpClient(serverIp, port);
            stream = client.GetStream();
            Debug.Log("Conexión al servidor establecida.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al conectar al servidor: {e}");
        }
    }

    private void SendMessageToServer(string message)
    {
        if (client == null || !client.Connected)
        {
            Debug.LogError("No se ha establecido una conexión con el servidor.");
            return;
        }

        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Debug.Log($"Enviado: {message}");

            // Recibe la respuesta del servidor
            data = new byte[1024];
            int bytesRead = stream.Read(data, 0, data.Length);
            string response = Encoding.UTF8.GetString(data, 0, bytesRead);
            Debug.Log($"Respuesta del servidor: {response}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al enviar/recibir datos: {e}");
        }
    }

    private void Update()
    {
        SendMessageToServer("Hola desde Unity!");
    }

    private void OnDestroy()
    {
        if (client != null)
        {
            stream.Close();
            client.Close();
        }
    }
}
