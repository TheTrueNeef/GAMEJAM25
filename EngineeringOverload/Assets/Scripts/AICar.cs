using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class AICar : MonoBehaviour
{
    public List<Transform> waypoints;
    public float speed = 5f;
    public float turnSpeed = 2f;
    public float loadWaitTime = 5f;
    public List<string> products;
    public TMP_Text loadingProgressText;
    public TMP_Text productCountText;

    private int currentWaypointIndex = 0;
    private bool isWaitingForLoad = true;
    private bool userSentVehicle = false;
    private float loadProgress = 0f;

    void Start()
    {
        if (loadingProgressText != null)
        {
            loadingProgressText.gameObject.SetActive(true);
            loadingProgressText.text = "Loading: 0%";
        }
        StartCoroutine(WaitForLoad());
        UpdateProductUI();
    }

    void Update()
    {
        if (isWaitingForLoad && !userSentVehicle) return;
        if (waypoints.Count == 0) return;

        MoveTowardsWaypoint();
    }

    IEnumerator WaitForLoad()
    {
        Debug.Log("Waiting for load...");
        loadProgress = 0f;

        while (loadProgress < 1f)
        {
            loadProgress += Time.deltaTime / loadWaitTime;
            if (loadingProgressText != null)
            {
                loadingProgressText.text = "Loading: " + Mathf.FloorToInt(loadProgress * 100) + "%";
            }
            yield return null;
        }

        isWaitingForLoad = false;
        Debug.Log("Vehicle loaded and ready to move.");
        if (loadingProgressText != null)
        {
            loadingProgressText.text = "Loading: 100%";
            loadingProgressText.gameObject.SetActive(false);
        }
    }

    public void SendVehicle()
    {
        isWaitingForLoad = false;
        userSentVehicle = true;
        Debug.Log("User sent vehicle manually.");
    }

    void MoveTowardsWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }
    }

    public void AddProduct(string product)
    {
        products.Add(product);
        UpdateProductUI();
    }

    void UpdateProductUI()
    {
        if (productCountText != null)
        {
            productCountText.text = "Products on board: " + products.Count;
        }
    }
}