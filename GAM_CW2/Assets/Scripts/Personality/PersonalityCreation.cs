using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using VoxelBusters.NativePlugins;

//! Ideal Personality Creation management
public class PersonalityCreation : MonoBehaviour {

    public Slider trait1Slider, trait2Slider, trait3Slider, trait4Slider, trait5Slider;
    public Button submitButton;
    public Text percentT1Text, percentT2Text, percentT3Text, percentT4Text, percentT5Text;
    private float trait1, trait2, trait3, trait4, trait5;

    //! Take the value of the slider for that attribute
    private float HandleAttribute(Slider slider, Text display)
    {
        float slider_value = Mathf.RoundToInt(slider.value);
        display.text = "" + slider_value;
        if (slider_value > 8) slider_value = 8;
        else if (slider_value < 2) slider_value = 2;
        return slider_value /10;
    }

    //! Keep actual attribute values up-to-date with sliders
    private void Update()
    {
        trait1 = HandleAttribute(trait1Slider, percentT1Text);
        trait2 = HandleAttribute(trait2Slider, percentT2Text);
        trait3 = HandleAttribute(trait3Slider, percentT3Text);
        trait4 = HandleAttribute(trait4Slider, percentT4Text);
        trait5 = HandleAttribute(trait5Slider, percentT5Text);
    }

    //! PUT ideals into server for comparisons
    private IEnumerator PutIdeals(string json)
    {
        UnityWebRequest uwr = new UnityWebRequest(Server.Address("submit_ideals") + UserSession.user_session.user.GetID(), "PUT");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            NPBinding.UI.ShowToast("Communication Error. Please try again later.", eToastMessageLength.SHORT);
        }
        else
        {
            if (uwr.downloadHandler.text == Server.fail_auth)
            {
                NPBinding.UI.ShowToast("Server Error. Please try again.", eToastMessageLength.SHORT);
            }
            else
            {
                UpdateSessions.JSON_Session("player",uwr.downloadHandler.text);
                NPBinding.UI.ShowToast("Welcome to The Battle Within!", eToastMessageLength.SHORT);
                gameObject.AddComponent<ChangeScene>().Forward("Overworld");
            }
        }
        StopCoroutine(PutIdeals(json));
    }

    //! Create JSON to pass to server
    public void Submit () {                                             
        string json = JsonUtility.ToJson(new Ideals(UserSession.user_session.user.GetID(), trait1, trait2, trait3, trait4, trait5));
        StartCoroutine(PutIdeals(json));
    }
}
