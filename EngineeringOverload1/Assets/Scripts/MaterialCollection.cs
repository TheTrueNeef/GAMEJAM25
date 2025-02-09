using Unity.VisualScripting;
using UnityEngine;

public class MaterialCollection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
            Destroy(other.GameObject());
    }
}
