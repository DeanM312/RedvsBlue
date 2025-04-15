using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IDamagable
{
    public Base owner;
    public float movespeed;
    public int hp;
    public int maxhp;
    public uint range;
    public GameObject weaponPrefab;
    [System.NonSerialized]
    public Weapon weapon;
    private uint jump;
    [System.NonSerialized]
    public int right;
    public uint buildTime;
    public BoxCollider2D hitbox;
    private float jumpDelay;
    private float flashTime;
    private bool hit;
    private int down;
    public bool onLadder;
    private bool isGrounded = false;
    private float yVelocity = 0;
    public GameObject hpBar;
    private GameObject barInstance;
    [HideInInspector]
    public bool healing;


    // Start is called before the first frame update
    void Start()
    {
        GameObject weaponObject = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        weaponObject.transform.parent = transform;
        weaponObject.GetComponent<SpriteRenderer>().color = (this.GetComponent<SpriteRenderer>().color);
        weapon = weaponObject.GetComponent<Weapon>();
        weapon.range = range - 0.1f;
        hitbox = GetComponent<BoxCollider2D>();

        if (hpBar)
        {
            barInstance = Instantiate(hpBar, transform.position, Quaternion.identity);
            barInstance.transform.Translate(0, 0.7f, 0);
            barInstance.transform.parent = transform;
        }

        maxhp = hp;
        healing = false;
    }

    private void Update()
    {
        float test = Mathf.Sign(yVelocity);
        Vector3 bottomPos = new Vector3(transform.position.x, transform.position.y + (hitbox.bounds.extents.y * test) + (0.01f * test));
        RaycastHit2D h = Physics2D.Linecast(bottomPos, bottomPos + new Vector3(0, yVelocity / 100, 0), 1);

        if (h)
        {
            if (test < 0)
            {
                isGrounded = true;
            }

            yVelocity = 0;
        }
        else
        {
            jump = 0;
            transform.Translate(0, yVelocity/ DataManager.FRAMERATE, 0);
        }

        if (isGrounded)
        {
            switch (jump)
            {
                case 1:
                    isGrounded = false;
                    jump = 0;
                    yVelocity = 8;
                    break;
                case 2:
                    isGrounded = false;
                    jump = 0;
                    yVelocity = 12;
                    break;
            }
        }

        if (yVelocity > -11)
        {
            yVelocity -= 9.8f/ DataManager.FRAMERATE;
        }

        if (onLadder)
        {
            yVelocity = 10;
        }

        if (flashTime > 0)
        {
            flashTime = flashTime - 1 * Time.deltaTime;
        }
        else if (hit)
        {
            this.GetComponent<SpriteRenderer>().color = this.owner.GetComponent<SpriteRenderer>().color;
            hit = false;
        }
        Vector3 rightPos = new Vector3(transform.position.x + (hitbox.bounds.extents.x * right) + (0.01f * right), transform.position.y);
        RaycastHit2D xHit = Physics2D.Linecast(rightPos, rightPos + new Vector3(0, (right * movespeed) / 1000, 0), 1);

        if (!xHit)
        {
            transform.Translate((right * movespeed)/ DataManager.FRAMERATE, 0, 0);
        }
    }

    public void Jump()
    {
        jump = 1;
    }

    public void PadJump()
    {
        jump = 2;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Projectile")
        {
            this.TakeDamage(col.gameObject.GetComponent<Projectile>().damage);
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Mex")
        {
            Mex mex = other.gameObject.GetComponent<Mex>();
            uint fac = 2;

            if (owner.faction1)
            {
                fac = 1;
            }

            if (fac != mex.owner)
            {
                mex.owner = fac;
                mex.GetComponent<SpriteRenderer>().color = (this.GetComponent<SpriteRenderer>().color);
                mex.ownerObj = owner;
            }

        }
        else if (other.gameObject.CompareTag("Ladder"))
        {
            onLadder = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            onLadder = false;
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        this.GetComponent<SpriteRenderer>().color = Color.white;
        flashTime = 0.1f;
        hit = true;

        if (hp <= 0)
        {
            owner.units.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
        RefreshBar();
    }

    public void Down()
    {
        down = 1;
    }

    public void RefreshBar()
    {
        if (barInstance)
        {
            Vector3 scale = barInstance.transform.localScale;
            scale.x = (hp * 1.0f / maxhp);
            barInstance.transform.localScale = scale;
        }
    }
}
