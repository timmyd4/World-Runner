using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 50f;
    private Vector3 moveDirection;
    private Rigidbody rb;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        moveDirection = (transform.forward * v + transform.right * h).normalized;
    }
    
    void FixedUpdate()
    {
        rb.velocity = moveDirection * speed;
    }
}
