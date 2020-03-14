﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemClickable : MonoBehaviour
{
    public IInventoryItem item;
    public Inventory inventory;

    public void OnItemClicked()
    {
        if (item != null)
        {
            inventory.useItem(item);
        }
    }
}
