using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    // Reference to the camera's transform
    public Transform cameraTransform;

    // Rotation speed for the camera
    public float rotationSpeed = 5f;

    // Current angle of the camera rotation
    private float currentAngle = 0f;

    // Minimum and maximum angles for camera rotation
    private float minAngle = -45f;
    private float maxAngle = 45f;

    // Change the rotation type based on the provided type parameter
    public void ChangeRotationType(int type)
    {
        // Set the rotation speed based on the type (0 or 1)
        rotationSpeed = Mathf.Abs(rotationSpeed) * (type == 0 ? 1 : -1);

        // Update the current angle based on the rotation speed and time
        currentAngle += rotationSpeed * Time.deltaTime;

        // Clamp the current angle within the specified range
        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

        // Apply the new rotation to the camera transform
        cameraTransform.localRotation = Quaternion.Euler(currentAngle, 0f, 0f);
    }
}
