using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        int coins = PersistentObjects.singleton.player.coins;
        if (coins > 0) PersistentObjects.singleton.player.coins -= 50;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
