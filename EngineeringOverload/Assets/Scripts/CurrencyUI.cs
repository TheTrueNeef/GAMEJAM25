using UnityEngine;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    public TMP_Text currencyText;

    void Start()
{
    if (currencyText == null)
    {
        Debug.LogError("currencyText is NOT assigned in the Inspector!");
    }
    else
    {
        UpdateUI();
    }
}
    public void UpdateUI()
    {
        currencyText.text = "Coins: " + CurrencyManager.Instance.GetCoins();
    }
}