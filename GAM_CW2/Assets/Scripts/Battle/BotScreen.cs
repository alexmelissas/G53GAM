using UnityEngine;
using VoxelBusters.NativePlugins;

//! PvE screen and difficulty management
public class BotScreen : MonoBehaviour {

    public GameObject selected_easy, selected_medium, selected_hard, loading;
    public AudioSource audiosrc;
    public static string difficulty;

    //! Pre-select the stored difficulty setting, play sounds
    private void Start()
    {
        loading.SetActive(false);
        int difficultynumber = PlayerPrefs.GetInt("bot_difficulty");
        switch (difficultynumber)
        {
            case 0: difficulty = "easy"; break;
            case 1: difficulty = "medium"; break;
            case 2: difficulty = "hard"; break;
            default: break;
        }
        audiosrc.volume = PlayerPrefs.GetFloat("fx")/2;
        audiosrc.playOnAwake = true;
    }

    //! Keep the difficulty updated
    private void Update()
    {
        string selected = "selected_" + difficulty;
        switch (selected)
        {
            case "selected_easy":
                PlayerPrefs.SetInt("bot_difficulty", 0);
                selected_easy.SetActive(true);
                selected_medium.SetActive(false);
                selected_hard.SetActive(false);
                break;
            case "selected_medium":
                PlayerPrefs.SetInt("bot_difficulty", 1);
                selected_easy.SetActive(false);
                selected_medium.SetActive(true);
                selected_hard.SetActive(false);
                break;
            case "selected_hard":
                PlayerPrefs.SetInt("bot_difficulty", 2);
                selected_easy.SetActive(false);
                selected_medium.SetActive(false);
                selected_hard.SetActive(true);
                break;
        }
    }

    //! Update the difficulty to the selected one.
    public void ChangeDifficulty(string given_difficulty) { difficulty = given_difficulty; }
    
    //! Initiate the PvE match sequence
    public void Play()
    {
        if (PlayerSession.player_session.plays_left_unranked <= 0)
        {
            NPBinding.UI.ShowToast("No plays left. Check back tomorrow!", eToastMessageLength.SHORT);
            return;
        }
        loading.SetActive(true);
        PlayerPrefs.SetInt("battle_type", 0);
        StartCoroutine(Server.GetEnemy(0));
        Invoke("PlayDelayed", 0.5f);
    }

    //! Forward to the Battle scene
    private void PlayDelayed() { gameObject.AddComponent<ChangeScene>().Forward("Battle"); }

}
