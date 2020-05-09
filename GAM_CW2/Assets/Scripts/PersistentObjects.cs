using UnityEngine;

public class PersistentObjects : MonoBehaviour
{
    public static PersistentObjects singleton;

    public Player player;
    public int currentHP;
    public bool inBattle;
    public Player playerBeforeBattle;
    public Player playerLevelStart;

    public Player enemy;

    public bool unlocked1 = true;
    public bool unlocked2 = false;
    public bool unlocked3 = false;

    void Awake()
    {
        if (singleton == null)
        {
            DontDestroyOnLoad(gameObject);
            singleton = this;
        }
        else if (singleton != this) Destroy(gameObject);
    }

}
