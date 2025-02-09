using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ShopSystem : MonoBehaviour
{
    public GameObject shopPanel; // The UI panel for the shop
    public GameObject notificationPanel; // The UI panel for notifications
    public TextMeshProUGUI notificationText; // The UI text to show messages
    public CurrencyManager currencyManager;
    public AICar aiCar;
    public List<MachineProduction> machineProductions; // List of MachineProduction objects
    public playerController playerController; // Reference to playerController script
    public Transform player; // Reference to the player's transform
    public Transform shopOwner; // Reference to the shop owner's position

    private bool shopOpen = false;
    private bool nearShopOwner = false; // Tracks if the player is near the shop

    public int vehicleSpeedCost = 5;
    public int playerSpeedCost = 8;
    public int machineSpeedCost = 10;

    private float vehicleSpeedIncrease = 5f;
    private float vehicleMaxSpeedIncrease = 10f;

    private bool playerSpeedUpgraded = false;
    private bool machineSpeedUpgraded = false;
    private bool vehicleSpeedUpgraded = false;

    public int sellPriceCost = 20;
    private float sellPriceIncrease = 1.2f;
    private bool sellPriceUpgraded = false;

    public SellPoint sellPoint;

    void ToggleShop()
    {
        shopOpen = !shopOpen;
        shopPanel.SetActive(shopOpen);

        // Optional: Add menu sound effect here
    }

    void Update()
    {
        // Check if the player is near the shop owner
        if (Vector3.Distance(player.position, shopOwner.position) < 3f)
        {
            nearShopOwner = true;
        }
        else
        {
            nearShopOwner = false;
        }

        // Open/close shop with E when near owner
        if (nearShopOwner && Input.GetKeyDown(KeyCode.E))
        {
            ToggleShop();
        }

        // Close shop with Tab regardless of location
        if (shopOpen && Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleShop();
            CloseNotificationPanel(); // Close notification when closing shop
        }

        if (!shopOpen) return;

        // Handle upgrade purchases
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UpgradeVehicleSpeed();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UpgradePlayerSpeed();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UpgradeMachineSpeed();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UpgradeSellPrice();
        }
    }

    void ShowNotification(string message)
    {
        notificationPanel.SetActive(true);
        notificationText.text = message;
    }

    void CloseNotificationPanel()
    {
        notificationPanel.SetActive(false);
    }

    void UpgradeVehicleSpeed()
    {
        if (vehicleSpeedUpgraded)
        {
            ShowNotification("Vehicle speed upgrade already purchased!");
            return;
        }

        if (aiCar == null)
        {
            ShowNotification("AICar reference is missing! Assign it in the Inspector.");
            return;
        }

        if (currencyManager.SpendCurrency(vehicleSpeedCost))
        {
            aiCar.speed += vehicleSpeedIncrease;
            aiCar.maxSpeed += vehicleMaxSpeedIncrease;
            vehicleSpeedUpgraded = true;

            ShowNotification($"Vehicle Speed Upgraded! New Max Speed: {aiCar.maxSpeed}");
        }
        else
        {
            ShowNotification("Not enough money!");
        }
    }

    void UpgradePlayerSpeed()
    {
        if (playerSpeedUpgraded)
        {
            ShowNotification("Player speed upgrade already purchased!");
            return;
        }

        if (playerController == null)
        {
            ShowNotification("playerController reference is missing! Assign it in the Inspector.");
            return;
        }

        if (currencyManager.SpendCurrency(playerSpeedCost))
        {
            playerController.walkSpeed += 2f;
            playerController.sprintSpeed += 3f;
            playerController.jumpHeight += 1f;
            playerSpeedUpgraded = true;

            ShowNotification($"Player Speed Upgraded! New Walk Speed: {playerController.walkSpeed}, Sprint Speed: {playerController.sprintSpeed}, Jump Height: {playerController.jumpHeight}");
        }
        else
        {
            ShowNotification("Not enough money!");
        }
    }

    void UpgradeMachineSpeed()
    {
        if (machineProductions == null || machineProductions.Count == 0)
        {
            ShowNotification("MachineProduction references are missing! Assign them in the Inspector.");
            return;
        }

        if (machineSpeedUpgraded)
        {
            ShowNotification("Machine speed upgrade already purchased!");
            return;
        }

        if (currencyManager.SpendCurrency(machineSpeedCost))
        {
            // Apply the upgrade to all machines in the list
            foreach (var machine in machineProductions)
            {
                machine.pressureBuildRate = 7f;
                machine.pressureReleaseRate = 6f;
                machine.cooldownTime = 2f;
            }
            machineSpeedUpgraded = true; // Prevent future upgrades

            ShowNotification("Machine Speed Upgraded for all machines!");
        }
        else
        {
            ShowNotification("Not enough money!");
        }
    }

    void UpgradeSellPrice()
    {
        if (sellPriceUpgraded)
        {
            ShowNotification("Sell price upgrade already purchased!");
            return;
        }

        if (sellPoint == null)
        {
            ShowNotification("SellPoint reference is missing! Assign it in the Inspector.");
            return;
        }

        if (currencyManager.SpendCurrency(sellPriceCost))
        {
            sellPoint.productSellPrice *= sellPriceIncrease;
            sellPriceUpgraded = true;

            ShowNotification($"Sell Price Increased! New price: {sellPoint.productSellPrice:F2}");
        }
        else
        {
            ShowNotification("Not enough money!");
        }
    }
}
