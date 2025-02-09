using UnityEngine;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    public GameObject shopPanel; // The UI panel for the shop
    public CurrencyManager currencyManager;
    public AICar aiCar;
    public MachineProduction machineProduction;
    public playerController playerController; // Reference to playerController script
    public Transform player; // Reference to the player's transform
    public Transform shopOwner; // Reference to the shop owner's position

    private bool shopOpen = false;
    private bool nearShopOwner = false; // Tracks if the player is near the shop

    private int vehicleSpeedCost = 5;
    private int playerSpeedCost = 8;
    private int machineSpeedCost = 10;

    private float vehicleSpeedIncrease = 5f;
    private float vehicleMaxSpeedIncrease = 10f;

    private bool playerSpeedUpgraded = false;
    private bool machineSpeedUpgraded = false;
    private bool vehicleSpeedUpgraded = false;

    private int sellPriceCost = 20;
    private float sellPriceIncrease = 1.2f;
    private bool sellPriceUpgraded = false;

    public SellPoint sellPoint;

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

        // Open the shop UI when pressing "E" near the shop owner
        if (nearShopOwner && Input.GetKeyDown(KeyCode.E))
        {
            shopOpen = !shopOpen;
            shopPanel.SetActive(shopOpen);
        }

        if (!shopOpen) return; // If shop is closed, do nothing

        // Handle upgrade purchases
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UpgradeVehicleSpeed();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UpgradePlayerSpeed();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UpgradeMachineSpeed();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UpgradeSellPrice();
        }
    }

    void UpgradeVehicleSpeed()
    {
        if (vehicleSpeedUpgraded)
        {
            Debug.Log("Vehicle speed upgrade already purchased!");
            return;
        }

        if (aiCar == null)
        {
            Debug.LogError("AICar reference is missing! Assign it in the Inspector.");
            return;
        }

        if (currencyManager.SpendCurrency(vehicleSpeedCost))
        {
            aiCar.speed += vehicleSpeedIncrease;
            aiCar.maxSpeed += vehicleMaxSpeedIncrease;
            vehicleSpeedUpgraded = true;

            Debug.Log("Vehicle Speed Upgraded! New Max Speed: " + aiCar.maxSpeed);
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    void UpgradePlayerSpeed()
    {
        if (playerSpeedUpgraded)
        {
            Debug.Log("Player speed upgrade already purchased!");
            return;
        }

        if (playerController == null)
        {
            Debug.LogError("playerController reference is missing! Assign it in the Inspector.");
            return;
        }

        if (currencyManager.SpendCurrency(playerSpeedCost))
        {
            playerController.walkSpeed += 2f;
            playerController.sprintSpeed += 3f;
            playerController.jumpHeight += 1f;
            playerSpeedUpgraded = true;

            Debug.Log("Player Speed Upgraded! New Walk Speed: " + playerController.walkSpeed +
                      ", Sprint Speed: " + playerController.sprintSpeed +
                      ", Jump Height: " + playerController.jumpHeight);
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }


    void UpgradeMachineSpeed()
    {
        if (machineProduction == null)
        {
            Debug.LogError("MachineProduction reference is missing! Assign it in the Inspector.");
            return;
        }

        // Ensure the upgrade only runs once
        if (machineSpeedUpgraded)
        {
            Debug.Log("Machine speed upgrade already purchased!");
            return;
        }

        if (currencyManager.SpendCurrency(machineSpeedCost))
        {
            machineProduction.pressureBuildRate = 7f;
            machineProduction.pressureReleaseRate = 6f;
            machineProduction.cooldownTime = 2f;
            machineSpeedUpgraded = true; // Prevent future upgrades

            Debug.Log("Machine Speed Upgraded! New Build Rate: 7, Release Rate: 4");
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    void UpgradeSellPrice()
    {
        if (sellPriceUpgraded)
        {
            Debug.Log("Sell price upgrade already purchased!");
            return;
        }

        if (sellPoint == null)
        {
            Debug.LogError("SellPoint reference is missing! Assign it in the Inspector.");
            return;
        }

        if (currencyManager.SpendCurrency(sellPriceCost))
        {
            // Apply the sell price increase
            sellPoint.productSellPrice *= sellPriceIncrease;
            sellPriceUpgraded = true;

            Debug.Log($"Sell Price Increased! New price: {sellPoint.productSellPrice:F2}");
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }
}
