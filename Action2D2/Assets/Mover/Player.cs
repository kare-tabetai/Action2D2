using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : CharacterMover
{
    public float jumpForce;
    public float moveSpeed;
    public float e;

    Rigidbody2D rb;
    SpriteRenderer sprRenderer;
    List<Collision2D> contactsCollisions=new List<Collision2D>();

    double ColliderOffset { get { return sprRenderer.sprite.bounds.size.y/2; } }
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
        var hitSize = sprRenderer.sprite.bounds.size.y / 2;
        var pointA = (Vector2)transform.position + new Vector2(-hitSize, -hitSize);
        var pointB = (Vector2)transform.position + new Vector2(hitSize, -hitSize);
        var hit = Physics2D.OverlapArea(pointA, pointB, LayerMask.GetMask("Stage"));
        if (hit) return true;
        return false;
    }

    private void OnDrawGizmos()
    {
        var hitSize = sprRenderer.sprite.bounds.size.y / 2;
        var pointA = (Vector2)transform.position + new Vector2(-hitSize, -hitSize);
        var pointB = (Vector2)transform.position + new Vector2(hitSize, -hitSize);
        Gizmos.DrawWireCube((Vector2)transform.position + new Vector2(0, -hitSize), hitSize * Vector3.one);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        contactsCollisions.Add(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        contactsCollisions.Remove(collision);
    }

    protected override void Move()
    {
        bool grounded = GroundedCheck();
        print(grounded);

        //Walk
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * timeScale * moveSpeed, rb.velocity.y);
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            sprRenderer.flipX = true;
        }
        if (0 < Input.GetAxisRaw("Horizontal"))
        {
            sprRenderer.flipX = false;
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
}