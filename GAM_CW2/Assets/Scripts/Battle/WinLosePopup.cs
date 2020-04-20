using UnityEngine;
using UnityEngine.UI;

//! Visualize the rewards gained from winning/losing a match
public class WinLosePopup : MonoBehaviour {

    public Text winMoneyText, winNextLevelText;
    public Text loseMoneyText, loseNextLvlText;
    public Slider winExpSlider, loseExpSlider;
    public AudioSource soundsrc;
    public AudioClip levelup_Sound;

    private Text moneyGained, currentLevel;
    private Slider expSlider;

    private Player before, after;

    private bool didSetup = false;
    private bool gainedLevel = false;
    private float addedExp = 0;

    //! When battle ends, display the Win/Lose popup and animate the EXP and money gain
    private void Update()
    {
        if(Gameplay.updatePlayer!=-1 && didSetup==false)
        {
            Setup();
            didSetup = true;
        }

        if(didSetup==true)
        {
            if (before.experience < after.experience)
            {
                if (expSlider.normalizedValue != 1)
                {
                    expSlider.normalizedValue += 0.002f;

                    if (gainedLevel) addedExp += after.exptolevel * 0.002f;
                    else addedExp += before.exptolevel * 0.002f;

                    if (addedExp >= 1)
                    {
                        before.experience++;
                        addedExp = 0;
                    }
                }
            }
            else if (before.level != after.level)
            {
                if (expSlider.normalizedValue != 1)
                {
                    expSlider.normalizedValue += 0.002f;

                    if (gainedLevel) addedExp += after.exptolevel * 0.002f;
                    else addedExp += before.exptolevel * 0.002f;

                    if (addedExp >= 1)
                    {
                        before.experience++;
                        addedExp = 0;
                    }
                }
                else
                {
                    expSlider.normalizedValue = 0;
                    soundsrc.PlayOneShot(levelup_Sound, PlayerPrefs.GetFloat("fx")/2);
                    before.level++;
                    before.experience = 0;
                    gainedLevel = true;
                    currentLevel.text = "" + before.level;
                }
            }
        }
        
    }

    // Calculate the changes to the Player (rewards)
    private void Setup()
    {
        before = PlayerSession.player_session.player_before_battle;
        after = PlayerSession.player_session.player;
        
        addedExp = 0;

        if(Gameplay.updatePlayer==1)
        {
            moneyGained = loseMoneyText;
            currentLevel = loseNextLvlText;
            expSlider = loseExpSlider;
        }
        else
        {
            moneyGained = winMoneyText;
            currentLevel = winNextLevelText;
            expSlider = winExpSlider;
        }
        
        expSlider.normalizedValue = (float)before.experience / (float)before.exptolevel;
        currentLevel.text = "" + before.level;
        moneyGained.text = "" + (after.money - before.money);

        PlayerSession.player_session.player_before_battle = new Player();
    }
}
