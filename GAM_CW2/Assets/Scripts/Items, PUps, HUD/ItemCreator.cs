using UnityEngine;

public class ItemCreator : RPGItems {

	public ItemCreator(string type, int level) : base()
    {
        string fileName = "";

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
                    case 1: fileName = "swordTier1"; atk = 5;  crit = 5;  price = 99; break;
                    case 2: fileName = "swordTier2"; atk = 15; crit = 10;  price = 299; break;
                    case 3: fileName = "swordTier3"; atk = 30; crit = 17; price = 499; break;
                    default: break;
                }break;

            case "shield":
                switch (level)
                {
                    case 1: fileName = "shieldTier1"; def = 4;  hp = 20; price = 99; break;
                    case 2: fileName = "shieldTier2"; def = 10; hp = 60; price = 279; break;
                    case 3: fileName = "shieldTier3"; def = 20; hp = 95; price = 449; break;
                    default: break;
                }break;

            case "boots":
                switch (level)
                {
                    case 1: fileName = "bootsTier1"; spd = 5;  agility = 1;  price = 99; break;
                    case 2: fileName = "bootsTier2"; spd = 15; agility = 4;  price = 249; break;
                    case 3: fileName = "bootsTier3"; spd = 30; agility = 10; price = 399; break;
                    default: break;
                }break;
        }

        icon = Resources.Load(fileName) as Texture2D;
    }
}
