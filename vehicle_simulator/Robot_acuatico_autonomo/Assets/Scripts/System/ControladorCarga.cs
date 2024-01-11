using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class ControladorCarga : MonoBehaviour
{
    public string escenaACargar;
    public Image panelCarga;
    public RectTransform cuadradoMovible; // Agrega la referencia al cuadrado movible en el Inspector.
    public float velocidadMovimiento = 100f; // Ajusta la velocidad de movimiento según tus necesidades.

    private bool moverDerecha = true;

    private void Start()
    {
        StartCoroutine(CargarEscenaAsincrona());
        StartCoroutine(MoverCuadrado());
    }

    IEnumerator CargarEscenaAsincrona()
    {
        AsyncOperation cargaOperacion = SceneManager.LoadSceneAsync(escenaACargar);
        cargaOperacion.allowSceneActivation = false;

        while (!cargaOperacion.isDone)
        {
            float progreso = Mathf.Clamp01(cargaOperacion.progress / 0.9f);
            panelCarga.fillAmount = progreso;
            if (progreso >= 0.9f)
            {
                cargaOperacion.allowSceneActivation = true; // Cuando la carga está casi completa, activa la escena.
            }

            yield return null;
        }
    }

    IEnumerator MoverCuadrado()
    {
        while (true) // Un bucle infinito para mover continuamente el cuadrado.
        {
            // Mueve el cuadrado de izquierda a derecha.
            float movimiento = velocidadMovimiento * Time.deltaTime;

            if (moverDerecha)
            {
                cuadradoMovible.anchoredPosition += new Vector2(movimiento, 0f);

                // Si el cuadrado alcanza cierta posición, cambia la dirección.
                if (cuadradoMovible.anchoredPosition.x >= 200f) // Ajusta el valor según tus necesidades.
                {
                    moverDerecha = false;
                }
            }
            else
            {
                cuadradoMovible.anchoredPosition -= new Vector2(movimiento, 0f);

                // Si el cuadrado regresa a su posición inicial, cambia la dirección.
                if (cuadradoMovible.anchoredPosition.x <= 0f)
                {
                    moverDerecha = true;
                }
            }

            yield return null;
        }
    }
}
