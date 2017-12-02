﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : CharacterMover
{
    public Animation2D walkingAnime;
    public Animation2D jumpingAnime;
    public float jumpForce;
    public float moveSpeed;

    Rigidbody2D rb;
    BoxCollider2D boxCol;
    SpriteRenderer sprRenderer;
    RaycastHit2D grounded;
    State state = State.walking;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    void Update () {
        grounded = Physics2D.BoxCast(
            (Vector2)transform.position+Vector2.up*Mathf.Epsilon,
            boxCol.bounds.size,
            0.0f,
            Vector2.down,
            0.1f* boxCol.bounds.size.y,
            LayerMask.GetMask("Stage")
            );
        if (!grounded.collider) SetState(State.jumping);
        else SetState(State.walking);
        Move();
    }

    void FixedUpdate()
    {
        //横移動Update内だと壁接触時がたつくため
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal")* Time.deltaTime*timeScale*moveSpeed, rb.velocity.y);
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            sprRenderer.flipX = false;
            SetState(State.walking);
        }
        if (0 < Input.GetAxisRaw("Horizontal"))
        {
            sprRenderer.flipX = true;
            SetState(State.walking);
        }
    }

    protected override void Move()
    {
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

    void SetState(State st)
    {
        /*
        switch (st)
        {
            case State.walking:
                state = State.walking;
                walkingAnime.enabled = true;
                jumpingAnime.enabled = false;
                break;

            case State.jumping:
                state = State.jumping;
                walkingAnime.enabled = false;
                jumpingAnime.enabled = true;
                break;
        }
        */
    }
}