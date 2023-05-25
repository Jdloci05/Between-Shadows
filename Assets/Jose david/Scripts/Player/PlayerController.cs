using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 1f;

    public float collisionOffset = 0.05f;

    public ContactFilter2D movementFilter;

    Animator animator;

    Vector2 movementInput;
    Rigidbody2D rb;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    bool canMove = true;

    public GameObject attackUp;
    public GameObject attackDown;
    public GameObject attackLeft;
    public GameObject attackRight;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();   
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
        //if movement input is not 0, try to move
        if(movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);

            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));

                if(!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
            }
            animator.SetBool("isMoving", success);
        }else
        {
            animator.SetBool("isMoving", false);
        }

        }
    }

    private bool TryMove(Vector2 direction)
    {
        //Check for potential collitions
        int count = rb.Cast(
            movementInput,//X and Y values between -1 and 1 that represent the direction from the body to look for collisions
            movementFilter,//The settings that determine where a collision can occur on such as layers to collide with
            castCollisions,//List of collisions to store the found collisions into after the cast is finished
            moveSpeed * Time.fixedDeltaTime + collisionOffset);//The amount to cast equal to the movement plus an offset

        if (count == 0)
        {
            Vector2 moveVector = movementInput * moveSpeed * Time.fixedDeltaTime;

            rb.MovePosition(rb.position + moveVector);
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();

        if(movementInput != Vector2.zero)
        {
            animator.SetFloat("MovementX", movementInput.x);
            animator.SetFloat("MovementY", movementInput.y);
        }
    }

    void OnAttack()
    {
        animator.SetTrigger("spearAttack");
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    public void OnAttackUp()
    {
        attackUp.GetComponent<BoxCollider2D>().enabled = true;
    }
    public void OffAttackUp()
    {
        attackUp.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void OnAttackDown()
    {
        attackDown.GetComponent<BoxCollider2D>().enabled = true;
    }
    public void OffAttackDown()
    {
        attackDown.GetComponent<BoxCollider2D>().enabled = false;
    }
    public void OnAttackRight()
    {
        attackRight.GetComponent<BoxCollider2D>().enabled = true;
    }
    public void OffAttackRight()
    {
        attackRight.GetComponent<BoxCollider2D>().enabled = false;
    }
    public void OnAttackLeft()
    {
        attackLeft.GetComponent<BoxCollider2D>().enabled = true;
    }
    public void OffAttackLeft()
    {
        attackLeft.GetComponent<BoxCollider2D>().enabled = false;
    }
}
