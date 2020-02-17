using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletTransform;

    public float firerate = 0.5f;
    private float nextFire = 0.0f;

    float verticalVelocity = 0;
    public float moveSpeed;
    public float cameraSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rotation = Input.GetAxis("Mouse X");
        transform.Rotate(0, rotation * cameraSpeed, 0);

        float updown = Input.GetAxis("Mouse Y");
        float invertedUpdown = 0 - (updown * cameraSpeed); 
        Camera.main.transform.Rotate(invertedUpdown, 0, 0);

        float forwardSpeed = Input.GetAxis("Vertical") * moveSpeed;
        float lateralSpeed = Input.GetAxis("Horizontal") * moveSpeed;

        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        CharacterController characterController = GetComponent<CharacterController>();

        if (Input.GetButton("Jump") && characterController.isGrounded) verticalVelocity = 5;

        Vector3 speed = new Vector3(lateralSpeed, verticalVelocity, forwardSpeed);

        speed = transform.rotation * speed;

        characterController.Move(speed * Time.deltaTime);

        if(Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + firerate;
            Instantiate(bullet,
                bulletTransform.position,
                Camera.main.transform.rotation);
        }

    }
}
