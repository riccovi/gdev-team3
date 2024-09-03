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

    public Transform Graphics;
    public Vector3 GraphicsOrigPos;
    public float MaxSpeed;
    public float movementSpeed;

    public bool MoveRight;

    public Vector3 YPlatformVelocity;

    public float PenalyMovement;

    public LayerMask StopMovementMask;
    public float CircleRadiusForCollisionChecks=0.5f;

    public ParticleSystem movementEffect;
    [Header("Player OnAir")]
    public float jumpHeight;
    public float jumpHeight2;
    public int ExtraJumps;
    public float fallMultiplier=1.55f;
    public float JumpTimeHold;

    public bool canPlayerJump;

    public ParticleSystem PlayerJumpEffect;

     [Header("Player Ladder")]

     private float ladderY;
     public float climbSpeed;
    public LayerMask Ladder;

    public float detectDistance=15f;

    public bool isClimbing;

    public float inputVertical;

    [Header("Player Interact")]

    public float ThrowLenght=5;

    [Header("Player Animation")]

    public Animator anim;
    public AnimatorOverrideController animatorOverrideController_idle;
    public AnimatorOverrideController animatorOverrideController_runThrow;
    public AnimatorOverrideController animatorOverrideController_push;
    public AnimatorOverrideController animatorOverrideController_pull;
    public AnimatorOverrideController animatorOverrideController_callBackIdle;

    public AnimatorOverrideController animatorOverrideController_callBackRun;

    [Header("Player Interactable")]
    public Rigidbody2D pullBox;

    public string side;

    public bool isPooling;
    public float PenalyPullSpeed;

    [Header("Random")]

    //Private
    public float moveInput;
    private Rigidbody2D rb;

    [HideInInspector]public bool FacingRight = true;

     public bool isGrounded;
    
    [HideInInspector]public int CurrentJumps;

    private float jumpTimeCounter;

    public bool isJumping;

    private bool firstJump;
    

    public bool ThrowMode;

    public bool attackAnim;

    public bool playerIsFailing;

    public float lastY;

    // Start is called before the first frame update
    void Start()
    {
        Graphics=transform.Find("PlayerGraphics_Sound").Find("Sprite");
        GraphicsOrigPos = Graphics.transform.localPosition;
        CurrentJumps = ExtraJumps;
        rb = GetComponent<Rigidbody2D>();
        movementSpeed=MaxSpeed;
    }

    public void restorePlayerMovement()
    {
        canPlayerJump=true;
        isPooling=false;
        pullBox=null;
        movementSpeed=MaxSpeed;
        PenalyMovement=0;
    }


    // Handle Physics
    private void FixedUpdate()
    {
        if(GameManager.instance.currentState==GameManager.gameStatus.Run)
        {
            if(GameManager.instance.currentState==GameManager.gameStatus.Run)
            {
                MovementHandler();
            }        

            LadderHandler();

            CheckAndIncreaseFallSpeed();
        }
        else
        {
            rb.velocity=Vector2.zero;
        }
        
        
    }
    public bool CheckForWallsAheadCircle()
    {
        // Define the radius of the circle (2D equivalent of a sphere)
        float circleRadius = CircleRadiusForCollisionChecks;

        // Define the position from which to check (e.g., the current position of the object)
        Vector2 circlePosition = transform.position;

        // Use OverlapCircle to check for any colliders in the specified layer in 2D space
        Collider2D hitCollider = Physics2D.OverlapCircle(circlePosition, circleRadius, StopMovementMask);

        if (hitCollider != null)
        {
            Debug.Log("Wall is nearby: " + hitCollider.name);
            return true;
        }
        else
        {
            return false;
        }
    }


    public void DeathAnimation()
    {
        ResetAllAnimationTrigger();
        anim.SetTrigger("Die");

        rb.gravityScale=40f;
    }

    private void MovementHandler()
    {
        
        var shakeOrNotToShake=false;
        if(!isGrounded)
        {
            shakeOrNotToShake=true;
        }
        // Handle ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayers);

        
        
        // Handle horizontal movement
        moveInput = Input.GetAxisRaw("Horizontal");
        


        //EffectHadnler
        if(isGrounded)
        {
            if(moveInput!=0)
            {
                Debug.Log("Play");
                movementEffect.Play();
            }
            else
            {
                movementEffect.Stop();
                Debug.Log("Stop");
            }
        }
        else
        {
            movementEffect.Stop();
        }

        var pullorPush="";
        if(moveInput>0)
        {
            if(isPooling)
            {
                Debug.Log("Move 1");
                if(side=="Left")
                {
                    ReplaceRunAnimationPush();
                    pullorPush="Push";
                }
                else
                {
                    ReplaceRunAnimationPull();
                    pullorPush="Pull";
                }
                
            }
        }
        else if(moveInput<0)
        {
            if(isPooling)
            {
                Debug.Log("Move -1");
                if(side=="Right")
                {
                    ReplaceRunAnimationPush();
                    pullorPush="Push";
                }
                else
                {
                    ReplaceRunAnimationPull();
                    pullorPush="Pull";
                }
            }
        }
        var boxspeed=moveInput * movementSpeed;
        if(pullorPush=="Pull")
        {
            boxspeed=moveInput * movementSpeed*1.4f;
        }

        var ymove = 0f;

        ymove=rb.velocity.y;

        rb.velocity = new Vector2(moveInput * (movementSpeed - PenalyMovement), ymove);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isPooling && pullBox != null)
        {
            if (movementSpeed == MaxSpeed)
            {
                movementSpeed = movementSpeed - PenalyPullSpeed;
            }
            Rigidbody2D boxRb = pullBox;

            boxRb.velocity = new Vector2(boxspeed, boxRb.velocity.y);
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
            else
            {
                if(!FacingRight)
                {
                    if(side!="Right")
                    {
                        //Debug.Log("Pullside from:"+side+",facing : Right ");
                        flip();
                    }
                }
                else
                {
                    if(side=="Right")
                    {
                        //Debug.Log("TEST");
                        flip();
                    }
                }
            }

        }

        //is Landed
        if(isGrounded)
        {
            if(shakeOrNotToShake)
            {
                Debug.Log("shake");
                
                //GameManager.instance.ShakeCamera();
            }

            playerIsFailing=false;
            
            
        }

        if(CheckForWallsAheadCircle() && !isPooling)
        {
            //Move 1 -Left
            Debug.Log("Wall Ahead" + moveInput);
            if(moveInput<0 && !FacingRight)
            {
                Debug.Log("Wall Ahead" + moveInput);
                moveInput=Mathf.Clamp(moveInput,0,1f);
                Debug.Log("Wall After" + moveInput);     
                          
            }
            
            if(moveInput>0 && FacingRight)
            {
                //Right Collision
                Debug.Log("Wall Ahead" + moveInput);
                moveInput=Mathf.Clamp(moveInput,-1,0f);
                Debug.Log("Wall After" + moveInput);
                
            }

            
        }
        else
        {
            Debug.Log("No wall");
        }

        //Animations
        if (isGrounded && !isClimbing)
        {
            if (moveInput != 0)
            {
                anim.ResetTrigger("Idle");
                anim.ResetTrigger("DoubleJump");
                anim.SetTrigger("Run");

                //walking sound
                //Sound_Manager.instance.playSound("Player_Loop",Sound_Manager.instance.Run);
            }
            else
            {
                if(!ThrowMode)
                {
                    if(!attackAnim)
                    {
                        anim.ResetTrigger("Run");
                        anim.ResetTrigger("DoubleJump");
                        anim.SetTrigger("Idle");  
                    }
                                      
                }
                else
                {
                    ReplaceRunAnimationIdle();
                }
                
            }
        }
        else if(isClimbing)
        {
                anim.ResetTrigger("Idle");
                anim.ResetTrigger("Run");
                anim.ResetTrigger("DoubleJump");
                anim.SetTrigger("Climb");
        }
        else
        {
            if (CurrentJumps == ExtraJumps)
            {
                //delay animation
                ResetAllAnimationTrigger();
                anim.SetTrigger("Jump");
            }
            else
            {
                ResetAllAnimationTrigger();
                anim.SetTrigger("DoubleJump");
            }

        }
    }

    public void ResetAllAnimationTrigger()
    {
        anim.ResetTrigger("Idle");
        anim.ResetTrigger("Run");
        anim.ResetTrigger("Jump");
        anim.ResetTrigger("DoubleJump");
        anim.ResetTrigger("Climb");
        anim.ResetTrigger("Attack");
        anim.ResetTrigger("AttackRun");
        anim.ResetTrigger("AttackRange_ready");
        anim.ResetTrigger("AttackRange_release");
    }
    public void RangeAttack_getready()
    {
        //attackAnim=true;
        anim.ResetTrigger("Idle");
        anim.ResetTrigger("Run");
        anim.ResetTrigger("Jump");
        anim.ResetTrigger("DoubleJump");
        anim.ResetTrigger("AttackRange_release");
        anim.SetTrigger("AttackRange_ready");
    }

     public void RangeAttack_release()
    {
        anim.ResetTrigger("Idle");
        anim.ResetTrigger("Run");
        anim.ResetTrigger("Jump");
        anim.ResetTrigger("DoubleJump");
        anim.ResetTrigger("AttackRange_ready");
        
        anim.SetTrigger("AttackRange_release");
    }

    public void Attack()
    {
        
        anim.ResetTrigger("Idle");        
        anim.ResetTrigger("Jump");
        anim.ResetTrigger("DoubleJump");
        anim.ResetTrigger("Run");

        if(moveInput == 0)
        {
            attackAnim=true;
            anim.SetTrigger("Attack");
        }
        else
        {
            anim.SetTrigger("AttackRun");           
        }
        
    }
    public void ReplaceRunAnimationThorw()
    {      
          
        if(moveInput!=0)
        {
            // Replace the 'Run2' clip with the 'Run2Throw' clip
            anim.runtimeAnimatorController = animatorOverrideController_runThrow;
            // Now the 'Run' state will use the 'Run2Throw' clip instead of 'Run2'
        }

    }

    public void ReplaceRunAnimationPush()
    {
        
        // Replace the 'Run2' clip with the 'Run2Throw' clip
        anim.runtimeAnimatorController = animatorOverrideController_push;
        // Now the 'Run' state will use the 'Run2Throw' clip instead of 'Run2'
    }

    public void ReplaceRunAnimationPull()
    {
        // Replace the 'Run2' clip with the 'Run2Throw' clip
        anim.runtimeAnimatorController = animatorOverrideController_pull;
        // Now the 'Run' state will use the 'Run2Throw' clip instead of 'Run2'
    }

    public void ReplaceRunAnimation(AnimatorOverrideController newAnim)
    {
        // Replace the 'Run2' clip with the 'Run2Throw' clip
        anim.runtimeAnimatorController = newAnim;
        // Now the 'Run' state will use the 'Run2Throw' clip instead of 'Run2'
    }

    public void ReplaceRunAnimationIdle()
    {
        // Replace the 'Run2' clip with the 'Run2Throw' clip
        anim.runtimeAnimatorController = animatorOverrideController_idle;
        // Now the 'Run' state will use the 'Run2Throw' clip instead of 'Run2'
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
                    ladderY=hitInfo.collider.transform.parent.position.y;
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

            //walking sound
            //Sound_Manager.instance.playSound("Player_Loop",Sound_Manager.instance.Climb);

            inputVertical=Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x,inputVertical*climbSpeed);
            rb.gravityScale=0;


        }
        else
        {
            rb.gravityScale=1; 
        }
    }
    void OnDrawGizmos()
    {
        // Define the radius of the circle
        float circleRadius = CircleRadiusForCollisionChecks;

        // Define the position of the circle
        Vector2 circlePosition = transform.position;

        // Set the color for the gizmo (semi-transparent blue)
        Gizmos.color = new Color(0, 0, 1, 0.3f);

        // Draw the circle as a gizmo
        Gizmos.DrawSphere(circlePosition, circleRadius);
    }


    // Update is called once per frame
    void Update()
    {
        //Check if player is on falling mode
        float distancePerSecondSinceLastFrame = (transform.position.y - lastY) * Time.deltaTime;
        lastY = transform.position.y;  //set for next frame
        if (distancePerSecondSinceLastFrame < -0.005f)
        {
            playerIsFailing = true;            
        }


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
                //walking sound
                //Sound_Manager.instance.playSound("Player_Loop",Sound_Manager.instance.Run);
                isPooling=true;
                pullBox.bodyType=RigidbodyType2D.Dynamic;
                pullBox.constraints = RigidbodyConstraints2D.FreezeRotation;
                pullBox.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

                if(MoveRight)
                {
                    Debug.Log("Move 1");
                    ReplaceRunAnimationPush();
                }
                else
                {
                     Debug.Log("Move -1");
                    ReplaceRunAnimationPull();
                }
                
            }

            if(pullBox==null)
            {
                movementSpeed=MaxSpeed;
                ReplaceRunAnimationIdle();
                isPooling=false;
            }

            if(Input.GetKeyUp(KeyCode.E) && pullBox!=null)
            {
                pullBox.bodyType=RigidbodyType2D.Static;
                isPooling=false;
                movementSpeed=MaxSpeed;;
                ReplaceRunAnimationIdle();
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

            //Sound_Manager.instance.playSound("Player_Once",Sound_Manager.instance.Jump);
            PlayerJumpEffect.Play();
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
                Debug.Log("secondJump");
                currentJumpHeigh=jumpHeight2;

                
            }

            isJumping = true;
            if (isGrounded)
            {
                jumpTimeCounter = JumpTimeHold;
            }            

            if(CurrentJumps==ExtraJumps)
            {
                Debug.Log("firstJump");
                rb.velocity = Vector2.up * currentJumpHeigh;
            }
            else
            {
                rb.velocity = Vector2.up * (currentJumpHeigh-jumpHeight*0.75f);
                Debug.Log("secondJump");
                
            }
            //rb.velocity = Vector2.up * currentJumpHeigh;
            Debug.Log("Jump Height : " + currentJumpHeigh);           

            
       
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            //Sound_Manager.instance.playSound("Player_Once",Sound_Manager.instance.DoubleJump);
            if (firstJump)
            {
                PlayerJumpEffect.Play();
                

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

        if(Input.GetKeyDown(KeyCode.Space) && (isJumping || isGrounded))
        {
            Sound_Manager.instance.playerSoundOnce("Jump");
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
