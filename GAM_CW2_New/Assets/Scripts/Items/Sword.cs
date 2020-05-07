using UnityEngine;

//! Sword item class
public class Sword : Item {

    public static Texture2D base_icon; // set path to base icon here

	public Sword(int level) : base(base_icon, "", 0, 0, 0, 0, 0, 0, 0)
    {
        string item_icon_location = "";
        int attribute = 0;
        switch (level)
        {
            case 1:
                name = "Basic Blade";
                attribute = 5;
                price = 109;
                item_icon_location = "s1";
                break;
            case 2:
                name = "Aries' Dawnbringer";
                attribute = 15;
                price = 299;
                item_icon_location = "s2";
                break;
            case 3:
                name = "Zeus' Diamondcarver";
                attribute = 30;
                price = 519;
                item_icon_location = "s3";
                break;
            case 4:
                name = "Athena's AngelBlade";
                attribute = 50;
                price = 999;
                item_icon_location = "s4";
                break;
            default: break;
        }
        icon = Resources.Load(item_icon_location) as Texture2D;
        atk = attribute;
    }
}
