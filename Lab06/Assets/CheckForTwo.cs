using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckForTwo : MonoBehaviour
{
    public bool inRange = false;
    public GameObject silverKey;
    public GameObject potion;
    public GameObject goldBar;
    public Inventory inventory;

    public GameObject barrier;
    public GameObject silverKeyImage;
    public GameObject potionImage;
    public Text wintext;

    private bool usedKey = false;
    private bool usedPotion = false;

    void Start()
    {
        inventory.ItemUsed += Inventory_ItemUsed;
    }

    void Inventory_ItemUsed(object sender, InventoryEventArgs e)
    {
        Debug.Log("============HELLO");

        if ((e.item as MonoBehaviour).gameObject == silverKey)
        {
            if (inRange)
            {
                usedKey = true;
                silverKeyImage.SetActive(false);
            }
        }
        else if ((e.item as MonoBehaviour).gameObject == potion)
        {
            if (inRange)
            {
                usedPotion = true;
                potionImage.SetActive(false);
            }
        }

        if(usedKey && usedPotion)
        {
            barrier.SetActive(false);
        }

        if ((e.item as MonoBehaviour).gameObject == goldBar)
        {
            wintext.text = "I'M RICH !!!1!1!!!!1!!!!1";
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