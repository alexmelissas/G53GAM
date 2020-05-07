using UnityEngine;
using UnityEngine.UI;

//! Abstract Factory - For Items.
public abstract class Item {
    
    public Texture2D icon;
    public string name;
    public int hp;
    public int atk;
    public int def;
    public int spd;
    public int agility;
    public int crit;
    public int price;

    //! Create a generic item with icon and attributes
    protected Item(Texture2D _icon, string _name, int _hp, int _atk, int _def, int _spd, int _agility, int _crit, int _price)
    {
        icon = _icon; name = _name;  hp = _hp; atk = _atk; def = _def;
        spd = _spd; agility = _agility; crit = _crit; price = _price;
    }

    //! Factory - to create different kinds of items
    public static Item NewItem(string type, int level)
    {
        Item item;
        switch (type)
        {
            case "sword":
                item = new Sword(level);
                break;
            case "shield":
                item = new Shield(level);
                break;
            case "armour":
                item = new Armour(level);
                break;
            case "boots":
                item = new Boots(level);
                break;
            default:
                item = null;
                break;
        }
        return item;
    }
}
