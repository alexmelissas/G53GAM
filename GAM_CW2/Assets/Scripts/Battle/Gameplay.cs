using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! Entire Gameplay and Battle Screen management
public class Gameplay : MonoBehaviour {

    public GameObject battleScreen, hudManager;
    public GameObject attackButton, blockButton, runButton;
    public GameObject playerCharacter;

    public Slider playerHPSlider, enemyHPSlider;
    public Image playerHPColourImage, enemyHPColourImage;
    public Text playerNameText, enemyNameText, playerLevelText, enemyLevelText;
    public Text playerCurrentHPText, playerMaxHPText, enemyCurrentHPText, enemyMaxHPText, playerDmgLabelText, enemyDmgLabelText; 
    public AudioSource soundsrc, musicsrc;
    public AudioClip player_hit_Sound, enemy_hit_Sound, crit_Sound, miss_Sound, drop_sword_Sound;
    public GameObject winPopup, losePopup;

    //! For the popups at the end of the battle to know when to animate XP gain.
    public static int updatePlayer;
    public static string currentAnimation_player = "player_idle_Animation";
    public static string currentAnimation_enemy = "enemy_idle_Animation";

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

    private void Update() { UpdateHP(); }

    private void OnEnable() { InitiateBattle(); }

    private void OnDisable() { playerCharacter.SetActive(true); PlayerObjects.playerObjects.inBattle = false; }

    //! Setup the battle screen, including HP bars, Models, Damage Labels etc.
    private void InitiateBattle()
    {
        if (PlayerObjects.playerObjects.player.hp > 0)
        {
            death = false;
            PlayerObjects.playerObjects.inBattle = true;
            updatePlayer = -1;
            playerCharacter.SetActive(false);

            player = Player.Clone(PlayerObjects.playerObjects.player);
            PlayerObjects.playerObjects.player_before_battle = Player.Clone(PlayerObjects.playerObjects.player); // keep the player before gains

            Items.AttachItemsToPlayer(new Items(player), player);
            enemy = PlayerObjects.playerObjects.enemy;
            Items.AttachItemsToPlayer(new Items(enemy), enemy);

            playerNameText.text = "" + player.username;
            playerLevelText.text = "" + player.level;
            enemyNameText.text = "" + enemy.username;
            enemyLevelText.text = "" + enemy.level;
            playerDmgLabelText.enabled = enemyDmgLabelText.enabled = false;
            playerDmgLabelText.GetComponentInParent<Image>().enabled = enemyDmgLabelText.GetComponentInParent<Image>().enabled = false;

            player_newHP = -1;
            enemy_newHP = -1;
            player_maxHP = player.hp;
            enemy_maxHP = enemy.hp;

            player_currentHP = PlayerObjects.playerObjects.currentHP;
            playerCurrentHPText.text = "" + player_currentHP;
            enemyCurrentHPText.text = "" + enemy_maxHP;
            playerMaxHPText.text = "/" + player_maxHP;
            enemyMaxHPText.text = "/" + enemy_maxHP;

            playerHPColourImage.enabled = true;
            float hpBarValue = (float)player_currentHP / (float)player_maxHP;
            playerHPSlider.value = hpBarValue;
            if (playerHPSlider.value == 0) { playerHPColourImage.enabled = false; }
            else if (hpBarValue < 0.25) playerHPColourImage.color = Color.red;
            else if (hpBarValue < 0.5) playerHPColourImage.color = Color.yellow;
            else playerHPColourImage.color = Color.green;
            
            enemyHPColourImage.enabled = true;
            enemyHPSlider.normalizedValue = 1f;
            enemyHPColourImage.color = Color.green;

            PlayerPrefs.SetFloat("music", 5);
            PlayerPrefs.SetFloat("fx", 5);
            musicsrc.volume = PlayerPrefs.GetFloat("music") / 6;
            musicsrc.loop = true;
            musicsrc.Play();

            ShowPopup(false, true);
            ShowPopup(false, false);

            currentAnimation_player = "player_idle_Animation";
            currentAnimation_enemy = "enemy_idle_Animation";

            turn_counter = 0;
            last_block_turn = -4;
            EnableActions();
        }

    }


