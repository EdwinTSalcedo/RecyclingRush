using System.Collections;
using System.IO;
using UnityEngine;

public class Grabacion : MonoBehaviour
{
    public int numeroDeCamaras = 4; // Número de cámaras a almacenar
    public Camera[] camaras; // Array para almacenar las cámaras
    public float ritmoDeCaptura = 1.0f; // Ritmo de captura en segundos

    private int indiceCamaraActual = 0;
    private bool capturando = false;
    private int numeroDeFrames = 0;

    void Start()
    {
        // Inicializar el array de cámaras con el número especificado
        camaras = new Camera[numeroDeCamaras];

        // Obtener las cámaras y almacenarlas en el array
        for (int i = 0; i < numeroDeCamaras; i++)
        {
            camaras[i] = Camera.main; // Puedes ajustar esto para obtener las cámaras de otra manera
        }
    }

    void Update()
    {
        // Comenzar o detener la captura al presionar la tecla especificada (en este caso, la tecla 'C')
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!capturando)
            {
                StartCoroutine(CapturarSecuencia());
            }
            else
            {
                StopCoroutine(CapturarSecuencia());
            }
        }
    }

    IEnumerator CapturarSecuencia()
    {
        capturando = true;

        // Crear una carpeta para cada cámara en el directorio de Assets
        for (int i = 0; i < numeroDeCamaras; i++)
        {
            string nombreCarpeta = "Camara" + i.ToString();
            Directory.CreateDirectory(Application.dataPath + "/" + nombreCarpeta);
        }

        // Realizar la captura en bucle con el ritmo especificado
        while (capturando)
        {
            // Capturar imagen de la cámara actual
            CapturarImagen(camaras[indiceCamaraActual]);

            // Esperar el tiempo especificado antes de pasar a la siguiente cámara
            yield return new WaitForSeconds(ritmoDeCaptura);

            // Cambiar al siguiente índice de cámara (circular)
            indiceCamaraActual = (indiceCamaraActual + 1) % numeroDeCamaras;
        }
    }

    void CapturarImagen(Camera camara)
    {
        // Crear una textura temporal para almacenar la imagen de la cámara
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        camara.targetTexture = renderTexture;
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        
        // Renderizar la textura
        camara.Render();
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        camara.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // Convertir la textura a bytes y guardarla como archivo PNG
        byte[] bytes = tex.EncodeToPNG();
        string nombreCarpeta = "Camara" + indiceCamaraActual.ToString();
        string nombreArchivo = "Captura_" + numeroDeFrames.ToString("D5") + ".png"; // Añadir 0 a la izquierda para mantener el formato
        string rutaCompleta = Path.Combine(Application.dataPath, nombreCarpeta, nombreArchivo);
        File.WriteAllBytes(rutaCompleta, bytes);

        // Incrementar el número de frames
        numeroDeFrames++;

        Debug.Log("Captura realizada: " + rutaCompleta);
    }
}
