using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    public GameObject upgradePanel;
    public Text itemNameText, statText, priceText, balanceText;
    public Text coinsText;
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
        ShowShopPopup(false);
    }

    private void Update()
    {
        if (!(p.ComparePlayer(PlayerObjects.singleton.player)))
        {
            p = PlayerObjects.singleton.player;
            coinsText.text = "" + p.coins;

            currentSwordImage.GetComponent<RawImage>().texture = Item.NewItem("sword", p.sword).icon;
            currentShieldImage.GetComponent<RawImage>().texture = Item.NewItem("shield", p.shield).icon;
            currentArmourImage.GetComponent<RawImage>().texture = Item.NewItem("armour", p.armour).icon;
            currentBootsImage.GetComponent<RawImage>().texture = Item.NewItem("boots", p.boots).icon;
        }
    }

    private void ShowShopPopup(bool shown)
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
        ShowShopPopup(true);
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
            ShowShopPopup(false);
        }
    }

    public void Back()
    {
        SceneManager.LoadScene("Main");
    }
}

