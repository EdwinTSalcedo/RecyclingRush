using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;
using System;
public class time : MonoBehaviour
{
    public contador_lenteja cont;
    public int conta;
    public int conta1;
    public int conta2;
    public int conta3;
    public int conta4;
    public Transform objeto1; // El primer objeto
    public Transform objeto2; // El segundo objeto
    public Text textoCronometro;
    public float tiempoTranscurrido = 0f;
    string basePath;
    string momentumFolder;
    public Movimiento_autonomo movauto;
    
    private void Start() {
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Ruta a la carpeta "Documentos"
        string recyclingRushPath = System.IO.Path.Combine(documentsPath, "!Recycling Rush"); // Ruta a la carpeta "!Recycling Rush"
        basePath = System.IO.Path.Combine(recyclingRushPath, "servers"); // Ruta a la carpeta "servers"
        Time.timeScale = 1.0f;
    }
    public void aceleration (float acel)
    {
        Time.timeScale += acel ;
    }

    void Update()
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
            // Actualiza el tiempo transcurrido según la escala de tiempo actual.
            tiempoTranscurrido += Time.deltaTime * Time.timeScale;

            // Convierte el tiempo transcurrido a formato de tiempo (hh:mm:ss).
            int horas = Mathf.FloorToInt(tiempoTranscurrido / 3600);
            int minutos = Mathf.FloorToInt((tiempoTranscurrido % 3600) / 60);
            int segundos = Mathf.FloorToInt(tiempoTranscurrido % 60);
            // Calcula la distancia en el eje X
            float distanciaX = Mathf.Max(0, Mathf.Abs(objeto1.position.x - objeto2.position.x) - 0.7f);

            // Calcula la distancia en el eje Y y asegúrate de que los valores negativos sean cero
            float distanciaY = Mathf.Max(0, Mathf.Abs(objeto1.position.z - objeto2.position.z - 0.8f));
            // Muestra el tiempo en formato de tiempo en el TextMeshPro.
            string texto = string.Format("Time: {0:D2}:{1:D2}:{2:D2}\nVelocity: {3:F2}x\nAngle: {4:F2}\nX: {5:F2} Y: {6:F2}", horas, minutos, segundos, Time.timeScale, movauto.angle_predict, distanciaX, distanciaY);
            textoCronometro.text = texto;
            if(minutos ==20 && segundos>0)
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
                string rutaArchivo =  momentumFolder+ "/time"+conta.ToString()+".txt";
                File.WriteAllText(rutaArchivo, texto);
                Debug.Log("Valor guardado en el archivo: " + texto);
                cont.finishtime=true;
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
            if(cont.cantidadDeParticulas<30 && minutos >2)
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
                string rutaArchivo =  momentumFolder + "/time"+conta.ToString()+".txt";
                File.WriteAllText(rutaArchivo, texto);
                Debug.Log("Valor guardado en el archivo: " + texto);
                cont.cnt=true;
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
