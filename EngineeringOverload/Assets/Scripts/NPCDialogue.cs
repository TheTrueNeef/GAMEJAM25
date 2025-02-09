using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public GameObject dialogueUI;  // Assign the dialogue UI panel in the inspector
    public TMP_Text dialogueText;  // Assign the TextMeshPro element in the UI
    private bool playerNearby = false;
    
    void Start()
    {
        dialogueUI.SetActive(false);
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E)) // Press 'E' to interact
        {
            dialogueUI.SetActive(true);
            dialogueText.text = "Welcome to the Engineering Shop! 🚜\nUpgrade your factory and transport systems.";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            dialogueUI.SetActive(false);
        }
    }
}

