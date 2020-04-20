using UnityEngine;
using UnityEngine.SceneManagement;

//! Keep User/Player data stored when exiting app, reload them when coming back
public class PauseManager : MonoBehaviour {

    //! Check if this is the initial opening of the app
    private bool start = true;
    private bool skipSelected;

    //! Set the salt for the encryption
    private void Awake() { ZPlayerPrefs.Initialize("small fluffy puppies", "I'mASaltySalter"); }

    //! Save the settings and player id, or delete the account upon registration error
    private void Save()
    {
        skipSelected = (PlayerPrefs.GetInt("skip")) == 1 ? true : false;
        if (!skipSelected && SceneManager.GetActiveScene().name == "Battle") PlayerPrefs.SetInt("skip", 1);

        if (UserSession.user_session.user != null && UserSession.user_session.user.GetID() != "")
        {
            if(StartScreens() && !LoginTwitter.leftForTwitter && !AuthenticateUser.display_loginAnimation)
            {
                StartCoroutine(Server.DeleteAccount());
            }
            else
            {
                ZPlayerPrefs.SetString("id", UserSession.user_session.user.GetID());
                ZPlayerPrefs.Save();
            }
        }
    }

    //! Check if account registration is correctly completed
    private bool AccountComplete()
    {
        gameObject.AddComponent<UpdateSessions>().U_Player();
        if (PlayerSession.player_session.player.id == "") return false;
        return true;
    }
    
    //! Load the settings and player id
    private void Load()
    {
        if (!start)
        {
            if (skipSelected == false) PlayerPrefs.SetInt("skip", 0);
            if (ZPlayerPrefs.HasKey("id") && ZPlayerPrefs.GetRowString("id") != "")
            {
                if(StartScreens() && !LoginTwitter.leftForTwitter)
                    gameObject.AddComponent<ChangeScene>().Forward("Start");
            }
            else if(!LoginTwitter.leftForTwitter)
            {
                Debug.Log("Exception: no ID");
                gameObject.AddComponent<ChangeScene>().Forward("Start");
            }
        }         
    }

    //! Check if current screen is one of the Starting screens
    private bool StartScreens()
    {
        string scene = SceneManager.GetActiveScene().name;
        if (scene == "Login" || scene == "Twitter" || scene == "Ideals" || scene == "Register")
            return true;
        return false;
    }

    //! Handle exiting the app
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            start = false;
            Save();
        }
        else Load();
    }

}
