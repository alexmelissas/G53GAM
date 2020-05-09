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
            Player enemy;
            string username = type;
            int hp, atk, def, spd, crit, agility, sword, shield, armour, boots;
            hp = atk = def = spd = crit = agility = sword = shield = armour = boots = 0;

            switch (level)
            {
                case 1: hp = 100; atk = 10; def = 20; spd = 10; crit = 1; agility = 2; sword = 1; shield = 1; armour = 1; boots = 1; break;
                case 2: hp = 250; atk = 40; def = 20; spd = 45; crit = 2; agility = 2; sword = 2; shield = 1; armour = 3; boots = 4; break;
                case 3: hp = 440; atk = 80; def = 20; spd = 90; crit = 3; agility = 2; sword = 3; shield = 1; armour = 1; boots = 1; break;
                case 4: hp = 440; atk = 80; def = 20; spd = 90; crit = 3; agility = 2; sword = 3; shield = 1; armour = 1; boots = 1; break;
                case 5: hp = 440; atk = 80; def = 20; spd = 90; crit = 3; agility = 2; sword = 3; shield = 1; armour = 1; boots = 1; break;

            }

            enemy = new Player(username, level, 0, 0, 0, hp, atk, def, spd, crit, agility, sword, shield, armour, boots, 0, 0, 0, 0);
            PersistentObjects.singleton.enemy = Player.Clone(enemy);

            rpgscreen.SetActive(true);
            Destroy(gameObject);
        }

    }
}
