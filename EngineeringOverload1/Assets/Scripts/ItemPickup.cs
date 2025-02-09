using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Hotbar hotbar; // Assign the Hotbar GameObject in the Inspector
    public int slotIndex; // The predefined slot index for this item (0 to 4)

    private bool canPickup = false;

    void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.E))
        {
            TryPickup();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = false;
        }
    }

    void TryPickup()
    {
        if (hotbar == null)
        {
            Debug.LogError("Hotbar is not assigned in the Inspector!");
            return;
        }
        if (slotIndex < 0 || slotIndex >= 5)
        {
            Debug.LogError($"Invalid slot index: {slotIndex}. Must be between 0 and 4.");
            return;
        }

        if (hotbar.IsSlotEmpty(slotIndex)) // If slot is empty
        {
            Debug.Log($"Enabling item in slot {slotIndex}");
            hotbar.AddItem(slotIndex);
            Destroy(gameObject); // Remove the pickup from the world
        }
        else
        {
            Debug.Log($"Slot {slotIndex} is already occupied.");
        }
    }
}
