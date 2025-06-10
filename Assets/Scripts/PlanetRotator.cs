using UnityEngine;

public class PlanetRotator : MonoBehaviour
{
    public Transform cameraTransform;  // Assign the main camera
    public float rotationSpeed = 10f;

    void Update()
{
    float v = Input.GetAxis("Vertical");

    if (Mathf.Abs(v) > 0.01f)
    {
        // Get the forward vector of the camera, flattened
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        // Determine rotation axis
        Vector3 axis = Vector3.Cross(camForward, Vector3.up);

        // Rotate based on whether moving forward or backward
        float direction = v > 0 ? -1f : 1f;
        transform.Rotate(axis, rotationSpeed * Time.deltaTime * direction, Space.World);
    }
}
}
