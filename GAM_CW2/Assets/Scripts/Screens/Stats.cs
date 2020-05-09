using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public Text atkText, defText, spdText, agilityText, critText;
    public Text usernameText, lvlText, nextLevelText, xpText;
    public GameObject currentSwordImage, currentShieldImage, currentArmourImage, currentBootsImage;
    public Slider xpSlider;
    private Player p;

    void Start() { p = new Player(); }

    void Update()
    {
        //Only update displays if there's a change
        if (!p.ComparePlayer(PersistentObjects.singleton.player)) 
        {
            p = Player.Clone(PersistentObjects.singleton.player);
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
            xpSlider.normalizedValue = (float)p.xp / (float)p.levelupxp;

            currentSwordImage.GetComponent<RawImage>().texture = RPGItems.CreateItem("sword", p.sword).icon;
            currentShieldImage.GetComponent<RawImage>().texture = RPGItems.CreateItem("shield", p.shield).icon;
            currentArmourImage.GetComponent<RawImage>().texture = RPGItems.CreateItem("armour", p.armour).icon;
            currentBootsImage.GetComponent<RawImage>().texture = RPGItems.CreateItem("boots", p.boots).icon;
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
        output[0] = hpTotal + "(" + hpBase + "+" + hpItem + ")";
        output[1] = atkTotal + "(" + atkBase + "+" + atkItem + ")";
        output[2] = defTotal + "(" + defBase + "+" + defItem + ")";
        output[3] = spdTotal + "(" + spdBase + "+" + spdItem + ")";

        return output;
    }
}
