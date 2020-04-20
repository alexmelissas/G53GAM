using System.Collections.Generic;
using UnityEngine;

//! Handle the Battle Animations
public class AnimationManager : MonoBehaviour {

    public GameObject player_hurt_Animation, player_idle_Animation, player_attack_Animation, player_die_Animation;
    public GameObject enemy_hurt_Animation, enemy_idle_Animation, enemy_attack_Animation, enemy_die_Animation;

    //! Stores all available animations
    private Dictionary<string,GameObject> animations;
    private string current_player_Animation;
    private string current_enemy_Animation;

    //! Save all available animations, set both Players to Idle
    void Start()
    {
        animations = new Dictionary<string, GameObject>
        {
            { "player_hurt_Animation", player_hurt_Animation },
            { "player_idle_Animation", player_idle_Animation },
            { "player_attack_Animation", player_attack_Animation },
            { "player_die_Animation", player_die_Animation },
            { "enemy_hurt_Animation", enemy_hurt_Animation },
            { "enemy_idle_Animation", enemy_idle_Animation },
            { "enemy_attack_Animation", enemy_attack_Animation },
            { "enemy_die_Animation", enemy_die_Animation }
        };

        foreach (KeyValuePair<string,GameObject> animation in animations)
            animation.Value.SetActive(false);

        current_player_Animation = "player_idle_Animation";
        current_enemy_Animation = "enemy_idle_Animation";
        animations["player_idle_Animation"].SetActive(true);
        animations["enemy_idle_Animation"].SetActive(true);
    }

    //! Update the animations according to changes from the Gameplay class
    void Update () {

        if (Gameplay.ended)
        {
            animations[current_player_Animation].SetActive(false);
            animations[current_enemy_Animation].SetActive(false);
            return;
        }

        if (current_player_Animation != Gameplay.currentAnimation_player)
        {
            string new_animation = "" + Gameplay.currentAnimation_player;
            string old_animation = "" + current_player_Animation;
            animations[new_animation].SetActive(true);
            animations[old_animation].SetActive(false);
            current_player_Animation = Gameplay.currentAnimation_player;
        }

        if(current_enemy_Animation != Gameplay.currentAnimation_enemy)
        {
            string new_animation = "" + Gameplay.currentAnimation_enemy;
            string old_animation = "" + current_enemy_Animation;
            animations[new_animation].SetActive(true);
            animations[old_animation].SetActive(false);
            current_enemy_Animation = Gameplay.currentAnimation_enemy;
        }
            
    }

}
