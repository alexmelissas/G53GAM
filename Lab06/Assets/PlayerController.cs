using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        IInventoryItem item = hit.gameObject.GetComponent<IInventoryItem>();
        if (item != null)
        {
            inventory.addItem(item); // where inventory is the inventory object ref
        }
    }

}
