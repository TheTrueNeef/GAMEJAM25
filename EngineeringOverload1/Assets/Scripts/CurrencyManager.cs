using UnityEngine;
using TMPro; // For TextMeshPro
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public TextMeshProUGUI currencyText; // For TextMeshPro
    public Image dotIcon; // The dotYellow image
    private float currencyAmount = 0f;

    void Start()
    {
        // Initialize the text and icon
        UpdateCurrencyText();
    }

    void Update()
    {
        // Increase currency by 1 every second
        currencyAmount += Time.deltaTime; // Adds 1 every second
        UpdateCurrencyText();
    }

    void UpdateCurrencyText()
    {
        // Format the currency value to always show 7 digits
        string formattedCurrency = Mathf.FloorToInt(currencyAmount).ToString("0000000"); // Ensures 7 digits
        currencyText.text = formattedCurrency;
    }

    public bool SpendCurrency(float amount)
    {
        if (currencyAmount >= amount)
        {
            currencyAmount -= amount;
            UpdateCurrencyText();
            return true;
        }
        return false;
    }

    public float GetCurrencyAmount()
    {
        return currencyAmount;
    }
}
