using UnityEngine;

public class FireObject : MonoBehaviour
{
    private bool playerInRange = false;
    private playerController player;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.GetComponent<playerController>();
            Debug.Log("Player is near fire: " + gameObject.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            Debug.Log("Player left fire area: " + gameObject.name);
        }
    }

    void Update()
    {
        if (playerInRange && player != null)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButton("Fire1"))
            {
                ExtinguishFire();
            }
        }
    }

    void ExtinguishFire()
    {
        Debug.Log("Fire Extinguished: " + gameObject.name);
        gameObject.SetActive(false); // Disable fire object
    }
}
