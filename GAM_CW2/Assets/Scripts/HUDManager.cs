﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
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
        if (ItemAdded != null) { ItemAdded.Invoke(this, new InventoryEventArgs(item)); }
    }

    public void useItem(IInventoryItem item)
    {
        items.Remove(item);
        if (ItemUsed != null) { ItemUsed.Invoke(this, new InventoryEventArgs(item)); }
    }

    private void Start()
    {
        // Save initial player stats - to reset if die
        PersistentObjects.singleton.player_beginning_of_level = Player.Clone(PersistentObjects.singleton.player);
        battleScreen.SetActive(false);
        UpdateHPBar();
    }

    private void OnEnable() { Start(); }

    public void UpdateHPBar()
    {
        Player player = Player.Clone(PersistentObjects.singleton.player);
        player.AttachItems();

        playerNameText.text = "" + player.username;
        playerLevelText.text = "" + player.level;
        maxPlayerHPText.text = "/" + player.hp;
        actualPlayerHPText.text = "" + PersistentObjects.singleton.currentHP;


        playerHPColourImage.enabled = true;
        float hpBarValue = (float)PersistentObjects.singleton.currentHP / (float)player.hp;
        playerHPSlider.value = hpBarValue;
        if (playerHPSlider.value == 0) playerHPColourImage.enabled = false;
        else if (hpBarValue < 0.25) playerHPColourImage.color = Color.red;
        else if (hpBarValue < 0.5) playerHPColourImage.color = Color.yellow;
        else playerHPColourImage.color = Color.green;

        coinsText.text = "" + player.coins;
    }
}
