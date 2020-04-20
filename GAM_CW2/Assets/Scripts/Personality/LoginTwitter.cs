using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using VoxelBusters.NativePlugins;

//! Twitter login handler
public class LoginTwitter : MonoBehaviour {

    //! Allow user to exit app for twitter, without ending the session
    public static bool leftForTwitter = false;

    //! Allow user to go to Ideals, if not using twitter
    public static bool allowNextForSkip = false;

    //! Avoid having more than 1 popup open at a time
    private bool otherPopupOpen = false;

    private void Start() { otherPopupOpen = false; }
    
    private void EndSession() { leftForTwitter = false; }

    private void SetSkipBool() { allowNextForSkip = true;}

    //! Keep checking if twitter linkage was OK - until timeout (4s after they return from twitter page)
    private IEnumerator TryTwitter()
    {
        int repeats = 0;
        while (repeats < 3)
        {
            UnityWebRequest uwr = UnityWebRequest.Get(Server.Address("read_user") + UserSession.user_session.user.GetID());
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Debug.Log("Error While Sending: " + uwr.error);
                NPBinding.UI.ShowToast("Communication Error. Please try again.", eToastMessageLength.SHORT);
                yield break;
            }
            else
            {
                UpdateSessions.JSON_Session("user", uwr.downloadHandler.text);
                if(Server.CheckTwitter())
                {
                    EndSession();
                    yield break;
                }
                else
                {
                    Debug.Log("Not registered yet: "+uwr.downloadHandler.text);
                    repeats++;
                    yield return new WaitForSeconds(1.3f);
                }
            }

            if (repeats == 2 && !Server.CheckTwitter())
            {
                EndSession();
                ShowPopup("fail_connection");
            }
        }
        StopCoroutine(TryTwitter());
    }


    //! Play without Twitter - pass noauth token to server
    private IEnumerator NoTwitter()
    {
        UnityWebRequest uwr = UnityWebRequest.Get(Server.Address("skip_twitter") + UserSession.user_session.user.GetID());
        uwr.timeout = 10;

        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            NPBinding.UI.ShowToast("Communication Error. Please try again.", eToastMessageLength.SHORT);
        }
        else
        {
            SetSkipBool();
            EndSession();
        }
        
        yield break;
    }

    //! Try to open Twitter login page
    private void TryOpenTwitter(bool acceptedTerms)
    {
        if (acceptedTerms)
        {
            string url = Server.Address("login_twitter") + UserSession.user_session.user.GetID();
            Application.OpenURL(url); // need nicer way -eg window in app
            leftForTwitter = true;
            StartCoroutine(TryTwitter());
        }
    }

    //! Show popups for twitter terms confirmation/failed connection/skip twitter
    public void ShowPopup(string popup)
    {
        if(!otherPopupOpen)
        {
            otherPopupOpen = true;
            leftForTwitter = true;

            string[] _buttons;

            string terms = "By agreeing and linking your twitter you agree to " +
                        "allow access to your public Twitter profile, in order " +
                        "to analyse your personality. Only your tweets will be " +
                        "accessed, and you will be made aware when. Your tweets " +
                        "and data is never stored on our servers; the only personal " +
                        "data stored is your factor of similarity between your " +
                        "self-proclaimed ideal personality and your real one, " +
                        "which will be read from your tweets. Your tweets are merely " +
                        "passed temporarily into Watson, which analyses them and gives " +
                        "us a personality analysis which use to calculate your similarity " +
                        "factor, and then deleted.";
            string noTwitterWarning = "You can play this game without a Twitter account. \nHowever, by " +
                        "not linking your Twitter profile, you'll be missing out on " +
                        "the entire Personality improvement portion of this game." +
                        "This means lower stats and more effort to reach the top!\n" +
                        "Are you sure you'd like to proceed without Twitter?";

            switch (popup)
            {
                case "fail_connection":
                    _buttons = new string[] { "Cancel", "Retry" };
                    NPBinding.UI.ShowAlertDialogWithMultipleButtons("Personality bot is sad",
                        "Twitter Connection didn't work out...", _buttons, OnButtonPressed);
                    break;

                case "show_terms":
                    _buttons = new string[] { "Decline", "Accept" };
                    NPBinding.UI.ShowAlertDialogWithMultipleButtons("Terms of Use", terms, _buttons, OnButtonPressed);
                    break;

                case "terms_relog":
                    _buttons = new string[] { "Cancel", "Accept" };
                    NPBinding.UI.ShowAlertDialogWithMultipleButtons("Terms of Use", terms, _buttons, OnButtonPressed);
                    break;

                case "skip_twitter":
                    _buttons = new string[] { "Link Twitter", "Play without Twitter" };
                    NPBinding.UI.ShowAlertDialogWithMultipleButtons("You're missing out!", noTwitterWarning, _buttons, OnButtonPressed);
                    break;
            }
        }
    }

    //! Handle dialog box button options
    private void OnButtonPressed(string _buttonPressed)
    {
        leftForTwitter = false;
        switch (_buttonPressed)
        {
            case "Retry":
                otherPopupOpen = false;
                string scene = SceneManager.GetActiveScene().name;
                if (scene == "Settings") ShowPopup("terms_relog");
                else ShowPopup("show_terms");
                break;
            case "Accept":
                TryOpenTwitter(true);
                break;
            case "Decline":
                TryOpenTwitter(false);
                break; //dud
            case "Cancel":
                TryOpenTwitter(false);
                break;
            case "Link Twitter":
                otherPopupOpen = false;
                ShowPopup("show_terms");
                break;
            case "Play without Twitter":
                StartCoroutine(NoTwitter());
                break;
        }
        if(_buttonPressed!="Link Twitter")
            otherPopupOpen = false;
        return;
    }
}
