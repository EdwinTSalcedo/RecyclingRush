using System.Diagnostics;
using UnityEngine;
using System;

public class modality2 : MonoBehaviour 
{
    public GameObject prefab;
    public GameObject instancia;
    public gameagain game;
    public bool servercomplete;
    public float intcontac;
    public float clearconsole;
    public bool instaserver;
    public bool lagserver;
    public bool initialserver;
    public effnetclient script;
    public  Process pross;
    private void OnEnable() 
    {
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Ruta a la carpeta "Documentos"
        string recyclingRushPath = System.IO.Path.Combine(documentsPath, "!Recycling Rush"); // Ruta a la carpeta "!Recycling Rush"
        string serverFolderPath = System.IO.Path.Combine(recyclingRushPath, "servers"); // Ruta a la carpeta "servers"
        string pythonFilePath = System.IO.Path.Combine(serverFolderPath, "servermodality2.py"); // Ruta al archivo Python
        Process pross = Process.Start("cmd.exe", $"/K cd /D \"{serverFolderPath}\" && python \"{pythonFilePath}\"");
        servercomplete = true;
        intcontac = Time.time +35f;
        clearconsole = Time.time + 25f;
        
    }
    private void OnDisable() 
    {
        Destroy(instancia);
        intcontac = Time.time +10f;
        servercomplete = true;
        lagserver = false;
        initialserver = false;
        instaserver = false;
        Process.Start(new ProcessStartInfo 
{ 
    FileName = "taskkill", 
    Arguments = "/im cmd.exe /f /t", 
    CreateNoWindow = true, 
    UseShellExecute = false 
}).WaitForExit();
    }
    private void Update() {
        if(servercomplete || !lagserver)
        {
            
            if(Time.time>=intcontac)
            {
                if(!instaserver)
                {
                    UnityEngine.Debug.Log("Hola pew");
                    instancia = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
                    script = instancia.GetComponent<effnetclient>();
                    instaserver = true;
                     initialserver = true;
                    game.Again();
                }
                
            }
        }
        else
        {
            Destroy(instancia);
            intcontac = Time.time +35f;
            servercomplete = true;
            lagserver = false;
             initialserver = false;
            instaserver = false;
        }
        if( initialserver)
        {
            if(Time.time >= clearconsole)
            {
                if(script.contant <= script.cont )
                {
                    script.contant = script.cont;
                    UnityEngine.Debug.Log("Cliente conectado");
                }
                else
                {
                    UnityEngine.Debug.Log("Servidor Caido Reconectando");
                    lagserver = true;
                    Clear();
                }
                clearconsole = Time.time +25f;
            }
        }
    }
    public static void Clear()
    {
        // Limpia la consola usando el comando "ClearLog"
        System.Type type = System.Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
        System.Reflection.MethodInfo method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

}
