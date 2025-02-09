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

    public GameObject[] fireObjects = new GameObject[3]; // Objects that act as fire

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
        if(isMachineOn && !isProducing) 
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

        if (!isOnFire && isMachineOn) // Only enable if machine didn't overheat and is still on
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
        if (!isOnFire) // Ensure fire only triggers once
        {
            isProducing = false;
            isMachineOn = false;
            isOnFire = true;

            if (producedObject != null && producedObject.activeSelf)
            {
                producedObject.SetActive(false); // Only disable if it was produced
            }

            foreach (var obj in fireObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(true); // Activate fire objects
                }
            }

            Debug.Log("Machine has overheated! Fire started. Handle the emergency.");
        }
    }

    void CheckFireObjects()
    {
        foreach (var obj in fireObjects)
        {
            if (obj != null && obj.activeSelf)
            {
                return; // Exit if any fire object is still active
            }
        }

        // All fire objects are disabled, fire is put out
        Debug.Log("Fire extinguished. Starting cooldown...");
        StartCoroutine(Cooldown());
    }
    public void StartMachine()
    {
        if (!isProducing && !isMachineOn)
        {
            isMachineOn = true;
        }
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
