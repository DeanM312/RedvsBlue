using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float movementSpeed = 10f;
    public float jumpSpeed = 500f;
    private bool isGrounded = false;
    private float yVelocity = 0;
    Collider2D hitbox;
    int xDir;

    private void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        hitbox = this.GetComponent<Collider2D>();
    }

    void Update()
    {
        float test = Mathf.Sign(yVelocity);
        Vector3 bottomPos = new Vector3(transform.position.x, transform.position.y + (hitbox.bounds.extents.y * test) + (0.01f * test));
        RaycastHit2D h = Physics2D.Linecast(bottomPos, bottomPos + new Vector3(0, yVelocity / 100, 0), 1);

        if (h)
        {
            if (test < 0)
            {
                isGrounded = true;
            }

            yVelocity = 0;
        }
        else
        {
            transform.Translate(0, yVelocity * Time.deltaTime, 0);
        }

        xDir = 0;

        if (Input.GetKey("a"))
        {
            xDir = -1;

        }
        if (Input.GetKey("d"))
        {
            xDir = 1;
        }

        Vector3 rightPos = new Vector3(transform.position.x + (hitbox.bounds.extents.x * xDir) + (0.01f * xDir), transform.position.y);
        RaycastHit2D xHit = Physics2D.Linecast(rightPos, rightPos + new Vector3(0, (xDir * movementSpeed) / 1000, 0), 1);

        if (!xHit)
        {
            transform.Translate(movementSpeed * Time.deltaTime * xDir, 0, 0);
        }
        if (Input.GetKey("s"))
        {
            if (!isGrounded)
            {
                //rb.gravityScale += 2;
            }
        }
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            if (h)
            {
                yVelocity = 10;
                isGrounded = false;
            }
        }

        if (yVelocity > -11)
        {
            yVelocity -= 9.8f * Time.deltaTime;
        }
    }
}
