using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    private Player p;
    public Text atkText, defText, spdText, agilityText, critText;
    public Slider hpSlider;
    public Image playerHPColourImage;
    public Text playerNameText, playerLevelText, maxPlayerHPText, actualPlayerHPText;
    public Slider xpSlider;
    public Text usernameText, lvlText, nextLevelText, xpText;
    public GameObject currentSwordImage, currentShieldImage, currentBootsImage;

    void Start() { p = new Player("",0); }

    void Update()
    {
        p = Player.HardCopy(PersistentObjects.singleton.player);
        string[] split_stats = SplitStats();

        atkText.text = split_stats[1];
        defText.text = split_stats[2];
        spdText.text = split_stats[3];
        agilityText.text = "" + p.agility;
        critText.text = "" + p.crit;

        if (xpSlider != null)
        {
            usernameText.text = p.username;
            xpText.text = "" + p.xp + "/" + p.levelupxp;
            lvlText.text = "" + p.level;
            nextLevelText.text = "" + (p.level + 1);
            xpSlider.normalizedValue = (float)p.xp / (float)p.levelupxp;
        }
        else if (hpSlider != null)
        {
            Player player = Player.HardCopy(PersistentObjects.singleton.player);
            player.AttachItems();

            playerNameText.text = "" + player.username;
            playerLevelText.text = "" + player.level;
            maxPlayerHPText.text = "/" + player.hp;
            actualPlayerHPText.text = "" + PersistentObjects.singleton.currentHP;

            playerHPColourImage.enabled = true;
            float hpBarValue = (float)PersistentObjects.singleton.currentHP / (float)player.hp;
            hpSlider.value = hpBarValue;
            if (hpSlider.value == 0) playerHPColourImage.enabled = false;
            else if (hpBarValue < 0.25) playerHPColourImage.color = Color.red;
            else if (hpBarValue < 0.5) playerHPColourImage.color = Color.yellow;
            else playerHPColourImage.color = Color.green;
        }
        

        currentSwordImage.GetComponent<RawImage>().texture = RPGItems.CreateItem("sword", p.sword).icon;
        currentShieldImage.GetComponent<RawImage>().texture = RPGItems.CreateItem("shield", p.shield).icon;
        currentBootsImage.GetComponent<RawImage>().texture = RPGItems.CreateItem("boots", p.boots).icon;
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
