//! Container class for splitting the Player's stats to their counterparts
public class Stats {

    public int hpTotal, hp_base, hp_item;
    public int atkTotal, atk_base, atk_item;
    public int defTotal, def_base, def_item;
    public string rank;

    //! Calculate the stats based on Player given (including base/item/factor splits)
    public Stats(Player p)
    {
        // Calculate the base stats
        hp_base = p.hp;
        atk_base = p.atk;
        def_base = p.def;

        // Calculate the item bonuses
        hp_item = (new Armour(p.armour)).hp;
        atk_item = (new Sword(p.sword)).atk;
        def_item = (new Shield(p.shield)).def;

        // Calculate the totals
        hpTotal = p.hp + hp_item;
        atkTotal = p.atk + atk_item;
        defTotal = p.def + def_item;
    }

    //! Format the stats (base-bonus-totals) to array of colour-coded strings
    public string[] StatsToStrings()
    {
        string[] output = {"","",""};
        
        output[0] = "<b>" + hpTotal + "</b>" // HP display
            + "<color=black> (</color>"
            + "<color=yellow>" + hp_base + "</color>"
            + "<color=black>+</color>"
            + "<color=red>" + hp_item + "</color>"
            + "<color=black>)</color>";
        
        output[1] = "<b>" + atkTotal + "</b>" // atk display
            + "<color=black> (</color>"
            + "<color=yellow>" + atk_base + "</color>"
            + "<color=black>+</color>"
            + "<color=red>" + atk_item + "</color>"
            + "<color=black>)</color>";

        output[2] = "<b>" + defTotal + "</b>" // def display
            + "<color=black> (</color>"
            + "<color=yellow>" + def_base + "</color>"
            + "<color=black>+</color>"
            + "<color=red>" + def_item + "</color>"
            + "<color=black>)</color>";

        return output;
    }
}
