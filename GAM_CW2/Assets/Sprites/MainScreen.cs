using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour
{
    public GameObject levelMenu;

    public void OpenShop() { SceneManager.LoadScene("Shop"); }

    public void OpenLevelMenu() { levelMenu.SetActive(true); }

    public void LaunchLevel(int stage)
    {
        switch (stage)
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
                    SceneManager.LoadScene("Level3");
                break;
        }
    }

}
