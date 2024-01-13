using UnityEngine;
using System.Threading.Tasks;
using System;

public class AutonomousMovement : MonoBehaviour
{
    private Rigidbody rb;  // Rigidbody component for physical interactions
    private bool stopped;   // Flag to check if movement is stopped
    public float targetAngle;  // Target angle for autonomous movement
    private bool rotating;  // Flag to check if the object is currently rotating
    private bool calculating;  // Flag to check if angle calculation is in progress
    public float anglePrediction;  // Predicted angle for autonomous movement
    public int modeSelection;   // Modality selection for movement behavior
    public float linearSpeed = 5f;  // Linear movement speed
    public float angularSpeed = 5f;   // Rotational movement speed
    public float minAngleRange = 140f;  // Minimum angle range for autonomous movement
    public float maxAngleRange = 150f;  // Maximum angle range for autonomous movement
    public float angularTolerance = 10f;   // Tolerance for angle comparisons
    public bool autonomous;   // Flag to indicate autonomous movement
    private float intelReset;
    private float desiredAngle;   // Desired angle for rotation
    private float angleRange;   // Angle range for autonomous movement
    private DateTime lastCallTime = DateTime.MinValue;   // Timestamp for the last method call
    private TimeSpan waitTime = TimeSpan.FromSeconds(5);   // Wait time for method calls
    private float waitInterval = 1.0f;   // Wait interval for method calls
    private float lastDetectionTime = 0.0f;   // Timestamp for the last collision detection
    private float changeTime = 0.0f;   // Modality change time
    private Vector3 movementDirection;   // Linear movement direction
    public modality1 intvalid1;
    public modality2 intvalid2;
    public modality3 intvalid3;
    public modality4 intvalid4;
    private bool mode4Active;
    private bool mode3Active;
    private bool mode2Active;
    private bool mode1Active;
    private bool objectCollision;
    public GameObject plane;
    public GameObject plane1;
    public GameObject plane2;
    public GameObject plane3;

    private void Update()
    {
        plane = GameObject.Find("Plane");
        plane1 = GameObject.Find("Plane (1)");
        plane2 = GameObject.Find("Plane (2)");
        plane3 = GameObject.Find("Plane (3)");
    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        stopped = false;
        rotating = false;
        calculating = false; // Added to initialize the variable
        autonomous = false; // Added to initialize the variable
        desiredAngle = 0; // Added to initialize the variable
        angleRange = 0; // Added to initialize the variable
        movementDirection = Vector3.zero; // Added to initialize the variable
        modeSelection = 0; // Added to initialize the variable
        targetAngle = 0; // Added to initialize the variable
        intelReset = Time.time + 25f;
    }

