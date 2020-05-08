using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//! Entire Gameplay and Battle Screen management
public class Gameplay : MonoBehaviour {

    public GameObject battleScreen, hudManager;
    public GameObject attackButton, blockButton, runButton;
    public GameObject playerCharacter;

    public GameObject playerModel, enemyModel;
    public Slider playerHPSlider, enemyHPSlider;
    public Image playerHPColourImage, enemyHPColourImage;
    public Text playerNameText, enemyNameText, playerLevelText, enemyLevelText;
    public Text playerCurrentHPText, playerMaxHPText, enemyCurrentHPText, enemyMaxHPText;
    public Text playerDmgLabelText, enemyDmgLabelText; 
    public AudioClip playerHitSFX, enemyHitSFX, critSFX, missSFX, deathSFX;
    public AudioSource soundsrc, musicsrc;
    public GameObject winPopup, losePopup;

    //! For the popups at the end of the battle to know when to animate XP gain.
    public static int updatePlayer;

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

    private void OnDisable() { playerCharacter.SetActive(true); PlayerObjects.singleton.inBattle = false; }

    private void InitiateBattle()
    {
        if (PlayerObjects.singleton.player.hp > 0)
        {
            death = false;
            PlayerObjects.singleton.inBattle = true;
            updatePlayer = -1;
            playerCharacter.SetActive(false);

            player = Player.Clone(PlayerObjects.singleton.player);
            PlayerObjects.singleton.player_before_battle = Player.Clone(PlayerObjects.singleton.player); // keep the player before gains

            player.AttachItems();
            enemy = PlayerObjects.singleton.enemy;
            enemy.AttachItems();

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

            player_currentHP = PlayerObjects.singleton.currentHP;
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

            PlayerPrefs.SetFloat("music", 1);
            PlayerPrefs.SetFloat("fx", 1);
            musicsrc.volume = PlayerPrefs.GetFloat("music") / 6;
            musicsrc.loop = true;
            musicsrc.Play();

            ShowPopup(false, true);
            ShowPopup(false, false);

            playerModel.SetActive(true);
            //enemyModel.GetComponent<Image>()  // set to type of enemy
            enemyModel.SetActive(true);

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
            } else playerCurrentHPText.text = "" + PlayerObjects.singleton.currentHP;
        }
        if (playerHPSlider.value == 0 && !death)
        {
            soundsrc.PlayOneShot(deathSFX, PlayerPrefs.GetFloat("fx"));
            playerHPColourImage.enabled = false;
            death = true;
        }
        else if (playerHPSlider.value < 0.25) playerHPColourImage.color = Color.red;
        else if (playerHPSlider.value < 0.5) playerHPColourImage.color = Color.yellow;



