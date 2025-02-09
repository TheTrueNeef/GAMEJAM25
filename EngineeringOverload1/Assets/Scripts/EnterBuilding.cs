using UnityEngine;

public class EnterBuilding : MonoBehaviour
{
    public GameObject enterPrompt; // UI/GameObject that appears when in range
    public Transform teleportInsideLocation; // Where the player teleports inside
    public GameObject player; // Assign this in the Inspector

    private bool isPlayerInTrigger = false;
    private CharacterController playerController;

    private void Start()
    {
        if (enterPrompt != null)
            enterPrompt.SetActive(false); // Hide the prompt initially

        if (player != null)
            playerController = player.GetComponent<CharacterController>(); // Get CharacterController
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            if (enterPrompt != null)
                enterPrompt.SetActive(true); // Show the enter prompt
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            if (enterPrompt != null)
                enterPrompt.SetActive(false); // Hide the enter prompt
        }
    }

    private void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (enterPrompt != null)
                enterPrompt.SetActive(false); // Hide the enter prompt
            isPlayerInTrigger = false;
            TeleportPlayer();
        }
    }

    private void TeleportPlayer()
    {
        if (player != null && teleportInsideLocation != null)
        {
            Debug.Log("Teleporting Player...");

            if (playerController != null)
            {
                playerController.enabled = false; // Disable CharacterController before teleporting
            }

            player.transform.position = teleportInsideLocation.position;

            if (playerController != null)
            {
                playerController.enabled = true; // Re-enable CharacterController
            }
        }
        else
        {
            Debug.LogError("Player or Teleport Location is missing!");
        }
    }
}
