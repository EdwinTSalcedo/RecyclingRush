using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysOnTop : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<Renderer>().sortingOrder = 5000;
    }
}
