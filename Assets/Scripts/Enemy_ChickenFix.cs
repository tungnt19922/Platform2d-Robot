using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemy_ChickenFix : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Move info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float idleTime = 2;
    private float idleTimeCounter;

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;

    private bool groundDetected;
    private bool wallDetected;
    private int facingDirection = -1;

    private bool isKilled;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("xVelocity", rb.velocity.x);
        CollisionCheck();

        idleTimeCounter -= Time.deltaTime;
        if (idleTimeCounter < 0)
            rb.velocity = new Vector2(moveSpeed * facingDirection, rb.velocity.y);

        else
            rb.velocity = new Vector2(0, 0);

        if (wallDetected || !groundDetected)
        {
            Flip();
            idleTimeCounter = idleTime;
        }
        AnimatorController();
    }

    private void Flip()
    {
        facingDirection = facingDirection * -1;
        transform.Rotate(0, 180, 0);
    }

    private void CollisionCheck()
    {
        wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));
    }

    private void AnimatorController()
    {
        anim.SetBool("isKilled", isKilled);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isKilled = true;
            Debug.Log("tieu diet enemy");
            Destroy(gameObject);
        }
    }

}
