using UnityEngine;
using UnityEngine.UI;

//! Handle the Settings screen
public class SetSettings : MonoBehaviour {

    public Slider musicSlider, fxSlider;
    public Toggle skipToggle;
    public Text currentloginText, usernameText;

    //! Display the currently selected settings properly
    private void Start()
    {
        musicSlider.normalizedValue = PlayerPrefs.GetFloat("music", 0.65f);
        fxSlider.normalizedValue = PlayerPrefs.GetFloat("fx", 0.7f);
        skipToggle.isOn = (PlayerPrefs.GetInt("skip") != 0);
    }

    //! Update the settings based on changes made to the sliders/checkboxes
    void Update () {
        PlayerPrefs.SetFloat("music",musicSlider.value);
        PlayerPrefs.SetFloat("fx", fxSlider.value);
        PlayerPrefs.SetInt("skip", skipToggle.isOn ? 1 : 0);

        currentloginText.GetComponentInChildren<Text>().text = "Playing as: ";
        usernameText.GetComponentInChildren<Text>().text = PlayerObjects.playerObjects.player.username;

    }      
}
