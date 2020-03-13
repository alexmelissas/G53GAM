using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<IInventoryItem> items = new List<IInventoryItem>();

    // Start is called before the first frame update
    void Start()
    {         

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addItem(IInventoryItem item)
    {
        items.Add(item); item.onPickup();
        Debug.Log("Pickup item");
        // broadcast event to the hud
    }

}