        if (enemy_newHP > -1)
        {
            if (enemyHPSlider.value > enemy_newHP)
            {
                enemyHPSlider.normalizedValue -= 0.01f;
                enemyCurrentHPText.text = "" + Mathf.RoundToInt(enemyHPSlider.value * enemy_maxHP);
            } else enemyCurrentHPText.text = "" + Mathf.RoundToInt(enemy_newHP * enemy_maxHP);
        }
        if (enemyHPSlider.value == 0 && !death)
        {
            soundsrc.PlayOneShot(deathSFX, PlayerPrefs.GetFloat("fx"));
            enemyHPColourImage.enabled = false;
            death = true;
        }
        else if (enemyHPSlider.value < 0.25) enemyHPColourImage.color = Color.red;
        else if (enemyHPSlider.value < 0.5) enemyHPColourImage.color = Color.yellow;
    }

    public void Execute_Action(string action)
    {
        switch (action)
        {
            case "attack": DisableActions(); Execute_Attack(); break;
            case "block": if (turn_counter - last_block_turn >= 2)
                { DisableActions(); Execute_Block(); } break;
            case "run": DisableActions(); Execute_Run(); break;
            default: break;
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
            result = turn.PlayTurn();
            player = Player.Clone(turn.player); //Pass updated Player and Enemy to the next Turn
            enemy = Player.Clone(turn.enemy);
            if (result == 0) player_turn = player_turn ? false : true; // swap whose turn it is
            else { battleOver = true; i++; } //If there's a final outcome, battle over
        }
        StartCoroutine(PlayTurns(turns,battleOver));
    }

    private void Execute_Block()
    {
        result = 0;
        turn_counter++;
        last_block_turn = turn_counter;
        StartCoroutine(PlayBlock());
    }

    private void Execute_Run() { Invoke("CloseBattleScreen", 0.5f); }

    private void EndBattle()
    {
        StopAllCoroutines();
        death = true;
        if (result == 1) playerModel.SetActive(false);
        else if (result == 2) enemyModel.SetActive(false);
        UpdatePlayer(); // Update the player based on outcome

        gameObject.GetComponent<BattleResultPopup>().didSetup = false; // Setup the popup
        if (result == 1) ShowPopup(true, false); // Player LOSE popup
        else if (result == 2) ShowPopup(true, true); // Player WIN popup
        updatePlayer = result; //Enable BattleResultPopup animation
    }

    public void CloseBattleScreen()
    {
        PlayerObjects.singleton.inBattle = false;
        ShowPopup(false, true);
        ShowPopup(false, false);
        hudManager.GetComponent<HUDManager>().UpdateHPBar();
        playerCharacter.SetActive(true);
        musicsrc.Stop();
        if (result == 1)
        {
            PlayerObjects.singleton.player = Player.Clone(PlayerObjects.singleton.player_beginning_of_level);
            Player p = Player.Clone(PlayerObjects.singleton.player);
            p.AttachItems();
            PlayerObjects.singleton.currentHP = p.hp;
            SceneManager.LoadScene("Level1");
        }
        battleScreen.SetActive(false);
        result = 0;
    }

    private void UpdatePlayer()
    {
        Player oldPlayer = Player.Clone(PlayerObjects.singleton.player_before_battle);
        BattleResult battleResult = new BattleResult(oldPlayer, enemy, (result == 2) ? true : false);
        Player updatedPlayer = Player.Clone(battleResult.CalculateGains());
        PlayerObjects.singleton.player = Player.Clone(updatedPlayer);

        PlayerObjects.singleton.player_beginning_of_level.xp = updatedPlayer.xp;
        PlayerObjects.singleton.player_beginning_of_level.level = updatedPlayer.level;
        PlayerObjects.singleton.player_beginning_of_level.coins = updatedPlayer.coins;
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

    private void ShowPopup(bool shown, bool win) { (win ? winPopup : losePopup).SetActive(shown); }


    IEnumerator PlayTurns(List<Turn> turns, bool battleOver)
    {
        foreach (Turn turn in turns)
        {
            string attacker = turn.playersTurn ? "player" : "enemy";
            Text dmgLabel = attacker == "player" ? enemyDmgLabelText : playerDmgLabelText;
            Image pow = attacker == "player" ? enemyDmgLabelText.GetComponentInParent<Image>() : playerDmgLabelText.GetComponentInParent<Image>();
            AudioClip sound;
            if (turn.damage != 0)
            {
                sound = missSFX;
                if (turn.critLanded)
                {
                    dmgLabel.color = Color.red;
                    sound = critSFX;
                }
                else
                {
                    dmgLabel.color = Color.black;
                    sound = (attacker == "player") ? playerHitSFX : enemyHitSFX;
                }
                dmgLabel.text = "" + turn.damage;
            }
            else
            {
                dmgLabel.text = "MISS";
                dmgLabel.color = Color.grey;
                sound = missSFX;
            }
            soundsrc.PlayOneShot(sound, PlayerPrefs.GetFloat("fx"));
            dmgLabel.enabled = true;
            pow.enabled = true;
            yield return new WaitForSeconds(0.3f);

            if (turn.damage!=0)
            {
                float currenthp = (attacker == "player") ? turn.enemy.hp : PlayerObjects.singleton.currentHP;
                float maxhp = (attacker == "player") ? enemy_maxHP : player_maxHP;
                float hp_bar_value = currenthp / maxhp;
                if (attacker == "enemy") player_newHP = hp_bar_value;
                else enemy_newHP = hp_bar_value;
            }
            dmgLabel.enabled = false;
            pow.enabled = false;

            yield return new WaitForSeconds(0.5f);
        }

        // END BATTLE IF DEATH
        if (battleOver) Invoke("EndBattle", 0.5f);
        else EnableActions();
    }

    IEnumerator PlayBlock()
    {
        Text dmgLabel = playerDmgLabelText;
        Image pow = playerDmgLabelText.GetComponentInParent<Image>();
        AudioClip sound;
        dmgLabel.text = "BLOCK";
        dmgLabel.color = Color.grey;
        sound = missSFX;
        soundsrc.PlayOneShot(sound, PlayerPrefs.GetFloat("fx"));
        dmgLabel.enabled = true;
        pow.enabled = true;
        yield return new WaitForSeconds(1.05f);
        dmgLabel.enabled = false;
        pow.enabled = false;
        yield return new WaitForSeconds(0.5f);

        dmgLabel = enemyDmgLabelText;
        pow = enemyDmgLabelText.GetComponentInParent<Image>();
        dmgLabel.text = "-";
        dmgLabel.color = Color.grey;
        soundsrc.PlayOneShot(sound, PlayerPrefs.GetFloat("fx"));
        dmgLabel.enabled = true;
        pow.enabled = true;
        yield return new WaitForSeconds(1.05f);
        dmgLabel.enabled = false;
        pow.enabled = false;
        yield return new WaitForSeconds(0.5f);

        EnableActions();
    }
}
