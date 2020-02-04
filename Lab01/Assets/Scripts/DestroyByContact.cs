using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Can't find 'GameController' script.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || gameObject.tag == "Player")
        {
            //reset score but w/e
            Application.LoadLevel(Application.loadedLevel);
            return;
        }
        gameController.AddScore(10);
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
