using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class Player
{
    public string username;
    public int level, xp, levelupxp, coins;
    public int hp, atk, def, spd, crit, agility;
    public int sword, shield, boots;

    public Player(string _username, int _level)
    {
        username = _username;
        level = _level;
        if (xp <= 0) xp = 0;
        SetLevelUpXP(this);
        if (coins<=0) coins = 0;
        CalculateBaseStats(this);
        if (sword <= 1) sword = 1;
        if (shield <= 1) shield = 1;
        if (boots <= 1) boots = 1;
    }

    //Based on: https://stackoverflow.com/questions/129389/how-do-you-do-a-deep-copy-of-an-object-in-net-c-specifically
    public static Player HardCopy(Player original)
    {
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, original);
            ms.Position = 0;
            return (Player)formatter.Deserialize(ms);
        }
    }

    public void AttachItems() { RPGItems.AttachItemsToPlayer(this); }

    public static void SetLevelUpXP(Player p)
    {
        int nextLevel = p.level + 1;
        int pow = Mathf.RoundToInt(Mathf.Pow(nextLevel, 2));
        int calc = Mathf.RoundToInt((3 * pow) / 5);
        if (calc < 1) calc = 1;
        p.levelupxp = calc; 
    }

    public static void CalculateBaseStats(Player p)
    {
        int lvlFactor = p.level + 14;

        double m1 = 1.2;
        double m2 = 1;
        double bonus = 50;
        p.hp = (int)(m1 * (lvlFactor^2) + m2 * lvlFactor + bonus);

        m1 = 0.2;
        m2 = 0.2;
        bonus = 30;
        p.atk = (int)(m1 * (lvlFactor ^ 2) + m2 * lvlFactor + bonus);

        m1 = 0.15;
        m2 = 0.15;
        bonus = 15;
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
}

