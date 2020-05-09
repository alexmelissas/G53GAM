using UnityEngine;

public class ItemCreator : RPGItems {

	public ItemCreator(string type, int level) : base(null, "", 0, 0, 0, 0, 0, 0, 0)
    {
        string iconPath = "";
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
                    case 1: name = "Basic Shield"; def = 2;  spd -= 3; price = 109; iconPath = "sh1"; break;
                    case 2: name = "Poseidon's Wavebreaker"; def = 11; spd = -10; price = 219; iconPath = "sh2"; break;
                    case 3: name = "Persephone's Defender"; def = 22; spd = -19; price = 549; iconPath = "sh3"; break;
                    default: break;
                }break;

            case "armour":
                switch (level)
                {
                    // NOT BOOTS FFS
                    case 1: name = "Peasant Boots"; hp = 20; price = 99; iconPath = "a1"; break;
                    case 2: name = "Zeus' Thundercap"; hp = 48; price = 259; iconPath = "a2"; break;
                    case 3: name = "Hephaestus' Flame"; hp = 79; price = 489; iconPath = "a3"; break;
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
