using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public HUDManager hudManager;
    public GameObject activePowerupStar, activePowerupAcorn;
    public SpriteRenderer playerSprite;
    public Text timeText;

    int isIdleKey = Animator.StringToHash("isIdle");
    bool isIdle;
    bool isLeft;
    public bool alive = true;

    bool canJump;
    int groundMask = 1 << 8;
    int speed = 4;
    float jumpHeight = 4;

    void Start()
    {
        hudManager.ItemUsed += Inventory_ItemUsed;
        OnEnable();
    }

    // RESET ACORN/STAR POWERUP EFFECTS ON LOAD
    private void OnEnable()
    {
        StopAllCoroutines();
        playerSprite.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        jumpHeight = 4;
        timeText.text = "";
        activePowerupAcorn.SetActive(false);
        activePowerupStar.SetActive(false);
    }

    void Update()
    {
        Animator a = GetComponent<Animator>();
        a.SetBool(isIdleKey, isIdle);

        SpriteRenderer r = GetComponent<SpriteRenderer>();
        r.flipX = isLeft;
    }

    // HANDLE HUD ITEMS/COIN PICKUP/ENTER DOOR
    private void OnCollisionEnter2D(Collision2D hit)
    {
        IInventoryItem item = hit.gameObject.GetComponent<IInventoryItem>();
        if (item != null) { hudManager.addItem(item); }

        // PICKUP COINPACK
        else if (hit.gameObject.tag == "coinsPowerup")
        { 
            hit.gameObject.SetActive(false);
            PersistentObjects.singleton.player.coins += 250;
            hudManager.UpdateHUD();
        }

        // WALK INTO EXIT [DOOR]
        else if (hit.gameObject.tag == "door")
        {
            PersistentObjects.singleton.player.coins += 100;
            switch (SceneManager.GetActiveScene().name)
            {
                case "Level1": PersistentObjects.singleton.unlocked2 = true;
                               SceneManager.LoadScene("Level2");  break;
                case "Level2": PersistentObjects.singleton.unlocked3 = true;
                               SceneManager.LoadScene("Level3"); break;
                default: SceneManager.LoadScene("Main"); break;
            }
        }

        // FALL OUT OF WORLD
        else if (hit.gameObject.tag == "boundary")
        {
            int coins = PersistentObjects.singleton.player.coins;
            if (coins > 0) PersistentObjects.singleton.player.coins -= 50;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // HIT HAZARD
        else if (hit.gameObject.tag == "hazard")
        {
            int coins = PersistentObjects.singleton.player.coins;
            if (coins > 0) PersistentObjects.singleton.player.coins -= 25;
            hudManager.UpdateHUD();
        }
    }

    // CLICK HUD ITEM
    void Inventory_ItemUsed(object sender, InventoryEventArgs e)
    {
        // CARROT - HEALTHPACK
        if ((e.item as MonoBehaviour).gameObject.GetComponent<PickupableItem>().itemName == "carrot")
            { if(RPGCharacter.Heal(20)) hudManager.UpdateHUD(); }
        // HIGHJUMP
        else if ((e.item as MonoBehaviour).gameObject.GetComponent<PickupableItem>().itemName == "star") StartCoroutine(HighJump(15));
        // SHRINK
        else if ((e.item as MonoBehaviour).gameObject.GetComponent<PickupableItem>().itemName == "acorn") StartCoroutine(Shrink(20));

    }

    // ENABLE HIGHJUMP POWERUP
    private IEnumerator HighJump(int duration)
    {
        StopCoroutine(Shrink(20));
        timeText.text = "";
        playerSprite.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        activePowerupAcorn.SetActive(false);

        activePowerupStar.SetActive(true);
        jumpHeight = 7.5f;
        for (int i = duration; i > 0; i--)
        {
            timeText.text = "" + i;
            yield return new WaitForSeconds(1f);
        }
        jumpHeight = 4;
        timeText.text = "";
        activePowerupStar.SetActive(false);
        StopAllCoroutines();
    }

    // ENABLE SHRINK POWERUP
    private IEnumerator Shrink(int duration)
    {
        StopCoroutine(HighJump(15));
        jumpHeight = 4;
        timeText.text = "";
        activePowerupStar.SetActive(false);

        activePowerupAcorn.SetActive(true);
        playerSprite.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        for (int i = duration; i > 0; i--)
        {
            timeText.text = "" + i;
            yield return new WaitForSeconds(1f);
        }
        playerSprite.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        timeText.text = "";
        activePowerupAcorn.SetActive(false);
        StopAllCoroutines();
    }

    // HANLDE MOVEMENT & INPUT
    void FixedUpdate()
    {
        Vector2 physicsVelocity = Vector2.zero;
        Rigidbody2D r = GetComponent<Rigidbody2D>();
        isIdle = true;

        if (Input.GetKey(KeyCode.Escape)) { SceneManager.LoadScene("Main"); }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            physicsVelocity.x -= speed;
            isIdle = false;
            isLeft = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            physicsVelocity.x += speed;
            isIdle = false;
            isLeft = false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (canJump)
            {
                canJump = false;
                r.velocity = new Vector2(physicsVelocity.x, jumpHeight);
                isIdle = false;
            }
        }

        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), -Vector2.up, 0.5f, groundMask)) { canJump = true; }

        r.velocity = new Vector2(physicsVelocity.x, r.velocity.y);
    }
}