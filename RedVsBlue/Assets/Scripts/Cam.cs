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
        transform.Translate(Input.GetAxis("Horizontal") * 30 * Time.deltaTime, Input.GetAxis("Vertical") * 30 * Time.deltaTime, 0);


        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            Collider2D hit = null;

            if (DataManager.team == "Faction1")
            {
                hit = Physics2D.OverlapPoint(mousePos2D, LayerMask.GetMask("Faction1"));
            }
            else
            {
                hit = Physics2D.OverlapPoint(mousePos2D, LayerMask.GetMask("Faction2"));
            }
            
            if (hit != null)
            {
                if (hit.gameObject.CompareTag("Unit"))
                {
                    IAI script = hit.gameObject.GetComponent<IAI>();
                    script.Disable();
                    Player player = hit.gameObject.AddComponent<Player>();
                    player.user = hit.gameObject.GetComponent<Unit>();
                    player.cam = this;
                    Camera.main.orthographicSize = player.user.range / 2.5f;
                }
            }
        }
    }


}
