using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public HUDManager inventory;

    void Start()
    {
        inventory.ItemAdded += InventoryItemAdded;
        inventory.ItemUsed += InventoryItemUsed;
    }

    private void InventoryItemAdded(object sender, InventoryEventArgs e)
    {
        Transform panel = transform.Find("slots");
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
        Transform panel = transform.Find("slots");
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
