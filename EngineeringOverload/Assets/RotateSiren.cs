using UnityEngine;

public class RotateSiren : MonoBehaviour
{
    public float rotationSpeed = 100f; // Rotation speed in degrees per second
    public AudioSource audioSource; // Reference to the AudioSource component

    private bool isPlaying = false;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        RotateObject();
    }

    void RotateObject()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        if (!isPlaying && audioSource != null)
        {
            audioSource.Play();
            isPlaying = true;
        }
    }
}
