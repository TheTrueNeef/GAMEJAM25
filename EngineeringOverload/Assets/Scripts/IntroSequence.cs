using UnityEngine;
using TMPro;
using System.Collections;
using Unity.Cinemachine;

public class IntroSequence : MonoBehaviour
{
    public TMP_Text terminalText;
    public float typingSpeed = 0.05f;
    public CinemachineCamera mainGameCamera;
    public CinemachineCamera introCamera;
    public Canvas gameCanvas;
    public TrainingModule trainingModule; // Reference to the Training Module script

    private string introText = "Microsoft Windows [Version 10.0.19045.5371]\n(c) Microsoft Corporation. All rights reserved.\n\n\nC:\\Users\\UWStudent>\\Coop-Student-Init.exe\n\nHello, Welcome to generic poorly managed engineering company. \nWe are so happy for you to be joining our team!\n[INPUT REQUIRED] Do you need training? (y/n): ";
    private bool awaitingTrainingInput = true;

    void Start()
    {
        terminalText.text = "";
        if (gameCanvas != null)
        {
            gameCanvas.gameObject.SetActive(false);
        }
        StartCoroutine(TypeText(introText));
    }

    void Update()
    {
        if (awaitingTrainingInput)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                terminalText.text += "\nInitiating Training Module...\n";
                awaitingTrainingInput = false;
                StartCoroutine(SwitchToMainCamera(true));  // Pass `true` to start training
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                terminalText.text += "\nSkipping Training Module...\n";
                awaitingTrainingInput = false;
                StartCoroutine(SwitchToMainCamera(false)); // Pass `false` to skip training
            }
        }
    }

    IEnumerator TypeText(string text)
    {
        foreach (char c in text)
        {
            terminalText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    IEnumerator SwitchToMainCamera(bool startTraining)
    {
        yield return new WaitForSeconds(2f); // Optional delay before switching cameras
        introCamera.Priority = 0;
        mainGameCamera.Priority = 100;
        if (gameCanvas != null)
        {
            gameCanvas.gameObject.SetActive(true);
        }

        // Start Training if the player chose 'Y'
        if (startTraining && trainingModule != null)
        {
            trainingModule.StartTraining();
        }
    }
}
