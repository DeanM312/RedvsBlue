using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Building building;
    public GameObject weaponPrefab;
    private Weapon weaponScript;
    private int tick;
    public int range;
    private int delay;
    private Vector3 lastPos;
    // Start is called before the first frame update
    void Start()
    {
        GameObject weapon = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        weapon.transform.parent = transform;
        weaponScript = weapon.GetComponent<Weapon>();
        weaponScript.range = range;
        weapon.GetComponent<SpriteRenderer>().color = (this.GetComponent<SpriteRenderer>().color);
    }

    // Update is called once per frame
    void Update()
    {
        if (tick > building.owner.enemy.units.Count - 1)
        {
            tick = 0;
        }


        if (building.owner.enemy.units.Count > 0)
        {
            GameObject unit = building.owner.enemy.units[tick];
            if (unit)
            {

                Vector3 predict = Vector3.zero;
                if (Random.Range(0, 2) == 0 && !weaponScript.arty && unit.GetComponent<Rigidbody2D>())
                {
                    predict = (Vector2.Distance(unit.transform.position, transform.position) / weaponScript.speed) * unit.GetComponent<Rigidbody2D>().velocity;
                }

                if (Mathf.Abs(unit.transform.position.x - transform.position.x) < range+1)
                {
                    RaycastHit2D h = Physics2D.Linecast(transform.position, unit.transform.position, 1);

                    if (!h || weaponScript.arty)
                    {
                        if (delay > 9)
                        {

                            weaponScript.Fire(unit.transform.position + predict, building.owner.faction1);
                            lastPos = unit.transform.position;

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
                    tick++;
                }
            }
            else
            {
                tick++;
            }
        }
        if (lastPos != Vector3.zero)
        {
            weaponScript.Fire(lastPos, building.owner.faction1);
        }

    }
}
