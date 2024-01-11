using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rest : MonoBehaviour
{
    public bool point=false;
    public depthclient clientmont;
    public float nextTime;
    void Update()
    {
        if(point)
        {
            clientmont.enabled = false;
            nextTime = Time.time + 6;
            point=false;
            
        }
        if (Time.time >= nextTime)
        {
            //clientmont.enabled = true;
        }
    }
}
