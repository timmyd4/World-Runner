using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SphericalPlayerController : MonoBehaviour
{
    public Transform planet;
    public Transform cameraTransform;
    public float moveSpeed = 10f;
    public float gravityForce = 10f;
    public float rotationSmooth = 2f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        // 1. Gravity
        Vector3 gravityDir = (transform.position - planet.position).normalized;
        rb.AddForce(-gravityDir * gravityForce, ForceMode.Acceleration);

        // 2. Align with surface
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, gravityDir) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmooth * Time.deltaTime);

        // 3. Input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 4. Camera-relative input
        Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, gravityDir).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, gravityDir).normalized;
        Vector3 moveDir = (camForward * v + camRight * h).normalized;

        // 5. Move
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Vector3 targetPos = rb.position + moveDir * moveSpeed * Time.deltaTime;
            rb.MovePosition(targetPos);

            Quaternion faceRotation = Quaternion.LookRotation(moveDir, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, faceRotation, rotationSmooth * Time.deltaTime);
        }
    }
}
