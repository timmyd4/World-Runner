using UnityEngine;

public class SphericalCamera : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform planetTransform;
    [SerializeField] private float distance = 10f;
    [SerializeField] private float height = 5f;
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private float movementFollowSpeed = 2f;

    private float yaw = 0f;
    private float pitch = 20f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        HandleInput();
        AutoAlignToMovement();
        UpdateCameraPosition();
    }

    private void HandleInput()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -60f, 60f);
    }

    private void AutoAlignToMovement()
    {
        Vector3 gravityUp = (playerTransform.position - planetTransform.position).normalized;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(h, 0, v);

        if (inputDir.sqrMagnitude > 0.1f)
        {
            // Convert input to world space relative to gravity up
            Vector3 camForward = Vector3.ProjectOnPlane(transform.forward, gravityUp).normalized;
            Vector3 camRight = Vector3.Cross(gravityUp, camForward);

            Vector3 moveWorldDir = (camForward * v + camRight * h).normalized;

            // Calculate angle difference and smoothly rotate yaw
            float targetYaw = Mathf.Atan2(moveWorldDir.x, moveWorldDir.z) * Mathf.Rad2Deg;
            yaw = Mathf.LerpAngle(yaw, targetYaw, movementFollowSpeed * Time.deltaTime);
        }
    }

    private void UpdateCameraPosition()
    {
        Vector3 gravityUp = (playerTransform.position - planetTransform.position).normalized;

        Quaternion rotation = Quaternion.AngleAxis(yaw, gravityUp) * Quaternion.AngleAxis(pitch, Vector3.right);

        Vector3 offset = rotation * new Vector3(0, height, -distance);
        transform.position = playerTransform.position + offset;

        transform.LookAt(playerTransform, gravityUp);
    }
}
