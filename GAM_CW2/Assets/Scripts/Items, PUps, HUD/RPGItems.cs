using UnityEngine;

public class RPGItems {

    public Texture2D icon;
    public int hp, atk, def, spd, agility, crit;
    public int cost;

    protected RPGItems() {}

    // CREATE ITEMCREATOR (SUBCLASS OF THIS)
    public static ItemCreator CreateItem(string type, int level) { return new ItemCreator(type, level); }

    // ADD ALL ADDED ITEM STATS TO RPGCHARACTER BASE STATS
    public static void AttachItemToCharacter(RPGCharacter p)
    {
        RPGItems[] itemList = { null, null, null };
        itemList[0] = CreateItem("sword", p.sword);
        itemList[1] = CreateItem("shield", p.shield);
        itemList[2] = CreateItem("boots", p.boots);

        // ADD ALL BONUS STATS OF ITEM TO THE PLAYER
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