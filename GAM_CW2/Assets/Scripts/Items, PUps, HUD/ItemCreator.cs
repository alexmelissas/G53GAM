using UnityEngine;

public class ItemCreator : RPGItems {

	public ItemCreator(string type, int level) : base()
    {
        string iconPath = "";
        hp = 0;
        atk = 0;
        def = 0;
        spd = 0;
        agility = 0;
        crit = 0;

        switch (type)
        {
            case "sword":
                switch (level)
                {
                    case 1: name = "Basic Blade"; atk = 5; crit = 1; price = 109; iconPath = "s1"; break;
                    case 2: name = "Aries' Dawnbringer"; atk = 15; crit = 5; price = 299; iconPath = "s2"; break;
                    case 3: name = "Zeus' Diamondcarver"; atk = 30; crit = 10; price = 519; iconPath = "s3"; break;
                    default: break;
                }break;

            case "shield":
                switch (level)
                {
                    case 1: name = "Basic Shield"; def = 2;  hp = 20; price = 109; iconPath = "sh1"; break;
                    case 2: name = "Poseidon's Wavebreaker"; def = 11; hp = 60; price = 219; iconPath = "sh2"; break;
                    case 3: name = "Persephone's Defender"; def = 22; hp = 95; price = 549; iconPath = "sh3"; break;
                    default: break;
                }break;

            case "boots":
                switch (level)
                {
                    case 1: name = "Basic Blade"; spd = 5; agility = 1; price = 109; iconPath = "b1"; break;
                    case 2: name = "Aries' Dawnbringer"; spd = 15; agility = 4; price = 299; iconPath = "b2"; break;
                    case 3: name = "Zeus' Diamondcarver"; spd = 30; agility = 10; price = 519; iconPath = "b3"; break;
                    default: break;
                }break;
        }

        icon = Resources.Load(iconPath) as Texture2D;
    }
}
