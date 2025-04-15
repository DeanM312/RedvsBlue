using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Unit user;
    public Cam cam;
    private BoxCollider2D hitbox;

    // Start is called before the first frame update
    void Start()
    {
        cam.enabled = false;

        hitbox = user.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cam.gameObject.transform.position = transform.position + (mousePos - new Vector3(transform.position.x, transform.position.y, 1))/ 2;

        if (Input.GetKey("d"))
        {
            user.right = 1;
        }
        else if (Input.GetKey("a"))
        {
            user.right = -1;
        }
        else
        {
            user.right = 0;
        }

        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        if (Input.GetMouseButton(0))
        {
            if (user.weapon.arty)
            {
                if (Vector2.Distance(transform.position, mousePos2D) > user.range)
                {
                    Vector2 pos2D = new Vector2(transform.position.x, transform.position.y);
                    Vector2 vect = mousePos2D - pos2D;
                    mousePos2D = pos2D + vect.normalized*user.range;
                }
            }

            user.weapon.Fire(mousePos2D, user.owner.faction1);
        }


        if (Input.GetKey("w"))
        {
            user.Jump();
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            user.Down();
        }

        user.weapon.Rotate(mousePos2D);

        if (Input.GetKey("x"))
        {
            NormalAI ai = user.GetComponent<NormalAI>();
            if (ai)
            {
                ai.enabled = true;
            }    
            Destroy(this);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Pad")
        {
            if (Input.GetKey("w"))
            {
                user.PadJump();
            }
        }
    }

    void OnDestroy()
    {
        cam.enabled = true;
    }
}
