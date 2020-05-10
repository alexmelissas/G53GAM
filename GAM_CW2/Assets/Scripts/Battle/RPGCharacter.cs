using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class RPGCharacter : IEquatable<RPGCharacter>
{
    public string username;
    public int level, xp, levelupxp, coins;
    public int hp, atk, def, spd, crit, agility;
    public int sword, shield, boots;

    public RPGCharacter(string _username, int _level)
    {
        username = _username;
        level = _level;
        if (xp <= 0) xp = 0;
        SetLevelUpXP(this);
        if (coins<=0) coins = 3000;
        CalculateBaseStats(this);
        if (sword <= 1) sword = 1;
        if (shield <= 1) shield = 1;
        if (boots <= 1) boots = 1;
    }

    //Code based on: <https://stackoverflow.com/questions/129389/how-do-you-do-a-deep-copy-of-an-object-in-net-c-specifically>
    public static RPGCharacter HardCopy(RPGCharacter original)
    {
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, original);
            ms.Position = 0;
            return (RPGCharacter)formatter.Deserialize(ms);
        }
    }

    // Algorithm based on <https://bulbapedia.bulbagarden.net/wiki/Experience>
    public static void SetLevelUpXP(RPGCharacter p)
    {
        int nextLevel = p.level + 1;
        int pow = Mathf.RoundToInt(Mathf.Pow(nextLevel, 2));
        int calc = Mathf.RoundToInt((3 * pow) / 5);
        if (calc < 1) calc = 1;
        p.levelupxp = calc; 
    }

    // Algorithm helped by <http://howtomakeanrpg.com/a/how-to-make-an-rpg-levels.html>
    public static void CalculateBaseStats(RPGCharacter p)
    {
        int lvlFactor = p.level + 16;

        double m1 = 1.2;
        double m2 = 1;
        double bonus = 50;
        p.hp = (int)(m1 * (lvlFactor^2) + m2 * lvlFactor + bonus);

        m1 = m2 = 0.2;
        bonus = 30;
        p.atk = (int)(m1 * (lvlFactor ^ 2) + m2 * lvlFactor + bonus);

        m1 = m2 = 0.15;
        bonus = 20;
        p.def = (int)(m1 * (lvlFactor ^ 2) + m2 * lvlFactor + bonus);

        m1 = 0.1;
        m2 = 0.15;
        bonus = 5;
        p.spd = (int)(m1 * (lvlFactor ^ 2) + m2 * lvlFactor + bonus);

        m1 = 0.05;
        m2 = 0.5;
        bonus = 5;
        p.agility = (int)((m1 * (lvlFactor ^ 2) + m2 * lvlFactor + bonus) * 0.2);
        p.crit = (int)(m1 * (lvlFactor ^ 2) + m2 * lvlFactor + bonus * 0.2);
    }

    public void AttachItems() { RPGItems.AttachItemToCharacter(this); }

    // [BROKEN] OVERRIDE EQUAL TO CHECK FOR EQUALITY OF RPGCHARACTER OBJECTS
    // WOULD BE USED TO CHECK FOR DIFFERENCES IN STAT SCREENS
    public bool Equals(RPGCharacter other)
    {
        if (username == other.username && level == other.level && xp == other.xp
            && levelupxp == other.levelupxp && coins == other.coins && hp == other.hp
            && atk == other.atk && def == other.def && spd == other.spd && crit == other.crit
            && agility == other.agility && sword == other.sword && shield == other.shield
            && boots == other.boots) return true;
        else return false;
    }

    // HEAL THE PLAYER - ADD TO CURRENTHP
    public static bool Heal(int heal)
    {
        RPGCharacter player = RPGCharacter.HardCopy(PersistentObjects.singleton.player);
        player.AttachItems();
        int currentHP = PersistentObjects.singleton.currentHP;
        if (currentHP < player.hp)
        {
            if (currentHP + heal <= player.hp) PersistentObjects.singleton.currentHP += heal;
            else PersistentObjects.singleton.currentHP = player.hp;
            return true;
        }
        else return false;
    }
}

