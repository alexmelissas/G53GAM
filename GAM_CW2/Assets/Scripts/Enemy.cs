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

    // Start is called before the first frame update
    void Start()
    {
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (PersistentObjects.singleton.inBattle==false)
        {
            if (vertical) transform.position = new Vector3(x, y + curve.Evaluate((Time.time % curve.length)), z);
            else transform.position = new Vector3(x + curve.Evaluate((Time.time % curve.length)), y, z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (PersistentObjects.singleton.currentHP > 0)
        {
            RPGCharacter enemy = new RPGCharacter(type, level);
            Nerf(enemy);
            PersistentObjects.singleton.enemy = RPGCharacter.HardCopy(enemy);

            rpgscreen.SetActive(true);
            Destroy(gameObject);
        }
    }

    private void Nerf(RPGCharacter enemy)
    {
        float nerfFactor = 0f;
        switch (type)
        {
            case "squirrel": nerfFactor = 0.3f; break;
            case "fox": nerfFactor = 0.2f; break;
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
