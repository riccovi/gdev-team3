using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float jumpHeight;
    public float jumpHeight2;

    private float moveInput;

    public Rigidbody2D rb;

    public bool FacingRight = true;

    public bool isGrounded;

    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayers;

    public int ExtraJumps;
    public int CurrentJumps;

    public float jumpTimeCounter;
    public float JumpTime;

    public bool isJumping;

    public bool firstJump;

    public float fallMultiplier=1.35f;

    // Start is called before the first frame update
    void Start()
    {
        CurrentJumps = ExtraJumps;
        rb = GetComponent<Rigidbody2D>();
    }

    // Handle Physics
    private void FixedUpdate()
    {
        // Handle ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayers);

        // Handle horizontal movement
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * movementSpeed, rb.velocity.y);

        // Method to flip sprite
        if (FacingRight == false && moveInput > 0)
        {
            flip();
        }
        else if (FacingRight == true && moveInput < 0)
        {
            flip();
        }

        CheckAndIncreaseFallSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            CurrentJumps = ExtraJumps;
            firstJump = true;
        }

        HandleJumpInput();
    }

     private void CheckAndIncreaseFallSpeed()
    {
        // Increase gravity scale when the player is falling
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallMultiplier;
        }
    }

    private void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (CurrentJumps > 0 || isGrounded))
        {
            // Add debug log to indicate the jump number
            if (!isGrounded)
            {
                CurrentJumps--;
            }

            var currentJumpHeigh=jumpHeight;
            if(CurrentJumps==ExtraJumps)
            {
                Debug.Log("firstJump");
            }
            else
            {
                currentJumpHeigh=jumpHeight2;
            }

            isJumping = true;
            if (isGrounded)
            {
                jumpTimeCounter = JumpTime;
            }
            rb.velocity = Vector2.up * currentJumpHeigh;
            Debug.Log("Jump Height : " + currentJumpHeigh);
            

            
       
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (firstJump)
            {
                if (jumpTimeCounter > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpHeight); // Maintain horizontal velocity
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                    firstJump = false;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    void flip()
    {
        FacingRight = !FacingRight;

        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
