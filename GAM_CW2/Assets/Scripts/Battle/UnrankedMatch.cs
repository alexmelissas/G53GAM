using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//! Ranked PvP screen handling
public class UnrankedMatch : MonoBehaviour
{
    public GameObject loading_spin_Animation;
    public AudioSource audiosrc;

    private int attempts;
    private float max_volume;

    //! Setup the screen
    private void Start()
    {
        loading_spin_Animation.SetActive(false);
        max_volume = PlayerPrefs.GetFloat("fx") / 2;
        audiosrc.playOnAwake = true;
        audiosrc.volume = 0;
    }

    //! Used for fading in the music
    private void Update()
    {
        if (audiosrc.volume < max_volume)
        {
            audiosrc.volume += 0.008f; // fade-in for music
        }
    }

    //! Initiate the PvP match
    public void Play()
    {
        if (PlayerSession.player_session.plays_left_unranked <= 0)
        {
            Debug.Log("No plays left. Check back tomorrow!");
            return;
        }
        loading_spin_Animation.SetActive(true);
        PlayerPrefs.SetInt("battle_type", 2);
        attempts = 0;
        Invoke("StartCheck", 0.4f);
    }

    //! Press button to initiate matchmaking
    private void StartCheck() { StartCoroutine(CheckEnemy()); }

    //! Increase attempts
    private void IncreaseAttempts() { attempts++; }

    //! Recursively try to find an enemy 3 times (in case of errors). If not found after 4 tries, stop.
    private IEnumerator CheckEnemy()
    {
        //StartCoroutine(Server.GetEnemy(1));
        //yield return new WaitUntil(() => Server.findEnemy_done == true);

        if (PlayerSession.player_session.enemy.id != "")
        {
            gameObject.AddComponent<ChangeScene>().Forward("Battle");
        }
        else if (attempts < 3) //recursively try to find enemy 3 times
        {
            IncreaseAttempts();
            StartCoroutine(CheckEnemy());
        }
        else
            //NPBinding.UI.ShowToast("No enemy found. Try again later.", eToastMessageLength.SHORT);

        yield break;
    }

}
