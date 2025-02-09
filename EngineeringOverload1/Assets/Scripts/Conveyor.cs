using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public float speed = 2.0f; // Speed at which the box moves
    public Transform front; // Reference to the front position

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            if (front != null)
            {
                Vector3 direction = (front.position - other.transform.position).normalized;
                other.transform.position += direction * speed * Time.deltaTime;
            }
        }
    }
}
