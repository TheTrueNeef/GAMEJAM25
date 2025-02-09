using System.Collections;
using UnityEngine;
using TMPro;
using Unity.Cinemachine;

public class TrainingModule : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI instructionText;
    public float typingSpeed = 0.05f;
    public GameObject hotbarUI;  // Assign hotbar UI GameObject

    [Header("Cinemachine Cameras")]
    public CinemachineCamera playerCamera;   // The player's default camera (Active after training)
    public CinemachineCamera dollyCamera;
    public CinemachineCamera truckCamera;
    public CinemachineCamera shipCamera;
    public CinemachineCamera machinesCamera;

    [Header("Indicators")]
    public GameObject fireExtinguisher;
    public GameObject fireExtinguisherArrow;
    public GameObject exitArrow;
    public GameObject[] factoryArrows;
    public GameObject nuclearArrow;

    [Header("Player Settings")]
    public Transform player;
    public MonoBehaviour playerMovementScript;

    private bool isTrainingActive = false;

    public void StartTraining()
    {
        if (!isTrainingActive)
        {
            isTrainingActive = true;
            DisablePlayerUI();  // Disable UI & cursor
            StartCoroutine(TrainingSequence());
        }
    }

    IEnumerator TrainingSequence()
    {
        // Step 1: Pick up Fire Extinguisher
        yield return StartCoroutine(DisplayInstruction("Pick up the fire extinguisher."));
        fireExtinguisherArrow.SetActive(true);
        yield return new WaitUntil(() => FireExtinguisherPickedUp());
        fireExtinguisherArrow.SetActive(false);

        // Step 2: Leave Office
        yield return StartCoroutine(DisplayInstruction("Leave the office."));
        exitArrow.SetActive(true);
        yield return new WaitUntil(() => player.position.y > -50);
        exitArrow.SetActive(false);

        // Step 3: Switch to Dolly Camera (Show Map)
        yield return StartCoroutine(DisplayInstruction("This is the factory map."));
        DisablePlayerMovement();
        SwitchCamera(dollyCamera);

        // Step 4: Show Factories & Nuclear Plant
        yield return new WaitForSeconds(2f);
        ToggleArrows(factoryArrows, true);
        nuclearArrow.SetActive(true);
        yield return new WaitForSeconds(4f);
        ToggleArrows(factoryArrows, false);
        nuclearArrow.SetActive(false);

        // Step 5: Show Truck
        yield return StartCoroutine(DisplayInstruction("This is the delivery truck."));
        SwitchCamera(truckCamera);
        yield return new WaitForSeconds(3f);

        // Step 6: Show Cargo Ship
        yield return StartCoroutine(DisplayInstruction("This is the cargo ship."));
        SwitchCamera(shipCamera);
        yield return new WaitForSeconds(3f);

        // Step 7: Show Machines
        yield return StartCoroutine(DisplayInstruction("These are the machines in operation."));
        SwitchCamera(machinesCamera);
        yield return new WaitForSeconds(3f);

        // Step 8: Training Complete - Switch back to Player Camera & Reactivate Movement/UI
        yield return StartCoroutine(DisplayInstruction("Training Complete!"));
        SwitchCamera(playerCamera);
        EnablePlayerMovement();
        EnablePlayerUI();  // Enable UI & cursor
    }

    IEnumerator DisplayInstruction(string text)
    {
        instructionText.text = "";
        foreach (char letter in text)
        {
            instructionText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(2f);
    }

    void SwitchCamera(CinemachineCamera newCamera)
    {
        dollyCamera.Priority = 0;
        truckCamera.Priority = 0;
        shipCamera.Priority = 0;
        machinesCamera.Priority = 0;
        playerCamera.Priority = 0;

        newCamera.Priority = 10;
    }

    void ToggleArrows(GameObject[] arrows, bool state)
    {
        foreach (GameObject arrow in arrows)
        {
            arrow.SetActive(state);
        }
    }

    void DisablePlayerMovement()
    {
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;
    }

    void EnablePlayerMovement()
    {
        if (playerMovementScript != null)
            playerMovementScript.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
    }

    void DisablePlayerUI()
    {
        if (hotbarUI != null)
            hotbarUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void EnablePlayerUI()
    {
        if (hotbarUI != null)
            hotbarUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
    }

    bool FireExtinguisherPickedUp()
    {
        return fireExtinguisher.activeSelf == false;
    }
}
