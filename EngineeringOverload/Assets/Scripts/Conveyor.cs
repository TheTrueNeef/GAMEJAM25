using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float speed = 2.0f; // Speed at which the box moves
    public Transform front; // Reference to the front position

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && front != null)
            {
                Vector3 direction = (front.position - other.transform.position).normalized;
                rb.linearVelocity = direction * speed;
            }
        }
    }
}
