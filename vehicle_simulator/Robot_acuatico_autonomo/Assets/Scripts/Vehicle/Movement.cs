using UnityEngine;

public class Movement : MonoBehaviour
{
    // Reference to the camera GameObject
    public GameObject cameraObject;

    // Reference to the angle GameObject
    public GameObject angleObject;

    // Linear acceleration for forward/backward movement
    public float acceleration = 50f;

    // Angular acceleration for turning
    public float angularAcceleration = 5f;

    // Current linear acceleration
    public float currentAcceleration = 0f;

    // Current angular acceleration
    public float currentAngularAcceleration = 0f;

    // Rigidbody component attached to the main GameObject
    public Rigidbody rb;

    // Rigidbody component attached to the angle GameObject
    public Rigidbody angleRb;

    // Input value for horizontal movement (-1 to 1)
    public int horizontalMovement = 0;

    // Input value for vertical movement (-1 to 1)
    public float verticalMovement = 0;

    // Current orientation angle in degrees
    public float currentAngle = 0;

    // Current speed magnitude
    public float currentSpeed = 0;

    // Angle value displayed in the user interface
    public float angleUI = 0;

    // Multiplier for turning rate
    public float turnRateMultiplier;

    // Speed value displayed in the user interface
    public float speedUI = 0;

    // Region variable (unused in the provided code)
    public int region;

    // Variables to store x and z values (unused in the provided code)
    private int xValue;
    private int zValue;

    // Strings to store angle and speed values for transmission
    public string angleToSend;
    public string speedToSend;

    // Called on the frame when a script is enabled
    private void Start()
    {
        // Get Rigidbody components
        rb = GetComponent<Rigidbody>();
        angleRb = angleObject.GetComponent<Rigidbody>();
    }

    // Called every fixed frame-rate frame
    private void FixedUpdate()
    {
        // Get the current rotation of the angle GameObject
        Vector3 currentRotation = angleObject.transform.localEulerAngles;

        // Get input values for vertical and horizontal movement
        verticalMovement = Input.GetAxis("Vertical");
        horizontalMovement = (int)Input.GetAxis("Horizontal");

        // Calculate linear acceleration based on input
        currentAcceleration = acceleration * verticalMovement;

        // Apply forward/backward force to the main GameObject
        rb.AddForce((transform.position - cameraObject.transform.position) * currentAcceleration);

        // Check if the angle is within a specific range for additional turning acceleration
        if (angleObject.transform.localEulerAngles.y < 24 || angleObject.transform.localEulerAngles.y > 336)
        {
            if (horizontalMovement != 0)
            {
                // Increment the turnRateMultiplier for additional turning acceleration
                turnRateMultiplier += 0.01f;
                currentAngle = angleObject.transform.localEulerAngles.y;
            }
        }

        // Calculate angular acceleration based on input and turnRateMultiplier
        currentAngularAcceleration = angularAcceleration * horizontalMovement * turnRateMultiplier;

        // Apply torque to the main GameObject and the angle GameObject
        rb.AddTorque(Vector3.up * currentAngularAcceleration * Time.deltaTime);
        angleRb.AddTorque(Vector3.up * currentAngularAcceleration * 0.21f * Time.deltaTime);

        // Calculate the current speed magnitude
        currentSpeed = rb.velocity.magnitude;

        // Adjust the angle to a specific range
        if (currentAngle <= 360 && currentAngle > 200)
        {
            currentAngle = -360 + currentAngle;
        }

        // Ensure the speed is positive
        currentSpeed = Mathf.Abs(currentSpeed);

        // Round angle and speed values for UI display
        angleUI = Mathf.Round(currentAngle * 100f) / 100f;
        currentAngle = Mathf.Round(currentAngle * 10000f) / 10000f;
        currentSpeed = Mathf.Round(currentSpeed * 10000f) / 10000f;
        speedUI = Mathf.Round(currentSpeed * 100f) / 100f;

        // Convert angle value to a string for transmission
        angleToSend = currentAngle.ToString();

        // Handle special cases for positive, zero, and negative angles
        if (currentAngle > 0)
            angleToSend = angleToSend.Insert(0, "p");
        else if (currentAngle == 0)
            angleToSend = angleToSend.Insert(0, "z");
        else
            angleToSend = angleToSend.Replace("-", "n");

        // Replace decimal separator with a character for transmission
        if (angleToSend.Contains(","))
            angleToSend = angleToSend.Replace(",", "c");

        // Convert speed value to a string for transmission
        speedToSend = currentSpeed.ToString();

        // Handle special case for zero speed
        if (currentSpeed == 0)
            speedToSend = speedToSend.Insert(0, "z");

        // Replace decimal separator with a character for transmission
        if (speedToSend.Contains(","))
            speedToSend = speedToSend.Replace(",", "c");

        // Handle specific cases when no horizontal movement is detected
        if (horizontalMovement == 0)
        {
            // Adjust the angle and turnRateMultiplier values
            if (currentAngle > 0.9f)
                currentAngle -= 0.3f;
            else if (currentAngle < -0.9f)
                currentAngle += 0.3f;
            else
                currentAngle = 0;

            // Decrease the turnRateMultiplier if it's greater than 0
            if (turnRateMultiplier > 0)
                turnRateMultiplier -= 0.05f;
            else
                turnRateMultiplier = 0;

            // Reset the angle if A or D keys are pressed
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                angleObject.transform.localEulerAngles = new Vector3(currentRotation.x, 0, currentRotation.z);
                currentAngle = 0;
            }
            else
            {
                // Set the angle based on the adjusted currentAngle value
                angleObject.transform.localEulerAngles = new Vector3(currentRotation.x, currentAngle, currentRotation.z);
            }
        }
    }
}
