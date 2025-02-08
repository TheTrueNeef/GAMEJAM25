using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.linearVelocity = transform.right * rb.linearVelocity.magnitude;
            }
        }
    }
    void Update()
    {
        
    }
}
