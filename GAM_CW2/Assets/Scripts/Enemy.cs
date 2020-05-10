using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string type;
    public int level;

    public bool vertical;
    public GameObject rpgscreen;
    public AnimationCurve curve;

    private float x,y,z;

    void Start()
    {
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;
    }

    // I LIKE TO MOVE IT MOVE IT
    void Update()
    {
        if (PersistentObjects.singleton.inBattle==false && gameObject.tag != "boss") // BOSS DOESNT MOVE
        {
            if (vertical) transform.position = new Vector3(x, y + curve.Evaluate((Time.time % curve.length)), z);
            else transform.position = new Vector3(x + curve.Evaluate((Time.time % curve.length)), y, z);
        }
    }

    // LAUNCH THE BATTLE WHEN PLAYER TOUCHES ENEMY / DMG IF HAZARD
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.tag != "hazard" && PersistentObjects.singleton.currentHP > 0)
        {
            RPGCharacter enemy = new RPGCharacter(type, level);
            Nerf(enemy);
            //Debug.Log("ENEMY: atk:" + enemy.atk + ", def:" + enemy.def + ", spd:" + enemy.spd);
            PersistentObjects.singleton.enemy = RPGCharacter.HardCopy(enemy);

            if (gameObject.tag == "boss") PersistentObjects.singleton.bossFight = true;
            else PersistentObjects.singleton.bossFight = false;

            rpgscreen.SetActive(true);
            Destroy(gameObject);
        }
    }

    // NERF THE ENEMY STATS BASED ON TYPE
    private void Nerf(RPGCharacter enemy)
    {
        float nerfFactor = 0.1f;
        switch (type)
        {
            case "squirrel": nerfFactor = 0.12f; break;
            case "fox": nerfFactor = 0.11f; break;
            case "snowman": nerfFactor = 0.1f; break;
        }
        enemy.hp -= Mathf.RoundToInt(enemy.hp * nerfFactor);
        enemy.atk -= Mathf.RoundToInt(enemy.atk * nerfFactor);
        enemy.def -= Mathf.RoundToInt(enemy.def * nerfFactor);
        enemy.spd -= Mathf.RoundToInt(enemy.spd * nerfFactor);
        enemy.agility -= Mathf.RoundToInt(enemy.agility * nerfFactor);
        enemy.crit -= Mathf.RoundToInt(enemy.crit * nerfFactor);
    }
}
