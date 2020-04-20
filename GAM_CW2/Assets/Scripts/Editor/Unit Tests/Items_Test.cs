using NUnit.Framework;

//! Unit Tests for Items
public class Items_Test {

    [Test]
    public void CreateItems_Test()
    {
        int sword_level = 2; // attack given is 15
        int shield_level = 1; // defense given is 2
        int armour_level = 4; // hp given is 149

        Sword test_sword = (Sword)Item.NewItem("sword", sword_level);
        Shield test_shield = (Shield)Item.NewItem("shield", shield_level);
        Armour test_armour = (Armour)Item.NewItem("armour", armour_level);

        Assert.That(test_sword.attack, Is.EqualTo(15));
        Assert.That(test_shield.defense, Is.EqualTo(2));
        Assert.That(test_armour.hp, Is.EqualTo(149));
    }

    [Test]
    public void CreateItemSet_Test()
    {
        int sword_level = 2; // attack given is 15
        int shield_level = 1; // defense given is 2
        int armour_level = 4; // hp given is 149
        Sword test_sword = (Sword)Item.NewItem("sword", sword_level);
        Shield test_shield = (Shield)Item.NewItem("shield", shield_level);
        Armour test_armour = (Armour)Item.NewItem("armour", armour_level);
        Player player = new Player("", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // Create a player with 0 base stats and just the added item stats
            sword_level, shield_level, armour_level, 0, 0);

        Items items = new Items(player);

        Assert.That(player.sword, Is.EqualTo(sword_level));
        Assert.That(player.shield, Is.EqualTo(shield_level));
        Assert.That(player.armour, Is.EqualTo(armour_level));
    }

    [Test]
    public void AttachItemsToPlayer_Test()
    {
        int sword_level = 2; // attack given is 15
        int shield_level = 1; // defense given is 2
        int armour_level = 4; // hp given is 149
        Sword test_sword = (Sword)Item.NewItem("sword", sword_level);
        Shield test_shield = (Shield)Item.NewItem("shield", shield_level);
        Armour test_armour = (Armour)Item.NewItem("armour", armour_level);
        Player player = new Player("", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // Create a player with 0 base stats and just the added item stats
            sword_level, shield_level, armour_level, 0, 0);
        Items items = new Items(player);

        Items.AttachItemsToPlayer(items, player);

        Assert.That(player.attack, Is.EqualTo(15));
        Assert.That(player.defense, Is.EqualTo(2));
        Assert.That(player.hp, Is.EqualTo(149));

    }
}
