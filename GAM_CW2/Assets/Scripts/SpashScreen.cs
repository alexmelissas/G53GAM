using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpashScreen : MonoBehaviour
{
    void Start()
    {
        RPGCharacter player = new RPGCharacter("Alex", 1);
        PersistentObjects.singleton.player = player;
        PersistentObjects.singleton.currentHP = player.hp;

        // TEST
        PersistentObjects.singleton.currentHP = 40;

        SceneManager.LoadScene("Main");
    }
}
