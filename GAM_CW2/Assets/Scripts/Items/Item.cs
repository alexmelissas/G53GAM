using UnityEngine;
using UnityEngine.UI;

//! Abstract Factory - For Items.
public abstract class Item {
    
    public Texture2D icon;
    public string name;
    public int hp;
    public int attack;
    public int defense;
    public int agility;
    public int critical_strike;
    public int price;

    //! Create a generic item with icon and attributes
    protected Item(Texture2D _icon, string _name, int _hp, int _attack, int _defense, int _agility, int _critical_strike, int _price)
    {
        icon = _icon; name = _name;  hp = _hp; attack = _attack; defense = _defense;
        agility = _agility; critical_strike = _critical_strike; price = _price;
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
            default:
                item = null;
                break;
        }
        return item;
    }
}
