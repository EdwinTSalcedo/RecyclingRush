using UnityEngine;
using UnityEngine.UI;

public class MoverImagen : MonoBehaviour
{
    public RectTransform imagenRectTransform; // Arrastra la RectTransform de tu imagen aquí desde el Inspector
    public float velocidadMovimiento = 100.0f; // Velocidad de movimiento
    
    private void Update()
    {
        // Obtén la posición actual de la imagen
        Vector3 posicionActual = imagenRectTransform.anchoredPosition;

        // Calcula la nueva posición en función de la velocidad y el tiempo transcurrido
        float movimientoHorizontal = Input.GetAxis("Horizontal"); // Puedes cambiar esto según tus necesidades
        float desplazamiento = movimientoHorizontal * velocidadMovimiento * Time.deltaTime;

        // Actualiza la posición horizontal de la imagen
        posicionActual.x += desplazamiento;

        // Limita la posición a un rango específico si es necesario
        // Ejemplo: posicionActual.x = Mathf.Clamp(posicionActual.x, minX, maxX);

        // Asigna la nueva posición a la imagen
        imagenRectTransform.anchoredPosition = posicionActual;
    }
}
