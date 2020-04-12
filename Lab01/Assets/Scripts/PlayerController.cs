using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject shot;
    public Transform shotTransform;

    public float speed = 5.0f;
    public float xMin, xMax, zMin, zMax;
    public float fireRate = 0.5F;

    private float nextFire = 0.0F;

    // Start is called before the first frame update
    void Start()
    {
        zMin = -4.145926f;
        zMax = 4.194956f;
        xMin = -4.362964f;
        xMax = 4.481626f;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        //Debug.Log("Input: " + horizontalMovement + " " + verticalMovement);

        Rigidbody r = GetComponent<Rigidbody>();
        r.velocity = new Vector3(
            horizontalMovement * speed,
            0.0f,
            verticalMovement * speed
        );

        r.position = new Vector3(
            Mathf.Clamp(r.position.x, xMin, xMax),
            r.position.y,
            Mathf.Clamp(r.position.z, zMin, zMax)
        );

        if(Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotTransform.position, shotTransform.rotation);
        }
    }
}
