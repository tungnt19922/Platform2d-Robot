using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemy_ChickenFix : MonoBehaviour
{
    private Rigidbody2D enemyrb;
    private Rigidbody2D playerrb;
    private Animator anim;
    private PlayerController playerController;

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
        enemyrb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerrb = FindObjectOfType<PlayerController>().GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("xVelocity", enemyrb.velocity.x);
        CollisionCheck();

        idleTimeCounter -= Time.deltaTime;
        if (idleTimeCounter < 0)
            enemyrb.velocity = new Vector2(moveSpeed * facingDirection, enemyrb.velocity.y);

        else
            enemyrb.velocity = new Vector2(0, 0);

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
            moveSpeed = 0;
            if (playerrb != null)
            {
                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                float pushForce = 20f;
                playerrb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
                Debug.Log("co luc day len");
            }
            StartCoroutine(DestroyAfterAnimation());
        }
    }
    IEnumerator DestroyAfterAnimation()
    {       
        if (anim != null)
        {
            anim.SetTrigger("isKilled"); 
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        }
        Destroy(gameObject);
    }
}
