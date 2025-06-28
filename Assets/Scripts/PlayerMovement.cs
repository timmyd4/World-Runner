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

    private CharacterController controller;
    private CapsuleCollider capsule;
    private Vector3 velocity;
    private bool isGrounded;
    private bool canJump = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        capsule = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        isGrounded = IsGroundedByCollider();

        // Draw the ray in Scene view for debug
        Debug.DrawRay(GetRayOrigin(), Vector3.down * GetRayLength(), Color.red);

        if (isGrounded && velocity.y <= 0)
        {
            canJump = true;
            if (velocity.y < 0)
                velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && canJump && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            canJump = false;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0)
            transform.Rotate(Vector3.up, h * rotationSpeed * Time.deltaTime);

        Vector3 move = transform.forward * v;

        velocity.y += gravity * Time.deltaTime;

        controller.Move((move * speed + velocity) * Time.deltaTime);
    }

    bool IsGroundedByCollider()
    {
        return Physics.Raycast(GetRayOrigin(), Vector3.down, GetRayLength(), groundMask);
    }

    Vector3 GetRayOrigin()
    {
        return transform.position + Vector3.up * 0.1f;
    }

    float GetRayLength()
    {
        return capsule.bounds.extents.y + 0.2f;
    }
}
