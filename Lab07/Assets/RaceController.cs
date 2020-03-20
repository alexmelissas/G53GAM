using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceController : MonoBehaviour
{
    public Text resultText;
    public Text timeText;
    private float startTime;

    RaceState raceState;

    enum RaceState
	{
        START, RACING, FINISHED
	};

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startCountdown());
        raceState = RaceState.START;
    }

    IEnumerator startCountdown()
	{
        int count = 3;
		while (count > 0)
		{
            resultText.text = "" + count;
            count--;
            yield return new WaitForSeconds(1);
		}
        raceState = RaceState.RACING;
        startTime = Time.time;
        resultText.text = "GO";

        yield return new WaitForSeconds(1);
        resultText.enabled = false;
	}

    // Update is called once per frame
    void Update()
    {
		if (raceState == RaceState.RACING)
		{
            timeText.text = "" + (Time.time - startTime);
		}
    }
}
