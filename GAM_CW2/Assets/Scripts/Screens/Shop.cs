using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    public GameObject shopPopup;
    public Text itemNameText, statText, priceText, balanceText;
    public Text coinsText;
    public GameObject armourIconImage, shieldIconImage, swordIconImage, bootsIconImage, statIconImage;
    public Sprite atkIcon, defIcon, hpIcon, spdIcon;
    public AudioSource soundsrc;
    public AudioClip purchase_sound;

    private Player p;
    private RPGItems displayItem;
    private string displayItemType;
    private int itemLevel;

    private void Start()
    {
        p = PersistentObjects.singleton.player;
        coinsText.text = "" + p.coins;
        ShowShopPopup(false);
    }

    private void ShowShopPopup(bool shown) { shopPopup.SetActive(shown); }

    public void BuildShopPopup(int itemType)
    {
        switch (itemType)
        {
            case 0: itemLevel = p.sword; displayItemType = "sword"; break;
            case 1:  itemLevel = p.shield; displayItemType = "shield"; break;
            case 2: itemLevel = p.armour; displayItemType = "armour"; break;
            case 3: itemLevel = p.boots; displayItemType = "boots"; break;
            default: return;
        }

        if (itemLevel >= 3) { Debug.Log("You can't upgrade this further, Karen."); return; }
        else displayItem = RPGItems.CreateItem(displayItemType, itemLevel + 1);

        GameObject iconImage;
        switch (displayItemType)
        {
            case "sword": iconImage = swordIconImage; break;
            case "shield": iconImage = shieldIconImage; break;
            case "armour": iconImage = armourIconImage; break;
            case "boots": iconImage = bootsIconImage; break;
            default: iconImage = swordIconImage; break;
        }
        armourIconImage.SetActive(false);
        shieldIconImage.SetActive(false);
        swordIconImage.SetActive(false);
        bootsIconImage.SetActive(false);
        iconImage.SetActive(true);

        iconImage.GetComponent<RawImage>().texture = displayItem.icon;

        UpdateLabels(itemType);
        ShowShopPopup(true);
    }

    private void UpdateLabels(int itemType)
    {
        int stat = 0;
        Sprite statIcon = null;

        switch (itemType)
        {
            case 0:
                statIcon = atkIcon;
                stat = displayItem.atk;
                break;
            case 1:
                statIcon = defIcon;
                stat = displayItem.def;
                break;
            case 2:
                statIcon = hpIcon;
                stat = displayItem.hp;
                break;
            case 3:
                statIcon = spdIcon;
                stat = displayItem.spd;
                break;
            default: return;
        }

        statIconImage.GetComponent<Image>().sprite = statIcon;
        itemNameText.text = displayItem.name;
        priceText.text = "" + displayItem.price;
        statText.text = "" + stat;

        balanceText.text = "" + p.coins;
    }

    public void Execute_Purchase()
    {
        if (p.coins >= displayItem.price)
        {
            p.coins -= displayItem.price;
            switch (displayItemType)
            {
                case "sword": p.sword++; break;
                case "shield": p.shield++; break;
                case "armour": p.armour++; break;
                case "boots": p.boots++; break;
                default: break;
            }
            soundsrc.PlayOneShot(purchase_sound, PlayerPrefs.GetFloat("fx"));
        }
        else Debug.Log("YOU'RE TOO POOR FOR THAT, KAREN.");
        SceneManager.LoadScene("Shop");
    }

    public void Cancel_Purchase() { ShowShopPopup(false); }

    public void Back() { SceneManager.LoadScene("Main"); }

}