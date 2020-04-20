using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! Entire Gameplay and Battle Screen management
public class Gameplay : MonoBehaviour {

     /* ==== Class Responsibilities =====
     *  1. Calculate and store Turns until one dies
     *  2. Calculate result
     *  3. Pass result to server, calculate bonuses
     *  4. Display full battle animations until end of match/until skipped
     * =================================*/
    
    public Slider playerHPSlider, enemyHPSlider;
    public Image playerHPColourImage, enemyHPColourImage;
    public Text playerNameText, enemyNameText, playerLevelText, enemyLevelText;
    public Text actualPlayerHPText, maxPlayerHPText, actualEnemyHPText, maxEnemyHPText, playerDmgLabelText, enemyDmgLabelText; 
    public AudioSource soundsrc, musicsrc;
    public AudioClip player_hit_Sound, enemy_hit_Sound, crit_Sound, miss_Sound, drop_sword_Sound;
    public GameObject winPopup, losePopup;

    //! For the popups at the end of the battle to know when to animate EXP gain.
    public static int updatePlayer;
    public static string currentAnimation_player = "player_idle_Animation";
    public static string currentAnimation_enemy = "enemy_idle_Animation";
    public static bool ended = false;

    //! Store all the Turns of the battle until a player lost.
    private List<Turn> turns;
    private Player player, enemy;
    private int result;
    private int player_maxHP, enemy_maxHP;
    float player_newHP = -1;
    float enemy_newHP = -1;
    private bool skip = false;
    private bool death = false;
    private bool ranked;

    //! Skip button handler
    public void Press_Skip() { skip = true; }

    //! Hide/Show the end game popups
    private void ShowPopup(bool shown, bool win)
    {
        Vector3 hide = new Vector3(7630, 2430, 0);
        Vector3 show = new Vector3(0,0,0);
        GameObject panel = win ? winPopup : losePopup;
        panel.transform.position = shown ? show : hide;
    }

    //! Setup the battle screen, including HP bars, Models, Damage Labels etc.
    private void Start()
    {
        ended = false;
        updatePlayer = -1;
        player = PlayerSession.player_session.player;
        PlayerSession.player_session.player_before_battle = Player.Clone(PlayerSession.player_session.player); // keep the player before gains
        Items.AttachItemsToPlayer(new Items(player), player);

        if (!PlayerPrefs.HasKey("battle_type"))
            gameObject.AddComponent<ChangeScene>().Forward("Overworld");
        int battle_type = PlayerPrefs.GetInt("battle_type");
        ranked = (battle_type == 1);

        if(battle_type!=0 && battle_type!=1 && battle_type!=2)
            gameObject.AddComponent<ChangeScene>().Forward("Overworld"); 
        enemy = PlayerSession.player_session.enemy;
        if (battle_type == 0)
        {
            enemy.id = "bot";
            enemy.characterName = BotScreen.difficulty + "Bot";
        }
        Items.AttachItemsToPlayer(new Items(enemy), enemy);

        turns = new List<Turn>();
        player_maxHP = player.hp;
        enemy_maxHP = enemy.hp;

        playerNameText.text = "" + player.characterName;
        playerLevelText.text = "" + player.level;
        enemyNameText.text = "" + enemy.characterName;
        enemyLevelText.text = "" + enemy.level;

        playerDmgLabelText.enabled = enemyDmgLabelText.enabled = false;
        playerDmgLabelText.GetComponentInParent<Image>().enabled = enemyDmgLabelText.GetComponentInParent<Image>().enabled = false;

        playerHPSlider.normalizedValue = enemyHPSlider.normalizedValue = 1f;

        musicsrc.volume = PlayerPrefs.GetFloat("music")/6;
        musicsrc.loop = true;
        musicsrc.playOnAwake = true;

        ShowPopup(false, true);
        ShowPopup(false, false);

        currentAnimation_player = "player_idle_Animation";
        currentAnimation_enemy = "enemy_idle_Animation";

        RunBattle();
    }

    //! Check if animation should be displayed: If not skipped, and if nobody won yet
    private void Update()
    {
        if (PlayerPrefs.HasKey("skip") && PlayerPrefs.GetInt("skip") == 1) skip = true;
        if (skip) Invoke("EndMatch", 0.5f);

        //Update HP bars
        float nhpp = player_newHP * player_maxHP;
        float nhpe = enemy_newHP * enemy_maxHP;
        Mathf.RoundToInt(nhpp);
        Mathf.RoundToInt(nhpe);
        actualPlayerHPText.text = "" + ((nhpp >= 0) ? nhpp : player_maxHP); //bug when dying
        actualEnemyHPText.text = "" + ((nhpe >= 0) ? nhpe : enemy_maxHP);
        maxPlayerHPText.text = "/" + player_maxHP;
        maxEnemyHPText.text = "/" + enemy_maxHP;

        if (player_newHP > -1) if (playerHPSlider.value > player_newHP) playerHPSlider.normalizedValue -= 0.005f;
        if (playerHPSlider.value == 0 && !death) { soundsrc.PlayOneShot(drop_sword_Sound, PlayerPrefs.GetFloat("fx")); playerHPColourImage.enabled = false; death = true; }
        else if (playerHPSlider.value < 0.25) playerHPColourImage.color = Color.red; else if (playerHPSlider.value < 0.5) playerHPColourImage.color = Color.yellow;

        if (enemy_newHP > -1) if (enemyHPSlider.value > enemy_newHP) enemyHPSlider.normalizedValue -= 0.005f;
        if (enemyHPSlider.value == 0 && !death) { soundsrc.PlayOneShot(drop_sword_Sound, PlayerPrefs.GetFloat("fx")); enemyHPColourImage.enabled = false; death = true; }
        else if (enemyHPSlider.value < 0.25) enemyHPColourImage.color = Color.red; else if (enemyHPSlider.value < 0.5) enemyHPColourImage.color = Color.yellow;
    }

