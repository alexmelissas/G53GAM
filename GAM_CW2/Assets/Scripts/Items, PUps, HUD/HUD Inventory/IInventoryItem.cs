using UnityEngine;

public interface IInventoryItem {
    string itemName { get; }
    Sprite itemImage { get; }
    void onPickup();
}
