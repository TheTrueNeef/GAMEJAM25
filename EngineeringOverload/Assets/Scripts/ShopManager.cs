using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public GameObject shopUI;  // The Shop UI Panel
    public TMP_Text shopText;  // The Text displaying the upgrade information
    private bool playerNearby = false; // To track if the player is near the NPC
    private bool shopOpen = false;  // To track if the shop menu is open
    private int factoryLevel = 1;  // Starting at Level 1
    private int[] upgradeCosts = { 100, 200}; // Costs for Level 2 and 3
    public CurrencyManager currencyManager;

    void Start()
    {
        shopUI.SetActive(false); // Initially, the shop UI is hidden
    }

    void Update()
    {
        // Open the shop menu if the player is nearby and presses E
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            shopOpen = !shopOpen; // Toggle shop menu
            shopUI.SetActive(shopOpen); // Show or hide the shop menu
            UpdateShopText();
        }

        // If the shop menu is open and player presses U, upgrade the factory
        if (shopOpen && Input.GetKeyDown(KeyCode.U))
        {
            BuyUpgrade();
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
            shopOpen = false; // Close shop menu if player leaves area
            shopUI.SetActive(false);
        }
    }

    // Function to handle the upgrade purchase
    void BuyUpgrade()
    {
        if (factoryLevel < 3) // Only upgrade until Level 3
        {
            int cost = upgradeCosts[factoryLevel - 1];
            if (currencyManager.SpendCurrency(cost)) // Check if the player can afford it
            {
                factoryLevel++; // Upgrade to the next level
                UpdateShopText();
            }
            else
            {
                shopText.text = "Not enough currency!";
            }
        }
        else
        {
            shopText.text = "Max Level Reached!";
        }
    }

    // Update shop UI text based on current level and upgrade costs
    void UpdateShopText()
    {
        if (factoryLevel == 1)
            shopText.text = $"Upgrade to Level 2: Cost: {upgradeCosts[0]} Flex Dollars";
        else if (factoryLevel == 2)
            shopText.text = $"Upgrade to Level 3: Cost: {upgradeCosts[1]} Flex Dollars";
        else
            shopText.text = "Max Level Reached!";
    }
}
