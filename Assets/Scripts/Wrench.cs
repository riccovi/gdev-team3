using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.Mathematics;
using UnityEngine;
//using VSCodeEditor;

public class Wrench : MonoBehaviour
{
    public GameObject graphics;
    public float rotateSpead;
    public bool isRotating;

    public float moveSpeed;
    public float CallBackSpeed;
    public Vector3 targetPosition;

    public Camera mainCamera;

    [HideInInspector]public bool isClicked;

    private bool isDamaged=false;
    [HideInInspector]public bool CanCallBack=false;

    private bool returnWrench;

    public Transform wrenchHolder;

    public Quaternion origRotationGlobal;
    public Quaternion origRotation;
    public Transform origPos;
    //Use trigger to check damage

    public float distanceTwoLine;

    private bool ReachedDesiredLength = false;

    public Vector3 initialVelocity;
    public float gravity = -9.81f;
    private bool hasChangedDirection = false;

    public float launchForce;

    public bool WrenchImpact;

    public Rigidbody2D rb;

    public BoxCollider2D wrenchCollider;

    public ParticleSystem stopWrenchForce;

    public playerMovement player;

    public string facing;

    public DrawLineToMouse ThrowPOV;

    public LineRenderer lineRenderer;

    public bool onWayGround;

    public LayerMask WrenchCollision;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = ThrowPOV.lineRenderer;
        lineRenderer.enabled=false;

        rb=GetComponent<Rigidbody2D>();
        wrenchCollider=GetComponent<BoxCollider2D>();

