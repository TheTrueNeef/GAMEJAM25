using UnityEngine;
using System.Collections;

public class MachineProduction : MonoBehaviour
{
    public GameObject producedObject; // The object created after production
    public float productionTime = 5f; // Time before object is produced
    public float maxPressure = 100f; // Max pressure before machine stops
    public float pressureBuildRate = 10f; // Pressure increase per second when ON
    public float pressureReleaseRate = 5f; // Pressure decrease per second when OFF
    public float cooldownTime = 3f; // Cooldown time after emergency is handled

    public GameObject[] fireObjects = new GameObject[3]; // Fire objects with triggers

    public float currentPressure = 0f;
    public bool isMachineOn = false;
    public bool isProducing = false;
    public bool isCoolingDown = false;
    public bool isOnFire = false;

    void Start()
    {
        if (producedObject != null)
        {
            producedObject.SetActive(false); // Ensure production object starts disabled
        }

        foreach (var obj in fireObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false); // Ensure fire objects start disabled
            }
        }
    }

    void Update()
    {
        if (isMachineOn && !isProducing)
        {
            StartProduction();
        }

        if (isMachineOn && isProducing)
        {
            BuildPressure();
        }
        else if (!isMachineOn && currentPressure > 0)
        {
            ReleasePressure();
        }

        if (isOnFire)
        {
            CheckFireObjects();
        }
    }

    public void ToggleMachine()
    {
        if (isOnFire || isCoolingDown)
        {
            Debug.Log("Cannot start machine while on fire or cooling down.");
            return;
        }

        isMachineOn = !isMachineOn;

        if (isMachineOn)
        {
            StartProduction();
        }
        else
        {
            StopProduction();
        }
    }

    void StartProduction()
    {
        if (!isProducing && !isCoolingDown && !isOnFire)
        {
            StartCoroutine(ProduceItem());
        }
    }

    void StopProduction()
    {
        isProducing = false;
        Debug.Log("Machine turned off. Pressure is slowly releasing...");
    }

    IEnumerator ProduceItem()
    {
        isProducing = true;
        Debug.Log("Production started. Waiting for " + productionTime + " seconds...");

        yield return new WaitForSeconds(productionTime);

        if (!isOnFire && isMachineOn)
        {
            if (producedObject != null)
            {
                producedObject.SetActive(true);
                Debug.Log("Production complete! " + producedObject.name + " enabled.");
            }
        }
        else
        {
            Debug.Log("Production failed. Fire or machine turned off.");
        }

        isProducing = false;
    }

    void BuildPressure()
    {
        if (currentPressure < maxPressure)
        {
            currentPressure += pressureBuildRate * Time.deltaTime;
            Debug.Log($"Current Pressure: {currentPressure}/{maxPressure}");
        }
        else
        {
            TriggerFire();
        }
    }

    void ReleasePressure()
    {
        currentPressure -= pressureReleaseRate * Time.deltaTime;
        if (currentPressure < 0) currentPressure = 0;
        Debug.Log($"Pressure releasing: {currentPressure}/{maxPressure}");
    }

    void TriggerFire()
    {
        if (!isOnFire)
        {
            isProducing = false;
            isMachineOn = false;
            isOnFire = true;

            if (producedObject != null && producedObject.activeSelf)
            {
                producedObject.SetActive(false);
            }

            foreach (var obj in fireObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }

            Debug.Log("Machine has overheated! Fire started. Handle the emergency.");
        }
    }

    public void ExtinguishFire(GameObject fireObject)
    {
        if (fireObject != null)
        {
            fireObject.SetActive(false); // Disable this fire object
            Debug.Log("Fire extinguished: " + fireObject.name);

            CheckFireObjects(); // Check if all fires are out
        }
    }

    void CheckFireObjects()
    {
        foreach (var obj in fireObjects)
        {
            if (obj != null && obj.activeSelf)
            {
                return; // Fire still exists, no cooldown yet
            }
        }

        // All fire objects are disabled, fire is put out
        Debug.Log("All fires extinguished. Starting cooldown...");
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        isOnFire = false;
        isCoolingDown = true;

        yield return new WaitForSeconds(cooldownTime);

        isCoolingDown = false;
        Debug.Log("Machine ready for next production.");
    }
}
