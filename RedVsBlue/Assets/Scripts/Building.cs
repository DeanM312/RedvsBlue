using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int hp = 100;
    private int inithp;
    public Base owner;
    private float timer = 0f;
    public uint cost;
    public bool defensive;
    public uint regenRate = 1;
    // Start is called before the first frame update
    void Start()
    {
        inithp = hp;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * regenRate;

        if (timer >= 1 && hp < inithp)
        {
            timer = 0f;
            hp++;
        }
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
}
