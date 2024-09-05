using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadeHandler : MonoBehaviour
{
    public PlatformEffector2D effector;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player Trigger");
            if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                effector.rotationalOffset = 180;
            }
            else if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                effector.rotationalOffset = 0;
            }
        }
    }
}
