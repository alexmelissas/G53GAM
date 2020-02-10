using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{

    public AnimationCurve curve;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x,
            curve.Evaluate((Time.time % curve.length)),
            transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
