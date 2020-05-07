using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreen : MonoBehaviour
{
    public GameObject levelMenu;

    // Start is called before the first frame update
    void Start()
    {
        levelMenu.SetActive(false);

        //test
        Player test = new Player(
            "Alex",
            12, //level
            100, //xp
            200, //xptolevel
            10, //coins
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenLevelMenu()
    {
        levelMenu.SetActive(true);
    }

    public void LaunchLevel(int level)
    {
        switch (level)
        {
            case 1:
                if (PlayerObjects.singleton.unlocked1)
                Application.LoadLevel("Level1");
                break;
            case 2:
                if (PlayerObjects.singleton.unlocked2)
                    Application.LoadLevel("Level1");
                break;
            case 3:
                if (PlayerObjects.singleton.unlocked3)
                    Application.LoadLevel("Level1");
                break;
        }
    }
}
