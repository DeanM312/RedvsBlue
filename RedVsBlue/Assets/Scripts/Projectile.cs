using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public Vector2 velocity;
    public uint drop;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {

        rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = velocity;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
}
