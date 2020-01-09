using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{
    public GameObject explosionEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        explosionEffect = GameObject.Instantiate(explosionEffect, transform.position, transform.rotation) as GameObject;
        GetComponent<AudioSource>().Play();
        GameObject.Destroy(this.gameObject);
        GameObject.Destroy(explosionEffect, 2);
        if (other.tag.Contains("Player")) {
            other.SendMessage("RecieveDamage");     // call RecieveDamage method in TankController
        }
    }
}
