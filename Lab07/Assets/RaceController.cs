using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class RaceController : MonoBehaviour
{
    public Text resultText;
    public Text timeText;
    public ScoreManager scoreManager;

    private float startTime;

    RaceState raceState;
    GameObject[] AICars;

    enum RaceState
	{
        START, RACING, FINISHED
	};

    // Start is called before the first frame update
    void Start()
    {
        AICars = GameObject.FindGameObjectsWithTag("AICar");
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
        foreach(GameObject car in AICars)
        {
            car.GetComponent<CarAIControl>().enabled = true;
        }

        yield return new WaitForSeconds(1);
        resultText.enabled = false;
	}

    // Update is called once per frame
    void Update()
    {
		if (raceState == RaceState.RACING)
		{
            timeText.text = "" + Math.Round((Time.time - startTime), 2);
		}
    }

    private void onTriggerEnter3D(Collision other)
    {
        Debug.Log("MEEPMOOPMOP");
        if(other.gameObject.tag == "Player")
        {
            scoreManager.setTime((Time.time - startTime), SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(0);
        }
    }
}
