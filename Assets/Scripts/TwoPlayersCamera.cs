using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoPlayersCamera : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public float zoomSpeed;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        zoomSpeed = 0.1f;
        offset = transform.position - (player1.position + player2.position) / 2;
        // this.transform.eulerAngles = new Vector3(45, 55, 0);     // set up rotation with (x, y, z)
    }

    // Update is called once per frame
    void Update()
    {
        if (player1 == null || player2 == null) {
            if (player1 == null)  transform.position = offset + player2.position;
            else transform.position = offset + player1.position;
            this.GetComponent<Camera>().orthographicSize = Mathf.SmoothDamp(this.GetComponent<Camera>().orthographicSize, 7, ref zoomSpeed, 0.1f);
        }
        else
        {
            transform.position = offset + (player1.position + player2.position) / 2;
            float distance = Vector3.Distance(player1.position, player2.position);      // calculate distance between players
            if (distance >= 25)
            {
                this.GetComponent<Camera>().orthographicSize = distance * 0.25f;       // increase/decrease size of camera's view (with rate = 0.4f)
            }
            // this.transform.eulerAngles = new Vector3(transform.rotation.x * 1.1f, transform.rotation.y, transform.rotation.z);   // rotate base on original rotation & fixed rate
        }
    }
}
