using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Range(0f, 100f)]
    public float Speed;

    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    private bool grounded;

    [Header("Rigidbody")]
    public Rigidbody rb;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;


    void Start()
    {
        
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);

        MyInput();
        SpeedControl();

        if(grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate() 
    {
       MovePlayer(); 
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = Vector3.forward * verticalInput + Vector3.right * horizontalInput;

        rb.AddForce(moveDirection * Speed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > Speed)
        {
            Vector3 limitedVel = flatVel.normalized * Speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

}
