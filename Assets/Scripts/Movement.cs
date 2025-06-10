using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SphericalPlayerController : MonoBehaviour
{
    public Transform planet;
    public Transform cameraTransform;
    public float moveSpeed = 10f;
    public float gravityForce = 10f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        // 1. Apply custom gravity
        Vector3 gravityDirection = (transform.position - planet.position).normalized;
        Vector3 gravity = -gravityDirection * gravityForce;
        rb.AddForce(gravity, ForceMode.Acceleration);

        // 2. Align player's up with gravity
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, gravityDirection) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);

        // 3. Get input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 4. Get camera-relative movement direction
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveInput = (camForward * v + camRight * h).normalized;

        // 5. Project movement to planet surface
        Vector3 moveDir = Vector3.ProjectOnPlane(moveInput, gravityDirection).normalized;

        // 6. Move player
        Vector3 targetPosition = rb.position + moveDir * moveSpeed * Time.deltaTime;
        rb.MovePosition(targetPosition);
    }
}
