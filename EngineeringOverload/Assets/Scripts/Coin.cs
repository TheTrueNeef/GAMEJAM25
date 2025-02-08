using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1; // Amount of currency given

    private void OnTriggerEnter(Collider other) // Use OnTriggerEnter2D for 2D
    {
        if (other.CompareTag("Player")) // Only trigger when the player touches it
        {
            CurrencyManager.Instance.AddCoins(coinValue); // Add currency
            Destroy(gameObject); // Remove coin after collection
        }
    }
}

