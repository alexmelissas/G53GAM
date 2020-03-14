using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForKey : MonoBehaviour
{
    public bool inRange = false;
    public GameObject key;
    public Inventory inventory;

    void Start()
    {
        inventory.ItemUsed += Inventory_ItemUsed;
    }

    void Inventory_ItemUsed(object sender, InventoryEventArgs e)
    {
        if((e.item as MonoBehaviour).gameObject == key)
        {
            if (inRange) gameObject.GetComponent<Door>().Open();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        inRange = false;
    }

}
