using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformColDetection : MonoBehaviour
{
    public MovingPlatform PlatformHandler;
    
    private Vector3 _previousPosition;
    private Vector3 _velocity;
    // Start is called before the first frame update
    void Start()
    {
        PlatformHandler=transform.parent.GetComponent<MovingPlatform>();
        _previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _velocity = (transform.position - _previousPosition) / Time.deltaTime;
        _previousPosition = transform.position;
    }

    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player on platform : " + other.name);
            other.transform.SetParent(PlatformHandler.Platform.transform);
            other.GetComponent<playerMovement>().YPlatformVelocity=GetVelocity();
        }
    }

    

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player off platform: " + other.name);
            other.transform.SetParent(null);
            other.GetComponent<playerMovement>().YPlatformVelocity=Vector3.zero;

        }
    }


    // player script gets the platform's velocity from here
    public Vector3 GetVelocity()
    {
        return _velocity;
    }
}
