using UnityEngine;
using UnityEngine.UI;

public class RPGItems {

    public Texture2D icon;
    public int hp, atk, def, spd, agility, crit;
    public string name;
    public int price;

    protected RPGItems() {}

    public static ItemCreator CreateItem(string type, int level) { return new ItemCreator(type, level); }

    public static void AttachItemsToPlayer(Player p)
    {
        RPGItems[] itemList = { null, null, null };
        itemList[0] = CreateItem("sword", p.sword);
        itemList[1] = CreateItem("shield", p.shield);
        itemList[2] = CreateItem("boots", p.boots);

        foreach (ItemCreator item in itemList)
        {
            p.hp += item.hp;
            p.atk += item.atk;
            p.def += item.def;
            p.spd += item.spd;
            p.agility += item.agility;
            p.crit += item.crit;
        }
       
        return;
    }

}