using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Velocity_script : MonoBehaviour
{
    Text textfield;
    public GameObject angulo_bote;

    void Start()
    {
        textfield=GetComponent<Text>();
        textfield.text="0.00";
    }

    private void Update()
    {
        textfield.text="Velocity: "+angulo_bote.GetComponent<Movimiento>().velocidadreal.ToString();
    }
}
