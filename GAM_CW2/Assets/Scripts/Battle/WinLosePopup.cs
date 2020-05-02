using UnityEngine;
using UnityEngine.UI;

//! Visualize the rewards gained from winning/losing a match
public class WinLosePopup : MonoBehaviour {

    public Text winMoneyText, winNextLevelText;
    public Text loseMoneyText, loseNextLvlText;
    public Slider winExpSlider, loseExpSlider;
    public AudioSource soundsrc;
    public AudioClip levelup_Sound;

    private Text coinsGained, currentLevel;
    private Slider xpSlider;

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
            if (before.xp < after.xp)
            {
                if (xpSlider.normalizedValue != 1)
                {
                    xpSlider.normalizedValue += 0.002f;

                    if (gainedLevel) addedExp += after.levelupxp * 0.002f;
                    else addedExp += before.levelupxp * 0.002f;

                    if (addedExp >= 1)
                    {
                        before.xp++;
                        addedExp = 0;
                    }
                }
            }
            else if (before.level != after.level)
            {
                if (xpSlider.normalizedValue != 1)
                {
                    xpSlider.normalizedValue += 0.002f;

                    if (gainedLevel) addedExp += after.levelupxp * 0.002f;
                    else addedExp += before.levelupxp * 0.002f;

                    if (addedExp >= 1)
                    {
                        before.xp++;
                        addedExp = 0;
                    }
                }
                else
                {
                    xpSlider.normalizedValue = 0;
                    soundsrc.PlayOneShot(levelup_Sound, PlayerPrefs.GetFloat("fx")/2);
                    before.level++;
                    before.xp = 0;
                    gainedLevel = true;
                    currentLevel.text = "" + before.level;
                }
            }
        }
        
    }

    // Calculate the changes to the Player (rewards)
    private void Setup()
    {
        before = PlayerObjects.playerObjects.player_before_battle;
        after = PlayerObjects.playerObjects.player;
        
        addedExp = 0;

        if(Gameplay.updatePlayer==1)
        {
            coinsGained = loseMoneyText;
            currentLevel = loseNextLvlText;
            xpSlider = loseExpSlider;
        }
        else
        {
            coinsGained = winMoneyText;
            currentLevel = winNextLevelText;
            xpSlider = winExpSlider;
        }

        xpSlider.normalizedValue = (float)before.xp / (float)before.levelupxp;
        currentLevel.text = "" + before.level;
        coinsGained.text = "" + (after.coins - before.coins);

        PlayerObjects.playerObjects.player_before_battle = new Player();
    }
}
