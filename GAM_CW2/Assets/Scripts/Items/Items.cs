//! Handle item sets
public class Items {

    public Item sword;
    public Item shield;
    public Item armour;
    public Item boots;

    //! Create items based on the Player object's item attributes
    public Items(Player _player)
    {
        sword = Item.NewItem("sword", _player.sword);
        shield = Item.NewItem("shield", _player.shield);
        armour = Item.NewItem("armour", _player.armour);
        boots = Item.NewItem("boots", _player.boots); 
    }

    //! Add item stats to Player
    public static void AttachItemsToPlayer(Items i, Player p)
    {
        p.hp += i.armour.hp;
        p.atk += i.sword.atk;
        p.def += (i.shield.def + i.armour.def);
        //p.spd += i.boots.spd;
        //p.agility += i.boots.agility;
        p.crit += i.sword.crit;
        return;
    }
}
