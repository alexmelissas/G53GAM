using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float y = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rotation = Input.GetAxis("Mouse X");
        transform.Rotate(0, 5.0f * rotation, 0);

        float updown = Input.GetAxis("Mouse Y");
        if(y+updown > 60 || y+updown < -100)
        {
            updown = 0;
        }

        y += updown;

        Camera.main.transform.RotateAround(transform.position,
            transform.right, updown);
        Camera.main.transform.LookAt(transform);
    }
}
