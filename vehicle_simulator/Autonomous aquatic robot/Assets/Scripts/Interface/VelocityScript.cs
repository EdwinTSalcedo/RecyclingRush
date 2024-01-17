using UnityEngine;
using UnityEngine.UI;

public class VelocityScript : MonoBehaviour
{
    // Reference to the Text component
    private Text textField;

    // Reference to the GameObject controlling the velocity
    public GameObject boatMovement;

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
        UpdateVelocityText(0f);
    }

    // Called once per frame
    private void Update()
    {
        // Update the text to display the current velocity value
        UpdateVelocityText(boatMovement.GetComponent<Movement>().speedUI);
    }

    // Updates the text with the provided velocity value
    private void UpdateVelocityText(float velocity)
    {
        textField.text = "Velocity: " + velocity.ToString("F2");
    }
}
