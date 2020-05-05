using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDInventory : MonoBehaviour
{
    // RPG elements
    public GameObject battleScreen;
    public Slider playerHPSlider;
    public Image playerHPColourImage;
    public Text playerNameText, playerLevelText,actualPlayerHPText, maxPlayerHPText, coinsText;

    // Powerups
    public event EventHandler<InventoryEventArgs> ItemAdded;
    public event EventHandler<InventoryEventArgs> ItemUsed;

    List<IInventoryItem> items = new List<IInventoryItem>();

    public void addItem(IInventoryItem item)
    {
        items.Add(item);
        item.onPickup();

        // broadcast event to the hud
        if (ItemAdded != null)
        {
            ItemAdded.Invoke(this, new InventoryEventArgs(item));
        }
    }

    public void useItem(IInventoryItem item)
    {
        Debug.Log("Used item: " + item.itemName);
        items.Remove(item);

        if (ItemUsed != null)
        {
            ItemUsed.Invoke(this, new InventoryEventArgs(item));
        }
    }

    private void Start()
    {
        battleScreen.SetActive(false);

        //test
        PlayerObjects.playerObjects.player = new Player(
            "Alex", 12, 100, 2000, 455, 2000, 12, 35, 10, 10, 20, 1, 1, 1, 1, 10, 2, 3, 1
        );
        PlayerObjects.playerObjects.currentHP = 1300;

        int currentHP = PlayerObjects.playerObjects.currentHP;
        Player player = PlayerObjects.playerObjects.player;

        playerNameText.text = "" + player.username;
        playerLevelText.text = "" + player.level;
        maxPlayerHPText.text = "/" + player.hp;
        actualPlayerHPText.text = "" + PlayerObjects.playerObjects.currentHP;
        

        float hpBarValue = currentHP / player.hp;
        Debug.Log("CURRENT:" + currentHP +"/Max:" + player.hp + "HPBAR: "+ hpBarValue);
        playerHPSlider.value = hpBarValue;
        if (playerHPSlider.value == 0) { /*soundsrc.PlayOneShot(drop_sword_Sound, PlayerPrefs.GetFloat("fx"));*/ playerHPColourImage.enabled = false; } // death = true; }
        else if (hpBarValue < 0.25) playerHPColourImage.color = Color.red;
        else if (hpBarValue < 0.5) playerHPColourImage.color = Color.yellow;
        else playerHPColourImage.color = Color.green;

        coinsText.text = "" + player.coins;

    }
}
