using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using VoxelBusters.NativePlugins;

// Usage: add this line to the other class and choose the function
// gameObject.AddComponent<UpdateSessions>()

//! Keep User and Player objects up to date with the server, keep data consistent when exiting app
public class UpdateSessions : MonoBehaviour{

    //! Update all singleton objects in the app
    public void U_All() { StartCoroutine(GetUser(true)); }

    //! Update just the User object
    public void U_User() { StartCoroutine(GetUser(false)); }

    //! Update just the Player object
    public void U_Player() { StartCoroutine(GetPlayer()); }

    //! Check the user's remaining Plays for the day
    private void InvokableGetPlaysLeft() { StartCoroutine(GetPlaysLeft()); }

    //! Check the user's Rank points
    private void InvokableGetRankPoints() { StartCoroutine(GetRankPoints()); }

    //! Update a User/Player object from JSON
    public static void JSON_Session(string session, string json)
    {
        if (session == "user") UserSession.user_session.user = User.CreateUserFromJSON(json);
        else if (session == "player") PlayerSession.player_session.player = Player.CreatePlayerFromJSON(json);
        else return;
    }

    //! GET JSON for Player
    IEnumerator GetPlayer()
    {
        UnityWebRequest uwr = UnityWebRequest.Get(Server.Address("players") + ZPlayerPrefs.GetString("id"));
        uwr.timeout = 10;
        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            NPBinding.UI.ShowToast("Communication Error. Please try again later.", eToastMessageLength.SHORT);
        }
        else
        {
            UpdateSessions.JSON_Session("player", uwr.downloadHandler.text);
        }
        uwr.Dispose();

        Invoke("InvokableGetPlaysLeft", 0.3f);
        Invoke("InvokableGetRankPoints", 0.4f);

        StopCoroutine(GetPlayer());
    }

    //! Get JSON for User
    IEnumerator GetUser(bool all)
    {        
        UnityWebRequest uwr = UnityWebRequest.Get(Server.Address("read_user") + ZPlayerPrefs.GetString("id"));
        uwr.timeout = 10;
        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            NPBinding.UI.ShowToast("Communication Error. Please try again later.", eToastMessageLength.SHORT);
        }
        else
        {
            if (User.CreateUserFromJSON(uwr.downloadHandler.text).GetID() == "") yield break;
            UpdateSessions.JSON_Session("user", uwr.downloadHandler.text);          
            if (all) yield return StartCoroutine(GetPlayer());
        }
        uwr.Dispose();
        StopCoroutine(GetUser(all));
    }

    //! Get the player's remaining plays for the day
    IEnumerator GetPlaysLeft()
    {
        string unranked_address = Server.Address("get_plays") + UserSession.user_session.user.id;

        using (UnityWebRequest uwr = UnityWebRequest.Get(unranked_address))
        {
            uwr.timeout = 10;
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError)
                Debug.Log("Error While Sending: " + uwr.error);
            else
            {
                int played_today = Int32.Parse(uwr.downloadHandler.text);
                PlayerSession.player_session.plays_left_unranked = 10 - played_today;
            }
            uwr.Dispose();
        }

        string ranked_address = Server.Address("get_plays_ranked") + UserSession.user_session.user.id;

        using (UnityWebRequest uwr = UnityWebRequest.Get(ranked_address))
        {
            uwr.timeout = 10;
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError)
                Debug.Log("Error While Sending: " + uwr.error);
            else
            {
                int played_today = Int32.Parse(uwr.downloadHandler.text);
                PlayerSession.player_session.plays_left_ranked = 5 - played_today;
            }
            uwr.Dispose();
        }

        yield break;
    }

    //! Get the player's rank points
    IEnumerator GetRankPoints()
    {
        string unranked_address = Server.Address("ranked_points") + UserSession.user_session.user.id;

        using (UnityWebRequest uwr = UnityWebRequest.Get(unranked_address))
        {
            uwr.timeout = 10;
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError)
                Debug.Log("Error While Sending: " + uwr.error);
            else
                PlayerSession.player_session.PlaceInRank(Int32.Parse(uwr.downloadHandler.text));
            uwr.Dispose();
        }

        yield break;
    }

}
