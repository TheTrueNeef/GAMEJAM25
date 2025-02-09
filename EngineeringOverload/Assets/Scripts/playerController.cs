using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpHeight = 2f;
    public float gravity = 9.8f;
    public float mouseSensitivity = 2f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

    public Transform cameraHolder;
    private bool isSprinting;

    public List<GameObject> heldObjects = new List<GameObject>(5);
    public Transform objectHolder; // The empty Transform where objects will be placed

    public ParticleSystem extinguisherEffect; // Fire extinguisher effect
    public AudioSource extinguisherSound; // Fire extinguisher sound
    public float extinguisherRange = 3f; // How far the extinguisher reaches
    public LayerMask fireLayer; // Layer mask for fire objects

    private int selectedObjectIndex = 0;
    private bool isExtinguishing = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleJump();
        ApplyGravity();

        HandleItemSelection();

        if (Input.GetButton("Fire1"))
        {
            UseSelectedObject();
        }
        else if (isExtinguishing)
        {
            StopExtinguisher();
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 75f);
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        isSprinting = Input.GetKey(KeyCode.LeftShift) && (moveX != 0 || moveZ != 0);
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }
    }

    void ApplyGravity()
    {
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleItemSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectObject(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectObject(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectObject(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectObject(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectObject(4);
    }

    void SelectObject(int index)
    {
        if (index >= 0 && index < heldObjects.Count)
        {
            selectedObjectIndex = index;
            UpdateHeldObject();
        }
    }

    void UpdateHeldObject()
    {
        for (int i = 0; i < heldObjects.Count; i++)
        {
            if (heldObjects[i] != null)
            {
                heldObjects[i].SetActive(i == selectedObjectIndex);
            }
        }
    }

    void UseSelectedObject()
    {
        if (heldObjects[selectedObjectIndex] != null)
        {
            switch (selectedObjectIndex)
            {
                case 0: // Fire Extinguisher
                    UseFireExtinguisher();
                    break;
                case 1:
                    UseWrench();
                    break;
                case 2:
                    UseBlower();
                    break;
                case 3:
                    UseCard();
                    break;
                case 4:
                    UseDrill();
                    break;
                default:
                    Debug.Log("No function assigned to this item.");
                    break;
            }
        }
    }

    void UseFireExtinguisher()
    {
        if (!isExtinguishing)
        {
            isExtinguishing = true;
            if (extinguisherEffect != null) extinguisherEffect.Play();
            if (extinguisherSound != null) extinguisherSound.Play();
        }

        // Detect fire objects in front of the player
        Collider[] fireObjects = Physics.OverlapSphere(transform.position + transform.forward * 1.5f, extinguisherRange, fireLayer);
        foreach (Collider fire in fireObjects)
        {
            Debug.Log($"Extinguishing {fire.name}");
            fire.gameObject.SetActive(false);
        }

        // If no fire objects are detected, stop extinguishing
        if (fireObjects.Length == 0)
        {
            StopExtinguisher();
        }
    }

    void StopExtinguisher()
    {
        if (isExtinguishing)
        {
            isExtinguishing = false;
            if (extinguisherEffect != null) extinguisherEffect.Stop();
            if (extinguisherSound != null) extinguisherSound.Stop();
            Debug.Log("Stopped Fire Extinguisher.");
        }
    }

    void UseWrench()
    {
        Debug.Log("Using Wrench!");
    }

    void UseBlower()
    {
        Debug.Log("Using Blower!");
    }

    void UseCard()
    {
        Debug.Log("Using Card!");
    }

    void UseDrill()
    {
        Debug.Log("Using Drill!");
    }
}
