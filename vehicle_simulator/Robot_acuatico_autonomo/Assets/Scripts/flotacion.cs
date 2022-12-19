using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flotacion : MonoBehaviour
{
    /*Este script esta activo en el gameObject de "bote", dentro de cada elemento
    hijo del mismo, que actuan para flotar en ciertos puntos del bote.
    Este script se encarga de regular el nivel que se sumerge el objeto
    */
    //Esta variable se encarga de consultar el rigidbody del bote
    public GameObject Objeto;
    //Esta variable se encarga de definir el gameobject que actuara como el agua
    public GameObject agua;
    //Esta variable se encarga de definir el nivel de flotacion del agua, el cual
    //será el mismo nivel que el gamObject del agua
    public float nivelagua;
    //Esta variable se encarga de regular el crecimiento de la fuerza de empuje.
    public float umbralagua;
    //Esta variable se encarga de definir la velocidad  del cambio de direccion de la fuerza de empuje
    //Si la misma es muy alta, actua como una cama elastica.
    public float densidadagua;
    //Esta variable se encarga de agregar una fuerza extra a la fuerza de la gravedad
    public float fuerzaabajo;
    //Esta variable se trata de la fuerza de empuje
    float fuerzafactor;
    //Esta variable se encarga de la fuerza total que se aplica al objeto
    public Vector3 fuerzafloat;
    private void Start()
    {
        nivelagua=agua.transform.position.y;
    }
    private void FixedUpdate()
    {
        fuerzafactor=1.0f-((transform.position.y-nivelagua)/umbralagua);
        if(fuerzafactor>1.0f)
        {
            fuerzafloat=-Physics.gravity*(fuerzafactor-Objeto.GetComponent<Rigidbody>().velocity.y*densidadagua);
            fuerzafloat+=new Vector3(0.0f,-fuerzaabajo,0.0f);
            Objeto.GetComponent<Rigidbody>().AddForceAtPosition(fuerzafloat,transform.position);
        }
    }
}
