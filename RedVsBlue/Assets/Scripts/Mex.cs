using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mex : MonoBehaviour
{
    public uint owner;
    public Base ownerObj;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        owner = 0;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        timer += Time.fixedDeltaTime;

        if (timer >= 1 && owner > 0)
        {
            timer = 0f;
            ownerObj.mass++;
        }
    }
}
