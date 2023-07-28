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
    // Start is called before the first frame update
    void Start()
    {
        GameObject weapon = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        weapon.transform.parent = transform;
        weaponScript = weapon.GetComponent<Weapon>();
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

            if (Mathf.Abs(unit.transform.position.x - transform.position.x) < range)
            {
                RaycastHit2D h = Physics2D.Linecast(transform.position, unit.transform.position, 1);

                if (!h)
                {
                    weaponScript.Fire(unit.transform.position, building.owner.faction1);
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

        
    }
}
