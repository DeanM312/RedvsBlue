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
    private Building advSelected;
    public uint advMass;
    public List<GameObject> advBuildingList;
    public bool retreat;
    private double advantage;
    // Start is called before the first frame update
    void Start()
    {
        hp = 1000;
        mass = 0;
        advMass = 0;
        buildTime = 14;
        unitCap = 50;
        units.Add(this.gameObject);

        if (!faction1)
        {
            offset = 30;
        }

        selected = buildingList[Random.Range(0, buildingList.Count)].GetComponent<Building>();
        advSelected = advBuildingList[Random.Range(0, advBuildingList.Count)].GetComponent<Building>();
        //selected = buildingList[6].GetComponent<Building>();
        //mass = 60;
    }

    // Update is called once per frame
    void Update()
    {
        buildTime += 1 * Time.deltaTime;
        advantage = ((double)units.Count / enemy.units.Count);

        if (buildTime > 8 && units.Count < unitCap)
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
                /*
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
                */
                front = Random.Range(0, units.Count);
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
            BoxCollider2D hitbox = units[front].GetComponent<BoxCollider2D>();

            if (Physics2D.OverlapPoint(units[front].transform.position - (units[front].transform.up * hitbox.size.y)/2, 1) && Physics2D.OverlapPointAll(units[front].transform.position).Length < 2 && units[front].CompareTag("Unit"))
            {
                GameObject building = Instantiate(selected.gameObject, units[front].transform.position, Quaternion.identity);
                building.transform.Translate(0,(building.GetComponent<BoxCollider2D>().size.y - hitbox.size.y)/2, 0);

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

        if (advMass > advSelected.cost)
        {
            int front = 0;
            if (advSelected.defensive)
            {
                /*
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
                */
                front = Random.Range(0, units.Count);
            }
            else
            {
                front = units.Count - 1;

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
            BoxCollider2D hitbox = units[front].GetComponent<BoxCollider2D>();

            if (Physics2D.OverlapPoint(units[front].transform.position - (units[front].transform.up * hitbox.size.y) / 2, 1) && Physics2D.OverlapPointAll(units[front].transform.position).Length < 2 && units[front].CompareTag("Unit"))
            {
                GameObject building = Instantiate(advSelected.gameObject, units[front].transform.position, Quaternion.identity);
                building.transform.Translate(0, (building.GetComponent<BoxCollider2D>().size.y - hitbox.size.y) / 2, 0);

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

                advMass -= advSelected.cost;
                int selection = Random.Range(0, advBuildingList.Count);
                advSelected = advBuildingList[selection].GetComponent<Building>();

                units.Add(building);
            }
        }

        if (advantage < 0.9)
        {
            retreat = true;
        }
        else
        {
            retreat = false;
        }

    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10+ offset, 100, 20), mass.ToString());

        GUI.Label(new Rect(10, 110 + offset, 100, 20), advMass.ToString());

        GUI.Label(new Rect(10, 210 + offset, 100, 20), units.Count.ToString());

        GUI.Label(new Rect(10, 310 + offset, 100, 20), advantage.ToString());

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
