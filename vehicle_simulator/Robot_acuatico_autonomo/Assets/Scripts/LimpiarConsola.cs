using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimpiarConsola: MonoBehaviour
{
    // Llama a esta función para limpiar la consola
    public static void Clear()
    {
        // Limpia la consola usando el comando "ClearLog"
        System.Type type = System.Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
        System.Reflection.MethodInfo method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

    // Puedes llamar a esta función desde un botón u otro evento en tu juego
    public void Update()
    {
        Clear();
    }
}
