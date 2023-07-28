using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Unit user;
    public Cam cam;
    // Start is called before the first frame update
    void Start()
    {
        cam.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        cam.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y,-1);

        if (Input.GetAxis("Horizontal") > 0)
        {
            user.right = 1;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            user.right = -1;
        }
        else
        {
            user.right = 0;
        }


        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            user.weapon.Fire(mousePos2D, user.owner.faction1);
        }


            if (Input.GetAxis("Vertical") > 0 && Physics2D.OverlapPoint(transform.position - transform.up * 0.15f, 1))
        {
            user.Jump();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Pad")
        {
            if (Input.GetAxis("Vertical") > 0)
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
