using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public HUDManager inventory;
    // Start is called before the first frame update
    void Start()
    {
        inventory.ItemAdded += InventoryItemAdded;
        inventory.ItemUsed += InventoryItemUsed;
    }

    private void InventoryItemAdded(object sender, InventoryEventArgs e)
    {
        Transform panel = transform.Find("InventoryHUD");
        foreach(Transform slot in panel)
        {
            Image image = slot.GetComponent<Image>();
            InventoryItemClickable button = slot.GetComponent<InventoryItemClickable>();
            if (!image.enabled)
            {
                image.enabled = true;
                image.sprite = e.item.itemImage;
                button.enabled = true;
                button.item = e.item;
                break;
            }
        }
    }

    private void InventoryItemUsed(object sender, InventoryEventArgs e)
    {
        Transform panel = transform.Find("InventoryHUD");
        foreach (Transform slot in panel)
        {
            Image image = slot.GetComponent<Image>();
            InventoryItemClickable button = slot.GetComponent<InventoryItemClickable>();
            if (image.enabled)
            {
                image.enabled = false;
                image.sprite = null;
                button.enabled = false;
                break;
            }
        }
    }

}
