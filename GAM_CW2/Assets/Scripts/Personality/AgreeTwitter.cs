using UnityEngine;
using UnityEngine.UI;

//! Check for twitter linkage and allow user to proceed.
public class AgreeTwitter : MonoBehaviour {

    public Toggle agreeToggle;
    public Button nextButton;
    public GameObject consent;
    public AudioSource audioSrc;
    public AudioClip confirmSound;
    
    private void Awake()
    {
        nextButton.enabled = false;
        nextButton.GetComponent<Image>().color = Color.grey;
    }

    private void Start()
    {
        LoginTwitter.allowNextForSkip = false;
    }

    //! Keep checking whether to display the Twitter consent option
    void Update() {

        if (LoginTwitter.allowNextForSkip)
        {
            nextButton.enabled = true;
            nextButton.GetComponent<Image>().color = Color.white;
            return;
        }

        if (Server.CheckTwitter())
        {
            consent.SetActive(true);
            audioSrc.PlayOneShot(confirmSound, PlayerPrefs.GetFloat("fx"));
        }
        else
        {
            consent.SetActive(false);
        }

        if (agreeToggle.isOn == true) {
            nextButton.enabled = true;
            nextButton.GetComponent<Image>().color = Color.white;
        }
        else {
            nextButton.enabled = false;
            nextButton.GetComponent<Image>().color = Color.grey;
        }
	}

    //! Done with twitter (either logged in or skipped) - move to Ideals selection
    public void Next()
    {
        LoginTwitter.allowNextForSkip = false;
        gameObject.AddComponent<ChangeScene>().Forward("Ideals");
    }
}
