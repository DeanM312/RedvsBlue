using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int damage;
    public float aoe;
    public GameObject explosionEffect;
    private ParticleSystem particleEffect;
    private int damageLayer;


    void Start()
    {
        

        if (gameObject.layer == 8 || gameObject.layer == 11)
        {
            damageLayer = LayerMask.GetMask("Faction2");
        }
        else
        {
            damageLayer = LayerMask.GetMask("Faction1");
        }
    }


    void OnDestroy()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, aoe, damageLayer);
        GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        particleEffect = effect.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particleEffect.main;
        main.startColor = this.GetComponent<SpriteRenderer>().color;
        ParticleSystem.ShapeModule shape = particleEffect.shape;
        shape.radius = aoe;
        Destroy(effect, 1f);

        //Debug.Log(cols.Length);

        foreach (Collider2D col in cols)
        {
            //Debug.Log(col.transform.position);
            //Debug.Log(col.gameObject.layer);
            IDamagable obj = col.GetComponent<IDamagable>();
            obj.TakeDamage(damage);

        }

    }
}


