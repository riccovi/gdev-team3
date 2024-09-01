using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformColDetection : MonoBehaviour
{
    public MovingPlatform PlatformHandler;
    // Start is called before the first frame update
    void Start()
    {
        PlatformHandler=transform.parent.GetComponent<MovingPlatform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player on platform : " + other.name);
            other.transform.SetParent(PlatformHandler.Platform.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player off platform: " + other.name);
            other.transform.SetParent(null);
        }
    }
}
