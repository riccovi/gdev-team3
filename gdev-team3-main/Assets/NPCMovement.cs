using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform target;  // Set this to location B in the Unity Editor
    public float speed = 2.0f;

    private Rigidbody2D rb;
    private Animator animator;

    [Header("Special Conditions")]

    public List<Transform> blocks;

    [Header("Dialogue")]

    public SpriteRenderer bubleTransform;

    public Sprite Dialogue_GetBackWork;

    public Sprite Dialogue_Thanks;

    public Sprite Dialogue_Stacked;

    public Sprite Dialogue_ThanksOnceAgain;

    public Transform chatWhenFree;

    private bool OnMyWayToMachine;

    void Start()
    {
        chatWhenFree.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        
    }

    private void Update() {

        var countNullToActivate=CountNullTransforms(blocks);

        if(countNullToActivate==4 && !OnMyWayToMachine)
        {
            //All block Destroyed
            // Activate "panic" animation if the animator is present
            if (animator != null)
            {
                //animator.SetTrigger("panic");
            }

            // Start the movement coroutine
            StartCoroutine(MoveToTarget());
        }
        else if(countNullToActivate==1)
        {
            //Brake at least one change buble
            bubleTransform.sprite=Dialogue_Thanks;
            animator.SetTrigger("Idle");
        }
    }

    public bool AreAllTransformsNull(List<Transform> transformList)
    {
        foreach (Transform item in transformList)
        {
            if (item != null)
            {
                return false;  // Return false if any item is not null
            }
        }
        return true;  // Return true if all items are null
    }
    public int CountNullTransforms(List<Transform> transformList)
    {
        int nullCount = 0;

        foreach (Transform item in transformList)
        {
            if (item == null)
            {
                nullCount++;  // Increment the count if the item is null
            }
        }

        return nullCount;  // Return the count of null items
    }

    IEnumerator MoveToTarget()
    {
        OnMyWayToMachine=true;
        // Wait for a short time before starting the movement (useful if you want the "panic" animation to play for a bit)
        yield return new WaitForSeconds(1.5f);

        // Activate "move" animation when movement starts
        if (animator != null)
        {
            bubleTransform.sprite=Dialogue_GetBackWork;
            animator.SetTrigger("Run");
            //animator.SetBool("isMoving", true);
        }

        // Move towards the target position
        while (Mathf.Abs(target.position.x - transform.position.x) > 0.1f)
        {
            float directionX = target.position.x - transform.position.x;
            Vector2 movement = new Vector2(directionX, 0).normalized;

            rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + (movement * speed * Time.fixedDeltaTime));

            // Wait for the next frame
            yield return new WaitForFixedUpdate();
        }

        // Stop the NPC and trigger the "working" animation
        if (animator != null)
        {
            animator.ResetTrigger("Run");
            animator.SetTrigger("Work");
            chatWhenFree.gameObject.SetActive(true);
            //animator.SetTrigger("working");
        }

        Debug.Log("Chat Disable");
        
        bubleTransform.gameObject.SetActive(false);
        
    }
}