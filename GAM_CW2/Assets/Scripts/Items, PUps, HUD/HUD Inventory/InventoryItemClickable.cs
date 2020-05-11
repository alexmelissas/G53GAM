using UnityEngine;

public class InventoryItemClickable : MonoBehaviour
{
    public IInventoryItem item;
    public HUDManager inventory;

    public void OnItemClicked()
    {
        if (item != null) inventory.useItem(item);
    }
}
