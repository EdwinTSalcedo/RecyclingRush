using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;
using System;

public class TimeManager : MonoBehaviour
{
    public DuckweedCounter particleCounter; // Reference to the particle counter
    public int count;
    public int count1;
    public int count2;
    public int count3;
    public int count4;
    public Transform object1; // The first object
    public Transform object2; // The second object
    public Text timerText; // Text for displaying the timer
    public float elapsedTimer = 0f; // Elapsed time counter
    string basePath;
    string momentumFolder;
    public AutonomousMovement autonomousMovement;

    private void Start()
    {
        // Set up the base path for file operations
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string recyclingRushPath = Path.Combine(documentsPath, "!Recycling Rush");
        basePath = Path.Combine(recyclingRushPath, "servers");
        Time.timeScale = 1.0f; // Set the time scale to normal
    }

    // Function to adjust the time scale
    public void AdjustTimeScale(float acceleration)
    {
        if(Time.timeScale >= 0.25f && acceleration < 0.0f)
        {
            Time.timeScale += acceleration;
        }
        else if(Time.timeScale <= 9.75f && acceleration > 0.0f)
        {
            Time.timeScale += acceleration;
        }
    }

    void Update()
    {
        // Set the momentum folder based on the selected modality
        switch (autonomousMovement.modeSelection)
        {
            case 1:
                momentumFolder = Path.Combine(basePath, "Modality1");
                break;
            case 2:
                momentumFolder = Path.Combine(basePath, "Modality2");
                break;
            case 3:
                momentumFolder = Path.Combine(basePath, "Modality3");
                break;
            case 4:
                momentumFolder = Path.Combine(basePath, "Modality4");
                break;
            default:
                Debug.Log("Manual Mode");
                break;
        }

        // Update the elapsed time based on the current time scale.
        elapsedTimer += Time.deltaTime * Time.timeScale;

        // Convert elapsed time to time format (hh:mm:ss).
        int hours = Mathf.FloorToInt(elapsedTimer / 3600);
        int minutes = Mathf.FloorToInt((elapsedTimer % 3600) / 60);
        int seconds = Mathf.FloorToInt(elapsedTimer % 60);

        // Calculate the distance on the X-axis
        float distanceX = Mathf.Max(0, Mathf.Abs(object1.position.x - object2.position.x) - 0.7f);

        // Calculate the distance on the Y-axis and ensure negative values are zero
        float distanceY = Mathf.Max(0, Mathf.Abs(object1.position.z - object2.position.z - 0.8f));

        // Display time information in TextMeshPro
        string text = string.Format("Time: {0:D2}:{1:D2}:{2:D2}\nVelocity: {3:F2}x\nAngle: {4:F2}\nX: {5:F2} Y: {6:F2}", hours, minutes, seconds, Time.timeScale, autonomousMovement.anglePrediction, distanceX, distanceY);
        timerText.text = text;

        // Check if conditions for saving data are met
        if (minutes == 20 && seconds > 0)
        {
            SaveData();
            particleCounter.finishTime = true;
            UpdateCount();
        }

        if (particleCounter.particleCount < 30 && minutes > 2)
        {
            SaveData();
            particleCounter.saveData = true;
            UpdateCount();
        }
    }

    // Function to save data to a text file
    private void SaveData()
    {
        switch (autonomousMovement.modeSelection)
        {
            case 1:
                count = count1;
                break;
            case 2:
                count = count2;
                break;
            case 3:
                count = count3;
                break;
            case 4:
                count = count4;
                break;
            default:
                Debug.Log("Manual Mode");
                break;
        }

        string filePath = momentumFolder+ "/time"+count.ToString()+".txt";
         
        File.WriteAllText(filePath, timerText.text);
        Debug.Log("Value saved in the file: " + timerText.text);

        UpdateCount();
    }

    // Function to update the count based on the selected modality
    private void UpdateCount()
    {
        switch (autonomousMovement.modeSelection)
        {
            case 1:
                count1++;
                break;
            case 2:
                count2++;
                break;
            case 3:
                count3++;
                break;
            case 4:
                count4++;
                break;
            default:
                Debug.Log("Manual Mode");
                break;
        }
    }
}
