using UnityEngine;
using Unity.Cinemachine;

public class MoveOnSpline : MonoBehaviour
{
    public CinemachineSplineCart dollyCart; // Reference to the Cinemachine SplineCart
    public float speed = 5f; // Movement speed
    public GameObject targetObject; // The object that has the AddProduct() method
    private bool hasAddedProduct = false; // Prevent multiple calls

    void Update()
    {
        if (dollyCart == null) return;

        // Move along the spline
        dollyCart.SplinePosition += speed * Time.deltaTime;

        // Check if we reached SplinePosition 7 and haven't added the product yet
        if (Mathf.FloorToInt(dollyCart.SplinePosition) == 7 && !hasAddedProduct)
        {
            ReachedPosition7();
        }

        // Reset to start if we reach the end
        if (dollyCart.SplinePosition >= 7)
        {
            ResetToStart();
        }
    }

    void ReachedPosition7()
    {
        Debug.Log("Reached SplinePosition 7!");

        if (targetObject != null)
        {
            targetObject.GetComponent<ProductLoader>().addProduct(1);
        }

        hasAddedProduct = true; // Prevent multiple triggers
    }

    void ResetToStart()
    {
        dollyCart.SplinePosition = 0f;
        hasAddedProduct = false; // Reset trigger for next cycle
        gameObject.SetActive(false);
        
    }
}
