using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 200f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public LayerMask groundMask;
    public int maxJumps = 2;

    private CharacterController controller;
    private CapsuleCollider capsule;
    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGroundedLastFrame = false;
    private int jumpCount = 0;
    private float jumpCooldown = 0.1f;
    private float jumpCooldownTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        capsule = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        if (jumpCooldownTimer > 0f)
            jumpCooldownTimer -= Time.deltaTime;

        isGrounded = IsGroundedByCollider();

        if (isGrounded && !wasGroundedLastFrame && jumpCooldownTimer <= 0f)
        {
            jumpCount = 0;
            if (velocity.y < 0)
                velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;

            if (jumpCooldownTimer <= 0f)
                jumpCooldownTimer = jumpCooldown;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0)
            transform.Rotate(Vector3.up, h * rotationSpeed * Time.deltaTime);

        Vector3 move = transform.forward * v;

        velocity.y += gravity * Time.deltaTime;

        controller.Move((move * speed + velocity) * Time.deltaTime);

        wasGroundedLastFrame = isGrounded;
    }

    bool IsGroundedByCollider()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        float rayLength = capsule.bounds.extents.y + 0.05f;
        return Physics.Raycast(origin, Vector3.down, rayLength, groundMask);
    }
}
