using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAI : MonoBehaviour, IAI
{
    public Unit user;
    private Base owner;
    private int tick;
    private float backTime;
    private int dir;
    private bool aggressive;
    
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

        if (Random.Range(0,2) == 0)
        {
            aggressive = true;
        }

        
    }

    // Update is called once per frame
    void Update()
    {

        if (backTime > 0 && !aggressive)
        {
            backTime -= 1 * Time.deltaTime;
            user.right = -1 * dir;
        }
        else
        {
            user.right = 1 * dir;
        }




        if (tick > owner.enemy.units.Count - 1)
        {
            tick = 0;
        }

        if (owner.enemy.units.Count > 0)
        {
            GameObject unit = owner.enemy.units[tick];

            if (Mathf.Abs(unit.transform.position.x - transform.position.x) < user.range)
            {
                RaycastHit2D h = Physics2D.Linecast(transform.position, unit.transform.position, 1);
                //Debug.DrawLine(transform.position - new Vector3(0, 0.05f, 0), unit.transform.position + new Vector3(0, 0.05f, 0));

                if (!h)
                {
                    if (delay > 9)
                    {
                        user.weapon.Fire(unit.transform.position, user.owner.faction1);
                        backTime = 1f;
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

        //Debug.DrawLine(transform.position, transform.position - transform.up * hitbox.bounds.extents.y);

        if (Physics2D.OverlapPoint(transform.position + (transform.right * user.right * user.hitbox.size.x), 1))
        {
            user.Jump();
        }
    }

    void FixedUpdate()
    {
        if (Random.Range(0, 100) == 0)
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
