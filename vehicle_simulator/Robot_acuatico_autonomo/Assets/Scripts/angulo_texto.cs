using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class angulo_texto : MonoBehaviour
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
        textfield.text="Angle: "+angulo_bote.GetComponent<Movimiento>().anguloreal.ToString();
    }
}
