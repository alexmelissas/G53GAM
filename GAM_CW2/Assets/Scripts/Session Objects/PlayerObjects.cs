using UnityEngine;

//! Singleton - Store the Player objects for the user and the enemy
public class PlayerObjects : MonoBehaviour
{
    // The Singleton object
    public static PlayerObjects playerObjects;

    public Player player;
    //! An old copy of the player for comparisons before and after a battle - gain xp etc
    public Player player_before_battle;
    public Player enemy;

    //! Player's HP in this level (not max)
    public int currentHP;

    public bool inBattle; // to only load the RPG shit when in battle

    //! Handle the Singleton object
    void Awake()
    {
        if (playerObjects == null)
        {
            DontDestroyOnLoad(gameObject);
            playerObjects = this;
        }
        else if (playerObjects != this)
        {
            Destroy(gameObject);
        }
    }

}
