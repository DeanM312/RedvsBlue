using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAI : MonoBehaviour, IAI
{
    public Unit user;
    private Base owner;
    private int tick;
    private float backTime;
    private float forwardTime;
    private int dir;
    private float downTime;
    
    private uint delay;
    // Start is called before the first frame update
    void Start()
    {
        owner = user.owner;

        if (!owner.faction1)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (backTime > 0)
        {
            backTime -= 1 * Time.deltaTime;
            user.right = -1 * dir;
        }
        else
        {
            user.right = 1 * dir;
        }

        if (downTime > 0)
        {
            downTime -= 1 * Time.deltaTime;
            user.Down();
        }


        if (tick > owner.enemy.units.Count - 1)
        {
            tick = 0;
        }

        if (owner.enemy.units.Count > 0)
        {
            GameObject unit = owner.enemy.units[tick];

            if (unit)
            {
                Vector3 predict = Vector3.zero;
                if (Random.Range(0, 2) == 0 && !user.weapon.arty && unit.GetComponent<Rigidbody2D>())
                {
                    predict = (Vector2.Distance(unit.transform.position, transform.position) / user.weapon.speed) * unit.GetComponent<Rigidbody2D>().velocity;
                }
                if (Vector2.Distance(unit.transform.position, transform.position) < user.range)
                {
                    RaycastHit2D h = Physics2D.Linecast(transform.position, unit.transform.position, 1);
                    //Debug.DrawLine(transform.position - new Vector3(0, 0.05f, 0), unit.transform.position + new Vector3(0, 0.05f, 0));

                    if (!h || user.weapon.arty)
                    {
                        if (delay > 9)
                        {
                            user.weapon.Fire(unit.transform.position + predict, user.owner.faction1);
                            if (backTime <= 0)
                            {
                                if (!user.owner.retreat)
                                {
                                    if (Random.Range(0, 2) == 0)
                                    {
                                        backTime = Random.Range(1, (int)(user.weapon.firerate * 4));
                                    }
                                    else
                                    {
                                        backTime = ((float)user.weapon.firerate) / 2;
                                    }
                                }
                                else
                                {
                                    backTime = ((float)user.weapon.firerate) / 0.5f;
                                }
                            }
                        }
                        else
                        {
                            delay++;
                        }
                    }
                    else
                    {
                        delay = 0;
                        tick++;
                    }
                }
                else
                {
                    delay = 0;
                    tick++;
                }
            }
            else
            {
                delay = 0;
                tick++;
            }


        }

        //Debug.DrawLine(transform.position, transform.position - transform.up * hitbox.bounds.extents.y);

        if (Physics2D.OverlapPoint(transform.position + (transform.right * user.right * user.hitbox.size.x), 1))
        {
            user.Jump();
        }
    }

    void FixedUpdate()
    {
        if (backTime > 0)
        {

            user.Jump();


            if (Random.Range(0, 15) == 0 && !user.onLadder)
            {
                if (!Physics2D.OverlapPoint(transform.position - transform.up * user.hitbox.bounds.extents.y * 1.01f, 1))
                {
                    downTime = 0.1f;
                }
            }
        }
        else if (user.hp < user.maxhp)
        {
            user.Jump();
        }  
    }

    public void Disable()
    {
        this.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Pad")
        {
            if (Random.Range(0, 2) == 1)
            {
                user.PadJump();
            }
        }
    }
}
