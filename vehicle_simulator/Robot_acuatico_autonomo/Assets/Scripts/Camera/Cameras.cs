using UnityEngine;

public class Cameras : MonoBehaviour
{
    // Reference to the UI canvas
    public GameObject canvas;

    // Variables to load different cameras for observing the garbage collector
    public Camera spectatorCamera;
    public Camera pathCamera;
    public Camera sceneCamera;

    // Array to store the camera variables
    private Camera[] cameras;

    // Variable to store the currently selected camera
    private Camera currentCamera;

    // Variable to store the current camera index
    public int currentCameraIndex = 0;

    // Operations performed when the script is enabled
    private void OnEnable()
    {
        // Store the cameras in an array
        cameras = new Camera[] { pathCamera, spectatorCamera, sceneCamera };

        // Set the default camera
        currentCamera = pathCamera;
        spectatorCamera.enabled = false;

        // Switch the camera
        SwitchCamera();
    }

    // Operations performed every frame
    public void NextCamera()
    {
        currentCameraIndex++;

        // Wrap around to the first camera if index goes beyond the array length
        if (currentCameraIndex > cameras.Length - 1)
        {
            currentCameraIndex = 0;
        }

        // Switch the camera
        SwitchCamera();
    }

    // Switches the current camera
    private void SwitchCamera()
    {
        // Disable the current camera
        currentCamera.enabled = false;

        // Set the current camera to the selected camera
        currentCamera = cameras[currentCameraIndex];

        // Enable the new current camera
        currentCamera.enabled = true;
    }
}
