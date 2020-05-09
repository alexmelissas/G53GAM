using UnityEngine;

public class PersistentObjects : MonoBehaviour
{
    public static PersistentObjects singleton;

    public Player player;
    public int currentHP;
    public Player player_before_battle;
    public Player player_beginning_of_level;
    public bool inBattle; // to only load the RPG elements when in battle

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
