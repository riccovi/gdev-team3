using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class concreteHandler : MonoBehaviour
{
    public playerMovement player;
    public float movementSpeedPenalty=3;
    public float ResetJumpTime=2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Player on concrete");
        if(other.CompareTag("Player"))
        {
            player = other.GetComponent<playerMovement>();
            player.canPlayerJump=false;
            player.PenalyMovement=movementSpeedPenalty;
        }        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Player off concrete");
        if(other.CompareTag("Player") && player!=null)
        { 
            player.PenalyMovement=0;
            if(player.isGrounded)
            {
                player.canPlayerJump=true;
                player = null; 
            }   
            else
            {
                StartCoroutine(resetJump());
            }               

        }
    }   

    IEnumerator resetJump()
    {
        yield return new WaitForSeconds(ResetJumpTime);

        player.canPlayerJump=true;
        player = null;        

        yield return null;
    }


}
