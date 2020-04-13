using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class winPad : MonoBehaviour
{
    public Text winText;
    private void OnTriggerEnter(Collider other)
    {
        winText.enabled = true;
    }
}
