using UnityEngine;

//! Calculate damage, who plays and outcome of one Turn
public class Turn {

    public Player player;
    public Player enemy;
    public bool player_turn;
    public float damage;
    public int crit_landed;

    //! Basic Constructor
    public Turn(bool _turn, Player _player, Player _enemy)
    {
        player_turn = _turn;
        player = _player;
        enemy = _enemy;
    }

    //! Choose whose Turn this is, alternating every Turn
    private Player PlaysNow(bool playerPlays) { return playerPlays ? player : enemy; }

    //! Calculate the result of this Turn, including damage dealth, each Player's remaining HP etc
    public int PlayTurn()
    {
        Player attacker = PlaysNow(player_turn);
        Player victim = PlaysNow(!player_turn);

        int real_attack = attacker.atk;
        int applied_attack = Random.Range((int)(real_attack - (real_attack*0.05)),(int)(real_attack + (real_attack*0.05)));
        damage = (applied_attack - victim.def) * (victim.level > attacker.level ? victim.level - attacker.level : 1);
        if (damage < 1) damage = 1;
        int low_crit = 20;//attacker.critical_strike; // not sure about multiplying it? //FIX THIS TO 10
        int med_crit = low_crit/2;
        int high_crit = low_crit/10;

        int crit = Random.Range(0, 100);
        float crit_dmg = damage;
        if (crit <= low_crit) { crit_dmg = damage *3f; crit_landed = 3; }
        else if (crit < med_crit) { crit_dmg = damage * 2f; ; crit_landed = 2; }
        else if (crit < high_crit) { crit_dmg = damage * 1.5f; crit_landed = 1; }
        else crit_landed = 0;
        if (crit_landed > 0) damage = crit_dmg;

        int miss;
        if (victim.agility < 10) miss = 10;
        else miss = victim.agility; //need to be careful with agility scaling

        miss = 30; // FIX THIS

        int misschance = Random.Range(0, 100);
        if (misschance < miss) damage = 0;
        
        victim.hp -= Mathf.RoundToInt(damage);
        
        if (player.hp <= 0) return 1; //player lost
        else if (enemy.hp <= 0) return 2; //player won
        else return 0; //no death yet
    }
}
