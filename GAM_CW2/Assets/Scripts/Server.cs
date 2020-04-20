using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using VoxelBusters.NativePlugins;

//! Collection of server-side responses, path URLs and functions.
public class Server {

    //! Server's home directory IP address/URL.
    private static readonly string address = "http://35.178.2.5:8080";
    

    //! General success response (eg. For confirming Twitter linkage)
    private static readonly string register_success = "Saved";

    //! Username taken response
    private static readonly string username_taken = "Username has been taken";

    //! Failed to authorise login response (either wrong username or password)
    public static readonly string fail_auth = "Sorrry, authorization fails. Please try again later.";

    //! Successful no-twitter login
    public static readonly string no_twitter_success = "Account creation succss.";

    //! Determining when reanalysis of twitter is done (used by login class)
    public static bool reanalysis_done;

    //! Determining when find enemy is done (used by battle classes)
    public static bool findEnemy_done;

    //! Determining when updating player is done (used when purchasing items)
    public static bool updatePlayer_done;

    //! Returns the particular address for one server API function.
    public static string Address(string service){
        string path;
        switch (service) {
            case "register_user": path = "/users/"; break;
            case "login_user":path = "/users/login"; break;
            case "view_users": path = "/users"; break;
            case "read_user": path = "/users/"; break;
            case "delete_user": path = "/users/"; break;
            case "update_twitter": path = "/reanalysis/"; break;

            case "login_twitter": path = "/auth/"; break;
            case "skip_twitter":  path = "/noauth/"; break;
            case "logout_twitter": path = "/auth/cancel/"; break;

            case "submit_ideals": path = "/ideals/"; break;

            case "players": path = "/players/"; break;

            case "battle": path = "/battle"; break;
            case "get_battle": path = "/battle/"; break;
            case "get_plays": path = "/battle/count/"; break;
            case "get_ranked_players": path = "/battle/ranked/"; break;
            case "get_plays_ranked": path = "/battle/ranked/count/"; break;
            case "ranked_points": path = "/battle/ranked/score/"; break;

            default: path = ""; break;
        }
        return address + path;
    }

    //! Check if username is entire registration process is OK
    public static int CheckRegistration(string output)
    {
        if (output == register_success) return 1;
        else if (output == username_taken) return 0;
        else return 2;
    }

    //! Check if valid login, then update User object
    public static bool CheckLogin(string output)
    {
        if (output == null || output == "") return false;
        else
        {
            UpdateSessions.JSON_Session("user", output);
            ZPlayerPrefs.SetString("id", UserSession.user_session.user.id);
        }
        return true;
    }

    //! Check if user has linked twitter, eg. User entry has twitter token
    public static bool CheckTwitter()
    {
        return (UserSession.user_session.user.accessToken!="" 
            && UserSession.user_session.user.accessTokenSecret!=""
            && UserSession.user_session.user.accessToken != null
            && UserSession.user_session.user.accessTokenSecret != null) 
            ? true : false;
    }

    // +=+=+=+============ Coroutines for common actions ===============+=+=+=+ //

    //! Request the server to reanalyse the twitter of the user for personality changes
    public static IEnumerator Reanalyse()
    {
        reanalysis_done = false;
        string address = Server.Address("update_twitter") + UserSession.user_session.user.id;
        UnityWebRequest uwr = new UnityWebRequest((address), "PUT");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(UserSession.user_session.user.id);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.timeout = 10;

        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError)
        {
            reanalysis_done = true; // just to avoid waiting forever
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            reanalysis_done = true;
            PlayerSession.player_session.player = Player.CreatePlayerFromJSON(uwr.downloadHandler.text);
        }
        uwr.Dispose();

        yield return new WaitForSeconds(1);
        reanalysis_done = false;

        yield break;

    }

    //! Delete the user's account - upon registration error or demand
    public static IEnumerator DeleteAccount()
    {
        Debug.Log("Delete user");
        UnityWebRequest uwr = UnityWebRequest.Delete(Server.Address("delete_user") + UserSession.user_session.user.GetID());
        uwr.timeout = 10;
        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError) yield return DeleteAccount();
        uwr.Dispose();

        UserSession.user_session.user = new User("", "");
        LoginTwitter.allowNextForSkip = false;
        PlayerSession.player_session.player = new Player();
        ZPlayerPrefs.DeleteKey("id");
        ZPlayerPrefs.Save();
        yield break;
    }

    // +=+=+=+============ Battle-Related Coroutines ===============+=+=+=+ //
    
    //! Get either random player as enemy (PvP/Ranked) or a bot (PvE) from server
    public static IEnumerator GetEnemy(int battletype)
    {
        findEnemy_done = false;
        PlayerSession.player_session.enemy = new Player();
        string address = Server.Address("get_battle");
        if (battletype == 0) address += BotScreen.difficulty + "/";
        address += PlayerSession.player_session.player.id;
        using (UnityWebRequest uwr = UnityWebRequest.Get(address))
        {
            uwr.timeout = 10;
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                findEnemy_done = true;
                Debug.Log("Error While Sending: " + uwr.error);
                NPBinding.UI.ShowToast("Communication Error. Please try again later.", eToastMessageLength.SHORT);
            }
            else
            {
                findEnemy_done = true; 
                PlayerSession.player_session.enemy = Player.CreatePlayerFromJSON(uwr.downloadHandler.text);
            }
            uwr.Dispose();
        }
        yield break;
    }

    //! Pass the BattleResult object of the current battle to the server
    public static IEnumerator PassResult(string battle_result, bool ranked)
    {
        string url = Server.Address("battle");
        if (ranked) url += "/ranked";
        UnityWebRequest uwr = new UnityWebRequest(url, "PUT");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(battle_result);
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
            PlayerSession.player_session.player = Player.CreatePlayerFromJSON(uwr.downloadHandler.text);
        
        uwr.Dispose();
        yield break;
    }

    //! Update the Player in the Server (eg. when buying items)
    public static IEnumerator UpdatePlayer(string new_player)
    {
        updatePlayer_done = false;
        UnityWebRequest uwr = new UnityWebRequest(Server.Address("players") + PlayerSession.player_session.player.id, "PUT");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(new_player);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        uwr.timeout = 10;
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            NPBinding.UI.ShowToast("Communication Error. Please try again later.", eToastMessageLength.SHORT);
            updatePlayer_done = false;
        }
        else
        {
            if (uwr.downloadHandler.text == "Failed") updatePlayer_done = false;
            else if (uwr.downloadHandler.text == "Updated") updatePlayer_done = true;
        }

        uwr.Dispose();
        yield break;
    }
}
