using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{

    public bool vertical;

    public AnimationCurve curve;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (vertical)
        {
            transform.position = new Vector3(transform.position.x,
            curve.Evaluate((Time.time % curve.length)),
            transform.position.z);
        }
        else
        {
            transform.position = new Vector3(curve.Evaluate((Time.time % curve.length)),
            transform.position.y,
            transform.position.z);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Touched hazard");
        Application.LoadLevel(Application.loadedLevel);
    }
}
