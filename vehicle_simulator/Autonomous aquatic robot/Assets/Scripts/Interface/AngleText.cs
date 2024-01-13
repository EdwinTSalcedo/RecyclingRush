using UnityEngine;
using UnityEngine.UI;

public class AngleText : MonoBehaviour
{
    // Reference to the Text component
    private Text textField;

    // Reference to the GameObject controlling the angle
    public GameObject boatAngle;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Cache the Text component reference during Awake for efficiency
        textField = GetComponent<Text>();
    }

    // Called on the frame when a script is enabled
    private void Start()
    {
        // Set the initial text value to "0.00"
        UpdateAngleText(0f);
    }

    // Called once per frame
    private void Update()
    {
        // Update the text to display the current angle value
        UpdateAngleText(boatAngle.GetComponent<Movement>().angleUI);
    }

    // Updates the text with the provided angle value
    private void UpdateAngleText(float angle)
    {
        textField.text = "Angle: " + angle.ToString("F2");
    }
}
