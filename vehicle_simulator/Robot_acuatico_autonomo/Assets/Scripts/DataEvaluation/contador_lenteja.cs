using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
public class contador_lenteja : MonoBehaviour
{
    public gameagain game;
    public int conta;
    public int conta1;
    public int conta2;
    public int conta3;
    public int conta4;
    public bool finishtime;
    public Text textoRestante;
    public Text textoContador;
    public bool cnt;
    public string texto;
    public int duckweed=0;
    string basePath;
    string momentumFolder;
    public float intervalconnnect;
    public Movimiento_autonomo movauto;
    public modality3 intser;
    /* Este script está dentro del sistema de partículas que genera las lentejas de agua.
       Solo se activa cuando hay una colisión con una de las partículas.
       Al detectarlo, se manda una orden al script del bote, que aumenta en 1
       el contador de las lentejas destruidas.
    */
    public ParticleSystem sistemaDeParticulas;  // Asigna el sistema de partículas en el Inspector
    public int cantidadDeParticulas;
    private void Start() {
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Ruta a la carpeta "Documentos"
        string recyclingRushPath = System.IO.Path.Combine(documentsPath, "!Recycling Rush"); // Ruta a la carpeta "!Recycling Rush"
        basePath = System.IO.Path.Combine(recyclingRushPath, "servers"); // Ruta a la carpeta "servers"
    }
    private void Update()
    {
            switch (movauto.elec) 
            {
                case 1:
                    momentumFolder = System.IO.Path.Combine (basePath, "Modality1");
                    break;
                case 2:
                    momentumFolder = System.IO.Path.Combine (basePath, "Modality2");
                    break;
                case 3:
                    momentumFolder = System.IO.Path.Combine (basePath, "Modality3");
                    break;
                case 4:
                    momentumFolder = System.IO.Path.Combine (basePath, "Modality4");
                    break;
                default:
                    break;
            }
        sistemaDeParticulas = GetComponent<ParticleSystem>();
        cantidadDeParticulas = sistemaDeParticulas.particleCount;
        texto = "Existing duckweed: " + cantidadDeParticulas.ToString();
        textoRestante.text = texto;
        texto = "Picked duckweed: " +  (sistemaDeParticulas.main.maxParticles-cantidadDeParticulas).ToString();
        textoContador.text = texto;
        if(finishtime)
        {
            switch (movauto.elec) 
            {
                case 1:
                    conta=conta1;
                    break;
                case 2:
                    conta=conta2;
                    break;
                case 3:
                    conta=conta3;
                    break;
                case 4:
                    conta=conta4;
                    break;
                default:
                    break;
            }
            string rutaArchivo = momentumFolder + "/lenteja"+conta.ToString()+".txt";
            File.WriteAllText(rutaArchivo, texto);
            Debug.Log("Valor guardado en el archivo: " + texto);
            finishtime=false;
            intser.servercomplete = false;
            game.Again();
            switch (movauto.elec) 
            {
                case 1:
                    conta1++;
                    break;
                case 2:
                    conta2++;
                    break;
                case 3:
                    conta3++;
                    break;
                case 4:
                    conta4++;
                    break;
                default:
                    break;
            }
        }
        else if(cnt)
        {
            switch (movauto.elec) 
            {
                case 1:
                    conta=conta1;
                    break;
                case 2:
                    conta=conta2;
                    break;
                case 3:
                    conta=conta3;
                    break;
                case 4:
                    conta=conta4;
                    break;
                default:
                    break;
            }
            string rutaArchivo = momentumFolder + "/lenteja"+conta.ToString()+".txt";
            File.WriteAllText(rutaArchivo, texto);
            Debug.Log("Valor guardado en el archivo: " + texto);
            cnt=false;
            intser.servercomplete = false;
            game.Again();
            
            switch (movauto.elec) 
            {
                case 1:
                    conta1++;
                    break;
                case 2:
                    conta2++;
                    break;
                case 3:
                    conta3++;
                    break;
                case 4:
                    conta4++;
                    break;
                default:
                    break;
            }
        }
    }
}
