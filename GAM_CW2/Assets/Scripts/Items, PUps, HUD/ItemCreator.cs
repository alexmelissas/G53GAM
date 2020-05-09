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
                    case 1: atk = 5;  crit = 1;  price = 109; fileName = "swordTier1"; break;
                    case 2: atk = 15; crit = 5;  price = 299; fileName = "swordTier2"; break;
                    case 3: atk = 30; crit = 10; price = 519; fileName = "swordTier3"; break;
                    default: break;
                }break;

            case "shield":
                switch (level)
                {
                    case 1: def = 2;  hp = 20; price = 109; fileName = "shieldTier1"; break;
                    case 2: def = 11; hp = 60; price = 219; fileName = "shieldTier2"; break;
                    case 3: def = 22; hp = 95; price = 549; fileName = "shieldTier3"; break;
                    default: break;
                }break;

            case "boots":
                switch (level)
                {
                    case 1: spd = 5;  agility = 1;  price = 109; fileName = "bootsTier1"; break;
                    case 2: spd = 15; agility = 4;  price = 299; fileName = "bootsTier2"; break;
                    case 3: spd = 30; agility = 10; price = 519; fileName = "bootsTier3"; break;
                    default: break;
                }break;
        }

        icon = Resources.Load(fileName) as Texture2D;
    }
}
