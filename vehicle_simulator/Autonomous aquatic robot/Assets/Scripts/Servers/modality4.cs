using System.Diagnostics;
using UnityEngine;
using System;

public class modality4 : MonoBehaviour 
{
    // Public variables accessible from the Unity Editor
    public GameObject prefab;           // Prefab to instantiate
    public GameObject instancia;        // Instance of the prefab
    public gameagain game;              // Reference to the 'gameagain' script
    public bool servercomplete;         // Flag indicating if the server operation is complete
    public float intcontac;             // Time to wait before performing certain actions
    public float clearconsole;          // Time interval to clear the console
    public bool instaserver;            // Flag indicating if the server is instantiated
    public bool lagserver;              // Flag indicating if there is a server lag
    public bool initialserver;          // Flag indicating if the server is initially set up
    public depthclient script;          // Reference to the 'depthclient' script
    public Process pross;               // Process to run external commands

    // Called when the script is enabled
    private void OnEnable() 
    {
        // Set up file paths
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string recyclingRushPath = System.IO.Path.Combine(documentsPath, "!Recycling Rush");
        string serverFolderPath = System.IO.Path.Combine(recyclingRushPath, "servers");
        string pythonFilePath = System.IO.Path.Combine(serverFolderPath, "depthserver.py");

        // Start a command prompt and run a Python script
        Process pross = Process.Start("cmd.exe", $"/K cd /D \"{serverFolderPath}\" && python \"{pythonFilePath}\"");

        // Initialize variables
        servercomplete = true;
        intcontac = Time.time + 35f;
        clearconsole = Time.time + 25f;
    }

    // Called when the script is disabled
    private void OnDisable() 
    {
        // Clean up and terminate server-related processes
        Destroy(instancia);
        intcontac = Time.time + 10f;
        servercomplete = true;
        lagserver = false;
        initialserver = false;
        instaserver = false;

        // Terminate the command prompt process
        Process.Start(new ProcessStartInfo 
        { 
            FileName = "taskkill", 
            Arguments = "/im cmd.exe /f /t", 
            CreateNoWindow = true, 
            UseShellExecute = false 
        }).WaitForExit();
    }

    // Called every frame
    private void Update() 
    {
        if(servercomplete || !lagserver)
        {
            // Check if it's time to perform certain actions
            if(Time.time >= intcontac)
            {
                if(!instaserver)
                {
                    // Instantiate server and perform necessary setup
                    UnityEngine.Debug.Log("Hello pew");
                    instancia = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
                    script = instancia.GetComponent<depthclient>();
                    instaserver = true;
                    initialserver = true;
                    game.Again();
                }
            }
        }
        else
        {
            // Clean up and reset if the server operation is not complete or there is server lag
            Destroy(instancia);
            intcontac = Time.time + 35f;
            servercomplete = true;
            lagserver = false;
            initialserver = false;
            instaserver = false;
        }

        if(initialserver)
        {
            // Check if it's time to clear the console
            if(Time.time >= clearconsole)
            {
                if(script.constant <= script.cont)
                {
                    // Log a message when the client is connected
                    script.constant = script.cont;
                    UnityEngine.Debug.Log("Client connected");
                }
                else
                {
                    // Log a message and set a flag when the server is down, initiate cleanup
                    UnityEngine.Debug.Log("Server Down, Reconnecting");
                    lagserver = true;
                    Clear();
                }
                clearconsole = Time.time + 25f;
            }
        }
    }

    // Clear the Unity console
    public static void Clear()
    {
        System.Type type = System.Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
        System.Reflection.MethodInfo method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}
