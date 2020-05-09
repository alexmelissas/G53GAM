using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public HUDManager hudManager;
    public GameObject carrot, star;

    bool isIdle;
    bool isLeft;
    int isIdleKey = Animator.StringToHash("isIdle");

    public bool alive = true;
    bool canJump = true;
    int groundMask = 1 << 8;
    int speed = 4;

    // Start is called before the first frame update
    void Start()
    {
        hudManager.ItemUsed += Inventory_ItemUsed;
    }

    // Update is called once per frame
    void Update()
    {
        Animator a = GetComponent<Animator>();
        a.SetBool(isIdleKey, isIdle);

        SpriteRenderer r = GetComponent<SpriteRenderer>();
        r.flipX = isLeft;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main");
        }

        Vector2 physicsVelocity = Vector2.zero;
        Rigidbody2D r = GetComponent<Rigidbody2D>();
        isIdle = true;

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
                r.velocity = new Vector2(physicsVelocity.x, 4);
                canJump = false;
                isIdle = false;
            }
        }

        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), -Vector2.up, 1.0f, groundMask))
        {
            canJump = true;
        }

        r.velocity = new Vector2(physicsVelocity.x, r.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D hit)
    {
        IInventoryItem item = hit.gameObject.GetComponent<IInventoryItem>();
        if (item != null)
        {
            hudManager.addItem(item); // where inventory is the inventory object ref
        }
    }

    void Inventory_ItemUsed(object sender, InventoryEventArgs e)
    {
        if ((e.item as MonoBehaviour).gameObject == carrot) // HEAL
        {
            Player player = Player.Clone(PersistentObjects.singleton.player);
            player.AttachItems();
            int currentHP = PersistentObjects.singleton.currentHP;
            if (currentHP + 20 <= player.hp) PersistentObjects.singleton.currentHP += 20;
            else PersistentObjects.singleton.currentHP = player.hp;
            hudManager.UpdateHPBar();
        }
        else if ((e.item as MonoBehaviour).gameObject == star) // DOUBLEJUMP
        {

        }

    }

}