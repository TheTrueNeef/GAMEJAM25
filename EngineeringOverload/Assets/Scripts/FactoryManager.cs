using UnityEngine;
using TMPro; // UI text handling

public class FactoryManager : MonoBehaviour
{
    public int factoryLevel = 1; // Current factory level
    public int flexDollars = 0;  // Currency in the game
    public int gears = 0;        // Gears collected

    public TextMeshProUGUI flexDollarText; // UI Text for FlexDollars
    public TextMeshProUGUI gearCollectedText; // UI Text for gears collected (temporary)

    private float gearCollectionRate = 10f; // Gears per second (changes with level)
    private float flexDollarRate = 1f; // 1 FlexDollar per second

    private bool isCollecting = false; // Check if we are in the gear box

    void Start()
    {
        UpdateUI();
        InvokeRepeating("GenerateFlexDollars", 1f, 1f); // FlexDollars increase by 1 per second
    }

    void Update()
    {
        if (isCollecting)
        {
            gears += (int)(gearCollectionRate * Time.deltaTime);
            UpdateUI();
        }
    }

    void GenerateFlexDollars()
    {
        flexDollars += 1;
        UpdateUI();
    }

    void UpdateUI()
    {
        flexDollarText.text = "FlexDollars: " + flexDollars;
        gearCollectedText.text = "Gears: " + gears;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GearBox"))
        {
            isCollecting = true;
            Debug.Log("Collision detected with: " + other.name);
            gearCollectedText.gameObject.SetActive(true); // Show gear UI
            Invoke("HideGearUI", 2f); // Hide UI after 2 seconds
        }

        if (other.CompareTag("UpgradeBox"))
        {
            ShowUpgradeUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GearBox"))
        {
            isCollecting = false;
        }
    }

    void HideGearUI()
    {
        gearCollectedText.gameObject.SetActive(false);
    }

    void ShowUpgradeUI()
    {
        Debug.Log("Press 'Y' to Upgrade, 'N' to Cancel");
        StartCoroutine(WaitForUpgradeInput());
    }

    System.Collections.IEnumerator WaitForUpgradeInput()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                UpgradeFactory();
                yield break; // Stop coroutine
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                Debug.Log("Upgrade canceled");
                yield break; // Stop coroutine
            }
            yield return null;
        }
    }

    void UpgradeFactory()
    {
        if (factoryLevel == 1 && flexDollars >= 1000)
        {
            flexDollars -= 1000;
            factoryLevel = 2;
            gearCollectionRate = 100f;
        }
        else if (factoryLevel == 2 && flexDollars >= 5000)
        {
            flexDollars -= 5000;
            factoryLevel = 3;
            gearCollectionRate = 1000f;
        }
        else
        {
            Debug.Log("Not enough FlexDollars!");
            return;
        }

        Debug.Log("Factory upgraded to Level " + factoryLevel);
        UpdateUI();
    }
}
