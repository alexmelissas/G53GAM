using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour
{
    public GameObject levelMenu;

    // Start is called before the first frame update
    void Start()
    {
        levelMenu.SetActive(false);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenLevelMenu()
    {
        Debug.Log("TI MANA1 SOU");
        levelMenu.SetActive(true);
    }

    public void LaunchLevel(int level)
    {
        switch (level)
        {
            case 1:
                if (PlayerObjects.singleton.unlocked1)
                    SceneManager.LoadScene("Level1");
                break;
            case 2:
                if (PlayerObjects.singleton.unlocked2)
                    SceneManager.LoadScene("Level2");
                break;
            case 3:
                if (PlayerObjects.singleton.unlocked3)
                    SceneManager.LoadScene("Level2");
                break;
        }
    }

    public void OpenShop()
    {
        Debug.Log("TI MANA SOU");
        SceneManager.LoadScene("Shop");
    }
}
