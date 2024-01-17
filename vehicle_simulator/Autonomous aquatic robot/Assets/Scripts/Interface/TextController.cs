using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    bool counter = false;  // Flag to control the counter state
    Text textField;  // Reference to the Text component
    int number;

    void Start()
    {
        textField = GetComponent<Text>();  // Get the Text component attached to the GameObject
        textField.text = "Record";  // Set the initial text to "Record"
    }

    // Update is called once per frame

    // Method to change the text displayed
    public void ChangeText()
    {
        // Toggle the counter state and update the text accordingly
        if (counter == true)
        {
            counter = false;
            textField.text = "Record";
        }
        else
        {
            counter = true;
            textField.text = "Stop";
        }
    }
}
