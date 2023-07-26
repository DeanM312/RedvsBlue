using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public Base owner;
    private float movespeed;
    private Rigidbody2D rb;
    private float forwardSpeed;
    private int dir;
    private int forward;
    public int hp;
    private double firerate;
    private double fireCooldown;
    public GameObject prefab;
    private int tick;
    private float backTime;
    public uint range;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        movespeed = 1;
        forwardSpeed = 0;
        forward = 1;

        if (!owner.faction1)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }

        firerate = 0.8f;
        fireCooldown = 0f;

        tick = 0;
        hp = 100;

        backTime = 0;


    }

    // Update is called once per frame
    void Update()
    {



        if (fireCooldown > 0)
        {
            fireCooldown -= 1 * Time.deltaTime;
        }


        if (backTime > 0)
        {
            backTime -= 1 * Time.deltaTime;
            forward = -1;
        }
        else
        {
            forward = 1;
        }
        


        if (rb.velocity.x < movespeed && rb.velocity.x > -movespeed)
        {
            forwardSpeed = 5f * dir * forward;
        }
        else
        {
            forwardSpeed = 0;
        }
        


        //Debug.Log(rb.velocity.x);
    }


    void FixedUpdate()
    {
        /*
        if (owner.enemy.units.Count > 0)
        {
            foreach (GameObject unit in owner.enemy.units)
            {
                RaycastHit2D h = Physics2D.Linecast(transform.position, unit.transform.position, 1);

                if (!h)
                {
                    if (fireCooldown <= 0)
                    {
                        //Quaternion.LookRotation(Vector3.forward, rot) +

                        fireCooldown = firerate;
                        Vector2 rot = (unit.transform.position - this.transform.position);
                        GameObject bullet = Instantiate(prefab, transform.position, Quaternion.identity);
                        bullet.transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(rot.y,rot.x) * Mathf.Rad2Deg);
                        Projectile proj = bullet.GetComponent<Projectile>();
                        proj.velocity = rot.normalized * 10;
                        
                        if (!owner.faction1)
                        {
                            bullet.layer = 9;
                        }
                    }








                    forward = -1;
                    break;
                }
                else
                {
                    forward = 1;
                }
            }
        }
        */

        if (tick > owner.enemy.units.Count - 1)
        {
            tick = 0;
        }

        if (owner.enemy.units.Count > 0)
        {
            GameObject unit = owner.enemy.units[tick];

            if (Mathf.Abs(unit.transform.position.x - transform.position.x) < range)
            {

                //RaycastHit2D h = Physics2D.Linecast(transform.position - new Vector3(Mathf.Abs(rb.velocity.x) / 100, Mathf.Abs(rb.velocity.y) / 100,0), unit.transform.position, 1);
                RaycastHit2D h = Physics2D.Linecast(transform.position - new Vector3(0, 0.01f, 0), unit.transform.position + new Vector3(0, 0.01f, 0), 1);
                //RaycastHit2D h = Physics2D.Linecast(transform.position, unit.transform.position, 1);
                Debug.DrawLine(transform.position - new Vector3(0, 0.01f, 0), unit.transform.position + new Vector3(0, 0.01f, 0));

                if (!h)
                { 
                    if (fireCooldown <= 0)
                    {
                        //Quaternion.LookRotation(Vector3.forward, rot) +

                        fireCooldown = firerate;
                        Vector2 rot = (unit.transform.position - this.transform.position);
                        GameObject bullet = Instantiate(prefab, transform.position, Quaternion.identity);
                        bullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg);
                        Projectile proj = bullet.GetComponent<Projectile>();
                        proj.velocity = rot.normalized * 10;
                        proj.GetComponent<SpriteRenderer>().color = (this.GetComponent<SpriteRenderer>().color);

                        if (!owner.faction1)
                        {
                            bullet.layer = 9;
                        }
                    }








                    backTime = 1f;
                }
                else
                {
                    tick++;
                }
            }
            else
            {
                tick++;
            }


        }



        rb.AddForce(transform.right * forwardSpeed);


        if (Physics2D.OverlapPoint(transform.position + (transform.right * dir * forward * 0.1f) - transform.up * 0.1f, 1) && Physics2D.OverlapPoint(transform.position - transform.up * 0.15f, 1))
        {
            rb.AddForce(transform.up * 150);
        }

        



    }


    private void OnDrawGizmos()
    {
        //Vector3 vel = rb.velocity;
        //Gizmos.DrawSphere(transform.position - vel, 0.01f);
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
        if (other.gameObject.tag == "Pad")
        {
            if (Random.Range(0, 2) == 1)
            {
                rb.AddForce(transform.up * 300);
            }
        }

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
}
