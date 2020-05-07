using UnityEngine;
using UnityEngine.UI;

public class StatsScreen: MonoBehaviour {

    public Text atkText, defText, spdText, agilityText, critText;
    public Text usernameText, lvlText, nextLevelText, xpText;
    public Slider xpSlider;
    private Player p;

    void Start()
    {
        p = new Player();
    }

    void Update () {
        if (!p.ComparePlayer(PlayerObjects.singleton.player))
        {
            p = PlayerObjects.singleton.player;
            string[] split_stats = SplitStats();

            atkText.text = split_stats[1];
            defText.text = split_stats[2];
            spdText.text = split_stats[3];
            agilityText.text = "" + p.agility;
            critText.text = "" + p.crit;

            usernameText.text = p.username;

            xpText.text = "" + p.xp + "/" + p.levelupxp;
            lvlText.text = "" + p.level;
            nextLevelText.text = "" + (p.level + 1);

            float xpBarValue = (float)p.xp / (float)p.levelupxp;
            xpSlider.normalizedValue = xpBarValue;
        }        
    }

    private string[] SplitStats()
    {
        int hpTotal, hpBase, hpItem;
        int atkTotal, atkBase, atkItem;
        int defTotal, defBase, defItem;
        int spdTotal, spdBase, spdItem;

        hpBase = p.hp;
        atkBase = p.atk;
        defBase = p.def;
        spdBase = p.spd;

        p.AttachItems();

        hpTotal = p.hp;
        atkTotal = p.atk;
        defTotal = p.def;
        spdTotal = p.spd;

        hpItem = hpTotal - hpBase;
        atkItem = atkTotal - atkBase;
        defItem = defTotal - defBase;
        spdItem = spdTotal - spdBase;

        string[] output = { "", "", "", "" };

        output[0] = "<b>" + hpTotal + "</b>"
            + "<color=black> (</color>"
            + "<color=yellow>" + hpBase + "</color>"
            + "<color=black>+</color>"
            + "<color=red>" + hpItem + "</color>"
            + "<color=black>)</color>";

        output[1] = "<b>" + atkTotal + "</b>"
            + "<color=black> (</color>"
            + "<color=yellow>" + atkBase + "</color>"
            + "<color=black>+</color>"
            + "<color=red>" + atkItem + "</color>"
            + "<color=black>)</color>";

        output[2] = "<b>" + defTotal + "</b>"
            + "<color=black> (</color>"
            + "<color=yellow>" + defBase + "</color>"
            + "<color=black>+</color>"
            + "<color=red>" + defItem + "</color>"
            + "<color=black>)</color>";

        output[3] = "<b>" + spdTotal + "</b>"
            + "<color=black> (</color>"
            + "<color=yellow>" + spdBase + "</color>"
            + "<color=black>+</color>"
            + "<color=red>" + spdItem + "</color>"
            + "<color=black>)</color>";

        return output;
    }
}
