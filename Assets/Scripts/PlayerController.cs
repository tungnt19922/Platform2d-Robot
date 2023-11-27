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
    private int facingDirection = 1;
    [SerializeField] private Vector2 wallJumpDirection;


    [Header("Collision infor")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;

    [Header("PlayerHealth")]
    [SerializeField] private float health = 3;

    private bool isGrounded;
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
        CollisionCheck();
        FlipController();
        AnimatorController();
        CheckInput ();

        if (isGrounded)
        {
            canMove = true;
            canDoubleJump = true;
        }

        if (canWallSlide)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.1f);
        }
        Move();
    }

    private void CheckInput()
    {
        movingImput = Input.GetAxisRaw("Horizontal");

        if (Input.GetAxis("Vertical") < 0)
        {
            canWallSlide = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }
    }

    private void Move()
    {
        if (canMove)
            rb.velocity = new Vector2(movingImput * speed, rb.velocity.y);
    }

    private void JumpButton()
    {
        if(isWallSliding)
        {
            WallJump();
        }

        else if (isGrounded)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            canMove = true;
            canDoubleJump = false;
            Jump();
        }
        canWallSlide = false;

    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void WallJump()
    {
        canMove = false;
        canDoubleJump = true;

        rb.velocity = new Vector2(wallJumpDirection.x * -facingDirection, wallJumpDirection.y);

    }

    private void AnimatorController()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("isWallDetected", isWallDetected);
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
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
    }

    private void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down,groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);

        if (isWallDetected && rb.velocity.y < 0)
            canWallSlide = true;
        if (!isWallDetected)
        {
            canWallSlide = false;
            isWallSliding = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance * facingDirection, transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Va cham voi enemy");
        }
    }

}