        origRotation=transform.localRotation;
        origRotationGlobal = transform.rotation;
        origPos=transform;
        mainCamera=Camera.main;
        wrenchHolder=transform.parent;     
    }
    
    public void EnableGraphics()
    {
        graphics.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(1) &&  !CanCallBack && !isClicked)
        {
            Invoke("EnableGraphics",0.2f);
            lineRenderer.enabled=true;
            player.ThrowMode=true;

            player.ReplaceRunAnimationThorw();
            
            //Animation
            player.RangeAttack_getready();
        }

        selfRotation();

        if (Input.GetMouseButtonUp(1) && !CanCallBack && !isClicked)
        {
            //Animation
            player.RangeAttack_release();

            // Cast a line from startPoint to endPoint
            targetPosition = lineRenderer.GetPosition(1);
        
            RaycastHit2D hit = Physics2D.Raycast(origPos.position, targetPosition - origPos.position, Vector2.Distance(origPos.position, targetPosition),WrenchCollision);

            Vector2 hitPosition=Vector2.zero;
            // Check if the linecast hits something with the tag "Ground"
            // Save the hit position

            if (hit.collider != null )
            {
                // Save the hit position
                hitPosition = hit.point;
                Debug.Log("Hit Position: " + hit.transform.gameObject.name);
                onWayGround=true;
            }

            isClicked = true;
            lineRenderer.enabled = false;
            

            

            if(hit.collider!=null)
            {
                targetPosition= new Vector3(hitPosition.x,hitPosition.y,0);
            }
            targetPosition.z = 0; // Ensure z is 0 since it's a 2D plane

            transform.parent = null;

            distanceTwoLine = Vector2.Distance(origPos.position, targetPosition);

            initialVelocity = (targetPosition - origPos.position).normalized * moveSpeed; // Set initial velocity
            hasChangedDirection = false;

            StartCoroutine(ThrowWrenchCoroutine(targetPosition));

            
        }

        // Return wrench on left mouse button click
        if (Input.GetMouseButtonDown(0) && CanCallBack)
        {
            isDamaged = true;
            returnWrench = true;
        }

        if (returnWrench)
        {
            StartCoroutine(ReturnWrenchCoroutine());
            returnWrench = false;
        }
    }

    public void selfRotation()
    {
        if(isRotating)
        {
            transform.Rotate(0,0,rotateSpead*Time.deltaTime);
        }
        else
        {
            transform.Rotate(0,0,0);
        }
        
    }



    private IEnumerator ThrowWrenchCoroutine(Vector3 targetPosition)
    {
        Vector3 originalPos=transform.position;
        if(player.FacingRight)
        {
            facing="Right";
        }
        else
        {
            facing="Left";
        }

        //enable after 0.5sec in order not to collider with player
        //wrenchCollider.enabled=true;

        StartCoroutine(delayColliderEnable());
        isRotating = true;
        ReachedDesiredLength=false;

        Vector3 velocity = initialVelocity;
        Vector3 position = transform.position;
        
        //Travel Until You reach end point of lIne, maximum distance
        while (Vector2.Distance(transform.position, targetPosition) > 0.2f)
        {
            //Debug.Log("Distance" + Vector2.Distance(transform.position, targetPosition));
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
            
            yield return null;
        }


        transform.GetComponent<SpriteRenderer>().color=Color.red;

        //Apply force to mimic the falling movement          
        ApplyForce(originalPos,targetPosition);

        if(stopWrenchForce!=null)
        {
            stopWrenchForce.Play();
        }

        Debug.Log("Continue");

        //Wait untill hit ground
        while(!WrenchImpact)
        {
            Debug.Log("Impact");
            yield return null;
        }

        
        //Reset Variables and get it ready for return
        rb.velocity=Vector2.zero;
        rb.angularVelocity =0;
        rb.gravityScale=0;
        
        //Put it on waiForImpact
        transform.parent = null;
        isRotating = false;
        isDamaged = false;
        CanCallBack = true;
        isClicked = false;
        yield return null;
    }

    public IEnumerator delayColliderEnable()
    {
        yield return new WaitForSeconds(0.1f);

        //Debug.Break();

        wrenchCollider.enabled=true;
        
    }

    void ApplyForce(Vector2 currentPos,Vector2 endPos)
    {
        // Get the Rigidbody2D component
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale=1;

        if (rb != null)
        {
            // Apply a force to the Rigidbody2D
            Vector2 forceDirection = CalculateForceDirection(currentPos,endPos);
            //float forceMagnitude = 10f; // Adjust the force magnitude as needed
            rb.AddForce(forceDirection * launchForce, ForceMode2D.Impulse);
        }

        
    }

    public void StopThrowMovement()
    {
        StopAllCoroutines();

        //Reset Variables and get it ready for return
        rb.velocity=Vector2.zero;
        rb.angularVelocity =0;
        rb.gravityScale=0;
        
        //Put it on waiForImpact
        transform.parent = null;
        isRotating = false;
        isDamaged = false;
        CanCallBack = true;
        isClicked = false;

    }

    private void OnCollisionEnter2D(Collision2D other) {
        
        
        if(other.transform.CompareTag("Ground"))
        {
            Debug.Log("Collided with Ground");   
            WrenchImpact=true;
        }
        else if(other.transform.CompareTag("Player"))
        {
            Debug.Log("Collided with Player");  
            if(CanCallBack || ReachedDesiredLength)
            {
                StartCoroutine(ReturnWrenchCoroutine()); 
            }
        }
        else if(other.transform.CompareTag("LevelBoundaries"))
        {
            StartCoroutine(ReturnWrenchCoroutine()); 
        }
        
    }


    Vector2 CalculateForceDirection(Vector2 currentPos,Vector2 endPos)
{
    // Calculate the direction vector from C to B
    Vector2 direction = endPos - currentPos;
    return direction.normalized;
}

    private IEnumerator ReturnWrenchCoroutine()
    {
        Sound_Manager.instance.playerSoundOnce("RangeAttack_CallBack");
        
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale=0;
        wrenchCollider.enabled=false;

        if(player.moveInput==0)
        {
            player.ReplaceRunAnimation(player.animatorOverrideController_callBackIdle);
        }
        else
        {
            player.ReplaceRunAnimation(player.animatorOverrideController_callBackRun);
        }

        transform.GetComponent<SpriteRenderer>().color=Color.white;
        isRotating = true;

        while (Vector2.Distance(transform.position, wrenchHolder.position) > 0.5f)
        {
            transform.position = Vector2.MoveTowards(transform.position, wrenchHolder.position, CallBackSpeed * Time.deltaTime);
            yield return null;
        }

        transform.parent = wrenchHolder;      
        transform.localPosition = Vector3.zero; 

        rb.velocity=Vector2.zero;
        rb.angularVelocity =0;

        

        if(facing=="Right" && player.FacingRight || facing=="Left" && !player.FacingRight)
        {             
            transform.localRotation = origRotation;
        }
        else if(facing=="Left" && player.FacingRight)
        {
            //rotate
            transform.localRotation=Quaternion.Euler(0,0,-45);
        }
        else if(facing=="Right" && !player.FacingRight)
        {
            //rotate
            transform.localRotation=Quaternion.Euler(0,0,-45);
        }
        
        //transform.rotation = origRotationGlobal;

        Debug.Log("Rotation reset to: " + origRotation);

        graphics.SetActive(false);
        isRotating = false;
        CanCallBack = false;
        isDamaged = false;
        isClicked = false;

        WrenchImpact=false;

        player.ReplaceRunAnimationIdle();

    }

    private void ReturnWrench()
    {        
        isRotating=true;
        transform.position = Vector2.MoveTowards(transform.position,wrenchHolder.position,CallBackSpeed*Time.deltaTime);
    }
    public void ThrowWrench()
    {           
        CanCallBack=false;
        isDamaged=true;
        isRotating=true;

        transform.position = Vector2.MoveTowards(transform.position,targetPosition,moveSpeed*Time.deltaTime);
    }

}
