using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Base owner;
    public float movespeed;
    private Rigidbody2D rb;
    public int hp;
    public uint range;
    public GameObject weaponPrefab;
    [System.NonSerialized]
    public Weapon weapon;
    private uint jump;
    [System.NonSerialized]
    public int right;
    public uint buildTime;
    public BoxCollider2D hitbox;
    private float jumpDelay;


    // Start is called before the first frame update
    void Start()
    {
        GameObject weaponObject = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        weaponObject.transform.parent = transform;
        weaponObject.GetComponent<SpriteRenderer>().color = (this.GetComponent<SpriteRenderer>().color);
        weapon = weaponObject.GetComponent<Weapon>();
        weapon.range = range-0.1f;
        rb = this.GetComponent<Rigidbody2D>();

        hitbox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        switch (jump)
        {
            case 1:
                rb.AddForce(transform.up * 400);
                jump = 0;
                break;
            case 2:
                rb.AddForce(transform.up * 1000);
                jump = 0;
                break;
        }

        if (jumpDelay >= 0)
        {
            jumpDelay -= 1 * Time.fixedDeltaTime;
        }

        rb.AddForce(transform.right * right * 8);



        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -movespeed, movespeed),rb.velocity.y);
        

    }

    public void Jump()
    {
        if (Physics2D.OverlapPoint(transform.position - transform.up * hitbox.bounds.extents.y * 1.01f,1) && jumpDelay <= 0)
        {
            jumpDelay = 0.5f;
            jump = 1;
        }
    }

    public void PadJump()
    {
        jump = 2;
    }





    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Projectile")
        {
            hp = hp - col.gameObject.GetComponent<Projectile>().damage;
            if (hp <= 0)
            {
                owner.units.Remove(this.gameObject);
                Destroy(this.gameObject);
            }

        }


    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Mex")
        {
            Mex mex = other.gameObject.GetComponent<Mex>();
            uint fac = 2;

            if (owner.faction1)
            {
                fac = 1;
            }

            if (fac != mex.owner)
            {
                mex.owner = fac;
                mex.GetComponent<SpriteRenderer>().color = (this.GetComponent<SpriteRenderer>().color);
                mex.ownerObj = owner;
            }

        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            rb.AddForce(transform.up * 20);
        }
    }
}
