using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public double firerate;
    private double fireCooldown = 0f;
    public GameObject projectile;
    public int damage;
    public float range;
    public float speed;
    public bool arty;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        fireCooldown -= 1 * Time.deltaTime;
    }


    public void Fire(Vector3 pos, bool faction1)
    {
        if (fireCooldown <= 0)
        {
            //Quaternion.LookRotation(Vector3.forward, rot) +
            fireCooldown = firerate;
            Vector2 rot = (pos - this.transform.position);
            Vector3 spawnPos = (rot.normalized * Vector3.one);
            GameObject bullet = Instantiate(projectile, transform.position + spawnPos, Quaternion.identity);
            Projectile proj = bullet.GetComponent<Projectile>();
            if (!arty)
            {
                bullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg);
                transform.rotation = bullet.transform.rotation;
                proj.velocity = rot.normalized * speed;
                Destroy(proj.gameObject, range / speed);

                if (!faction1)
                {
                    bullet.layer = 9;
                }
            }
            else
            {
                Vector3 velocity = Vector3.zero;
                //velocity.x = (pos.x - this.transform.position.x) / 2;
                //velocity.y = 9.8f + ((pos.y - this.transform.position.y) / 2);
                velocity.x = (pos.x - this.transform.position.x) / 4;
                velocity.y = 19.6f + ((pos.y - this.transform.position.y) / 4);
                

                Debug.DrawRay(transform.position, velocity * 5, Color.cyan, 0.5f);
                proj.velocity = velocity;
                bullet.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg, Vector3.forward);
                transform.rotation = bullet.transform.rotation;
                Destroy(proj.gameObject,  4f);

                if (!faction1)
                {
                    bullet.layer = 12;
                }
            }
            
            proj.GetComponent<SpriteRenderer>().color = (this.GetComponent<SpriteRenderer>().color);
            proj.damage = damage;
        }
    }

    public bool canFire()
    {
        if (fireCooldown <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
