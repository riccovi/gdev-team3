using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.Mathematics;
using UnityEngine;
using VSCodeEditor;

public class Wrench : MonoBehaviour
{
    public float rotateSpead;
    public bool isRotating;

    public float moveSpeed;
    public float CallBackSpeed;
    public Vector3 targetPosition;

    public Camera mainCamera;

    private bool isClicked;

    private bool isDamaged=false;
    public bool CanCallBack=false;

    private bool returnWrench;

    public Transform wrenchHolder;

    public Quaternion origRotation;
    public Transform origPos;
    //Use trigger to check damage

    public float distance23;

    private bool isTwoThirdsReached = false;

    public Vector3 initialVelocity;
    public float gravity = -9.81f;
    private bool hasChangedDirection = false;

    public float launchForce;

    public bool WrenchImpact;

    public Rigidbody2D rb;

    public BoxCollider2D wrenchCollider;

    public ParticleSystem stopWrenchForce;

    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        wrenchCollider=GetComponent<BoxCollider2D>();

        origRotation=transform.localRotation;
        origPos=transform;
        mainCamera=Camera.main;
        wrenchHolder=transform.parent;

 
        
    }

    // Update is called once per frame
    void Update()
    {



        selfRotation();

        if (Input.GetMouseButtonDown(0) && !CanCallBack && !isClicked)
        {
            isClicked = true;
            targetPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
            targetPosition.z = 0; // Ensure z is 0 since it's a 2D plane

            transform.parent=null;
            distance23 = Vector2.Distance(origPos.position, targetPosition)*0.3f;
            
            initialVelocity = (targetPosition - origPos.position).normalized * moveSpeed; // Set initial velocity
            hasChangedDirection = false;
            
            StartCoroutine(ThrowWrenchCoroutine(targetPosition));
        }

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
        wrenchCollider.enabled=true;
        isRotating = true;
        isTwoThirdsReached=false;

        Vector3 velocity = initialVelocity;
        Vector3 position = transform.position;
        
        while (Vector2.Distance(transform.position, targetPosition) > 0.2f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
            if (!isTwoThirdsReached && Vector2.Distance(transform.position,targetPosition) <= distance23)
            {
                isTwoThirdsReached = true;
                transform.GetComponent<SpriteRenderer>().color=Color.red;
                Debug.Log("Wrench has traveled 2/3 of the distance");

                //stop travel            
                ApplyForce(transform.position,targetPosition);

                if(stopWrenchForce!=null)
                {
                    stopWrenchForce.Play();
                }
                break;

            }
            yield return null;
        }

        Debug.Log("Continue");
        while(!WrenchImpact)
        {
            Debug.Log("Impact");
            yield return null;
        }

        //Put it on waiForImpact
        transform.parent = null;
        isRotating = false;
        isDamaged = false;
        CanCallBack = true;
        isClicked = false;
        yield return null;
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

    private void OnCollisionEnter2D(Collision2D other) {
        
        
        if(other.transform.CompareTag("Ground"))
        {
            Debug.Log("Collided with Ground");   
            WrenchImpact=true;
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
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale=0;
        wrenchCollider.enabled=false;

        transform.GetComponent<SpriteRenderer>().color=Color.white;
        isRotating = true;

        while (Vector2.Distance(transform.position, wrenchHolder.position) > 0.5f)
        {
            transform.position = Vector2.MoveTowards(transform.position, wrenchHolder.position, CallBackSpeed * Time.deltaTime);
            yield return null;
        }

        transform.parent = wrenchHolder;
        transform.localPosition = Vector3.zero;
        transform.localRotation = origRotation;

        Debug.Log("Rotation reset to: " + origRotation);

        isRotating = false;
        CanCallBack = false;
        isDamaged = false;
        isClicked = false;


        WrenchImpact=false;

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
