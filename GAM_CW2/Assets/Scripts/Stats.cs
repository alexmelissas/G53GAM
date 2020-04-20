//! Container class for splitting the Player's stats to their counterparts
public class Stats {

    public int hpTotal, hp_base, hp_factor, hp_item;
    public int attackTotal, attack_base, attack_factor, attack_item;
    public int defenseTotal, defense_base, defense_factor, defense_item;
    public string rank;

    //! Calculate the stats based on Player given (including base/item/factor splits)
    public Stats(Player p)
    {
        // Calculate the base stats
        hp_base = (int)(p.hp / p.factor);
        attack_base = (int)(p.attack / p.factor);
        defense_base = (int)(p.defense / p.factor);

        // Calculate the factor bonuses
        hp_factor = p.hp - hp_base;
        attack_factor = p.attack - attack_base;
        defense_factor = p.defense - defense_base;

        // Calculate the item bonuses
        hp_item = (new Armour(p.armour)).hp;
        attack_item = (new Sword(p.sword)).attack;
        defense_item = (new Shield(p.shield)).defense;

        // Calculate the totals
        hpTotal = p.hp + hp_item;
        attackTotal = p.attack + attack_item;
        defenseTotal = p.defense + defense_item;
    }

    //! Format the stats (base-bonus-totals) to array of colour-coded strings
    public string[] StatsToStrings()
    {
        string[] output = {"","",""};
        
        output[0] = "<b>" + hpTotal + "</b>" // HP display
            + "<color=black> (</color>"
            + "<color=yellow>" + hp_base + "</color>"
            + "<color=black>+</color>"
            + "<color=green>" + hp_factor + "</color>"
            + "<color=black>+</color>"
            + "<color=red>" + hp_item + "</color>"
            + "<color=black>)</color>";
        
        output[1] = "<b>" + attackTotal + "</b>" // Attack display
            + "<color=black> (</color>"
            + "<color=yellow>" + attack_base + "</color>"
            + "<color=black>+</color>"
            + "<color=green>" + attack_factor + "</color>"
            + "<color=black>+</color>"
            + "<color=red>" + attack_item + "</color>"
            + "<color=black>)</color>";

        output[2] = "<b>" + defenseTotal + "</b>" // Defense display
            + "<color=black> (</color>"
            + "<color=yellow>" + defense_base + "</color>"
            + "<color=black>+</color>"
            + "<color=green>" + defense_factor + "</color>"
            + "<color=black>+</color>"
            + "<color=red>" + defense_item + "</color>"
            + "<color=black>)</color>";

        return output;
    }
}
