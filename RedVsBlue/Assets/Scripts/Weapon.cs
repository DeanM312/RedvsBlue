using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public double firerate;
    private double fireCooldown = 0f;
    public GameObject projectile;
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
            GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity);
            bullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg);
            transform.rotation = bullet.transform.rotation;
            Projectile proj = bullet.GetComponent<Projectile>();
            proj.velocity = rot.normalized * 10;
            proj.GetComponent<SpriteRenderer>().color = (this.GetComponent<SpriteRenderer>().color);

            if (!faction1)
            {
                bullet.layer = 9;
            }
        }
    }
}
