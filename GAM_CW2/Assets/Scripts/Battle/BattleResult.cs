using UnityEngine;
using System;

public class BattleResult {

    private Player player, enemy;
    private bool win;

    public BattleResult(Player _player, Player _enemy, bool _win)
    {
        player = Player.Clone(_player);
        enemy = Player.Clone(_enemy);
        win = _win;
    }

    public Player CalculateGains()
    {
        int missingxptolevel = player.levelupxp - player.xp;



        int xp_gains = 110;
        xp_gains += CalculateBonusXP();
        if (!win) xp_gains = Mathf.RoundToInt(xp_gains/2);
        // scaling and stuff
        // according to enemy type



        int coin_gains = 100; // scale
        coin_gains += CalculateBonusCoins();
        if (!win) coin_gains = -Mathf.RoundToInt(coin_gains / 2);








        player.xp += xp_gains;
        if(player.xp > player.levelupxp)
        {
            player.level += 1;
            player.levelupxp += 100; // SCALE






            player.xp = xp_gains - missingxptolevel;
        }

        player.coins += coin_gains;
        if (player.coins < 0) player.coins = 0;





        return Player.Clone(player);
    }

    //! Calculates bonus/less XP based on level comparison
    private int CalculateBonusXP()
    {
        int level_diff = player.level - enemy.level;
        if (level_diff >= 0) return 0;
        else return 4 * level_diff;
    }

    //! Calculate bonus/less coins based on level comparison
    private int CalculateBonusCoins()
    {
        int level_diff = player.level - enemy.level;
        if (level_diff>=0) return 0;
        else return 5*level_diff;
    }

    private void UpdatePlayerBeginLevel()
    {
        PlayerObjects.singleton.player_beginning_of_level.xp = player.xp;
        PlayerObjects.singleton.player_beginning_of_level.level = player.level;
        PlayerObjects.singleton.player_beginning_of_level.level = player.coins;

    }

}