    private void UpdateHP()
    {
        if (player_newHP > -1)
        {
            if (playerHPSlider.value > player_newHP)
            {
                playerHPSlider.normalizedValue -= 0.01f;
                playerCurrentHPText.text = "" + Mathf.RoundToInt(playerHPSlider.value * player_maxHP);
            }
            else playerCurrentHPText.text = "" + PlayerObjects.playerObjects.currentHP;
        }
        if (playerHPSlider.value == 0 && !death)
        {
            soundsrc.PlayOneShot(drop_sword_Sound, PlayerPrefs.GetFloat("fx"));
            playerHPColourImage.enabled = false;
            death = true;
        }
        else if (playerHPSlider.value < 0.25)
            playerHPColourImage.color = Color.red;
        else if (playerHPSlider.value < 0.5)
            playerHPColourImage.color = Color.yellow;



        if (enemy_newHP > -1)
        {
            if (enemyHPSlider.value > enemy_newHP)
            {
                enemyHPSlider.normalizedValue -= 0.01f;
                enemyCurrentHPText.text = "" + Mathf.RoundToInt(enemyHPSlider.value * enemy_maxHP);
            }
            else enemyCurrentHPText.text = "" + Mathf.RoundToInt(enemy_newHP * enemy_maxHP);

        }
        if (enemyHPSlider.value == 0 && !death)
        {
            soundsrc.PlayOneShot(drop_sword_Sound, PlayerPrefs.GetFloat("fx"));
            enemyHPColourImage.enabled = false;
            death = true;
        }
        else if (enemyHPSlider.value < 0.25)
            enemyHPColourImage.color = Color.red;
        else if
            (enemyHPSlider.value < 0.5) enemyHPColourImage.color = Color.yellow;
    }



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
            else { battleOver = true; i++; }
        }
        StartCoroutine(AnimateTurns(turns,battleOver));
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
        Invoke("ResetCharacter", 0.5f);
    }

    private void EndBattle()
    {
        StopAllCoroutines();
        death = true;
        UpdatePlayer(); // Update the player based on outcome
        gameObject.GetComponent<WinLosePopup>().didSetup = false; // Setup the popup
        if (result == 1) ShowPopup(true, false); // Player LOSE popup
        else if (result == 2) ShowPopup(true, true); // Player WIN popup
        updatePlayer = result; //enables the popups for the exp and money gain to animate
    }

    public void CloseBattleScreen()
    {
        PlayerObjects.playerObjects.inBattle = false;
        ShowPopup(false, true);
        ShowPopup(false, false);
        hudManager.GetComponent<HUDManager>().UpdateHPBar();
        playerCharacter.SetActive(true);
        musicsrc.Stop();
        if (result == 1)
        {
            PlayerObjects.playerObjects.currentHP = PlayerObjects.playerObjects.player.hp;
            Application.LoadLevel("Level1");
            battleScreen.SetActive(false);
        }
        else
            battleScreen.SetActive(false);
        result = 0;
    }

    private void UpdatePlayer()
    {
        Player updatedPlayer = Player.Clone(PlayerObjects.playerObjects.player);
        BattleResult battleResult = new BattleResult(player, enemy, (result == 2) ? true : false);
        updatedPlayer = Player.Clone(battleResult.CalculateGains());
        PlayerObjects.playerObjects.player = Player.Clone(updatedPlayer);
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
    IEnumerator AnimateTurns(List<Turn> turns, bool battleOver)
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
            dmgLabel.enabled = false;
            bam.enabled = false;

            // (4) Wait a bit and go to next turn
            yield return new WaitForSeconds(0.5f);
        }

        // END BATTLE IF DEATH
        if (battleOver) Invoke("EndBattle", 0.5f);
        else EnableActions();
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
