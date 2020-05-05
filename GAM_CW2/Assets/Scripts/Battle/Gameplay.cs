using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! Entire Gameplay and Battle Screen management
public class Gameplay : MonoBehaviour {

    /* ==== Class Responsibilities =====
    *  1. Calculate and store Turns until one dies
    *  2. Calculate result // ??????
    *  3. Display full battle animations until end of match/until skipped
    * =================================*/

    public GameObject battleScreen, gameController;
    public GameObject attackButton, blockButton, runButton;

    public Slider playerHPSlider, enemyHPSlider;
    public Image playerHPColourImage, enemyHPColourImage;
    public Text playerNameText, enemyNameText, playerLevelText, enemyLevelText;
    public Text actualPlayerHPText, maxPlayerHPText, actualEnemyHPText, maxEnemyHPText, playerDmgLabelText, enemyDmgLabelText; 
    public AudioSource soundsrc, musicsrc;
    public AudioClip player_hit_Sound, enemy_hit_Sound, crit_Sound, miss_Sound, drop_sword_Sound;
    public GameObject winPopup, losePopup;

    //! For the popups at the end of the battle to know when to animate XP gain.
    public static int updatePlayer;
    public static string currentAnimation_player = "player_idle_Animation";
    public static string currentAnimation_enemy = "enemy_idle_Animation";
    public static bool ended = false;

    //! Store all the Turns of the battle until a player lost.
    private List<Turn> turns;
    private Player player, enemy;
    private int result;
    private int player_maxHP, enemy_maxHP;
    private int player_currentHP;
    float player_newHP = -1;
    float enemy_newHP = -1;
    private bool death = false;
    private int turn_counter, last_block_turn;


    private void Start() { PlayerObjects.playerObjects.inBattle = false; }

    private void Update() { UpdateHP(); }

    private void OnEnable() { InitiateBattle(); }

    //! Setup the battle screen, including HP bars, Models, Damage Labels etc.
    private void InitiateBattle()
    {
        ended = false;
        PlayerObjects.playerObjects.inBattle = true;
        updatePlayer = -1;

        player = Player.Clone(PlayerObjects.playerObjects.player);
        Items.AttachItemsToPlayer(new Items(player), player);
        PlayerObjects.playerObjects.player_before_battle = Player.Clone(PlayerObjects.playerObjects.player); // keep the player before gains

        enemy = PlayerObjects.playerObjects.enemy;
        Items.AttachItemsToPlayer(new Items(enemy), enemy);

        player_maxHP = player.hp;
        enemy_maxHP = enemy.hp;

        playerNameText.text = "" + player.username;
        playerLevelText.text = "" + player.level;
        enemyNameText.text = "" + enemy.username;
        enemyLevelText.text = "" + enemy.level;

        playerDmgLabelText.enabled = enemyDmgLabelText.enabled = false;
        playerDmgLabelText.GetComponentInParent<Image>().enabled = enemyDmgLabelText.GetComponentInParent<Image>().enabled = false;

        player_currentHP = PlayerObjects.playerObjects.currentHP;

        actualPlayerHPText.text = "" + player_currentHP;
        playerHPSlider.normalizedValue = (float)player_currentHP  / (float)player_maxHP;

        enemyHPSlider.normalizedValue = 1f;
        actualEnemyHPText.text = "" + enemy_maxHP;

        musicsrc.volume = PlayerPrefs.GetFloat("music") / 6;
        musicsrc.loop = true;
        musicsrc.playOnAwake = true;

        ShowPopup(false, true);
        ShowPopup(false, false);

        currentAnimation_player = "player_idle_Animation";
        currentAnimation_enemy = "enemy_idle_Animation";

        turn_counter = 0;
        last_block_turn = -4;
        EnableActions();
    }

