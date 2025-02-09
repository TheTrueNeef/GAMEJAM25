using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    public RawImage[] slotImages; // UI background slot images
    public Texture selectedTexture;
    public Texture unselectedTexture;

    public GameObject[] itemImages; // UI item images (pre-assigned in Inspector)
    public GameObject[] heldItems;  // 3D objects the player holds (pre-assigned in Inspector)

    private int selectedIndex = 0;

    void Start()
    {
        if (slotImages.Length < 5 || itemImages.Length < 5 || heldItems.Length < 5)
        {
            Debug.LogError("Hotbar setup is incorrect! Ensure all slot images and held items are assigned.");
            return;
        }

        // Disable all item images and held items at the start
        foreach (var item in itemImages) item.SetActive(false);
        foreach (var heldItem in heldItems) heldItem.SetActive(false);

        UpdateUI();
    }

    void Update()
    {
        HandleInput();
        HandleScrollWheel();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
    }

    void HandleScrollWheel()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            SelectSlot((selectedIndex + 1) % 5);
        }
        else if (scroll < 0f)
        {
            SelectSlot((selectedIndex - 1 + 5) % 5);
        }
    }

    void SelectSlot(int index)
    {
        if (index >= 0 && index < 5)
        {
            selectedIndex = index;
            UpdateUI();
            UpdateHeldItem();
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            slotImages[i].texture = (i == selectedIndex) ? selectedTexture : unselectedTexture;
        }
    }

    void UpdateHeldItem()
    {
        for (int i = 0; i < heldItems.Length; i++)
        {
            heldItems[i].SetActive(i == selectedIndex && itemImages[i].activeSelf);
        }
    }

    public void AddItem(int slot)
    {
        if (slot < 0 || slot >= 5)
        {
            Debug.LogError($"Invalid slot index: {slot}. Slot index must be between 0 and 4.");
            return;
        }

        // Enable the corresponding item image
        itemImages[slot].SetActive(true);

        // If the slot is selected, enable the held item
        if (slot == selectedIndex)
        {
            UpdateHeldItem();
        }

        UpdateUI();
    }

    public bool IsSlotEmpty(int slot)
    {
        return !itemImages[slot].activeSelf;
    }
}
