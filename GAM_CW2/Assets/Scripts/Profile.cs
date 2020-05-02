using System;
using UnityEngine;
using UnityEngine.UI;

//! Display player stats in Profile screen
public class Profile: MonoBehaviour {

    public Text hpText, atkText, defText, spdText, agilityText, critText;
    public Text lvlText, nextLevelText, expText;
    public Slider expSlider;
    private Player p;
    
    void Start()
    {
        p = new Player();
        hpText.supportRichText = true;
        atkText.supportRichText = true;
        defText.supportRichText = true;
    }

    //! Display the player's stats - update only if a change is detected
    void Update () {
        if (!p.ComparePlayer(PlayerObjects.playerObjects.player))
        {
            p = PlayerObjects.playerObjects.player;
            Stats stats = new Stats(p);

            hpText.text = stats.StatsToStrings()[0];
            atkText.text = stats.StatsToStrings()[1];
            defText.text = stats.StatsToStrings()[2];
            // spd

            agilityText.text = "" + p.agility;
            critText.text = "" + p.crit;

            expText.text = "" + p.xp + "/" + p.levelupxp;
            lvlText.text = "" + p.level;
            nextLevelText.text = "" + (p.level + 1);

            float expBarValue = (float)p.xp / (float)p.levelupxp;
            expSlider.normalizedValue = expBarValue;

        }
    }
}