    //! Check if animation should be displayed: If not skipped, and if nobody won yet
    private void UpdateHP()
    {
        //Update HP bars
        float nhpp = player_newHP * player_maxHP;
        float nhpe = enemy_newHP * enemy_maxHP;
        Mathf.RoundToInt(nhpp);
        Mathf.RoundToInt(nhpe);
        actualPlayerHPText.text = "" + ((nhpp >= 0) ? nhpp : PlayerObjects.playerObjects.currentHP);
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

    public void Execute_Action(string action)
    {
        switch (action)
        {
            case "attack":
                DisableActions();
                Execute_Attack();
                break;
            case "block":
                if (turn_counter - last_block_turn >= 2)
                {
                    DisableActions();
                    Execute_Block();
                }
                break;
            case "run":
                DisableActions();
                Execute_Run();
                break;
        }
    }

    private void Execute_Attack()
    {
        result = 0;
        turn_counter++;
        bool player_turn;
        if (player.spd >= enemy.spd) player_turn = true;
        else player_turn = false;
        turns = new List<Turn>();

        bool battleOver = false;

        for(int i = 0; i<=1; i++)
        {
            Turn turn = new Turn(player_turn, player, enemy);
            turns.Add(turn);
            result = turn.PlayTurn(); // when result=1 => the battle has an outcome, so is over
            player = Player.Clone(turn.player); // get a new Player object with updated HP to pass to the next turn
            enemy = Player.Clone(turn.enemy); // same for enemy
            if (result == 0) player_turn = player_turn ? false : true; // swap whose turn it is
            else
            {
                battleOver = true;
                break;
            }
           
        }
        StartCoroutine(AnimateTurns(turns));
        if(battleOver) Invoke("EndBattle", 0.5f);

    }

    private void Execute_Block()
    {
        result = 0;
        turn_counter++;
        last_block_turn = turn_counter;
        StartCoroutine(AnimateBlock());
        
    }

    private void Execute_Run()
    {
        Invoke("BattleOver", 0.5f);
    }


    private void EndBattle()
    {
        if (ended) return;
        ended = true;
        StopAllCoroutines();
        if (result == 1) ShowPopup(true, false);
        else ShowPopup(true, true);
        updatePlayer = result; //enables the popups for the exp and money gain to animate
        result = 0;
    }

    public void BattleOver()
    {
        PlayerObjects.playerObjects.inBattle = false;
        ShowPopup(false, true);
        ShowPopup(false, false);
        gameController.GetComponent<HUDInventory>().UpdateHPBar();
        battleScreen.SetActive(false);

    }

    private void DisableActions()
    {
        attackButton.SetActive(false);
        blockButton.SetActive(false);
        runButton.SetActive(false);
    }

    private void EnableActions()
    {
        attackButton.SetActive(true);
        blockButton.SetActive(true);
        runButton.SetActive(true);
    }

    //! Hide/Show the end game popups
    private void ShowPopup(bool shown, bool win)
    {
        Vector3 hide = new Vector3(10000, 10000, 0);
        Vector3 show = new Vector3(950, 500, 0);
        GameObject panel = win ? winPopup : losePopup;
        panel.transform.position = shown ? show : hide;
    }

    //! Animate the battle, including Player animations, HP bars, Damage Labels etc.
    IEnumerator AnimateTurns(List<Turn> turns)
    {
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
                float currenthp = (attacker == "player") ? turn.enemy.hp : PlayerObjects.playerObjects.currentHP;
                float maxhp = (attacker == "player") ? enemy_maxHP : player_maxHP;
                float hp_bar_value = currenthp / maxhp;
                if (attacker == "enemy") player_newHP = hp_bar_value;
                else enemy_newHP = hp_bar_value;
            }
            yield return new WaitForSeconds(0.75f);
            dmgLabel.enabled = false;
            bam.enabled = false;
            
            // (4) Wait a bit and go to next turn
            yield return new WaitForSeconds(0.5f);
        }
        EnableActions();
    }

    //! Animate the block
    IEnumerator AnimateBlock()
    {
        string attacker = "enemy";
        currentAnimation_enemy = "enemy_hurt_Animation";
        yield return new WaitForSeconds(0.2f);
        currentAnimation_player = "player_idle_Animation"; // ideally block animation

        Text dmgLabel = attacker == "player" ? enemyDmgLabelText : playerDmgLabelText;
        Image bam = attacker == "player" ? enemyDmgLabelText.GetComponentInParent<Image>() : playerDmgLabelText.GetComponentInParent<Image>();
        AudioClip sound;
        if (attacker == "player") dmgLabel.text = "X";
        else dmgLabel.text = "BLOCKED!";
        dmgLabel.color = Color.grey;
        sound = miss_Sound;
        soundsrc.PlayOneShot(sound, PlayerPrefs.GetFloat("fx"));
        dmgLabel.enabled = true;
        bam.enabled = true;
        yield return new WaitForSeconds(0.3f);
        yield return new WaitForSeconds(0.75f);
        dmgLabel.enabled = false;
        bam.enabled = false;
        yield return new WaitForSeconds(0.5f);

        attacker = "player";
        currentAnimation_player = "player_idle_Animation";
        yield return new WaitForSeconds(0.2f);
        currentAnimation_enemy = "enemy_idle_Animation";

        dmgLabel = attacker == "player" ? enemyDmgLabelText : playerDmgLabelText;
        bam = attacker == "player" ? enemyDmgLabelText.GetComponentInParent<Image>() : playerDmgLabelText.GetComponentInParent<Image>();
        if(attacker == "player") dmgLabel.text = "X";
        else dmgLabel.text = "BLOCKED!";
        dmgLabel.color = Color.grey;
        sound = miss_Sound;
        soundsrc.PlayOneShot(sound, PlayerPrefs.GetFloat("fx"));
        dmgLabel.enabled = true;
        bam.enabled = true;
        yield return new WaitForSeconds(0.3f);
        yield return new WaitForSeconds(0.75f);
        dmgLabel.enabled = false;
        bam.enabled = false;
        yield return new WaitForSeconds(0.5f);

        EnableActions();

    }


}
