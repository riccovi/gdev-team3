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
    public float MaxSpeed;
    public float movementSpeed;

    public float PenalyMovement;
     [Header("Player OnAir")]
    public float jumpHeight;
    public float jumpHeight2;
    public int ExtraJumps;
    public float fallMultiplier=1.35f;
    public float JumpTimeHold;

    public bool canPlayerJump;

     [Header("Player Ladder")]
     public float climbSpeed;
    public LayerMask Ladder;

    public float detectDistance=15f;

    public bool isClimbing;

    public float inputVertical;

    [Header("Player Interact")]

    public float ThrowLenght=5;

    [Header("Player Animation")]

    public Animator anim;

    [Header("Player Interactable")]
    public Rigidbody2D pullBox;

    public bool isPooling;
    public float PenalyPullSpeed;

    [Header("Random")]

    //Private
    private float moveInput;
    private Rigidbody2D rb;

    [HideInInspector]public bool FacingRight = true;

    [HideInInspector] public bool isGrounded;
    
    private int CurrentJumps;

    private float jumpTimeCounter;

    private bool isJumping;

    private bool firstJump;
    

    [HideInInspector]public bool ThrowMode;

    // Start is called before the first frame update
    void Start()
    {
        CurrentJumps = ExtraJumps;
        rb = GetComponent<Rigidbody2D>();
        movementSpeed=MaxSpeed;
    }


    // Handle Physics
    private void FixedUpdate()
    {
        if(GameManager.instance.currentState==GameManager.gameStatus.Run)
        {
            MovementHandler();
        }        

        LadderHandler();

        CheckAndIncreaseFallSpeed();
    }

    private void MovementHandler()
    {
        // Handle ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayers);

        // Handle horizontal movement
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * (movementSpeed - PenalyMovement), rb.velocity.y);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isPooling && pullBox != null)
        {
            if (movementSpeed == MaxSpeed)
            {
                movementSpeed = movementSpeed - PenalyPullSpeed;
            }
            Rigidbody2D boxRb = pullBox;

            boxRb.velocity = new Vector2(moveInput * movementSpeed, boxRb.velocity.y);
        }

        if (isGrounded)
        {
            if (moveInput != 0)
            {
                anim.ResetTrigger("Idle");
                anim.ResetTrigger("DoubleJump");
                anim.SetTrigger("Run");
            }
            else
            {
                anim.ResetTrigger("Run");
                anim.ResetTrigger("DoubleJump");
                anim.SetTrigger("Idle");
            }
        }
        else
        {
            if (CurrentJumps == ExtraJumps)
            {
                //delay animation
                anim.ResetTrigger("Idle");
                anim.ResetTrigger("Run");
                anim.ResetTrigger("DoubleJump");
                anim.SetTrigger("Jump");
            }
            else
            {
                anim.ResetTrigger("Idle");
                anim.ResetTrigger("Run");
                anim.ResetTrigger("Jump");
                anim.SetTrigger("DoubleJump");
            }

        }


        // Method to flip sprite 
        //(moveInput > 0 ||
        //(moveInput < 0 ||

        if (ThrowMode)
        {
            if (FacingRight == false && mousePos.x > transform.position.x)
            {
                flip();
            }
            else if (FacingRight == true && (mousePos.x < transform.position.x))
            {
                flip();
            }
        }
        else
        {
            if (!isPooling)
            {
                if (FacingRight == false && moveInput > 0)
                {
                    flip();
                }
                else if (FacingRight == true && moveInput < 0)
                {
                    flip();
                }

            }

        }
    }

    public void LadderHandler()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,Vector2.up,detectDistance,Ladder);

        if(hitInfo.collider!=null)
        {
            if(hitInfo.collider.transform.CompareTag("Ladder"))
            {
                if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                {
                    isClimbing=true;
                }
            }
        }
        else
        {
            isClimbing=false;
        }

        if(isClimbing)
        {
            inputVertical=Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x,inputVertical*climbSpeed);
            rb.gravityScale=0;
        }
        else
        {
            rb.gravityScale=1; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            CurrentJumps = ExtraJumps;
            firstJump = true;
        }

        

        if(GameManager.instance.currentState==GameManager.gameStatus.Run)
        {
            if(canPlayerJump)
            {
                HandleJumpInput();            
            }

            if(pullBox!=null && Input.GetKeyDown(KeyCode.E))
            {
                isPooling=true;
            }

            if(Input.GetKeyUp(KeyCode.E))
            {
                isPooling=false;
                movementSpeed=MaxSpeed;;
            }
        }      

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
