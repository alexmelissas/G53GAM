﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
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
        Debug.Log("Used item: "+item.itemName);
        items.Remove(item);

        if (ItemUsed != null)
        {
            ItemUsed.Invoke(this, new InventoryEventArgs(item));
        }
    }

}
