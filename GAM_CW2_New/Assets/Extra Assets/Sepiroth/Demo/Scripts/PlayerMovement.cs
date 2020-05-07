using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;
    float horizontalMove;
    bool jump = false;
    bool crouch = false;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed; //좌우 이동
        animator.SetFloat("xSpeed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("isJumping", true);
        }
        if (Input.GetButton("Crouch"))
        {
            crouch = true;
        }
        else
        {
            crouch = false; //마우스랑 터치의 crouch의 탈출은 모두 여기서 이루어짐
        }


        if (Input.touchCount > 0)
        {
                Touch touch = Input.GetTouch(0);

                float direction = touch.position.x > Screen.width / 2 ? 1 : -1;//좌우 이동
                horizontalMove = direction * runSpeed;
                animator.SetFloat("xSpeed", Mathf.Abs(horizontalMove));

            if ((Input.touchCount > 1))
            {
                jump = true;
                animator.SetBool("isJumping", true);
            }
            if ((touch.position.y < Screen.width / 8))
            {
                crouch = true;
            }
        }

    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("isCrouching", isCrouching);

    }
    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
