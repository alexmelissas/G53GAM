using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEventArgs :EventArgs
{
    public IInventoryItem item;

    public InventoryEventArgs(IInventoryItem item)
    {
        this.item = item;
    }
}
