using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 200f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    public int maxJumps = 2;
    private int jumpCount = 0;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded)
        {
            jumpCount = 0;
            if (velocity.y < 0)
                velocity.y = -2f;
        }

        // Jump (before movement)
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;
        }

        // Input
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0)
            transform.Rotate(Vector3.up, h * rotationSpeed * Time.deltaTime);

        Vector3 move = transform.forward * v;

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Move controller
        controller.Move((move * speed + velocity) * Time.deltaTime);
    }
}
