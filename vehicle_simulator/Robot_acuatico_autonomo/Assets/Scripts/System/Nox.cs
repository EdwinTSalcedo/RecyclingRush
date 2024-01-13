using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Nox : MonoBehaviour
{
    // This code runs in both edit mode and play mode
    void Start()
    {
        foreach (GameObject obj in Object.FindObjectsOfType<GameObject>())
        {
            obj.SetActive(false);
        }
    }
}
