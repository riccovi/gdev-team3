using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxHandler : MonoBehaviour
{
    public playerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        
        if(other.CompareTag("Player"))
        {
            player = other.GetComponent<playerMovement>();
            Debug.Log("Player on on side of box");
        }        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && player!=null)
        {        
            Debug.Log("Player exiting side of box");
        }
    }  
}
