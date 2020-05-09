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

    private void OnCollisionEnter2D(Collision2D hit)
    {
        IInventoryItem item = hit.gameObject.GetComponent<IInventoryItem>();
        if (item != null) { hudManager.addItem(item); }
    }

    void Inventory_ItemUsed(object sender, InventoryEventArgs e)
    {
        if ((e.item as MonoBehaviour).gameObject.GetComponent<PickupableItem>().itemName == "carrot") // HEALTHPACK
        {
            Player player = Player.HardCopy(PersistentObjects.singleton.player);
            player.AttachItems();
            int currentHP = PersistentObjects.singleton.currentHP;
            if (currentHP + 20 <= player.hp) PersistentObjects.singleton.currentHP += 20;
            else PersistentObjects.singleton.currentHP = player.hp;
            hudManager.UpdateHPBar();
        }
        // HIGHJUMP
        else if ((e.item as MonoBehaviour).gameObject.GetComponent<PickupableItem>().itemName == "star") StartCoroutine(HighJump(15));
        // SHRINK
        else if ((e.item as MonoBehaviour).gameObject.GetComponent<PickupableItem>().itemName == "acorn") StartCoroutine(Shrink(20));

    }

    private IEnumerator HighJump(int duration)
    {
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

    private IEnumerator Shrink(int duration)
    {
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

        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), -Vector2.up, 1.0f, groundMask)) { canJump = true; }

        r.velocity = new Vector2(physicsVelocity.x, r.velocity.y);
    }
}