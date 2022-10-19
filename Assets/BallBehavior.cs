using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior: MonoBehaviour
{
    public float min, max;
    public GameManager gm;
    public bool isRunning;
    public AudioSource a;
    public AudioClip[] audios;
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

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pin")
        {
            if(!isRunning)
            {
                Debug.Log("Play strike sound");
                StartCoroutine(PlaySound(1, false));
            }
        }
    }

    public IEnumerator PlaySound(int i, bool b)
    {
        a.Stop();
        if(i == 1)
        {
            isRunning = true;
        }
        a.loop = b;
        a.clip = audios[i];
        a.Play();
        yield return null;
    }
}
