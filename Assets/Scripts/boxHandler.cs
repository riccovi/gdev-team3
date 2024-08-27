using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxHandler : MonoBehaviour
{
    public playerMovement player;
    public string side;
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
            player.pullBox= transform.parent.GetComponent<Rigidbody2D>();
            player.side=name;
            Debug.Log("Player on on side of box");
            Debug.Log("Side"+ name);
        }        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && player!=null)
        {        
            Debug.Log("Player exiting side of box");
            player.pullBox=null;
            player.side="";
            player=null;
            transform.parent.GetComponent<Rigidbody2D>().bodyType= RigidbodyType2D.Static;
        }
    }  
}
