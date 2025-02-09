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
    public GameObject ProductLoader;
    public GameObject ProductContainer;
    private bool isWaitingForLoad = false;
    private float loadProgress = 0f;
    private float currentSpeed = 0f;

    void Start()
    {
        UpdateProductUI();
    }

    void Update()
    {
        if (isWaitingForLoad || dollyCart == null) return;

        // Accelerate up to max speed
        currentSpeed = Mathf.Clamp(currentSpeed + acceleration * Time.deltaTime, 0, maxSpeed);

        // Move along the spline
        dollyCart.SplinePosition += currentSpeed * Time.deltaTime;

        // Check if the truck is within the loading zone (between 765 and 770)
        if (dollyCart.SplinePosition >= 758f && dollyCart.SplinePosition <= 762f && productCount == 0)
        {
            StartLoading();
        }
        if(productCount != 0) { ProductContainer.SetActive(true); }
        else { ProductContainer.SetActive(false); }
    }

    void StartLoading()
    {
        if (isWaitingForLoad) return; // Prevent multiple coroutine calls
        Debug.Log("Truck entered loading zone, starting loading...");
        currentSpeed = 0f;
        dollyCart.SplinePosition = 761;
        isWaitingForLoad = true;
        StartCoroutine(WaitForLoad());
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
        productCount += ProductLoader.GetComponent<ProductLoader>().LoadProducts();
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
            loadingText.text = isVisible ? $"Loading... {(loadProgress * 100f):F0}%" : "";
        }
    }
}
