using UnityEngine;
using TMPro;
using Unity.Cinemachine;
using System.Collections;

public class AICar : MonoBehaviour
{
    public CinemachineSplineCart dollyCart;
    public float speed = 5f; // Speed of the car
    public float acceleration = 1f;
    public float maxSpeed = 10f;
    public int productCount = 0; // Stores the number of products in the vehicle
    public TMP_Text productCountText;
    public TMP_Text loadingText; // Text for displaying loading progress
    public float loadWaitTime = 5f;

    private bool isWaitingForLoad = true;
    private float loadProgress = 0f;
    private float currentSpeed = 0f;
    private float splineLength = 1f; // Adjust this based on your spline length

    void Start()
    {
        StartCoroutine(WaitForLoad()); // Load at start
        UpdateProductUI();
    }

    void Update()
    {
        if (isWaitingForLoad || dollyCart == null) return;

        // Accelerate up to max speed
        currentSpeed = Mathf.Clamp(currentSpeed + acceleration * Time.deltaTime, 0, maxSpeed);

        // Move along the spline
        dollyCart.SplinePosition += currentSpeed * Time.deltaTime;

        // Check if the truck has reached the end of the spline
        if (dollyCart.SplinePosition >= splineLength && productCount == 0)
        {
            dollyCart.SplinePosition = splineLength; // Clamp to endpoint
            currentSpeed = 0f;

            if (productCount == 0) // Stop only if empty
            {
                isWaitingForLoad = true;
                StartCoroutine(WaitForLoad()); // Start loading
            }
            else
            {
                dollyCart.SplinePosition = 0f; // Instantly loop back
            }
        }
    }

    IEnumerator WaitForLoad()
    {
        Debug.Log("Waiting for load...");
        loadProgress = 0f;
        UpdateLoadingUI(true);

        while (loadProgress < 1f)
        {
            loadProgress += Time.deltaTime / loadWaitTime;
            UpdateLoadingUI(true);
            yield return null;
        }

        isWaitingForLoad = false;
        productCount = Random.Range(1, 10); // Load new products
        Debug.Log("Vehicle loaded with " + productCount + " products and ready to move.");
        UpdateLoadingUI(false);
        UpdateProductUI();
    }

    public void UpdateProductUI()
    {
        if (productCountText != null)
        {
            productCountText.text = "Products on board: " + productCount;
        }
    }

    void UpdateLoadingUI(bool isVisible)
    {
        if (loadingText != null)
        {
            if (isVisible)
            {
                loadingText.text = "Loading... " + (loadProgress * 100f).ToString("F0") + "%";
            }
            else
            {
                loadingText.text = "";
            }
        }
    }
}