using UnityEngine;
using System;

[Serializable]
//! JSON-able object to pass result of battle to server
public class BattleResult {

    //! Player ID
    public string id1;
    //! Enemy ID
    public string id2;
    //! True = Player win | False = Enemy win
    public bool result;
    //! Extra EXP gained/lost by defeating higher/lower level enemy
    public int additionalExp;
    //! Extra money gained/lost by defeating higher/lower level enemy
    public int additionalMoney;

    //! Constructor for the BattleResult to pass to server
    private BattleResult(Player _player1, Player _player2, bool _win)
    {
        id1 = _player1.id; id2 = _player2.id; result = _win;
        additionalExp = CalculateBonusExp(_player1,_player2);
        additionalMoney = CalculateBonusMoney(_player1,_player2);
    }

    //! Calculates bonus EXP based on level comparison
    private int CalculateBonusExp(Player p1, Player p2)
    {
        if (p1.level > p2.level) return -10;
        else return 10;
    }

    //! Calculate bonus money based on level comparison
    private int CalculateBonusMoney(Player p1, Player p2)
    {
        if (p1.level > p2.level) return -10;
        else return 10;
    }

    //! Get a JSON equivalent of this object
    public static string GetJSON(Player p1, Player p2, bool win)
    {
       return JsonUtility.ToJson(new BattleResult(p1, p2, win));
    }


}
