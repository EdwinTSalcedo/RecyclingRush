using System;
using UnityEngine;

public class ClearConsole : MonoBehaviour
{
    // Call this function to clear the console
    public static void Clear()
    {
        // Clear the console using the "ClearLog" command
        Type logEntriesType = Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
        
        if (logEntriesType != null)
        {
            System.Reflection.MethodInfo clearMethod = logEntriesType.GetMethod("Clear");
            
            if (clearMethod != null)
            {
                clearMethod.Invoke(new object(), null);
            }
        }
    }

    // You can call this function from a button or another event in your game
    private void Update()
    {
        Clear();
    }
}
