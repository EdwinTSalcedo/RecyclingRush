using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class contador_lenteja : MonoBehaviour
{
    /*Este script esta dentro del sistema de particulas que genera las lentejas de agua
    Solo se activa cuando hay una colision con una de las particulas
    al detectarlo, se manda una orden al script del bote, que aumenta en 1
    el contador de las lentejas destruirdas
    */
    private void OnParticleCollision(GameObject jugador)
    {
        if(jugador.tag=="bote")
        {
            jugador.GetComponent<contador>().conta+=1;
        }
    }
}
