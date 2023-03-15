using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10;
    [SerializeField] float jumpSpeed = 5;
    [SerializeField] float climbSpeed = 5;
    [SerializeField] Vector2 deathJump = new Vector2 (10f, 10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    Vector2 moveInput;
    Rigidbody2D MyRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;

    bool isAlive = true;
    void Start()
    {
        MyRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = MyRigidbody.gravityScale;
    }

    void Update()
    {
        if(!isAlive) {return;}
        Run();
        FlipSprite();
        Climbladder();
        Die();
    }

    void OnFire(InputValue value)
    {
        if(!isAlive) {return;}
        Instantiate(bullet, gun.position, transform.rotation);
    }

    void OnMove(InputValue value)
    {
        if(!isAlive) {return;}
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if(!isAlive) {return;}
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {return;}

        if(value.isPressed)
        {
            MyRigidbody.velocity += new Vector2 (0f, jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, MyRigidbody.velocity.y);
        MyRigidbody.velocity = playerVelocity;
        
        bool playerHasHorizontalSpeed = Mathf.Abs(MyRigidbody.velocity.x) > Mathf.Epsilon;
        
        
        myAnimator.SetBool("IsRunning", playerHasHorizontalSpeed);
        }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(MyRigidbody.velocity.x) > Mathf.Epsilon;

        if(playerHasHorizontalSpeed)
        {
        transform.localScale = new Vector2 (Mathf.Sign(MyRigidbody.velocity.x), 1f);
        }
    }

    void Climbladder()
    {
        

        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
         {
            MyRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("IsClimbing", false);
            return;
         }

        Vector2 climbVelocity = new Vector2 (MyRigidbody.velocity.x, moveInput.y * climbSpeed);
        MyRigidbody.velocity = climbVelocity;
        MyRigidbody.gravityScale = 0f;

        bool playerVerticalSpeed = Mathf.Abs(MyRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("IsClimbing", playerVerticalSpeed);
       
    }

    void Die()
    {
         if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
         {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            MyRigidbody.velocity = deathJump;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
         }

    }

}
