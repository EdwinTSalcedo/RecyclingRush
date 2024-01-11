using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameagain : MonoBehaviour
{   
    // Update is called once per frame
    public GameObject bote;
    public GameObject duckweed;
    public GameObject timeagain;
    public void Again()
    {
        bote.transform.position = new Vector3 (-22.73f,0.02f,132.43f);
        bote.transform.rotation = Quaternion.Euler(Vector3.zero);
        ParticleSystem particleSystem =duckweed.GetComponent<ParticleSystem>();
   
        contador_lenteja countduck = duckweed.GetComponent<contador_lenteja>();
        time timescript= timeagain.GetComponent<time>();
        countduck.duckweed = -1;
        if(countduck.duckweed==0)
        {
            countduck.duckweed = 0;
        }
        Time.timeScale = 1.0f;
        particleSystem.Stop();
        particleSystem.Clear();
        particleSystem.Play();
        timescript.tiempoTranscurrido = 0f;
    }
}
