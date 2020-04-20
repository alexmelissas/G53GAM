//! Handle item sets
public class Items {

    public Item sword;
    public Item shield;
    public Item armour;

    //! Create items based on the Player object's item attributes
    public Items(Player _player)
    {
        sword = Item.NewItem("sword", _player.sword);
        shield = Item.NewItem("shield", _player.shield);
        armour = Item.NewItem("armour", _player.armour);
    }

    //! Add item stats to Player
    public static void AttachItemsToPlayer(Items i, Player p) //FUTURE: also make this put the item icons onto player?
    {
        p.hp += i.armour.hp;
        p.attack += i.sword.attack;
        p.defense += (i.shield.defense + i.armour.defense);
        return;
    }
}
