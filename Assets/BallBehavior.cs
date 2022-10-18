using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior: MonoBehaviour
{
    public float min, max;
    public GameManager gm;
    void Update()
    {
        var y = transform.position.y;
        var z = transform.position.z;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min, max), y, z);
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Cam Change")
        {
            Debug.Log("ball reached pins");
            StartCoroutine(gm.PinCheck());
        }
    }
}
