using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using VoxelBusters.NativePlugins;

//! Logout: Either the User from app or unlink Twitter from User
public class Logout : MonoBehaviour {

    //! Remove User, Player and id from memory.
    public void LogoutUser()
    {
        UserSession.user_session.user = new User("","");
        PlayerSession.player_session.player = new Player();
        PlayerPrefs.DeleteKey("id");
        gameObject.AddComponent<ChangeScene>().Forward("Start");
    }

    //! Unlink the Twitter account from the User
    public void UnlinkTwitter() { StartCoroutine(UnauthTwitter()); }

    //! Server-request to unlink the Twitter account from the User
    private IEnumerator UnauthTwitter()
    {
        UnityWebRequest uwr = UnityWebRequest.Get(Server.Address("logout_twitter") + UserSession.user_session.user.GetID());
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            NPBinding.UI.ShowToast("Communication Error. Please try again.", eToastMessageLength.SHORT);
        }
        else
        {
            NPBinding.UI.ShowToast("Successfully unlinked Twitter.", eToastMessageLength.SHORT);
            gameObject.AddComponent<UpdateSessions>().U_All();
        }
        
        yield break;
    }
}