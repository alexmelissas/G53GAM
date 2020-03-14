using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open()
    {
        Debug.Log("Opening door");
        StartCoroutine(DoorAnimation(270, 0.1f));

    }

    public void Close()
    {
        Debug.Log("Closing door");
        StartCoroutine(DoorAnimation(180, 0.1f));
    }

    private IEnumerator DoorAnimation(int targetAngle, float animationSpeed)
    {
        for (int r = 0; r < animationSpeed; r += 1)
        {
            transform.localEulerAngles = new Vector3(0,
            Mathf.LerpAngle(transform.localEulerAngles.y, targetAngle,
            5f / animationSpeed), 0);
            yield return null;
        }
    }
}
