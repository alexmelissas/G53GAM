using NUnit.Framework;
using UnityEngine;

//! Unit tests for the User and Player objects
public class User_Player_Test {

	[Test]
    public void CreateUserFromJSON_Test()
    {
        string json = "{\"id\": \"test1234id\", \"username\": \"alex\", \"password\": \"password1234\", \"accessToken\": \"1334535325356\", \"accessTokenSecret\": \"2497849187412\"}";

        User user = User.CreateUserFromJSON(json);

        Assert.That(user.id, Is.EqualTo("test1234id"));
        Assert.That(user.username, Is.EqualTo("alex"));
        Assert.That(user.password, Is.EqualTo("password1234"));
        Assert.That(user.accessToken, Is.EqualTo("1334535325356"));
        Assert.That(user.accessTokenSecret, Is.EqualTo("2497849187412"));
    }

    [Test]
    public void CreatePlayer_Test()
    {
        Player player;

        player = new Player("pid", "alex", 1, 10, 5, 2, 3, 4, 15, 1, 300, 0.4, 2, 2, 2, 0, 1);

        Assert.That(player.id, Is.EqualTo("pid"));
        Assert.That(player.characterName, Is.EqualTo("alex"));
        Assert.That(player.level, Is.EqualTo(1));
        Assert.That(player.hp, Is.EqualTo(10));
        Assert.That(player.attack, Is.EqualTo(5));
        Assert.That(player.defense, Is.EqualTo(2));
        Assert.That(player.agility, Is.EqualTo(3));
        Assert.That(player.critical_strike, Is.EqualTo(4));
        Assert.That(player.money, Is.EqualTo(15));
        Assert.That(player.experience, Is.EqualTo(1));
        Assert.That(player.exptolevel, Is.EqualTo(300));
        Assert.That(player.factor, Is.EqualTo(0.4));
        Assert.That(player.sword, Is.EqualTo(2));
        Assert.That(player.shield, Is.EqualTo(2));
        Assert.That(player.armour, Is.EqualTo(2));
        Assert.That(player.win, Is.EqualTo(0));
        Assert.That(player.lose, Is.EqualTo(1));
    }


    [Test]
    public void CreatePlayerFromJSON_Test()
    {
        Player original = new Player("pid", "alex", 1, 10, 5, 2, 3, 4, 15, 1, 300, 0.4, 2, 2, 2, 0, 1);
        string json = JsonUtility.ToJson(original);

        Player player = Player.CreatePlayerFromJSON(json);

        Assert.That(player.id, Is.EqualTo("pid"));
        Assert.That(player.characterName, Is.EqualTo("alex"));
        Assert.That(player.level, Is.EqualTo(1));
        Assert.That(player.hp, Is.EqualTo(10));
        Assert.That(player.attack, Is.EqualTo(5));
        Assert.That(player.defense, Is.EqualTo(2));
        Assert.That(player.agility, Is.EqualTo(3));
        Assert.That(player.critical_strike, Is.EqualTo(4));
        Assert.That(player.money, Is.EqualTo(15));
        Assert.That(player.experience, Is.EqualTo(1));
        Assert.That(player.exptolevel, Is.EqualTo(300));
        Assert.That(player.factor, Is.EqualTo(0.4));
        Assert.That(player.sword, Is.EqualTo(2));
        Assert.That(player.shield, Is.EqualTo(2));
        Assert.That(player.armour, Is.EqualTo(2));
        Assert.That(player.win, Is.EqualTo(0));
        Assert.That(player.lose, Is.EqualTo(1));
    }


}
