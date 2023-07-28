using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal") * 10 * Time.deltaTime, Input.GetAxis("Vertical") * 10 * Time.deltaTime, 0);


        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            Collider2D hit = Physics2D.OverlapPoint(mousePos2D, LayerMask.GetMask("Faction1"));
            

            if (hit != null)
            {
                if (hit.gameObject.CompareTag("Unit"))
                {
                    IAI script = hit.gameObject.GetComponent<IAI>();
                    script.Disable();
                    Player player = hit.gameObject.AddComponent<Player>();
                    player.user = hit.gameObject.GetComponent<Unit>();
                    player.cam = this;
                }
            }
        }
    }


}
