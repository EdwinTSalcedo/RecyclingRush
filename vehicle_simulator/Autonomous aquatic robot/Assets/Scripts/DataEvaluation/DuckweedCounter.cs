using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class DuckweedCounter : MonoBehaviour
{
    // Public variables accessible from the Unity Editor
    public gameagain game;                  // Reference to the 'gameagain' script
    public int count;                       // General counter variable
    public int count1;                      // Counter variable for Modality 1
    public int count2;                      // Counter variable for Modality 2
    public int count3;                      // Counter variable for Modality 3
    public int count4;                      // Counter variable for Modality 4
    public bool finishTime;                 // Flag indicating if it's time to finish and save data
    public Text remainingText;              // Text object to display remaining duckweed
    public Text countText;                  // Text object to display picked duckweed count
    public bool saveData;                   // Flag indicating if it's time to save data
    public string displayText;              // Text variable for displaying information
    public int duckweedCount = 0;           // Counter for duckweed
    string basePath;                        // Base path for file operations
    string momentumFolder;                  // Folder for the current modality
    public float connectInterval;           // Interval for connection
    public AutonomousMovement autonomousMovement;     // Reference to the 'AutonomousMovement' script
    public modality1 serverModality1;           // Reference to the 'ServerModality1' script
    public modality2 serverModality2;           // Reference to the 'ServerModality2' script
    public modality3 serverModality3;           // Reference to the 'ServerModality3' script
    public modality4 serverModality4;           // Reference to the 'ServerModality4' script

    // Reference to the particle system
    public ParticleSystem particleSystem;

    // Number of particles in the system
    public int particleCount;

    // Called when the script starts
    private void Start()
    {
        // Set up the base path for file operations
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string recyclingRushPath = System.IO.Path.Combine(documentsPath, "!Recycling Rush");
        basePath = System.IO.Path.Combine(recyclingRushPath, "servers");
    }

    // Called every frame
    private void Update()
    {
        // Determine the modality folder based on the selected modality in AutonomousMovement script
        switch (autonomousMovement.modeSelection)
        {
            case 1:
                momentumFolder = System.IO.Path.Combine(basePath, "Modality1");
                break;
            case 2:
                momentumFolder = System.IO.Path.Combine(basePath, "Modality2");
                break;
            case 3:
                momentumFolder = System.IO.Path.Combine(basePath, "Modality3");
                break;
            case 4:
                momentumFolder = System.IO.Path.Combine(basePath, "Modality4");
                break;
            default:
                break;
        }

        // Get the reference to the ParticleSystem component
        particleSystem = GetComponent<ParticleSystem>();

        // Get the number of particles in the system
        particleCount = particleSystem.particleCount;

        // Display the remaining duckweed count
        displayText = "Existing duckweed: " + particleCount.ToString();
        remainingText.text = displayText;

        // Display the picked duckweed count
        displayText = "Picked duckweed: " +  (particleSystem.main.maxParticles - particleCount).ToString();
        countText.text = displayText;

        // Check if it's time to finish and save data
        if (finishTime)
        {
            // Set the general counter based on the selected modality
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
                    break;
            }

            // Create a file path for saving data
            string filePath = momentumFolder + "/duckweed" + count.ToString() + ".txt";

            // Write data to the file
            File.WriteAllText(filePath, displayText);

            // Log a message
            Debug.Log("Value saved in the file: " + displayText);

            // Reset flags and trigger game restart
            finishTime = false;
            serverModality1.servercomplete = false;
            serverModality2.servercomplete = false;
            serverModality3.servercomplete = false;
            serverModality4.servercomplete = false;
            game.Again();

            // Increment the counter for the selected modality
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
                    break;
            }
        }
        // Check if it's time to save data
        else if (saveData)
        {
            // Set the general counter based on the selected modality
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
                    break;
            }

            // Create a file path for saving data
            string filePath = momentumFolder + "/duckweed" + count.ToString() + ".txt";

            // Write data to the file
            File.WriteAllText(filePath, displayText);

            // Log a message
            Debug.Log("Value saved in the file: " + displayText);

            // Reset flags and trigger game restart
            saveData = false;
            serverModality1.servercomplete = false;
            serverModality2.servercomplete = false;
            serverModality3.servercomplete = false;
            serverModality4.servercomplete = false;
            game.Again();
            
            // Increment the counter for the selected modality
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
                    break;
            }
        }
    }
}
