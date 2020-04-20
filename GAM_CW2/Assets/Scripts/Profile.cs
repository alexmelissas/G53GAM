using System;
using UnityEngine;
using UnityEngine.UI;

//! Display player stats in Profile screen
public class Profile: MonoBehaviour {

    public Text hpText, atkText, defText, agilityText, critText;
    public Text rpText, rpTitleText, factorTagText, lvlText, nextLevelText, expText;
    public Slider expSlider, factorSlider;
    public GameObject rankImage;

    private Texture2D nullIcon, bronzeIcon, silverIcon, goldIcon;
    private Player p;
    
    //! Instantly update the Player's stats when opening the screen
    private void Awake() { gameObject.AddComponent<UpdateSessions>().U_All(); }

    void Start()
    {
        p = new Player();
        gameObject.AddComponent<UpdateSessions>().U_Player();
        hpText.supportRichText = true;
        atkText.supportRichText = true;
        defText.supportRichText = true;
        bronzeIcon = Resources.Load("bronze") as Texture2D;
        silverIcon = Resources.Load("silver") as Texture2D;
        goldIcon = Resources.Load("gold") as Texture2D;
        nullIcon = Resources.Load("unranked") as Texture2D;
    }

    //! Display the player's stats - update only if a change is detected
    void Update () {
        if (!(p.ComparePlayer(PlayerSession.player_session.player)))
        {
            p = PlayerSession.player_session.player;
            Stats stats = new Stats(p);
            
            Texture2D rankIcon = null;
            switch (PlayerSession.player_session.rank)
            {
                case "unranked": rankIcon = nullIcon; break;
                case "bronze": rankIcon = bronzeIcon; break;
                case "silver": rankIcon = silverIcon; break;
                case "gold": rankIcon = goldIcon; break;
            }
            rankImage.GetComponent<RawImage>().texture = rankIcon;
            
            if(PlayerSession.player_session.rank == "unranked")
            {
                rpTitleText.text = "";
                rpText.text = "";
            }
            else
                rpText.text = "" + PlayerSession.player_session.rank_points;

            hpText.text = stats.StatsToStrings()[0];
            atkText.text = stats.StatsToStrings()[1];
            defText.text = stats.StatsToStrings()[2];

            agilityText.text = "" + p.agility;
            critText.text = "" + p.critical_strike;

            double max_factor = 0.8f;

            double factor = (p.factor - 1) / max_factor;
            factorSlider.normalizedValue = (float)factor;
            double factor_percent = Math.Round(factor, 2) * 100;
            if (factor_percent == 0) factorTagText.text = "N/A";
            else factorTagText.text = "" + factor_percent + "%";


            expText.text = "" + p.experience + "/" + p.exptolevel;
            lvlText.text = "" + p.level;
            nextLevelText.text = "" + (p.level + 1);

            float expBarValue = (float)p.experience / (float)p.exptolevel;
            expSlider.normalizedValue = expBarValue;

        }
    }
}
