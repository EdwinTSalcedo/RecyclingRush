using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using System.Threading;
public class giro_camara : MonoBehaviour
{
    public Transform camaraTransform;
    public float velocidadGiro = 5f;
    private float anguloActual = 0f;
    private float anguloMinimo = -45f;
    private float anguloMaximo = 45f;


    public void CambiarATipo(int tipo)
    {
        if(tipo == 0)
        {
            velocidadGiro = Mathf.Abs(velocidadGiro);
        }
        else
        {
            velocidadGiro = -Mathf.Abs(velocidadGiro);
        }
        anguloActual += velocidadGiro * Time.deltaTime;
        anguloActual = Mathf.Clamp(anguloActual, anguloMinimo, anguloMaximo);    
        camaraTransform.localRotation = Quaternion.Euler(anguloActual, 0f, 0f);
    }

}
