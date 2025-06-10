using UnityEngine;
using Cinemachine;

public class CinemachineMouseLook : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCam;
    public float sensitivityX = 3f;
    public float sensitivityY = 2f;

    private CinemachineComposer composer;
    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        composer = virtualCam.GetCinemachineComponent<CinemachineComposer>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        xRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation - mouseY, -30f, 60f);

        composer.m_TrackedObjectOffset = Quaternion.Euler(yRotation, xRotation, 0) * Vector3.forward * 10f;
    }
}
