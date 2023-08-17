using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    [Header("Moveinfo")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 12;

    private bool canMove = true;

    private bool canDoubleJump = true;
    private bool canWallSlide;
    private bool isWallSliding;

    private bool facingRight = true;
    private float movingImput;

    [Header("Collision infor")]
    [SerializeField] private Transform groudCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    private bool isWallDetected;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput ();



        CollisionCheck();
        FlipController();
        AnimatorController();
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            canDoubleJump = true;
        }
        if (isWallDetected && canWallSlide)
        {
            isWallSliding = true;
            rb.velocity = new Vector2 (rb.velocity.x, rb.velocity.y * 0.1f);
        }
        else
        {
            isWallSliding = false;
            Move();
        }
    }

    private void CheckInput()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }
        if (canMove)
            movingImput = Input.GetAxisRaw("Horizontal");

    }

    private void Move()
    {
        if (canMove)
            rb.velocity = new Vector2(movingImput * speed, rb.velocity.y);
    }

    private void JumpButton()
    {
        if (isGrounded)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }


    private void AnimatorController()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isWallSliding", isWallSliding);
    }
    private void FlipController()
    {

        if (isGrounded && isWallDetected)
        {
            if (facingRight && movingImput < 0)
                Flip();
            if (!facingRight && movingImput > 0)
                Flip();
        }


        if (rb.velocity.x > 0 && !facingRight)
            Flip();
        else if (rb.velocity.x < 0 && facingRight)
            Flip();
    }
    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
    }

    private void CollisionCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groudCheck.position, groundCheckRadius, whatIsGround);
        isWallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, whatIsGround);

        if (!isGrounded && rb.velocity.y < 0)
            canWallSlide = true;

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groudCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }

}