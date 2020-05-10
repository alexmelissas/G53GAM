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

    // RESET AND SETUP ALL ELEMENTS OF THE BATTLE SCREEN
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

    // CHOOSE WHICH ENEMY MODEL TO SHOW BASED ON TYPE
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

    // ANIMATE THE HP BARS DROPPING
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

    // IMPLEMENT THE ACTION BUTTON FUNCTIONALITY - ATTACK/BLOCK/RUN
    public void Execute_Action(string action)
    {
        switch (action)
        {
            case "attack": ToggleActionButtons(false); Execute_Attack(); break;
            case "block": // ONLY ALLOW BLOCKING EVERY 3RD TURN
                if (turnCounter - lastBlockTurn >= 2)
                { ToggleActionButtons(false); Execute_Block(); }
                break;
            case "run": // CANT RUN FROM BOSS
                if(!PersistentObjects.singleton.bossFight)
                { ToggleActionButtons(false); Execute_Run(); }
                break;
            default: break;
        }
    }

    // HANDLE ATTACK COMMAND
    private void Execute_Attack()
    {
        List<BattleTurn> battleTurns = new List<BattleTurn>();
        turnCounter++;
        bool playerTurn;
        if (player.spd >= enemy.spd) playerTurn = true;
        else playerTurn = false;
        bool battleOver = false;
        result = 0;

        for (int i = 0; i < 2; i++)
        {
            BattleTurn battleTurn = new BattleTurn(player, enemy, playerTurn);
            battleTurns.Add(battleTurn);
            result = battleTurn.PlayTurn();
            player = RPGCharacter.HardCopy(battleTurn.player); // UPDATE TEMPORARY PLAYER/ENEMY AND PASS ON
            enemy = RPGCharacter.HardCopy(battleTurn.enemy);
            if (result == 0) playerTurn = playerTurn ? false : true; // SWAP WHOSE TURN IT IS
            else { battleOver = true; i++; } // FINISH BATTLE IF THERE'S A DEATH
        }
        StartCoroutine(PlayAttack(battleTurns, battleOver));
    }

    // SHOW THE 2 ATTACK TURNS PLAYING
    IEnumerator PlayAttack(List<BattleTurn> battleTurns, bool battleOver)
    {
        foreach (BattleTurn battleTurn in battleTurns)
        {
            AudioClip sound;
            string attacker = battleTurn.playersTurn ? "player" : "enemy";
            Text dmgLabel = attacker == "player" ? enemyDmgLabelText : playerDmgLabelText;
            Image pow = attacker == "player" ? enemyDmgLabelText.GetComponentInParent<Image>()
                                             : playerDmgLabelText.GetComponentInParent<Image>();
            if (battleTurn.damage != 0)
            {
                if (battleTurn.critLanded)
                {
                    dmgLabel.color = Color.red;
                    sound = critSFX;
                }
                else
                {
                    dmgLabel.color = Color.black;
                    sound = (attacker == "player") ? playerHitSFX : enemyHitSFX;
                }
                dmgLabel.text = "" + battleTurn.damage;
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

            if (battleTurn.damage != 0)
            {
                float currenthp = (attacker == "player") ? battleTurn.enemy.hp : PersistentObjects.singleton.currentHP;
                float maxhp = (attacker == "player") ? enemyMaxHP : playerMaxHP;
                float hp_bar_value = currenthp / maxhp;
                if (attacker == "enemy") playerNewHP = hp_bar_value;
                else enemyNewHP = hp_bar_value;
            }
            dmgLabel.enabled = false;
            pow.enabled = false;

            yield return new WaitForSeconds(0.5f);
        }
        if (battleOver) Invoke("EndBattle", 0.5f);
        else ToggleActionButtons(true);
    }

    // HANDLE BLOCK COMMAND
    private void Execute_Block()
    {
        result = 0;
        turnCounter++;
        lastBlockTurn = turnCounter;
        StartCoroutine(PlayBlock());
    }

    // SHOW THE PLAYER DODGING
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

    // HANDLE RUN COMMAND - CANT RUN FROM BOSS
    private void Execute_Run() { Invoke("CloseBattleScreen", 0.5f); }

    // INITIATE THE BATTLE-END PROCEDURE
    private void EndBattle()
    {
        StopAllCoroutines();
        death = true;
        if (result == 1) playerModel.SetActive(false);
        else if (result == 2) enemyModel.SetActive(false);
        UpdatePlayer(); // UPDATE THE PERSISTENT PLAYER OBJECT
        coinsGained = PersistentObjects.singleton.player.coins - PersistentObjects.singleton.playerBeforeBattle.coins;
        if (result == 1) ToggleResultPopup(true, false); // SHOW LOSE POPUP
        else if (result == 2) ToggleResultPopup(true, true); // SHOW WIN POPUP
    }

    // UPDATE THE PERSISTENT PLAYER RPGCHARACTER OBJECT AND CURRENTHP BASED ON BATTLE OUTCOME
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

    // CLOSE THE BATTLE SCREEN
    public void CloseBattleScreen()
    {
        PersistentObjects.singleton.inBattle = false;
        ToggleResultPopup(false, true);
        ToggleResultPopup(false, false);
        hudManager.GetComponent<HUDManager>().UpdateHUD();
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
}