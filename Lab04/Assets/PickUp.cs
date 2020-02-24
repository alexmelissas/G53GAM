using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform holdPosition;
    private GameObject heldObject;
    int layerMask = 1 << 8;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (heldObject == null)
            {
                RaycastHit colliderHit;
                if (Physics.Raycast(transform.position,
                    transform.forward,
                    out colliderHit,
                    10.0f,
                    layerMask))
                {
                    Debug.Log("GRAB");
                    heldObject = colliderHit.collider.gameObject;
                    heldObject.GetComponent<Rigidbody>().useGravity = false;
                }
            }
        }

        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().MovePosition(holdPosition.position);
            heldObject.GetComponent<Rigidbody>().MoveRotation(holdPosition.rotation);
        }

        if ((Input.GetButtonDown("Fire2")) && (heldObject != null))
        {
            Debug.Log("THROW");
            heldObject.GetComponent<Rigidbody>().AddForce(transform.forward * 10.0f, ForceMode.Impulse);
            heldObject.GetComponent<Rigidbody>().useGravity = true;
            heldObject = null;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (heldObject != null)
            {
                Debug.Log("RELEASE");
                heldObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                heldObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                heldObject.GetComponent<Rigidbody>().ResetInertiaTensor();
                heldObject.GetComponent<Rigidbody>().useGravity = true;

                heldObject = null;
            }
        }
    }
}
