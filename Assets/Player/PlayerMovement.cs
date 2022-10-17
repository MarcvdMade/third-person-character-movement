using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Range(0f, 100f)]
    public float Speed, RotationSpeed;

    public float groundDrag;

    public float jumpForce, jumpCooldown, airMultiplier;

    public bool readyToJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    public bool grounded;

    [Header("Components")]
    public Rigidbody rb;

    public Animator animator;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;


    void Start()
    {
        readyToJump = true;
    }

    void Update()
    {
        grounded = Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Vector3.down, 1.2f, groundLayer);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Vector3.down, Color.red);

        MyInput();
        SpeedControl();

        if(grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        if (rb.velocity.magnitude == 0)
            animator.SetBool("Moving", false);
    }

    private void FixedUpdate() 
    {
       MovePlayer(); 
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = Vector3.forward * verticalInput + Vector3.right * horizontalInput;
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, RotationSpeed);
        }

        if (grounded)
        {
            rb.AddForce(moveDirection * Speed * 10f, ForceMode.Force);
            animator.SetBool("Moving", true);
            animator.SetBool("Grounded", true);
        } else if (!grounded)
        {
            rb.AddForce(moveDirection * Speed * 10f * airMultiplier, ForceMode.Force);
            animator.SetBool("Moving", false);
            animator.SetBool("Grounded", false);
        }
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

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

}
