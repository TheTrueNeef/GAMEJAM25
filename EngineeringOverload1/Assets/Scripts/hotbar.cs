using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Hotbar : MonoBehaviour
{
    public int slotCount = 5;
    public List<GameObject> items = new List<GameObject>();
    public RawImage[] slotImages; // Background slot textures
    public Texture selectedTexture;
    public Texture unselectedTexture;
    public RawImage[] itemImages; // Item images in the hotbar

    private int selectedIndex = 0;

    void Start()
    {
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
            SelectSlot((selectedIndex + 1) % slotCount);
        }
        else if (scroll < 0f)
        {
            SelectSlot((selectedIndex - 1 + slotCount) % slotCount);
        }
    }

    void SelectSlot(int index)
    {
        if (index >= 0 && index < slotCount)
        {
            selectedIndex = index;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            slotImages[i].texture = (i == selectedIndex) ? selectedTexture : unselectedTexture;
        }
    }

    public void AddItem(GameObject item, int slot, Texture itemTexture)
    {
        if (slot >= 0 && slot < slotCount)
        {
            if (items.Count <= slot)
            {
                while (items.Count <= slot)
                {
                    items.Add(null);
                }
            }
            items[slot] = item;
            itemImages[slot].texture = itemTexture; // Set item image
            UpdateUI();
        }
    }

    public GameObject GetSelectedItem()
    {
        return (selectedIndex < items.Count) ? items[selectedIndex] : null;
    }
}
