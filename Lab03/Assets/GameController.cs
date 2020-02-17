using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text timeText;
    public GameObject victory;
    public GameObject defeat;

    public float timeLeft = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        victory.SetActive(false);
        defeat.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            StartCoroutine(WaitForRestart(false));
        }

        timeText.text = timeLeft.ToString("0");
    }

    public void TargetDestroyed()
    {
        if(GameObject.FindObjectsOfType<Destroyable>().Length == 0)
        {
            StartCoroutine(WaitForRestart(true));
        }
    }

    private IEnumerator WaitForRestart(bool win)
    {
        if (win)
        {
            Debug.Log("Victory!");
            victory.SetActive(true);
        }
        else
        {
            Debug.Log("Defeat");
            defeat.SetActive(true);
        }
        yield return new WaitForSeconds(5.0f);
        Application.LoadLevel(Application.loadedLevel);
    }
}
