using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using VoxelBusters.NativePlugins;

//! Login and Registration processing.
public class AuthenticateUser : MonoBehaviour {

    public GameObject loading_spin_Animation;
    public InputField usernameInputField;
    public InputField passwordInputField;
    public AudioSource audioSrc;
    public AudioClip confirmSound;

    public string authType;
    public static bool display_loginAnimation;
    private bool lock_attempt; //avoid duplicate register requests

    // Setup the screen
    void Start()
    {
        loading_spin_Animation.SetActive(false);
        display_loginAnimation = false;
        lock_attempt = false;
        passwordInputField.onEndEdit.AddListener(delegate { CheckUserPass(); });
    }

    //! Toggle the loading circle to be active/not
    private void Toggle(bool input) { display_loginAnimation = input; }

    //! Allow request for registration - avoid clashes
    private void Unlock() { lock_attempt = false; }

    //! Try to login with given credentials.
    IEnumerator TryLogin(bool firstLogin, string json, User user)
    {
        if (firstLogin && UserSession.user_session.user.GetUsername() == "") yield break;
        Toggle(true);
        UnityWebRequest uwr = new UnityWebRequest(Server.Address("login_user"), "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.timeout = 10;

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            NPBinding.UI.ShowToast("Communication Error. Please try again later.", eToastMessageLength.SHORT);
        }
        else
        {
            if (Server.CheckLogin(uwr.downloadHandler.text))
            {
                string next_scene = "Overworld";
                if (firstLogin)
                    next_scene = "Twitter";
                if (!firstLogin)
                {
                    gameObject.AddComponent<UpdateSessions>().U_Player();
                    if(Server.CheckTwitter())
                    {
                        StartCoroutine(Server.Reanalyse());
                        loading_spin_Animation.SetActive(true);
                        yield return new WaitUntil(() => Server.reanalysis_done == true);
                        loading_spin_Animation.SetActive(false);
                    }
                }
                if (!firstLogin) NPBinding.UI.ShowToast("Welcome back, " + user.GetUsername(), eToastMessageLength.SHORT);
                audioSrc.PlayOneShot(confirmSound, PlayerPrefs.GetFloat("fx"));
                yield return new WaitForSeconds(0.5f);
                gameObject.AddComponent<ChangeScene>().Forward(next_scene);
            }
            else
            {
                Debug.Log("Invalid Credentials");
                NPBinding.UI.ShowToast("Invalid Credentials.", eToastMessageLength.SHORT);
                passwordInputField.text = "";
                passwordInputField.Select();
            }
            Unlock();
            Toggle(false);
            uwr.Dispose();
            StopCoroutine(TryLogin(firstLogin,json, user));
        }
    }

    //! Try to register an account with given credentials.
    IEnumerator TryRegister(string json, User user)
    {
        UnityWebRequest uwr = new UnityWebRequest(Server.Address("register_user"), "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.timeout = 10;

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            NPBinding.UI.ShowToast("Communication Error. Please try again later.", eToastMessageLength.SHORT);
            Unlock();
        }
        else
        { 
            int response = Server.CheckRegistration(uwr.downloadHandler.text);
            if (response == 1)
            {
                UserSession.user_session.user = user;
                Debug.Log("Account created");
                NPBinding.UI.ShowToast("Account Created.", eToastMessageLength.SHORT);
                yield return StartCoroutine(TryLogin(true, json, user));
            }
            else
            {
                UserSession.user_session.user = new User("","");
                if (response == 0) NPBinding.UI.ShowToast("Sorry, that username is taken. Please try something else.", eToastMessageLength.SHORT);
                if (response == 0) Debug.Log("Username taken.");
                usernameInputField.text = "";
                usernameInputField.Select();
                Unlock();
            }
        }
        uwr.Dispose();
        StopCoroutine(TryRegister(json, user));
    }

    //! Check format of username/password, pass them to Login/Register if valid
    public void CheckUserPass()
    {
        if (lock_attempt) return;
        lock_attempt = true;

        string username = usernameInputField.text;
        string password = passwordInputField.text;
        
        if (username == "")
        {
            Debug.Log("Enter username.");
            NPBinding.UI.ShowToast("Need a username.", eToastMessageLength.SHORT);
            Unlock();
            return;
        }
        else if (password == "")
        {
            Debug.Log("Enter password.");
            NPBinding.UI.ShowToast("Need a password.", eToastMessageLength.SHORT);
            Unlock();
            return;
        }
        else if (username.Length > 25)
        {
            Debug.Log("Username max length 25 characters.");
            NPBinding.UI.ShowToast("Username max length 25 characters.", eToastMessageLength.SHORT);
            Unlock();
            return;
        }
        else
        {
            User user = new User(username, password);
            string json = JsonUtility.ToJson(user);
            if (authType == "register")
                StartCoroutine(TryRegister(json, user));
            else
                StartCoroutine(TryLogin(false, json, user));
        }
        return;
    }
}
