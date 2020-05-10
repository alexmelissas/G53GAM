using UnityEngine;
using System;

public class BattleResult {

    private RPGCharacter player, enemy;
    private bool win;

    public BattleResult(RPGCharacter _player, RPGCharacter _enemy, bool _win)
    {
        player = RPGCharacter.HardCopy(_player);
        enemy = RPGCharacter.HardCopy(_enemy);
        win = _win;
    }

    // CALCULATE XP/COINS GAINED FROM BATTLE, HANDLE LEVELUP
    public RPGCharacter CalculateGains()
    {
        int missingxptolevel = player.levelupxp - player.xp;
        int levelDiff = enemy.level - player.level;

        int baseXP = 3;
        switch (enemy.username)
        {
            case "squirrel": break;
            case "fox": baseXP += 1; break;
            case "snowman": baseXP += 2; break;
        }

        // BONUS XP IF BEAT HIGHER LEVEL ENEMY
        baseXP += (levelDiff >= 0) ? levelDiff^3 : 0;
        // LOSE 1/3 OF XP YOU'D GAIN IF LOSE
        if (!win) baseXP = Mathf.RoundToInt(baseXP/3);

        player.xp += baseXP;
        if (player.xp >= player.levelupxp)
        {
            player.level += 1;
            RPGCharacter.SetLevelUpXP(player);
            RPGCharacter.CalculateBaseStats(player);
            player.xp = baseXP - missingxptolevel;
        }


        int baseCoins = 100;
        switch (enemy.username)
        {
            case "squirrel": break;
            case "fox": baseCoins += 10; break;
            case "snowman": baseCoins += 20; break;
        }
        // BONUS COINS IF BEAT HIGHER LEVEL ENEMY
        baseCoins += (levelDiff >= 0) ? 5 * levelDiff : 0;
        // LOSE 1/3 OF COINS YOU'D GAIN IF LOSE
        if (!win) baseCoins = -Mathf.RoundToInt(baseCoins / 3);
        
        player.coins += baseCoins;
        // NO DEBT HERE KIDS
        if (player.coins < 0) player.coins = 0;


        return RPGCharacter.HardCopy(player);
    }
}
