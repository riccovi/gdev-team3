using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [Header("SerialisedField")]
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayers;

    [Header("Player Movement")]
    public float movementSpeed;
    public float jumpHeight;
    public float jumpHeight2;
    public int ExtraJumps;
    public float fallMultiplier=1.35f;
    public float JumpTimeHold;

    [Header("Player Interact")]

    public float ThrowLenght=5;

    //Private
    private float moveInput;
    private Rigidbody2D rb;

    [HideInInspector]public bool FacingRight = true;

    private bool isGrounded;
    
    private int CurrentJumps;

    private float jumpTimeCounter;

    private bool isJumping;

    private bool firstJump;

    public bool ThrowMode;

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

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Method to flip sprite 
        //(moveInput > 0 ||
        //(moveInput < 0 ||

        if(ThrowMode)
        {
            if (FacingRight == false && mousePos.x > transform.position.x)
            {
                flip();
            }
            else if (FacingRight == true && (mousePos.x < transform.position.x ))
            {
                flip();
            }
        }
        else
        {
            if (FacingRight == false && moveInput > 0 )
            {
                flip();
            }
            else if (FacingRight == true && moveInput < 0 )
            {
                flip();
            }
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
                jumpTimeCounter = JumpTimeHold;
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
