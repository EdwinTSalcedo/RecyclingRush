using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class texto : MonoBehaviour
{
    bool contador=false;
    Text textfield;
    int number;
    void Start()
    {
        textfield=GetComponent<Text>();
        textfield.text="Record";
    }

    // Update is called once per frame
    public void changetext()
    {
        if(contador==true)
        {
            contador=false;
            textfield.text="Record";
        }
        else
        {
            contador=true;
            textfield.text="Stop";
        }
    }
}
