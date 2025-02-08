using UnityEngine;
using TMPro;
using System.Collections;
using Unity.Cinemachine;

public class FactoryComputer : MonoBehaviour
{
    public CinemachineCamera computerCamera;
    public GameObject player;
    public MonoBehaviour playerController; // Reference to the player's controller script
    public Canvas computerCanvas;
    public TMP_Text commandPromptText;
    public GameObject interactIndicator; // Object that appears when in range
    public float typingSpeed = 0.05f;
    public int cameraPriority = 100;
    public int progress = 0; // Variable to track progress

    private string baseCommandText = "Microsoft Windows [Version 10.0.19045.5371]\n(c) Microsoft Corporation. All rights reserved.\n\n\nC:\\Users\\UWStudent>\\Factory-Overheat-Protection.exe\n[{0}%] OverHeat -Good";
    private bool isUsingComputer = false;
    private bool playerInRange = false;

    void Start()
    {
        if (interactIndicator != null)
        {
            interactIndicator.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isUsingComputer) // Interact key
        {
            StartCoroutine(UseComputer());
        }
    }

    IEnumerator UseComputer()
    {
        isUsingComputer = true;

        // Switch to computer camera
        computerCamera.Priority = cameraPriority;
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        computerCanvas.gameObject.SetActive(true);

        commandPromptText.text = "";
        string formattedText = string.Format(baseCommandText, progress);
        foreach (char c in formattedText)
        {
            commandPromptText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(2f);

        // Exit computer mode
        computerCamera.Priority = 0;
        if (playerController != null)
        {
            playerController.enabled = true;
        }
        computerCanvas.gameObject.SetActive(false);
        isUsingComputer = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactIndicator != null)
            {
                interactIndicator.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactIndicator != null)
            {
                interactIndicator.SetActive(false);
            }
        }
    }
}
