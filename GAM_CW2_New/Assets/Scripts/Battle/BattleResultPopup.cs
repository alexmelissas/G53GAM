using UnityEngine;
using UnityEngine.UI;

//! Visualize the rewards gained from winning/losing a match
public class BattleResultPopup : MonoBehaviour {

    public Text winCoinsText, winNextLevelText;
    public Text loseCoinsText, loseNextLvlText;
    public Slider winExpSlider, loseExpSlider;
    public AudioSource soundsrc;
    public AudioClip levelup_Sound;

    private Text coinsGained, currentLevel;
    private Slider xpSlider;

    private Player before, after;

    public bool didSetup = false;
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
                    xpSlider.normalizedValue = 0f;
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
        before = PlayerObjects.singleton.player_before_battle;
        after = PlayerObjects.singleton.player;
        
        addedExp = 0;

        if(Gameplay.updatePlayer==1)
        {
            coinsGained = loseCoinsText;
            currentLevel = loseNextLvlText;
            xpSlider = loseExpSlider;
        }
        else
        {
            coinsGained = winCoinsText;
            currentLevel = winNextLevelText;
            xpSlider = winExpSlider;
        }

        xpSlider.value = (float)before.xp / (float)before.levelupxp;
        currentLevel.text = "" + before.level;
        coinsGained.text = "" + (after.coins - before.coins);
        gainedLevel = false;
    }
}
