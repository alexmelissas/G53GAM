using UnityEngine;
using UnityEngine.SceneManagement;

public class SpashScreen : MonoBehaviour
{
    // INITIATE THE SINGLETON
    void Start()
    {
        RPGCharacter player = new RPGCharacter("Karen", 5);
        PersistentObjects.singleton.player = player;
        PersistentObjects.singleton.currentHP = player.hp;
        SceneManager.LoadScene("Main");
    }
}
