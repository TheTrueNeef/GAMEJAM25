using UnityEngine;

public class SellPoint : MonoBehaviour
{
    public float totalEarnings = 0;
    public float productSellPrice = 1.0f; // Price per product
    public AudioClip sellSound; // Optional sound effect for selling
    private AudioSource audioSource;

    void Start()
    {
        // Attach an AudioSource component if not already present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && sellSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"SellPoint Triggered by: {other.gameObject.name}");

        AICar aiCar = other.GetComponent<AICar>();
        if (aiCar != null)
        {
            Debug.Log("AICar Detected! Checking inventory...");
            if (aiCar.productCount > 0)
            {
                SellItems(aiCar);
            }
            else
            {
                Debug.Log("AICar has no products to sell.");
            }
        }
        else
        {
            Debug.Log("No AICar component found on this object.");
        }
    }

    private void SellItems(AICar aiCar)
    {
        int soldItems = aiCar.productCount;
        float earnings = soldItems * productSellPrice;
        totalEarnings += earnings;

        Debug.Log($"Sold {soldItems} items for {earnings:F2} credits. Total Earnings: {totalEarnings:F2}");

        // Play sell sound if available
        if (sellSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(sellSound);
        }

        // Empty the vehicle inventory
        aiCar.productCount = 0;

        // Update the UI if available
        if (aiCar.productCountText != null)
        {
            aiCar.UpdateProductUI();
        }
    }
}
