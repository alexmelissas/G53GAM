using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//! Manage the Inventory screen, including Upgrades
public class Inventory : MonoBehaviour
{

    public Text hpText, atkText, defText, spdText, coinsText;
    public GameObject upgradePanel;
    public Text itemNameText, statText, priceText, balanceText;
    public GameObject armourIconImage, shieldIconImage, swordIconImage, bootsIconImage, statIconImage;
    public GameObject currentSwordImage, currentShieldImage, currentArmourImage, currentBootsImage;
    public Sprite atk, def, hp, spd;
    public AudioSource soundsrc;
    public AudioClip purchase_sound;

    private Player p;
    private Item displayItem;
    private string displayItemType;
    private int currentItemLevel;

    private void Start()
    {
        p = new Player();
        hpText.supportRichText = true;
        atkText.supportRichText = true;
        defText.supportRichText = true;
        spdText.supportRichText = true;
        Displayed(false);
    }

    private void Update()
    {
        if (!(p.ComparePlayer(PlayerObjects.singleton.player)))
        {
            p = PlayerObjects.singleton.player;
            string[] split_stats = SplitStats();

            hpText.text = split_stats[0];
            atkText.text = split_stats[1];
            defText.text = split_stats[2];
            spdText.text = split_stats[3];
            coinsText.text = "" + p.coins;

            currentSwordImage.GetComponent<RawImage>().texture = Item.NewItem("sword", p.sword).icon;
            currentShieldImage.GetComponent<RawImage>().texture = Item.NewItem("shield", p.shield).icon;
            currentArmourImage.GetComponent<RawImage>().texture = Item.NewItem("armour", p.armour).icon;
            currentBootsImage.GetComponent<RawImage>().texture = Item.NewItem("boots", p.boots).icon;
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

    private void Displayed(bool shown)
    {
        Vector3 hide = new Vector3(-791.5f, -1231.1f, 0);
        Vector3 show = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        upgradePanel.transform.position = shown ? show : hide;
    }

    //! Setup the item upgrade panel with the next level item of selected type
    public void SetupUpgradePanel(int item_type)
    {
        if (item_type < 0 || item_type > 2) return;
        currentItemLevel = 0;

        switch (item_type)
        {
            case 0: // Sword
                currentItemLevel = PlayerObjects.singleton.player.sword;
                displayItemType = "sword";
                break;

            case 1: // Shield
                currentItemLevel = PlayerObjects.singleton.player.shield;
                displayItemType = "shield";
                break;

            case 2: // Armour
                currentItemLevel = PlayerObjects.singleton.player.armour;
                displayItemType = "armour";
                break;

            case 3: // Boots
                currentItemLevel = PlayerObjects.singleton.player.armour;
                displayItemType = "boots";
                break;
        }

        if (currentItemLevel >= 4)
        {
            Debug.Log("Item Fully Upgraded!");
            return;
        }
        else
            displayItem = Item.NewItem(displayItemType, currentItemLevel + 1);

        if (displayItemType == "sword") // Update the icon
        {
            armourIconImage.SetActive(false);
            shieldIconImage.SetActive(false);
            swordIconImage.SetActive(true);
            swordIconImage.GetComponent<RawImage>().texture = displayItem.icon;
        }
        else if (displayItemType == "shield")
        {
            armourIconImage.SetActive(false);
            shieldIconImage.SetActive(true);
            swordIconImage.SetActive(false);
            shieldIconImage.GetComponent<RawImage>().texture = displayItem.icon;
        }
        else if (displayItemType == "armour")
        {
            armourIconImage.SetActive(true);
            shieldIconImage.SetActive(false);
            swordIconImage.SetActive(false);
            armourIconImage.GetComponent<RawImage>().texture = displayItem.icon;
        }
        else if (displayItemType == "boots")
        {
            armourIconImage.SetActive(true);
            shieldIconImage.SetActive(false);
            swordIconImage.SetActive(false);
            armourIconImage.GetComponent<RawImage>().texture = displayItem.icon;
        }
        UpdateLabels(item_type);
        Displayed(true);
    }

    //! Helper for SetupUpgradePanels
    private void UpdateLabels(int item_type)
    {
        int stat = 0;
        Sprite statIcon = atk;

        switch (item_type)
        {
            case 0:
                statIcon = atk;
                stat = displayItem.atk;
                break;
            case 1:
                statIcon = def;
                stat = displayItem.def;
                break;
            case 2:
                statIcon = hp;
                stat = displayItem.hp;
                break;
        }

        statIconImage.GetComponent<Image>().sprite = statIcon;
        itemNameText.text = displayItem.name;
        statText.text = "" + stat;
        balanceText.text = "" + PlayerObjects.singleton.player.coins;
        priceText.text = "" + displayItem.price;
    }

    //! Perform the server-side updating of the Player based on the purchase
    private IEnumerator Purchase(string poorerPlayerJSON)
    {
        //StartCoroutine(Server.UpdatePlayer(poorerPlayerJSON));
        //yield return new WaitUntil(() => Server.updatePlayer_done == true);
        //soundsrc.PlayOneShot(purchase_sound, PlayerPrefs.GetFloat("fx"));
        //gameObject.AddComponent<UpdateSessions>().U_Player();
        //Displayed(false);
        yield break;
    }

    //! Check if player has enough funds, then make the purchase
    public void ConfirmPurchase()
    {
        if (PlayerObjects.singleton.player.coins >= displayItem.price)
        {
            Player poorerPlayer = Player.Clone(PlayerObjects.singleton.player);
            poorerPlayer.coins -= displayItem.price;
            switch (displayItemType)
            {
                case "sword": poorerPlayer.sword++; break;
                case "shield": poorerPlayer.shield++; break;
                case "armour": poorerPlayer.armour++; break;
            }

            string poorerPlayerJSON = JsonUtility.ToJson(poorerPlayer);
            StartCoroutine(Purchase(poorerPlayerJSON));
        }
        else
        {
            Debug.Log("Insufficient Funds.");
            //gameObject.AddComponent<UpdateSessions>().U_Player();
            Displayed(false);
        }
    }

}
