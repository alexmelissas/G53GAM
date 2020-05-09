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

    public RPGCharacter CalculateGains()
    {
        int missingxptolevel = player.levelupxp - player.xp;

        int baseXP = 1;
        switch (enemy.username)
        {
            case "squirrel": break;
            case "fox": baseXP += 1; break;
            case "snowman": baseXP += 2; break;
        }
        baseXP += CalculateBonusXP();
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
        baseCoins += CalculateBonusCoins();
        if (!win) baseCoins = -Mathf.RoundToInt(baseCoins / 3);
        
        player.coins += baseCoins;
        if (player.coins < 0) player.coins = 0;


        return RPGCharacter.HardCopy(player);
    }

    //! Calculates bonus/less XP based on level comparison
    private int CalculateBonusXP()
    {
        int level_diff = player.level - enemy.level;
        if (level_diff >= 0) return 0;
        else return level_diff;
    }

    //! Calculate bonus/less coins based on level comparison
    private int CalculateBonusCoins()
    {
        int level_diff = player.level - enemy.level;
        if (level_diff>=0) return 0;
        else return 5*level_diff;
    }
}
