using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool isIdle;
    bool isLeft;
    int isIdleKey = Animator.StringToHash("isIdle");

    bool canJump = true;
    int groundMask = 1<<8;
    int speed = 4;

    // Start is called before the first frame update
    void Start()
    {
        
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

}
