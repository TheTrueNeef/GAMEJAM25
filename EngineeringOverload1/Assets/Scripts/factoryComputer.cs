using UnityEngine;
using TMPro;
using System.Collections;
using Unity.Cinemachine;

public class FactoryComputer : MonoBehaviour
{
    public CinemachineCamera computerCamera;
    public CinemachineCamera playerCamera; // Player camera reference
    public GameObject player;
    public MonoBehaviour playerController; // Reference to the player's controller script
    public Canvas computerCanvas;
    public TMP_Text commandPromptText;
    public GameObject interactIndicator; // Object that appears when in range
    public float typingSpeed = 0.05f;
    public int cameraPriority = 100;

    public MachineProduction machine; // Reference to the machine being controlled

    private bool isUsingComputer = false;
    private bool playerInRange = false;
    private bool waitingForInput = false; // Wait for user input
    private bool isMonitoringPressure = false; // Keeps updating pressure live

    private string baseCommandText = "Microsoft Windows [Version 10.0.19045.5371]\n(c) Microsoft Corporation. All rights reserved.\n\n\nC:\\Users\\UWStudent>\\Factory-Computer-init.exe\n";

    void Start()
    {
        if (interactIndicator != null)
        {
            interactIndicator.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isUsingComputer)
        {
            StartCoroutine(UseComputer());
        }

        if (waitingForInput)
        {
            if (Input.GetKeyDown(KeyCode.Y)) // User pressed 'Y' (Turn on machine)
            {
                StartMachine();
            }
            else if (Input.GetKeyDown(KeyCode.N)) // User pressed 'N' (Cancel)
            {
                ExitComputer();
            }
        }
    }

    IEnumerator UseComputer()
    {
        isUsingComputer = true;
        waitingForInput = false;
        isMonitoringPressure = false;

        // Switch to Factory Computer Camera
        SwitchToComputerView();

        commandPromptText.text = "";
        string machineStats = GetMachineStats();
        string formattedText = baseCommandText + machineStats + "\nTurn on machine? (Y/N)_";

        foreach (char c in formattedText)
        {
            commandPromptText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        waitingForInput = true; // Now waiting for user input (Y/N)
    }

    string GetMachineStats()
    {
        if (machine == null) return "Error: No machine connected.\n";

        return $"Machine Status:\n" +
               $" - Pressure: {machine.currentPressure}/{machine.maxPressure}\n" +
               $" - Producing: {(machine.isProducing ? "Yes" : "No")}\n" +
               $" - Cooling Down: {(machine.isCoolingDown ? "Yes" : "No")}\n" +
               $" - Fire: {(machine.isOnFire ? "ACTIVE" : "None")}\n";
    }

    void StartMachine()
    {
        waitingForInput = false;

        if (machine != null && !machine.isOnFire && !machine.isCoolingDown)
        {
            machine.ToggleMachine();
            commandPromptText.text += "\nMachine started!\n";
            isMonitoringPressure = true;
            StartCoroutine(UpdatePressureLive());

            ExitComputer(); // Immediately switch back to player camera
        }
        else
        {
            commandPromptText.text += "\nError: Cannot start machine!\n";
            StartCoroutine(ExitAfterDelay());
        }
    }

    IEnumerator UpdatePressureLive()
    {
        while (isMonitoringPressure && machine.isProducing)
        {
            commandPromptText.text = baseCommandText + GetMachineStats();
            yield return new WaitForSeconds(0.5f); // Update every 0.5 seconds
        }

        commandPromptText.text += "\nMachine stopped or overheated.\n";
        isMonitoringPressure = false;
        StartCoroutine(ExitAfterDelay());
    }

    void ExitComputer()
    {
        waitingForInput = false;
        commandPromptText.text += "\nExiting...\n";
        StartCoroutine(ExitAfterDelay());
    }

    IEnumerator ExitAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        SwitchToPlayerView();
    }

    void SwitchToComputerView()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        computerCanvas.gameObject.SetActive(true);

        // Activate Factory Computer Camera
        if (computerCamera != null)
        {
            computerCamera.Priority = cameraPriority;
        }

        // Deactivate Player Camera
        if (playerCamera != null)
        {
            playerCamera.Priority = 0;
        }
    }

    void SwitchToPlayerView()
    {
        isUsingComputer = false;

        // Reactivate Player Controller
        if (playerController != null)
        {
            playerController.enabled = true;
        }
        computerCanvas.gameObject.SetActive(false);

        // Activate Player Camera
        if (playerCamera != null)
        {
            playerCamera.Priority = cameraPriority;
        }

        // Deactivate Factory Computer Camera
        if (computerCamera != null)
        {
            computerCamera.Priority = 0;
        }
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
        playerInRange = false;
        if (interactIndicator != null)
        {
            interactIndicator.SetActive(false);
        }
    }
}
