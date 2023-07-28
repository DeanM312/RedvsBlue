using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAI : MonoBehaviour
{
    public Unit user;
    private Base owner;
    private int tick;
    private float backTime;
    private int dir;
    private bool aggressive;
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
                RaycastHit2D h = Physics2D.Linecast(transform.position - new Vector3(0, 0.0f, 0), unit.transform.position + new Vector3(0, 0.0f, 0), 1);
                Debug.DrawLine(transform.position - new Vector3(0, 0.01f, 0), unit.transform.position + new Vector3(0, 0.01f, 0));

                if (!h)
                {
                    user.weapon.Fire(unit.transform.position, user.owner.faction1);
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



        if (Physics2D.OverlapPoint(transform.position + (transform.right * user.right * 0.1f) - transform.up * 0.1f, 1) && Physics2D.OverlapPoint(transform.position - transform.up * 0.15f, 1))
        {
            user.Jump();
        }
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
