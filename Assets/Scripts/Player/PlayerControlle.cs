using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController_RB : MonoBehaviour
{
    [Header("Player Settings")]
    public float speed = 10.0f;
    public float jumpForce = 50.0f;
    public float mouseSensitivity = 3.0f;

    [Header("References")]
    public Transform cameraRigTransform;  // Reference to CameraRig (NOT the camera directly!)
    public Transform meshTransform;
    private Rigidbody _playerRB;
    private FakeGravityBody _worldGravity;

    private Vector3 _moveDirection;
    private float verticalRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _playerRB = GetComponent<Rigidbody>();
        _worldGravity = GetComponent<FakeGravityBody>();
    }

    void Update()
    {
        if (_worldGravity.Attractor == null) return;

        HandleMouseLook();
        HandleMovement();
        RotateMesh();
    }

    void FixedUpdate()
    {
        _playerRB.MovePosition(_playerRB.position + _moveDirection * speed * Time.fixedDeltaTime);
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        Vector3 gravityUp = (transform.position - _worldGravity.Attractor.transform.position).normalized;

        // Yaw rotation (around gravity up)
        Quaternion yawRotation = Quaternion.AngleAxis(mouseX, gravityUp);
        cameraRigTransform.rotation = yawRotation * cameraRigTransform.rotation;

        // Pitch rotation (local right axis)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);

        Vector3 rightAxis = cameraRigTransform.right;
        cameraRigTransform.rotation = Quaternion.AngleAxis(-mouseY, rightAxis) * cameraRigTransform.rotation;
    }

    private void HandleMovement()
    {
        Vector3 gravityUp = (transform.position - _worldGravity.Attractor.transform.position).normalized;

        // Get camera forward/right relative to gravity
        Vector3 camForward = Vector3.ProjectOnPlane(cameraRigTransform.forward, gravityUp).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(cameraRigTransform.right, gravityUp).normalized;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        _moveDirection = (camForward * v + camRight * h).normalized;
    }

    private void RotateMesh()
    {
        if (_moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_moveDirection, transform.up);
            meshTransform.rotation = Quaternion.Slerp(meshTransform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }
}
