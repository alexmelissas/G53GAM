using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpashScreen : MonoBehaviour
{
    void Start()
    {
        RPGCharacter player = new RPGCharacter("Karen", 1);
        PersistentObjects.singleton.player = player;
        PersistentObjects.singleton.currentHP = player.hp;
        SceneManager.LoadScene("Main");
    }
}
