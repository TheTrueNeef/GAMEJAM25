using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Cinemachine;
using System.Collections;

public class NuclearReactor : MonoBehaviour
{
    public float meltdownThreshold = 100f; // Temperature at which meltdown occurs
    public float cooldownRate = 10f; // How much temperature decreases when cooling
    public float heatIncreaseRate = 5f; // How much temperature increases per second
    public int daysToSurvive = 7; // Number of days needed to win
    public TextMeshProUGUI pressureText; // UI Text to display current pressure
    public GameObject warningIndicator; // GameObject to enable when pressure is high
    public CinemachineCamera flashbangCamera; // Cinemachine camera for flashbang effect
    public float flashbangDuration = 2f; // Duration of flashbang effect

    public float currentTemperature = 50f;
    private int currentDay = 0;
    private bool gameOver = false;
    private bool playerInTrigger = false;
    private bool isCooling = false;

    void Update()
    {
        if (gameOver) return;

        // Increase reactor temperature over time
        currentTemperature += heatIncreaseRate * Time.deltaTime;

        // Check for meltdown condition
        if (currentTemperature >= meltdownThreshold)
        {
            TriggerMeltdown();
        }

        // Cooldown if cooling is active and player is in the trigger
        if (playerInTrigger && isCooling)
        {
            CoolReactor();
        }

        // Enable warning indicator if pressure reaches 80
        if (warningIndicator != null)
        {
            warningIndicator.SetActive(currentTemperature >= 80f && currentTemperature <= 99f);
        }

        // Update UI
        UpdatePressureText();
    }

    public void CoolReactor()
    {
        currentTemperature -= cooldownRate * Time.deltaTime;
        if (currentTemperature < 0f)
        {
            currentTemperature = 0f;
        }
    }

    public void ProgressDay()
    {
        if (gameOver) return;
        
        currentDay++;
        if (currentDay >= daysToSurvive)
        {
            WinGame();
        }
    }

    void TriggerMeltdown()
    {
        Debug.Log("Meltdown! Game Over.");
        warningIndicator.SetActive (false);
        gameOver = true;
        StartCoroutine(FlashbangEffect());
    }

    void WinGame()
    {
        Debug.Log("Reactor maintained! You win!");
        gameOver = true;
        SceneManager.LoadScene("WinScene");
    }

    private IEnumerator FlashbangEffect()
    {
        if (flashbangCamera != null)
        {
            flashbangCamera.Priority = 105;
            yield return new WaitForSeconds(flashbangDuration);
            //SceneManager.LoadScene("GameOverScene");
        }
        else
        {
            //SceneManager.LoadScene("GameOverScene");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            isCooling = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            isCooling = true;
        }
    }

    void UpdatePressureText()
    {
        if (pressureText != null)
        {
            pressureText.text = "Pressure: " + currentTemperature.ToString("F2");
        }
    }
}
