using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleScreen : MonoBehaviour
{

    public GameObject battleScreen, hudManager;
    public GameObject actionButtons;
    public GameObject playerCharacter;

    public GameObject playerModel, enemyModelSnowman, enemyModelFox, enemyModelSquirrel;
    public Slider playerHPSlider, enemyHPSlider;
    public Image playerHPColourImage, enemyHPColourImage;
    public Text playerNameText, enemyNameText, playerLevelText, enemyLevelText;
    public Text playerCurrentHPText, playerMaxHPText, enemyCurrentHPText, enemyMaxHPText;
    public Text playerDmgLabelText, enemyDmgLabelText;
    public AudioClip playerHitSFX, enemyHitSFX, critSFX, missSFX, deathSFX;
    public AudioSource soundsrc, musicsrc;
    public GameObject winPopup, losePopup;
    public Text winCoinsText, loseCoinsText;

    private RPGCharacter player, enemy;
    private GameObject enemyModel;
    private int result, turnCounter, lastBlockTurn;
    private int playerMaxHP, enemyMaxHP;
    private int coinsGained;
    private int playerCurrentHP;
    private bool death = false;
    float playerNewHP = -1;
    float enemyNewHP = -1;

    private void Update() { UpdateHP(); }

    private void OnEnable() { InitiateBattle(); }

    private void OnDisable() { playerCharacter.SetActive(true); PersistentObjects.singleton.inBattle = false; }

    private void ToggleActionButtons(bool on) { actionButtons.SetActive(on); }

    private void ToggleResultPopup(bool on, bool win)
    {
        (win ? winPopup : losePopup).SetActive(on);
        if (on) (win ? winCoinsText : loseCoinsText).text = "" + coinsGained;
    }

    private void InitiateBattle()
    {
        if (PersistentObjects.singleton.player.hp > 0)
        {
            death = false;
            PersistentObjects.singleton.inBattle = true;
            playerCharacter.SetActive(false);

            player = RPGCharacter.HardCopy(PersistentObjects.singleton.player);
            PersistentObjects.singleton.playerBeforeBattle = RPGCharacter.HardCopy(PersistentObjects.singleton.player); // keep the player before gains
            player.AttachItems();
            enemy = PersistentObjects.singleton.enemy;
            enemy.AttachItems();

            playerNameText.text = "" + player.username;
            playerLevelText.text = "" + player.level;
            enemyNameText.text = "" + enemy.username;
            enemyLevelText.text = "" + enemy.level;
            playerDmgLabelText.enabled = enemyDmgLabelText.enabled = false;
            playerDmgLabelText.GetComponentInParent<Image>().enabled = enemyDmgLabelText.GetComponentInParent<Image>().enabled = false;

            playerNewHP = -1;
            enemyNewHP = -1;
            playerMaxHP = player.hp;
            enemyMaxHP = enemy.hp;

            playerCurrentHP = PersistentObjects.singleton.currentHP;
            playerCurrentHPText.text = "" + playerCurrentHP;
            enemyCurrentHPText.text = "" + enemyMaxHP;
            playerMaxHPText.text = "/" + playerMaxHP;
            enemyMaxHPText.text = "/" + enemyMaxHP;

            playerHPColourImage.enabled = true;
            float hpBarValue = (float)playerCurrentHP / (float)playerMaxHP;
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

            ToggleResultPopup(false, true);
            ToggleResultPopup(false, false);

            playerModel.SetActive(true);
            enemyModelFox.SetActive(false);
            enemyModelSquirrel.SetActive(false);
            enemyModelSnowman.SetActive(false);
            PickEnemyModel();
            enemyModel.SetActive(true);

            turnCounter = 0;
            lastBlockTurn = -4;
            ToggleActionButtons(true);
        }
    }

    private void PickEnemyModel()
    {
        switch (enemy.username)
        {
            case "fox": enemyModel = enemyModelFox; break;
            case "squirrel": enemyModel = enemyModelSquirrel; break;
            case "snowman": enemyModel = enemyModelSnowman; break;
            default: enemyModel = enemyModelSnowman; break;
        }
    }

    private void UpdateHP()
    {
        if (playerNewHP > -1)
        {
            if (playerHPSlider.value > playerNewHP)
            {
                playerHPSlider.normalizedValue -= 0.01f;
                playerCurrentHPText.text = "" + Mathf.RoundToInt(playerHPSlider.value * playerMaxHP);
            }
            else playerCurrentHPText.text = "" + PersistentObjects.singleton.currentHP;
        }
        if (playerHPSlider.value == 0 && !death)
        {
            soundsrc.PlayOneShot(deathSFX, PlayerPrefs.GetFloat("fx"));
            playerHPColourImage.enabled = false;
            death = true;
        }
        else if (playerHPSlider.value < 0.25) playerHPColourImage.color = Color.red;
        else if (playerHPSlider.value < 0.5) playerHPColourImage.color = Color.yellow;



        if (enemyNewHP > -1)
        {
            if (enemyHPSlider.value > enemyNewHP)
            {
                enemyHPSlider.normalizedValue -= 0.01f;
                enemyCurrentHPText.text = "" + Mathf.RoundToInt(enemyHPSlider.value * enemyMaxHP);
            }
            else enemyCurrentHPText.text = "" + Mathf.RoundToInt(enemyNewHP * enemyMaxHP);
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
            case "attack": ToggleActionButtons(false); Execute_Attack(); break;
            case "block":
                if (turnCounter - lastBlockTurn >= 2)
                { ToggleActionButtons(false); Execute_Block(); }
                break;
            case "run": ToggleActionButtons(false); Execute_Run(); break;
            default: break;
        }
    }

    private void Execute_Attack()
    {
        List<Turn> turns = new List<Turn>();
        turnCounter++;
        bool playerTurn;
        if (player.spd >= enemy.spd) playerTurn = true;
        else playerTurn = false;
        bool battleOver = false;
        result = 0;

        for (int i = 0; i < 2; i++)
        {
            Turn turn = new Turn(playerTurn, player, enemy);
            turns.Add(turn);
            result = turn.PlayTurn();
            player = RPGCharacter.HardCopy(turn.player); //Pass updated Player and Enemy to the next Turn
            enemy = RPGCharacter.HardCopy(turn.enemy);
            if (result == 0) playerTurn = playerTurn ? false : true; // swap whose turn it is
            else { battleOver = true; i++; } //If there's a final outcome, battle over
        }
        StartCoroutine(PlayAttack(turns, battleOver));
    }

    private void Execute_Block()
    {
        result = 0;
        turnCounter++;
        lastBlockTurn = turnCounter;
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
        coinsGained = PersistentObjects.singleton.player.coins - PersistentObjects.singleton.playerBeforeBattle.coins;
        if (result == 1) ToggleResultPopup(true, false); // Player LOSE popup
        else if (result == 2) ToggleResultPopup(true, true); // Player WIN popup
    }

    private void UpdatePlayer()
    {
        RPGCharacter currentPlayer = RPGCharacter.HardCopy(PersistentObjects.singleton.playerBeforeBattle);
        BattleResult battleResult = new BattleResult(currentPlayer, enemy, (result == 2) ? true : false);
        RPGCharacter updatedPlayer = RPGCharacter.HardCopy(battleResult.CalculateGains());
        PersistentObjects.singleton.player = RPGCharacter.HardCopy(updatedPlayer);

        PersistentObjects.singleton.playerLevelStart.xp = updatedPlayer.xp;
        PersistentObjects.singleton.playerLevelStart.level = updatedPlayer.level;
        PersistentObjects.singleton.playerLevelStart.coins = updatedPlayer.coins;
    }

    public void CloseBattleScreen()
    {
        PersistentObjects.singleton.inBattle = false;
        ToggleResultPopup(false, true);
        ToggleResultPopup(false, false);
        hudManager.GetComponent<HUDManager>().UpdateHPBar();
        playerCharacter.SetActive(true);
        musicsrc.Stop();
        if (result == 1)
        {
            PersistentObjects.singleton.player = RPGCharacter.HardCopy(PersistentObjects.singleton.playerLevelStart);
            RPGCharacter p = RPGCharacter.HardCopy(PersistentObjects.singleton.player);
            p.AttachItems();
            PersistentObjects.singleton.currentHP = p.hp;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        battleScreen.SetActive(false);
        result = 0;
    }

    IEnumerator PlayAttack(List<Turn> turns, bool battleOver)
    {
        foreach (Turn turn in turns)
        {
            string attacker = turn.playersTurn ? "player" : "enemy";
            Text dmgLabel = attacker == "player" ? enemyDmgLabelText : playerDmgLabelText;
            Image pow = attacker == "player" ? enemyDmgLabelText.GetComponentInParent<Image>() : playerDmgLabelText.GetComponentInParent<Image>();
            AudioClip sound;
            if (turn.damage != 0)
            {
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

            if (turn.damage != 0)
            {
                float currenthp = (attacker == "player") ? turn.enemy.hp : PersistentObjects.singleton.currentHP;
                float maxhp = (attacker == "player") ? enemyMaxHP : playerMaxHP;
                float hp_bar_value = currenthp / maxhp;
                if (attacker == "enemy") playerNewHP = hp_bar_value;
                else enemyNewHP = hp_bar_value;
            }
            dmgLabel.enabled = false;
            pow.enabled = false;

            yield return new WaitForSeconds(0.5f);
        }

        // END BATTLE IF DEATH
        if (battleOver) Invoke("EndBattle", 0.5f);
        else ToggleActionButtons(true);
    }

    IEnumerator PlayBlock()
    {
        Text dmgLabel = playerDmgLabelText;
        Image pow = playerDmgLabelText.GetComponentInParent<Image>();
        AudioClip sound;
        dmgLabel.text = "X";
        dmgLabel.color = Color.grey;
        sound = missSFX;
        soundsrc.PlayOneShot(sound, PlayerPrefs.GetFloat("fx"));
        dmgLabel.enabled = true;
        pow.enabled = true;
        yield return new WaitForSeconds(1.05f);
        dmgLabel.enabled = false;
        pow.enabled = false;
        yield return new WaitForSeconds(0.5f);

        ToggleActionButtons(true);
    }
}