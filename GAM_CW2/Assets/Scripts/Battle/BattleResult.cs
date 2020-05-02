using UnityEngine;
using System;

[Serializable]
public class BattleResult {

    public string username;
    public bool result;
    //! Adjust XP earned based on level diff
    public int additionalXP;
    //! Adjust coins earned based on level diff
    public int additionalCoins;

    private BattleResult(Player player, Player enemy, bool _win)
    {
        username = player.username; result = _win;
        additionalXP = CalculateBonusXP(player,enemy);
        additionalCoins = CalculateBonusCoins(player,enemy);
    }

    //! Calculates bonus/less XP based on level comparison
    private int CalculateBonusXP(Player player, Player enemy)
    {
        if (player.level > enemy.level) return -10;
        else return 10;
    }

    //! Calculate bonus/less coins based on level comparison
    private int CalculateBonusCoins(Player player, Player enemy)
    {
        if (player.level > enemy.level) return -10;
        else return 10;
    }

    //! Get a JSON equivalent of this object
    public static string GetJSON(Player player, Player enemy, bool win)
    {
       return JsonUtility.ToJson(new BattleResult(player, enemy, win));
    }


}