    private void FixedUpdate()
    {
        desiredAngle = transform.eulerAngles.y;
        if (!stopped)
        {
            switch (modeSelection)
            {
                case 0:
                    MoveInControlledDirection();
                    autonomous = false;
                    mode3Active = false;
                    mode1Active = false;
                    mode2Active = false;
                    break;
                case 1:
                    MoveInRandomDirection();

                    if (Time.time >= changeTime)
                    {
                        mode3Active = false;
                        mode2Active = false;
                        mode4Active = false;
                        autonomous = true;

                        if (!mode1Active)
                        {
                            intvalid1.enabled = true;
                            intvalid2.enabled = false;
                            intvalid3.enabled = false;
                            intvalid4.enabled = false;
                            mode1Active = true;
                        }

                        if (Mathf.Abs(desiredAngle - angleRange) <= angularTolerance)
                        {
                            calculating = false;
                        }
                        else
                        {
                            float rotationDirection = Mathf.Sign(desiredAngle - angleRange) * -1;
                            rb.angularVelocity = transform.up * rotationDirection * angularSpeed;
                        }
                    }
                    break;
                case 2:
                    MoveInRandomDirection();

                    if (Time.time >= changeTime)
                    {
                        mode3Active = false;
                        mode1Active = false;
                        mode4Active = false;
                        autonomous = true;

                        if (!mode2Active)
                        {
                            intvalid1.enabled = false;
                            intvalid2.enabled = true;
                            intvalid3.enabled = false;
                            intvalid4.enabled = false;
                            mode2Active = true;
                        }

                        if (Mathf.Abs(desiredAngle - angleRange) <= angularTolerance)
                        {
                            calculating = false;
                        }
                        else
                        {
                            float rotationDirection = Mathf.Sign(desiredAngle - angleRange) * -1;
                            rb.angularVelocity = transform.up * rotationDirection * angularSpeed;
                        }
                    }
                    break;
                case 3:
                    MoveInRandomDirection();

                    if (Time.time >= changeTime)
                    {
                        autonomous = true;
                        mode1Active = false;
                        mode2Active = false;
                        mode4Active = false;

                        if (!mode3Active)
                        {
                            intvalid1.enabled = false;
                            intvalid2.enabled = false;
                            intvalid3.enabled = true;
                            intvalid4.enabled = false;
                            mode3Active = true;
                        }

                        if (Mathf.Abs(desiredAngle - angleRange) <= angularTolerance)
                        {
                            calculating = false;
                        }
                        else
                        {
                            float rotationDirection = Mathf.Sign(desiredAngle - angleRange) * -1;
                            rb.angularVelocity = transform.up * rotationDirection * angularSpeed;
                        }
                    }
                    break;
                case 4:
                    MoveInRandomDirection();

                    if (Time.time >= changeTime)
                    {
                        autonomous = true;
                        mode1Active = false;
                        mode2Active = false;
                        mode3Active = false;

                        if (!mode4Active)
                        {
                            intvalid1.enabled = false;
                            intvalid2.enabled = false;
                            intvalid3.enabled = false;
                            intvalid4.enabled = true;
                            mode4Active = true;
                        }

                        if (Mathf.Abs(desiredAngle - angleRange) <= angularTolerance)
                        {
                            calculating = false;
                        }
                        else
                        {
                            float rotationDirection = Mathf.Sign(desiredAngle - angleRange) * -1;
                            rb.angularVelocity = transform.up * rotationDirection * angularSpeed;
                        }
                    }
                    break;
                default:
                    MoveInControlledDirection();
                    break;
            }
        }
        else
        {
            if (!objectCollision)
            {
                MoveInRandomDirection();
            }
            RotateToRandomAngle();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!stopped)
        {
            if (other.gameObject.CompareTag("limit"))
            {
                objectCollision = false;
                string objectName = other.gameObject.name;

                if (objectName == "Plane (1)")
                {
                    targetAngle = 0;
                    plane.GetComponent<BoxCollider>().enabled = false;
                    plane2.GetComponent<BoxCollider>().enabled = false;
                    plane3.GetComponent<BoxCollider>().enabled = false;
                }
                else if (objectName == "Plane (2)")
                {
                    targetAngle = 90;
                    plane1.GetComponent<BoxCollider>().enabled = false;
                    plane.GetComponent<BoxCollider>().enabled = false;
                    plane3.GetComponent<BoxCollider>().enabled = false;
                }
                else if (objectName == "Plane (3)")
                {
                    targetAngle = 180;
                    plane.GetComponent<BoxCollider>().enabled = false;
                    plane2.GetComponent<BoxCollider>().enabled = false;
                    plane1.GetComponent<BoxCollider>().enabled = false;
                }
                else if (objectName == "Plane")
                {
                    targetAngle = 270;
                    plane1.GetComponent<BoxCollider>().enabled = false;
                    plane2.GetComponent<BoxCollider>().enabled = false;
                    plane3.GetComponent<BoxCollider>().enabled = false;
                }

                if (Time.time - lastDetectionTime >= waitInterval)
                {
                    stopped = true;
                    lastDetectionTime = Time.time;
                }
            }
        }
        else
        {
            if (Time.time - lastDetectionTime >= waitInterval)
            {
                stopped = false;
                objectCollision = false;
                lastDetectionTime = Time.time;
            }
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (!stopped)
        {
            if (other.gameObject.CompareTag("object"))
            {
                if (Time.time - lastDetectionTime >= waitInterval)
                {
                    rb.velocity = transform.forward * -0.3f * linearSpeed;
                    stopped = true;
                    objectCollision = true;
                    lastDetectionTime = Time.time;
                }
            }
        }
        else
        {
            if (Time.time - lastDetectionTime >= waitInterval)
            {
                stopped = false;
                objectCollision = false;
                lastDetectionTime = Time.time;
            }
        }
    }

    private void MoveInControlledDirection()
    {
        float verticalMovement = Input.GetAxis("Vertical");
        float horizontalMovement = Input.GetAxis("Horizontal");

        movementDirection = transform.forward * verticalMovement;
        Vector3 rotationDirection = transform.up * horizontalMovement;

        rb.velocity = movementDirection * linearSpeed;
        rb.angularVelocity = rotationDirection * angularSpeed;
    }

    private void MoveInRandomDirection()
    {
        rb.velocity = transform.forward * linearSpeed;
    }

    private void RotateToRandomAngle()
    {
        float currentAngle = transform.eulerAngles.y;

        if (!rotating)
        {
            targetAngle = UnityEngine.Random.Range(minAngleRange, maxAngleRange);
            targetAngle += currentAngle;

            if (targetAngle > 360)
            {
                targetAngle -= 360;
            }

            rotating = true;
        }

        if (Mathf.Abs(currentAngle - targetAngle) <= angularTolerance)
        {
            rotating = false;
            stopped = false;
            plane.GetComponent<BoxCollider>().enabled = true;
            plane1.GetComponent<BoxCollider>().enabled = true;
            plane2.GetComponent<BoxCollider>().enabled = true;
            plane3.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            float rotationDirection = 1;
            rb.angularVelocity = transform.up * rotationDirection * angularSpeed;
        }
    }

    public void ChangeModality(int numberlist)
    {
        changeTime = Time.time + 15f;
        intvalid1.enabled = false;
        intvalid2.enabled = false;
        intvalid3.enabled = false;
        intvalid4.enabled = false;
        modeSelection = numberlist;
    }

    public void RotateToAutonomousAngle(float drivingAngle)
    {
        anglePrediction = drivingAngle;

        if (autonomous)
        {
            angleRange = desiredAngle + drivingAngle;

            if (angleRange > 360)
            {
                angleRange -= 360;
            }

            if (angleRange < -360)
            {
                angleRange += 360;
            }
        }
    }
}
