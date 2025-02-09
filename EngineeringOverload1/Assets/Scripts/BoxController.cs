using UnityEngine;

public class BoxController : MonoBehaviour
{
    private Rigidbody rb;
    private int conveyorCount = 0; // Tracks how many conveyors the box is touching

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Conveyor"))
        {
            conveyorCount++;
            rb.useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Conveyor"))
        {
            conveyorCount--;

            if (conveyorCount <= 0) 
            {
                rb.useGravity = true;
                rb.linearVelocity = Vector3.zero;
            }
        }
    }
}
