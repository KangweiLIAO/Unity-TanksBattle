using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankFireController : MonoBehaviour
{
    public float fireSpeed;
    public float reloadTime;
    public GameObject shellPrefab;
    public Slider aimSlider;
    public KeyCode fireKey = KeyCode.Space;     // default fire keyboard button = space

    private float reloadTimer;
    private bool isReadyToFire;
    private Transform firePosition;

    // Start is called before the first frame update
    void Start()
    {
        isReadyToFire = true;
        reloadTimer = reloadTime;
        firePosition = transform.Find("FirePosition");      // find GameObject's transform with name = FirePosition
    }

    // Update is called once per frame
    private void Update()
    {
        reloadTimer -= Time.deltaTime;
        if (reloadTimer <= 0) {
            isReadyToFire = true;
            reloadTimer = reloadTime;
        }
    }

    void FixedUpdate()
    {
        // Fire control
        if (Input.GetKeyDown(fireKey) && isReadyToFire) {
            GameObject shell = GameObject.Instantiate(shellPrefab, firePosition.position, firePosition.rotation) as GameObject;      // initialize shell as a game object
            shell.GetComponent<Rigidbody>().AddForce(transform.forward * fireSpeed);     // provide an forward force for shell
            shell.GetComponent<Rigidbody>().AddForce(transform.up * (50));     // provide an upward force for shell
            isReadyToFire = false;
        }
    }
}
