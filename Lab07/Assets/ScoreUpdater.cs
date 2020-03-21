using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdater : MonoBehaviour
{
    public ScoreManager scoreManager;
    public Text record1;
    public Text record2;

    // Start is called before the first frame update
    void Start()
    {
        record1.text = "" + scoreManager.getTime(1);
        record2.text = "" + scoreManager.getTime(2);
    }
}
