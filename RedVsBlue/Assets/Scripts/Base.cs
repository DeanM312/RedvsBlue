using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public Base enemy;
    private int hp;
    public uint mass;
    private double buildTime;
    public bool faction1;
    public GameObject bot;
    public List<GameObject> units;
    private int unitCap;
    private int offset;
    public List<GameObject> buildingList;
    private Building selected;
    // Start is called before the first frame update
    void Start()
    {
        hp = 1000;
        mass = 0;
        buildTime = 14;
        unitCap = 50;
        units.Add(this.gameObject);

        if (!faction1)
        {
            offset = 30;
        }

        selected = buildingList[Random.Range(0, buildingList.Count)].GetComponent<Building>();
        //selected = buildingList[2].GetComponent<Building>();
        //mass = 60;
    }

    // Update is called once per frame
    void Update()
    {
        buildTime += 1 * Time.deltaTime;


        if (buildTime > 10 && units.Count < unitCap)
        {
            buildTime = 0;
            GameObject unit = Instantiate(bot, transform.position, Quaternion.identity);
            if (faction1)
            {
                unit.layer = 6;
            }
            else
            {
                unit.layer = 7;
            }
            unit.GetComponent<SpriteRenderer>().color = this.GetComponent<SpriteRenderer>().color;
            unit.GetComponent<Unit>().owner = this;

            units.Add(unit);
            

        }

        if (mass > selected.cost)
        {
            int front = 0;
            if (selected.defensive)
            {

                for (int i = front; i < units.Count; i++)
                {
                    if (units[front].CompareTag("Unit"))
                    {
                        break;
                    }
                    else
                    {
                        front++;
                    }
                }
            }
            else
            {
                front = units.Count-1;

                for (int i = front; i > 0; i--)
                {
                    if (units[front].CompareTag("Unit"))
                    {
                        break;
                    }
                    else
                    {
                        front++;
                    }
                }
            }


            if (Physics2D.OverlapPoint(units[front].transform.position - units[front].transform.up * 0.15f, 1) && Physics2D.OverlapPointAll(units[front].transform.position).Length < 2)
            {
                GameObject building = Instantiate(selected.gameObject, units[front].transform.position, Quaternion.identity);
                building.transform.Translate(0,(building.GetComponent<BoxCollider2D>().size.y - units[front].GetComponent<BoxCollider2D>().size.y)/2, 0);

                if (faction1)
                {
                    building.layer = 6;
                }
                else
                {
                    building.layer = 7;
                }


                building.GetComponent<SpriteRenderer>().color = this.GetComponent<SpriteRenderer>().color;
                building.GetComponent<Building>().owner = this;

                mass -= selected.cost;
                int selection = Random.Range(0, buildingList.Count);
                selected = buildingList[selection].GetComponent<Building>();

                units.Add(building);
            }
        }

    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10+ offset, 100, 20), mass.ToString());
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Projectile")
        {
            hp = hp - col.gameObject.GetComponent<Projectile>().damage;
            if (hp <= 0)
            {
                unitCap = 0;
                mass = 0;
            }

        }


    }


}
