using UnityEngine;

public class ExtinguisherTrigger : MonoBehaviour
{
    public GameObject extinguisherObject;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FireObject")) // Only affect objects with the "FireObject" tag
        {
            Debug.Log("Extinguished Fire: " + other.gameObject.name);
            other.gameObject.SetActive(false); // Disable fire object
        }
    }
}
