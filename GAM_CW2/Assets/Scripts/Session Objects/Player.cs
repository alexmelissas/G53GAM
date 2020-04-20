using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
//! JSON-able object with all Player-related attributes (eg. id, winrate, level, stats ...)
public class Player
{
    public string id, characterName;
    public int level, hp, attack, defense, agility, critical_strike ,money, experience, exptolevel;
    public int sword, shield, armour, win, lose;
    public double factor;

    //! Default constructor
    public Player()
    {
        id = ""; characterName = ""; level = 0; hp = 0; attack = 0; defense = 0; agility = 0; critical_strike = 0; money = 0;
        experience = 0; exptolevel = 0; factor = 0; sword = 0; shield = 0; armour = 0; win = 0; lose = 0;
    }

    //! Fully specific constructor
    public Player(string _id, string _characterName, int _level, int _hp, int _attack, int _defense, int _agility, int _critical_strike, 
        int _money, int _experience, int _exptolevel, double _factor, int _sword, int _shield, int _armour, int _win, int _lose)
    {
        id = _id; characterName = _characterName; level = _level; hp = _hp; attack = _attack; defense = _defense;
        agility = _agility; critical_strike = _critical_strike; money = _money; experience = _experience; exptolevel = _exptolevel;
        factor = _factor; sword = _sword; shield = _shield; armour = _armour; win = _win; lose = _lose;
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
        if (id != other.id || characterName != other.characterName || level != other.level || hp!=other.hp 
            || attack != other.attack || defense != other.defense || agility != other.agility 
            || critical_strike != other.critical_strike || money != other.money || experience != other.experience 
            || exptolevel != other.exptolevel || factor != other.factor || sword != other.sword || shield != other.shield 
            || armour != other.armour || win != other.win || lose != other.lose) return false;
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
}

