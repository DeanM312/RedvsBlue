using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    public Base owner;
    public uint maxUnits;
    public uint range;
    private int tick;
    public float healAmount;
    private uint healCount;
    
    void Start()
    {
        
    }

    void Update()
    {
        var total = owner.units.Count;
        if (tick > owner.units.Count - 1)
        {
            tick = 0;
            healCount = 0;
        }
        var unit = owner.units[tick];

        if (unit)
        {
            if (healCount < maxUnits && unit.tag == "Unit")
            {
                var target = unit.GetComponent<Unit>();
                if (target.hp < target.maxhp && Vector2.Distance(unit.transform.position, transform.position) < range)
                {
                    target.hp += Mathf.FloorToInt(healAmount);
                    if (target.hp >= target.maxhp)
                    {
                        target.hp = target.maxhp;
                        target.healing = false;
                    }
                    else
                    {
                        healCount += 1;
                        target.healing = true;
                    }
                    target.RefreshBar();
                }
            }
        }
        tick++;
    }
}
