using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController_RB : MonoBehaviour
{
    [Header("Player Settings")]
    public float speed = 10.0f;
    public float jumpForce = 10.0f;
    public float maxJumpHeight = 10.0f;

    [Header("References")]
    public ParticleSystem landingParticles;
    public ParticleSystem takeOffParticles;

    private Rigidbody _playerRB;
    private Transform _playerMesh;
    private FakeGravityBody _worldGravity;
    private List<GameObject> _worlds = new List<GameObject>();
    private int _currentWorld = 0;
    private Vector3 _moveDirection;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _playerRB = GetComponent<Rigidbody>();
        _playerMesh = transform.GetChild(0).transform;
        _worldGravity = GetComponent<FakeGravityBody>();

        _worlds.AddRange(GameObject.FindGameObjectsWithTag("World"));
        _currentWorld = GetCurrentWorldIndex();
    }

    private void Update()
    {
        if (_worldGravity.Attractor == null) return;

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        Vector3 gravityUp = (transform.position - _worldGravity.Attractor.transform.position).normalized;

        camForward = Vector3.ProjectOnPlane(camForward, gravityUp).normalized;
        camRight = Vector3.ProjectOnPlane(camRight, gravityUp).normalized;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        _moveDirection = (camForward * v + camRight * h).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        RotateForward();
    }

    private void FixedUpdate()
    {
        _playerRB.MovePosition(_playerRB.position + _moveDirection * speed * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        float jumpHeight = Vector3.Distance(_worlds[_currentWorld].transform.position, transform.position) - maxJumpHeight;

        if (jumpHeight < maxJumpHeight)
        {
            Vector3 gravityDir = (_worlds[_currentWorld].transform.position - transform.position).normalized;
            _playerRB.AddForce(-gravityDir * jumpForce, ForceMode.Impulse);
            takeOffParticles?.Play();
        }
    }

    private void RotateForward()
    {
        if (_moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_moveDirection, transform.up);
            _playerMesh.rotation = Quaternion.Slerp(_playerMesh.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    private int GetCurrentWorldIndex()
    {
        string worldName = _worldGravity.Attractor.gameObject.name;
        for (int i = 0; i < _worlds.Count; i++)
        {
            if (_worlds[i].name == worldName) return i;
        }
        return 0;
    }
}
