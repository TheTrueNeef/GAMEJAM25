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

    public GameObject extinguisherTrigger; // Assign in inspector (Box Collider trigger)
    public GameObject extinguisherEffectPrefab; // Fire extinguisher particle system prefab
    public AudioSource extinguisherSound; // Fire extinguisher sound
    public Transform extinguisherSpawnPoint; // Where the effect spawns

    private int selectedObjectIndex = 0;
    private bool isExtinguishing = false;
    private GameObject activeExtinguisherEffect; // Stores instantiated effect

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (extinguisherTrigger != null)
        {
            extinguisherTrigger.SetActive(false); // Ensure it's off at start
        }
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleJump();
        ApplyGravity();

        HandleItemSelection();

        if (Input.GetButtonDown("Fire1"))
        {
            UseSelectedObject();
        }
        if (Input.GetButtonUp("Fire1"))
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
                    StartExtinguisher();
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

    void StartExtinguisher()
    {
        if (!isExtinguishing)
        {
            isExtinguishing = true;

            // Instantiate extinguisher effect
            if (extinguisherEffectPrefab != null && extinguisherSpawnPoint != null)
            {
                activeExtinguisherEffect = Instantiate(extinguisherEffectPrefab, extinguisherSpawnPoint.position, extinguisherSpawnPoint.rotation);
                activeExtinguisherEffect.transform.SetParent(extinguisherSpawnPoint); // Attach to spawn point
            }

            if (extinguisherSound != null) extinguisherSound.Play();

            if (extinguisherTrigger != null)
            {
                extinguisherTrigger.SetActive(true);
            }

            Debug.Log("Fire Extinguisher Activated!");
        }
    }

    void StopExtinguisher()
    {
        if (isExtinguishing)
        {
            isExtinguishing = false;

            // Destroy extinguisher effect when stopping
            if (activeExtinguisherEffect != null)
            {
                Destroy(activeExtinguisherEffect);
                activeExtinguisherEffect = null;
            }

            if (extinguisherSound != null) extinguisherSound.Stop();

            if (extinguisherTrigger != null)
            {
                extinguisherTrigger.SetActive(false);
            }

            Debug.Log("Fire Extinguisher Stopped.");
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
