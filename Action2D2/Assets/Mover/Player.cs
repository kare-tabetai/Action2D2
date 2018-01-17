using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : CharacterMover
{
    public float jumpVelocity;
    public float moveSpeed;

    Rigidbody2D rb;
    SpriteRenderer sprRenderer;

    const float SpriteSize = 2.0f;
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    void Update () {
        Move();
    }

    protected override void Move()
    {
        bool grounded = GroundedCheck();
        print(grounded);

        Walk();
        Jump(grounded);
    }

    const float groundColliderOffest = 1.0f;
    bool GroundedCheck()
    {
        var hitSize = sprRenderer.sprite.bounds.size.y / 4.0f;
        var pointA = (Vector2)transform.position + new Vector2(-hitSize, 0);
        var pointB = (Vector2)transform.position + new Vector2(hitSize, -groundColliderOffest);
        var hit = Physics2D.OverlapArea(pointA, pointB, LayerMask.GetMask("Stage"));
        if (hit) return true;
        return false;
    }
    void DrawGroundCollier()
    {
        var hitSize = GetComponent<SpriteRenderer>().sprite.bounds.size.y / 4.0f;
        Gizmos.DrawWireCube((Vector2)transform.position + new Vector2(0, -groundColliderOffest / 2), new Vector2(hitSize * 2, groundColliderOffest));
    }

    void Walk()
    {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * timeScale * moveSpeed, rb.velocity.y);
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            sprRenderer.flipX = true;
        }
        if (0 < Input.GetAxisRaw("Horizontal"))
        {
            sprRenderer.flipX = false;
        }
    }

    public float jummpingTime;
    float jummpingTimer;
    bool jumppingFlag;
    void Jump(bool grounded)
    {
        if (jumppingFlag)
        {
            jummpingTimer += Time.deltaTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);

            if (jummpingTime <= jummpingTimer)
            {
                jumppingFlag = false;
            }
        }
        

        if (!Input.GetButton("Jump"))
        {
            jumppingFlag = false;
        }
        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                jumppingFlag = true;
                jummpingTimer = 0;
                rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
            }
            
        }
    }

    public override void Damaged(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("GameOver");
        }
    }
    private void OnDrawGizmos()
    {
        DrawGroundCollier();
    }
}