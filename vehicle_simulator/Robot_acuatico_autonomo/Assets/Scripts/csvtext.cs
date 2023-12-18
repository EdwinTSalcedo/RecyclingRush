using UnityEngine;
using System.Collections;
using System.IO;

public class csvtext : MonoBehaviour
{
    // Variables con datos
    float timestampStart;
    string timestampEnd;
    int collectedDuckweed;
    float missingDuckweed;
    public contador_lenteja contlen;
    float waitTime = 600f;
    bool loadCSV = false;
    string csvFilePath;
    string[] headers = { "timestamp_start", "timestamp_end", "collected_duckweed", "missing_duckweed" };

       
    void Start()
    {
        // Ruta donde se guardará el archivo CSV (en la carpeta "Assets" por defecto)
        csvFilePath = "Assets/csvtext.csv";
        // Contenido de las columnas
         

        // Crear o sobrescribir el archivo CSV
        
    }
    public void timestart()
    {
        timestampStart = Time.deltaTime;
        loadCSV=true;
    }
    void Update()
    {
        if(loadCSV)
        {
            // Calcula el tiempo transcurrido desde que comenzó la espera
            float elapsedTime = Time.deltaTime - timestampStart;
            if (elapsedTime >= waitTime)
            {
                using (StreamWriter sw = new StreamWriter(csvFilePath))
                {
                    timestampEnd = Time.deltaTime.ToString();
                    sw.WriteLine(string.Join(",", headers));

                    // Construir y escribir la fila de datos
                    string[] dataRow = { timestampStart.ToString(), timestampEnd, contlen.duckweed.ToString() , contlen.cantidadDeParticulas.ToString() };
                    sw.WriteLine(string.Join(",", dataRow));
                }

                Debug.Log("Archivo CSV generado en " + csvFilePath);
            }
        }
    }
}