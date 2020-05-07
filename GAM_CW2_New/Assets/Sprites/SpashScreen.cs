using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpashScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //test
        Player test = new Player(
            "Alex",
            1, //level
            0, //xp
            200, //xptolevel
            300, //coins
            120, //hp
            150, //atk
            50, //def
            10,
            10,
            20,
            1,
            1,
            1,
            1,
            10,
            2,
            3,
            1);
        PlayerObjects.singleton.player = test;
        PlayerObjects.singleton.currentHP = 40;


        SceneManager.LoadScene("Main");
    }
}
