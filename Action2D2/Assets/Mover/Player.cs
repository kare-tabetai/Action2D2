using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : CharacterMover
{
    public float jumpForce;
    public float moveSpeed;

    Rigidbody2D rb;
    SpriteRenderer sprRenderer;
    AnimationState animationState = AnimationState.Standing;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    void Update () {
        Move();
    }

    bool GroundedCheck()
    {
        RaycastHit2D grounded = Physics2D.BoxCast(
            (Vector2)transform.position + Vector2.up * Mathf.Epsilon,
            boxCol.bounds.size,
            0.0f,
            Vector2.down,
            0.1f * boxCol.bounds.size.y,
            LayerMask.GetMask("Stage")
            );
        if (!grounded.collider) SetAnimation(AnimationState.Jumping);
        else SetAnimation(AnimationState.Walking);

        return grounded.collider;
    }

    protected override void Move()
    {
        bool grounded = GroundedCheck();

        //Walk
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * timeScale * moveSpeed, rb.velocity.y);
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            sprRenderer.flipX = true;
            SetAnimation(AnimationState.Walking);
        }
        if (0 < Input.GetAxisRaw("Horizontal"))
        {
            sprRenderer.flipX = false;
            SetAnimation(AnimationState.Walking);
        }

        //Jump
        if (Input.GetButtonDown("Jump")&&grounded)
        {
            rb.AddForce(jumpForce * Vector2.up);
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

    void SetAnimation(AnimationState st)
    {
        /*
        switch (st)
        {
            case State.walking:
                state = State.Walking;
                walkingAnime.enabled = true;
                jumpingAnime.enabled = false;
                break;

            case State.jumping:
                state = State.Jumping;
                walkingAnime.enabled = false;
                jumpingAnime.enabled = true;
                break;
        }
        */
    }
}