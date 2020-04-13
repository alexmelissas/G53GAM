using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float y = 0.0f;
    float verticalVelocity = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        // rotate the player object about the Y axis
        float rotation = Input.GetAxis("Mouse X"); transform.Rotate(0, rotation, 0);
        // rotate the camera (the player's "head") about its X axis
        float updown = Input.GetAxis("Mouse Y"); Camera.main.transform.Rotate(updown, 0, 0);
        // moving forwards and backwards
        float forwardSpeed = Input.GetAxis("Vertical"); // moving left to right
        float lateralSpeed = Input.GetAxis("Horizontal"); // apply gravity
        verticalVelocity += Physics.gravity.y * Time.deltaTime; CharacterController characterController
        = GetComponent<CharacterController>();
        if (Input.GetButton("Jump") && characterController.isGrounded)
        {
            verticalVelocity = 5;
        }
        Vector3 speed = new Vector3(lateralSpeed, verticalVelocity, forwardSpeed);
        // transform this absolute speed relative to the player's current rotation // i.e. we don't want them to move "north", but forwards depending on where // they are facing
        speed = transform.rotation * speed;
        // what is deltaTime?
        // move at a different speed to make up for variable framerates 

        // Update is called once per frame
        //    void Update()
        //{
        //    float rotation = Input.GetAxis("Mouse X");
        //    transform.Rotate(0, 5.0f * rotation, 0);

        //    float updown = Input.GetAxis("Mouse Y");
        //    if(y+updown > 60 || y+updown < -60)
        //    {
        //        updown = 0;
        //    }

        //    y += -updown;

        //    Camera.main.transform.RotateAround(transform.position,
        //        transform.right, updown);
        //    Camera.main.transform.LookAt(transform);
        //}
    }
}
