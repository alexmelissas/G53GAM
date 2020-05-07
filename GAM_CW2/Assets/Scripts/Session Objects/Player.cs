using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class Player
{
    public string username;
    public int level, xp, levelupxp, coins;
    public int hp, atk, def, spd, crit, agility; // RPG stats
    public int sword, shield, armour, boots; // RPG items
    public int movespeed, jumpheight, jumps; // Platformer stats
    public int powerup; // Platformer powerup

    //! Default constructor
    public Player()
    {
        username = "-";
        level = 0; xp = 0; levelupxp = 0; coins = 0;
        hp = 0; atk = 0; def = 0; spd = 0; crit = 0; agility = 0;
        movespeed = 0; jumpheight = 0; jumps = 0;
        sword = 0; shield = 0; armour = 0; boots = 0;
        
        powerup = 0;
    }

    //! Full constructor
    public Player(string _username, int _level, int _xp, int _levelupxp, int _coins, int _hp, int _atk, int _def,
        int _spd, int _crit, int _agility, int _sword, int _shield, int _armour, int _boots, int _movespeed, int _jumpheight, int _jumps, int _powerup)
    {
        username = _username;
        level = _level; xp = _xp; levelupxp = _levelupxp; coins = _coins;
        hp = _hp; atk = _atk; def = _def; spd = _spd; crit = _crit; agility = _agility;
        movespeed = _movespeed; jumpheight = _jumpheight; jumps = _jumps;
        sword = _sword; shield = _shield; armour = _armour; boots = _boots;
        powerup = _powerup;
    }

    //! Create a Player object from JSON
    public static Player CreatePlayerFromJSON(string json)
    {
        Player temp = new Player();
        JsonUtility.FromJsonOverwrite(json, temp);
        return temp;
    }

    //! Check if two Player objects have identical attributes
    public bool ComparePlayer(Player other)
    {
        if (username != other.username
            || level != other.level || xp != other.xp || levelupxp != other.levelupxp || coins != other.coins
            || hp != other.hp || atk != other.atk || def != other.def || spd != other.spd || crit != other.crit || agility != other.agility
            || sword != other.sword || shield != other.shield || armour != other.armour || boots != other.boots
            || movespeed != other.movespeed || jumpheight != other.jumpheight || jumps != other.jumps
            || powerup!=other.powerup) return false;
        return true;
    }

    //! Fully clone the Player object to a new one
        // Code  inspiration from https://stackoverflow.com/questions/129389/how-do-you-do-a-deep-copy-of-an-object-in-net-c-specifically
    public static Player Clone(Player original)
    {
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, original);
            ms.Position = 0;
            return (Player)formatter.Deserialize(ms);
        }
    }

    public void AttachItems() { Items.AttachItemsToPlayer(this); }
}

