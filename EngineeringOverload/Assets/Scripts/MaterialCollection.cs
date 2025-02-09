using Unity.VisualScripting;
using UnityEngine;

public class MaterialCollection : MonoBehaviour
{
    int collectedMaterial = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box")) {
            Destroy(other.GameObject());
            collectedMaterial++;
        }
    }
}
