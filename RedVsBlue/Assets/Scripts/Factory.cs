using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public List<GameObject> unitList;
    public Building building;
    private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 25)
        {
            if (building.owner.mass > 0)
            {
                GameObject unit = Instantiate(unitList[Random.Range(0, unitList.Count)], transform.position, Quaternion.identity);
                if (building.owner.faction1)
                {
                    unit.layer = 6;
                }
                else
                {
                    unit.layer = 7;
                }
                unit.GetComponent<SpriteRenderer>().color = this.GetComponent<SpriteRenderer>().color;
                Unit unitScript = unit.GetComponent<Unit>();
                unitScript.owner = building.owner;

                building.owner.units.Add(unit);

                timer = 0f - unitScript.buildTime;
                building.owner.mass -= 1;
            }
        }
    }
}