    //! Run the battle, calculate all Turns and result
    public void RunBattle()
    {
        result = 0;
        bool player_turn = true; // think about who goes first
        while (result == 0)
        {
            Turn turn = new Turn(player_turn, player, enemy);
            turns.Add(turn);
            result = turn.PlayTurn();
            player = Player.Clone(turn.player); // get a new Player object with updated HP to pass to the next turn
            enemy = Player.Clone(turn.enemy);
            if (result==0) player_turn = player_turn ? false : true;
        }
        
        StartCoroutine(Server.PassResult(BattleResult.GetJSON(player, enemy, (result == 1) ? false : true),ranked));
        StartCoroutine(AnimateTurns(turns));
    }

    //! End the battle animations, show the Win/Lose popups
    private void EndMatch()
    {
        if (ended) return;

        ended = true;
        StopAllCoroutines();
        gameObject.AddComponent<UpdateSessions>().U_Player(); //now the Playersession player is updated
        
        if (result == 1) ShowPopup(true, false);
        else ShowPopup(true, true);
        updatePlayer = result; //enables the popups for the exp and money gain to animate

        result = 0;
    }

    //! Animate the battle, including Player animations, HP bars, Damage Labels etc.
    IEnumerator AnimateTurns(List<Turn> turns)
    {
        yield return new WaitForSeconds(1f);
        foreach (Turn turn in turns)
        {
            string attacker = turn.player_turn ? "player" : "enemy";
            
            // (1) Play animations
            if (attacker == "player")
            {
                if (turn.damage != 0)
                {
                    currentAnimation_player = "player_attack_Animation";
                    yield return new WaitForSeconds(0.2f);
                    currentAnimation_enemy = "enemy_hurt_Animation";
                }
                else
                {
                    currentAnimation_player = "player_idle_Animation";
                    yield return new WaitForSeconds(0.2f);
                    currentAnimation_enemy = "enemy_idle_Animation";
                }

                if (turns.Count == 1) // if last turn, then enemy died
                {
                    currentAnimation_enemy = "enemy_die_Animation";
                }
            }

            else if(attacker == "enemy")
            {
                if (turn.damage != 0)
                {
                    currentAnimation_enemy = "enemy_attack_Animation";
                    yield return new WaitForSeconds(0.2f);
                    currentAnimation_player = "player_hurt_Animation";
                }
                else
                {
                    currentAnimation_enemy = "enemy_idle_Animation";
                    yield return new WaitForSeconds(0.2f);
                    currentAnimation_player = "player_idle_Animation";
                }

                if (turns.Count == 1) // if last turn, then player died
                {
                    currentAnimation_player = "player_die_Animation";
                }
            }

            // (2) Display damage
            Text dmgLabel = attacker == "player" ? enemyDmgLabelText : playerDmgLabelText;
            Image bam = attacker == "player" ? enemyDmgLabelText.GetComponentInParent<Image>() : playerDmgLabelText.GetComponentInParent<Image>();
            AudioClip sound;
            if (turn.damage != 0)
            {
                dmgLabel.text = "" + turn.damage;
                sound = crit_Sound;
                switch (turn.crit_landed)
                {
                    case 1: dmgLabel.color = Color.green; break;
                    case 2: dmgLabel.color = Color.yellow; break;
                    case 3: dmgLabel.color = Color.red; break;
                    default: dmgLabel.color = Color.black;
                        sound = (attacker=="player") ? player_hit_Sound : enemy_hit_Sound;
                        break;
                }                
            }
            else
            {
                dmgLabel.text = "MISS";
                dmgLabel.color = Color.grey;
                sound = miss_Sound;
            }
            soundsrc.PlayOneShot(sound, PlayerPrefs.GetFloat("fx"));
            dmgLabel.enabled = true;
            bam.enabled = true;
            yield return new WaitForSeconds(0.3f);

            // (3) Update HP of victim
            if (turn.damage!=0)
            {
                float currenthp = (attacker == "player") ? turn.enemy.hp : turn.player.hp;
                float maxhp = (attacker == "player") ? enemy_maxHP : player_maxHP;
                float hp_bar_value = currenthp / maxhp;
                if (attacker == "player") enemy_newHP = hp_bar_value;
                else player_newHP = hp_bar_value;
            }
            yield return new WaitForSeconds(0.75f);
            dmgLabel.enabled = false;
            bam.enabled = false;
            
            // (4) Wait a bit and go to next turn
            yield return new WaitForSeconds(0.5f);
        }
        skip = true;
    }
}
