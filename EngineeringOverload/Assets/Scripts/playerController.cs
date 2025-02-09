using UnityEngine;
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

    private int selectedObjectIndex = 0;

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

        if (Input.GetButtonDown("Fire1"))
        {
            UseSelectedObject();
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

    public void SnapToHolder(GameObject item)
    {
        if (objectHolder == null)
        {
            Debug.LogError("Object Holder transform is not assigned!");
            return;
        }

        item.transform.SetParent(objectHolder);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
    }

    void UseSelectedObject()
    {
        if (heldObjects[selectedObjectIndex] != null)
        {
            // Call the function based on the selected object
            switch (selectedObjectIndex)
            {
                case 0:
                    useFire();
                    break;
                case 1:
                    useWrench();
                    break;
                case 2:
                    useBlower();
                    break;
                case 3:
                    useCard();
                    break;
                case 4:
                    useDrill();
                    break;
                default:
                    Debug.Log("No function assigned to this item.");
                    break;
            }
        }
    }

    // Example functions for each object
    void useFire()
    {
        Debug.Log("Using Fire tool!");
    }

    void useWrench()
    {
        Debug.Log("Using Wrench!");
    }

    void useBlower()
    {
        Debug.Log("Using Blower!");
    }

    void useCard()
    {
        Debug.Log("Using Card!");
    }

    void useDrill()
    {
        Debug.Log("Using Drill!");
    }
}
