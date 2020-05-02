using UnityEngine;

//! Shield item class
public class Shield : Item {

    public static Texture2D base_icon; //set path to base icon here

    public Shield(int level) : base(base_icon, "", 0, 0, 0, 0, 0, 0, 0)
    {
        string item_icon_location = "";
        int attribute = 0;
        switch (level)
        {
            case 1:
                name = "Basic Shield";
                attribute = 2;
                price = 89;
                item_icon_location = "sh1";
                break;
            case 2:
                name = "Poseidon's Wavebreaker";
                attribute = 10;
                price = 219;
                item_icon_location = "sh2";
                break;
            case 3:
                name = "Persephone's Defender";
                attribute = 19;
                price = 549;
                item_icon_location = "sh3";
                break;
            case 4:
                name = "Athena's Star Guard";
                attribute = 37;
                price = 999;
                item_icon_location = "sh4";
                break;
            default: break;
        }
        icon = Resources.Load(item_icon_location) as Texture2D;
        def = attribute;
    }


    
}