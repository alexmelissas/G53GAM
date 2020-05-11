using UnityEngine;

// SUBCLASS OF RPGITEMS
public class ItemCreator : RPGItems {

    // CREATE ITEM BASED ON TYPE AND LEVEL
	public ItemCreator(string type, int level) : base()
    {
        string fileName = "";

        // 1. INITIALISE ITEM WITH 0 STAT GAINS

        hp = 0;
        atk = 0;
        def = 0;
        spd = 0;
        agility = 0;
        crit = 0;

        // 2. DETERMINE THE ICON AND 2 STAT GAINS OF THE ITEM BASED ON TYPE & LEVEL

        switch (type)
        {
            case "sword":
                switch (level)
                {
                    case 1: fileName = "swordTier1"; atk = 1;  crit = 1;  cost = 99; break;
                    case 2: fileName = "swordTier2"; atk = 5; crit = 5; cost = 599; break;
                    case 3: fileName = "swordTier3"; atk = 15; crit = 9; cost = 999; break;
                    default: break;
                }break;

            case "shield":
                switch (level)
                {
                    case 1: fileName = "shieldTier1"; def = 1;  hp = 5; cost = 99; break;
                    case 2: fileName = "shieldTier2"; def = 4; hp = 15; cost = 599; break;
                    case 3: fileName = "shieldTier3"; def = 8; hp = 30; cost = 999; break;
                    default: break;
                }break;

            case "boots":
                switch (level)
                {
                    case 1: fileName = "bootsTier1"; spd = 1;  agility = 1;  cost = 99; break;
                    case 2: fileName = "bootsTier2"; spd = 15; agility = 4;  cost = 599; break;
                    case 3: fileName = "bootsTier3"; spd = 30; agility = 10; cost = 999; break;
                    default: break;
                }break;
        }

        // 3. LOAD THE ICON

        icon = Resources.Load(fileName) as Texture2D;
    }
}
