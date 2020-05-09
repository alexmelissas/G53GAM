using UnityEngine;
using UnityEngine.UI;

public class RPGItems {

    public RPGItems sword, shield, armour, boots;
    public Texture2D icon;
    public int hp, atk, def, spd, agility, crit;
    public string name;
    public int price;

    protected RPGItems(Texture2D _icon, string _name, int _hp, int _atk, int _def, int _spd, int _agility, int _crit, int _price)
    {
        icon = _icon;
        name = _name;
        hp = _hp;
        atk = _atk;
        def = _def;
        spd = _spd;
        agility = _agility;
        crit = _crit;
        price = _price;
    }

    public static ItemCreator CreateItem(string type, int level) { return new ItemCreator(type, level); }

    public static void AttachItemsToPlayer(Player p)
    {
        RPGItems[] itemList = { null, null, null, null };
        itemList[0] = new ItemCreator("sword", p.sword);
        itemList[1] = new ItemCreator("shield", p.shield);
        itemList[2] = new ItemCreator("armour", p.armour);
        itemList[3] = new ItemCreator("boots", p.boots);

        foreach(ItemCreator item in itemList)
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