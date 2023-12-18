using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class MiScript : MonoBehaviour
{
    // Este código se ejecuta tanto en el modo de edición como en el modo de juego
    void Start()
    {
        foreach (GameObject o in Object.FindObjectsOfType<GameObject> ()) {
  o.SetActive (false);
}

    }
}