using UnityEngine;

//! Armour item class
public class Armour : Item {

    public static Texture2D base_icon; // set path to base icon here

    public Armour(int level) : base(base_icon, "", 0, 0, 0, 0, 0, 0, 0)
    {
        string item_icon_location = "";
        int attribute = 0;
        switch (level)
        {
            case 1: // NOT BOOTS FFS
                name = "Peasant Boots";
                attribute = 20;
                price = 99;
                item_icon_location = "a1";
                break;
            case 2:
                name = "Zeus' Thundercap";
                attribute = 48;
                price = 259;
                item_icon_location = "a2";
                break;
            case 3:
                name = "Hephaestus' Flame";
                attribute = 79;
                price = 489;
                item_icon_location = "a3";
                break;
            case 4:
                name = "Athena's Headplate";
                attribute = 149;
                price = 999;
                item_icon_location = "a4";
                break;
            default:
                break;
        }
        icon = Resources.Load(item_icon_location) as Texture2D;
        hp = attribute;
    }
}
