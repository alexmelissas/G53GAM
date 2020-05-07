using UnityEngine;

//! Calculate damage, who plays and outcome of one Turn
public class Turn {

    public Player player;
    public Player enemy;
    public bool playersTurn;
    public float damage;
    public bool critLanded;

    //! Basic Constructor
    public Turn(bool _turn, Player _player, Player _enemy)
    {
        playersTurn = _turn;
        player = _player;
        enemy = _enemy;
    }

    private Player PlayingNow(bool playerPlays) { return playerPlays ? player : enemy; }

    public int PlayTurn()
    {
        Player attacker = PlayingNow(playersTurn);
        Player victim = PlayingNow(!playersTurn);

        // REAL DMG CALCULATION
        int atk = attacker.atk;
        int minFluctuate = (int)(atk - (atk * 0.05));
        int maxFluctuate = (int)(atk + (atk * 0.05));
        int rawDamage = Random.Range(minFluctuate,maxFluctuate);

        bool giveBoost = victim.level > attacker.level;
        int levelDiff = victim.level - attacker.level;
        float boostAmount = giveBoost ? levelDiff * 3 : 0;
        damage = (rawDamage - victim.def) + boostAmount;
        if (damage < 1) damage = 1;

        // CRIT
        int critChance = attacker.crit;
        int crit = Random.Range(0, 100);
        if (crit <= critChance) { damage *= 2; critLanded = true; }
        else critLanded = false;

        // MISS
        int miss;
        if (victim.agility < 5) miss = 5;
        else miss = victim.agility;
        int misschance = Random.Range(0, 100);
        if (misschance < miss) damage = 0;

        // DEAL DMG
        victim.hp -= Mathf.RoundToInt(damage);

        if (playersTurn == false)
        {
            if (PlayerObjects.singleton.currentHP - Mathf.RoundToInt(damage) < 0)
                PlayerObjects.singleton.currentHP = 0;
            else PlayerObjects.singleton.currentHP -= Mathf.RoundToInt(damage);
        }

        if (PlayerObjects.singleton.currentHP <= 0) return 1; //player died
        else if (enemy.hp <= 0) return 2; //player won
        else return 0; //no death yet
    }
}
