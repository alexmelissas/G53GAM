﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody b = other.gameObject.GetComponent<Rigidbody>();
        b.AddForce(new Vector3(0, 50, 0), ForceMode.Force);
    }
}
