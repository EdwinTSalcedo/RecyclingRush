using System;
using System.IO;
using UnityEngine;

public class ImageCapture : MonoBehaviour
{
    // Cooldown time in milliseconds between captures
    public float cooldown = 5000;

    // Reference to the camera used for image capture
    public Camera pathCamera;

    // Reference to the boat GameObject
    public GameObject boat;

    // Flag to indicate whether image capture is enabled
    private bool captureEnabled = false;

    // Counter for naming captured images
    private int fileCounter = 0;

    // Timer to control the cooldown between captures
    private float cooldownTimer = 0f;

    // Folder path to store captured images
    private string timeFolderPath;

    // Array to store image data in bytes
    private byte[] bytes;

    // Called every fixed frame-rate frame
    private void Update()
    {
        // Check if capture is enabled and the boat is moving
        if (captureEnabled && (boat.GetComponent<Movement>().horizontalMovement != 0 || boat.GetComponent<Movement>().verticalMovement != 0))
        {
            // Increment the cooldown timer
            cooldownTimer += Time.deltaTime * 1000f;

            // Check if cooldown time has elapsed
            if (cooldownTimer >= cooldown)
            {
                // Reset the cooldown timer
                cooldownTimer = 0f;

                // Capture the current frame as an image
                CaptureImage();
            }
        }
    }
    // Captures the current frame as an image
    private void CaptureImage()
    {
        // Get the screen dimensions
        int screenWidth = pathCamera.pixelWidth;
        int screenHeight = pathCamera.pixelHeight;

        // Create a render texture for capturing the camera view
        RenderTexture renderTexture = new RenderTexture(screenWidth, screenHeight, 24);
        // Create a Texture2D to read the pixels from the render texture
        Texture2D image = new Texture2D(screenWidth, screenHeight, TextureFormat.RGB24, false);
        pathCamera.targetTexture = renderTexture;
        pathCamera.Render();
        RenderTexture.active = renderTexture;
       

        // Set the active render texture and encode the image to PNG format
        image.ReadPixels(new Rect(0, 0, pathCamera.targetTexture.width, pathCamera.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = renderTexture;
        bytes = image.EncodeToPNG();
        Destroy(image);

        // Generate a unique name for the image based on boat properties
        string imageName = $"Image{fileCounter}angle{boat.GetComponent<Movement>().angleToSend}velocity{boat.GetComponent<Movement>().speedToSend}.png";

        // Combine the time folder path and image name to create the full image path
        string imagePath = Path.Combine(timeFolderPath, imageName);

        // Write the image data to a file
        File.WriteAllBytes(imagePath, bytes);

        // Increment the file counter for the next image
        fileCounter++;
    }

    // Toggles image capture on/off
    public void ToggleCapture()
    {
        // If capture is currently enabled, disable it and reset the file counter
        if (captureEnabled)
        {
            captureEnabled = false;
            fileCounter = 0;
        }
        // If capture is currently disabled, enable it and create the necessary directories
        else
        {
            timeFolderPath = CreateDirectories();
            captureEnabled = true;
        }
    }
    

    // Creates the necessary directories for storing captured images
    private string CreateDirectories()
    {
        // Get the current time in a specific format as a string
        string time = DateTime.UtcNow.ToLocalTime().ToString("dd_MM_yyyy_HH_mm_ss");

        // Get the path to the Documents folder
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // Define paths for the Recycling Rush and Self-Driving folders
        string recyclingRushPath = Path.Combine(documentsPath, "!Recycling Rush");
        string selfDrivingPath = Path.Combine(recyclingRushPath, "Self-Driving");

        // Check if the Recycling Rush folder exists, and create it if not
        if (!Directory.Exists(recyclingRushPath))
        {
            Directory.CreateDirectory(recyclingRushPath);
        }

        // Check if the Self-Driving folder exists, and create it if not
        if (!Directory.Exists(selfDrivingPath))
        {
            Directory.CreateDirectory(selfDrivingPath);
        }

        // Combine the Self-Driving path with the current time to create a unique folder
        string timeFolderPath = Path.Combine(selfDrivingPath, time);

        // Create the time folder
        Directory.CreateDirectory(timeFolderPath);

        // Return the full path to the time folder
        return timeFolderPath;
    }
}
