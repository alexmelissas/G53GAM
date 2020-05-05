using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{

    public bool vertical;
    public GameObject rpgscreen;
    public AnimationCurve curve;

    private float x,y,z;

    // Start is called before the first frame update
    void Start()
    {
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (vertical) transform.position = new Vector3(x, y + curve.Evaluate((Time.time % curve.length)), z);
        else transform.position = new Vector3(x + curve.Evaluate((Time.time % curve.length)), y, z);        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Touched hazard");
        rpgscreen.SetActive(true);
        Application.LoadLevel(Application.loadedLevel);
    }
}
