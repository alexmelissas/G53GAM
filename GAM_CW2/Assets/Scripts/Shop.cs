using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    public GameObject shopPopup;
    public Text stat1Text, stat2Text, priceText, balanceText, coinsText;
    public GameObject shieldIconImage, swordIconImage, bootsIconImage, stat1IconImage, stat2IconImage;
    public Sprite atkIcon, defIcon, hpIcon, spdIcon, agilityIcon, critIcon;
    public AudioSource soundsrc;
    public AudioClip purchase_sound;

    private RPGCharacter p;
    private RPGItems selectedItem;
    private string displayItemType;
    private int itemLevel;

    private void Start()
    {
        p = PersistentObjects.singleton.player;
        coinsText.text = "" + p.coins;
        ShowShopPopup(false);
    }

    private void ShowShopPopup(bool shown) { shopPopup.SetActive(shown); }

    // SETUP THE PURCHASE POPUP WITH THE NEXT UPGRADE OF SELECTED ITEM TYPE
    public void BuildShopPopup(int itemType)
    {
        switch (itemType)
        {
            case 0: itemLevel = p.sword; displayItemType = "sword"; break;
            case 1: itemLevel = p.shield; displayItemType = "shield"; break;
            case 2: itemLevel = p.boots; displayItemType = "boots"; break;
            default: return;
        }

        if (itemLevel >= 3) { Debug.Log("You can't upgrade this further, Karen."); return; }
        else selectedItem = RPGItems.CreateItem(displayItemType, itemLevel + 1);

        GameObject iconImage;
        switch (displayItemType)
        {
            case "sword": iconImage = swordIconImage; break;
            case "shield": iconImage = shieldIconImage; break;
            case "boots": iconImage = bootsIconImage; break;
            default: iconImage = swordIconImage; break;
        }
        shieldIconImage.SetActive(false);
        swordIconImage.SetActive(false);
        bootsIconImage.SetActive(false);
        iconImage.SetActive(true);

        iconImage.GetComponent<RawImage>().texture = selectedItem.icon;

        SetupSelectedItemLabels(itemType);
        ShowShopPopup(true);
    }

    // HELPER FOR ABOVE
    private void SetupSelectedItemLabels(int itemType)
    {
        int stat1 = 0;
        int stat2 = 0;
        Sprite stat1Icon = null;
        Sprite stat2Icon = null;

        switch (itemType)
        {
            case 0:
                stat1Icon = atkIcon;
                stat2Icon = critIcon;
                stat1 = selectedItem.atk;
                stat2 = selectedItem.crit;
                break;
            case 1:
                stat1Icon = defIcon;
                stat2Icon = hpIcon;
                stat1 = selectedItem.def;
                stat2 = selectedItem.hp;
                break;
            case 2:
                stat1Icon = spdIcon;
                stat2Icon = agilityIcon;
                stat1 = selectedItem.spd;
                stat2 = selectedItem.agility;
                break;
            default: return;
        }

        stat1IconImage.GetComponent<Image>().sprite = stat1Icon;
        stat1Text.text = "" + stat1;
        stat2IconImage.GetComponent<Image>().sprite = stat2Icon;
        stat2Text.text = "" + stat2;
        priceText.text = "" + selectedItem.cost;

        balanceText.text = "" + p.coins;
    }

    // PERFORM ITEM PURCHASE IF HAVE ENOUGH COINS
    public void Execute_Purchase()
    {
        if (p.coins >= selectedItem.cost)
        {
            p.coins -= selectedItem.cost;
            switch (displayItemType)
            {
                case "sword": p.sword++; break;
                case "shield":
                    p.shield++;
                    int addedHP = RPGItems.CreateItem("shield", p.shield).hp
                        - RPGItems.CreateItem("shield", p.shield - 1).hp;
                    PersistentObjects.singleton.currentHP += addedHP;
                    break;
                case "boots": p.boots++; break;
                default: break;
            }
            soundsrc.PlayOneShot(purchase_sound, PlayerPrefs.GetFloat("fx"));
        }
        else Debug.Log("YOU'RE TOO POOR FOR THAT, KAREN.");
        SceneManager.LoadScene("Shop");
    }

    // PERFORM CARROT PURCHASE IF HAVE ENOUGH COINS
    public void ClickCarrot()
    {
        if (p.coins >= 200)
        {
            if (RPGCharacter.Heal(20)) p.coins -= 200;
            else Debug.Log("FULL HP");
        } else Debug.Log("YOU'RE TOO POOR FOR THAT, KAREN.");
        SceneManager.LoadScene("Shop");
    }

    public void Cancel_Purchase() { ShowShopPopup(false); }

    public void Back() { SceneManager.LoadScene("Main"); }

}