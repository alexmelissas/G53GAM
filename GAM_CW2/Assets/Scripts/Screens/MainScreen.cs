using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour
{
    public GameObject levelMenu;
    public Image level2lock, level3lock;
    public GameObject coins2, coins3;

    public void Start()
	{
        if (PersistentObjects.singleton.unlocked2) { level2lock.color = Color.green; coins2.SetActive(false); }
        else { level2lock.color = Color.red; coins2.SetActive(true); }
        if (PersistentObjects.singleton.unlocked3) { level3lock.color = Color.green; coins3.SetActive(false); }
        else { level3lock.color = Color.red; coins3.SetActive(true); }
    }

    public void OpenShop() { SceneManager.LoadScene("Shop"); }

    public void OpenLevelMenu() { levelMenu.SetActive(true); }

    public void LaunchLevel(int stage)
    {
        switch (stage)
        {
            case 1:
                if (PersistentObjects.singleton.unlocked1) SceneManager.LoadScene("Level1");
                break;
            case 2:
                if (PersistentObjects.singleton.unlocked2) SceneManager.LoadScene("Level2");
                else if (PersistentObjects.singleton.player.coins >= 350)
				{
                    PersistentObjects.singleton.player.coins -= 350;
                    PersistentObjects.singleton.unlocked2 = true;
                    SceneManager.LoadScene("Level2");
                }

                break;
            case 3:
                if (PersistentObjects.singleton.unlocked2)
				{
                    if (PersistentObjects.singleton.unlocked3) SceneManager.LoadScene("Level3");
                    else if (PersistentObjects.singleton.player.coins >= 600)
                    {
                        PersistentObjects.singleton.player.coins -= 600;
                        PersistentObjects.singleton.unlocked3 = true;
                        SceneManager.LoadScene("Level3");
                    }
                }
                break;
        }
    }

}
