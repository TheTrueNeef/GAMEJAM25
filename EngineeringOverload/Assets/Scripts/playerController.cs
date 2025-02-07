using UnityEngine;

public class playerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpHeight = 2f;
    public float gravity = 9.8f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the game window
    }

    void Update()
    {
        // Check if the player is on the ground
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Prevents sticking to slopes
        }

        // Get input
        float moveX = Input.GetAxis("Horizontal");  // A/D or Left/Right
        float moveZ = Input.GetAxis("Vertical");    // W/S or Up/Down

        // Convert to world space movement
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Apply movement
        controller.Move(move * speed * Time.deltaTime);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }

        // Apply gravity
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
