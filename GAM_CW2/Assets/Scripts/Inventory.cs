using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.NativePlugins;

//! Manage the Inventory screen, including Upgrades
public class Inventory : MonoBehaviour {

    public Text hpText, atkText, defText, moneyText;
    public GameObject upgradePanel;
    public Text itemNameText, statText, priceText, balanceText;
    public GameObject armourIconImage, shieldIconImage, swordIconImage, statIconImage;
    public GameObject currentSwordImage, currentShieldImage, currentArmourImage;
    public Sprite atk, def, hp;
    public AudioSource soundsrc;
    public AudioClip purchase_sound;

    private Player p;
    private Item displayItem;
    private string displayItemType;
    private int currentItemLevel;
    

    private void Awake() { gameObject.AddComponent<UpdateSessions>().U_All(); }

    //! General Setup
    private void Start()
    {
        p = new Player();
        hpText.supportRichText = true;
        atkText.supportRichText = true;
        defText.supportRichText = true;
        Displayed(false);
    }

    //! Keep the player's stats display updated
    private void Update()
    {
        if (!(p.ComparePlayer(PlayerSession.player_session.player)))
        {
            p = PlayerSession.player_session.player;
            Stats stats = new Stats(p);

            hpText.text = stats.StatsToStrings()[0];
            atkText.text = stats.StatsToStrings()[1];
            defText.text = stats.StatsToStrings()[2];
            moneyText.text = "" + p.money;

            currentSwordImage.GetComponent<RawImage>().texture = Item.NewItem("sword", p.sword).icon;
            currentShieldImage.GetComponent<RawImage>().texture = Item.NewItem("shield", p.shield).icon;
            currentArmourImage.GetComponent<RawImage>().texture = Item.NewItem("armour", p.armour).icon;
        }
    }
    
    //! Show/Hide the item upgrade panels
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

        switch(item_type)
        {
            case 0: // Sword
                currentItemLevel = PlayerSession.player_session.player.sword;
                displayItemType = "sword";
                break;

            case 1: // Shield
                currentItemLevel = PlayerSession.player_session.player.shield;
                displayItemType = "shield";
                break;

            case 2: // Armour
                currentItemLevel = PlayerSession.player_session.player.armour;
                displayItemType = "armour";
                break;
        }

        if (currentItemLevel >= 4)
        {
            NPBinding.UI.ShowToast("Item Fully Upgraded!", eToastMessageLength.SHORT);
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
        else
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
                stat = displayItem.attack;
                break;
            case 1:
                statIcon = def;
                stat = displayItem.defense;
                break;
            case 2:
                statIcon = hp;
                stat = displayItem.hp;
                break;
        }

        statIconImage.GetComponent<Image>().sprite = statIcon;
        itemNameText.text = displayItem.name;
        statText.text = "" + stat;
        balanceText.text = "" + PlayerSession.player_session.player.money;
        priceText.text = "" + displayItem.price;
    }

    //! Perform the server-side updating of the Player based on the purchase
    private IEnumerator Purchase(string poorerPlayerJSON)
    {
        StartCoroutine(Server.UpdatePlayer(poorerPlayerJSON));
        yield return new WaitUntil(() => Server.updatePlayer_done == true);
        soundsrc.PlayOneShot(purchase_sound, PlayerPrefs.GetFloat("fx"));
        gameObject.AddComponent<UpdateSessions>().U_Player();
        Displayed(false);
        yield break;
    }

    //! Check if player has enough funds, then make the purchase
    public void ConfirmPurchase()
    {
        if (PlayerSession.player_session.player.money >= displayItem.price)
        {
            Player poorerPlayer = Player.Clone(PlayerSession.player_session.player);
            poorerPlayer.money -= displayItem.price;
            switch(displayItemType)
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
            NPBinding.UI.ShowToast("Insufficient Funds.", eToastMessageLength.SHORT);
            gameObject.AddComponent<UpdateSessions>().U_Player();
            Displayed(false);
        }
    }
    
}
