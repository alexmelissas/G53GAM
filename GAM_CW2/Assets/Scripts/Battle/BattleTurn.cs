using UnityEngine;

public class BattleTurn {

    public RPGCharacter player;
    public RPGCharacter enemy;
    public bool playersTurn;
    public bool critLanded;
    public float damage;

    public BattleTurn(RPGCharacter _player, RPGCharacter _enemy, bool _playersTurn)
    {
        playersTurn = _playersTurn;
        player = _player;
        enemy = _enemy;
    }

    // RUN THIS BATTLETURN: CALCULATE DAMAGE AND DEATHS
    public int PlayTurn()
    {
        RPGCharacter damager = PlayingNow(playersTurn);
        RPGCharacter damagee = PlayingNow(!playersTurn);

        // RAW DMG CALCULATION (ATK-DEF AND RANDOM FLUCTUATION)
        int minFluctuate = (int)(damager.atk - (damager.atk * 0.07));
        int maxFluctuate = (int)(damager.atk + (damager.atk * 0.07));
        int rawDamage = Random.Range(minFluctuate, maxFluctuate);

        // BOOST BASED ON LEVEL DIFFERENCE
        int levelDiff = damagee.level - damager.level;
        float boostAmount = (levelDiff>0) ? levelDiff^4 : 0;
        damage = (rawDamage - damagee.def) + boostAmount;
        damage = (damage < 1) ? 1 : damage;

        //Debug.Log("RAW DMG - SHIELD:" + rawDamage + ", BOOSTED DMG:" + damage);

        // CALCULATE CRIT CHANCE AND DAMAGE
        int critChance = Random.Range(0, 100);
        if (critChance <= damager.crit) { damage *= 2; critLanded = true; }
        else critLanded = false;

        // CALCULATE MISS CHANCE
        int missChance = Random.Range(0, 100);
        if (missChance < damagee.agility) damage = 0;

        // DEAL THE DAMAGE
        damagee.hp -= Mathf.RoundToInt(damage);

        if (playersTurn == false) // IF PLAYER IS TAKING DMG NOW
        {
            if (PersistentObjects.singleton.currentHP - Mathf.RoundToInt(damage) < 0)
                PersistentObjects.singleton.currentHP = 0;
            else PersistentObjects.singleton.currentHP -= Mathf.RoundToInt(damage);
        }

        // PASS THE RESULTS : 0 = NO OUTCOME // 1 = PLAYER LOSE // 2 = PLAYER WIN
        if (PersistentObjects.singleton.currentHP <= 0) return 1;
        else if (enemy.hp <= 0) return 2;
        else return 0;
    }

    private RPGCharacter PlayingNow(bool playerPlays) { return playerPlays ? player : enemy; }

}
