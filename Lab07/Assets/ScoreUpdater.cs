using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdater : MonoBehaviour
{
    public ScoreManager scoreManager;
    public Text record1;
    public Text record2;
    public Button button2;

    // Start is called before the first frame update
    void Start()
    {
        double score1 = scoreManager.getTime(1);
        record1.text = "" + score1;
        if (score1 >= 50.0f)
        {
            button2.enabled = false;
            record2.text = "Locked. Finish 1 under 50s.";
        }
        else
        {
            button2.enabled = true;
            record2.text = "" + scoreManager.getTime(2);
        }
    }
}
