using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler_Activations : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(other.CompareTag("Levier"))
        {
            var activator = other.transform.parent.GetComponent<Activator>();

            transform.parent.GetComponent<Wrench>().StopThrowMovement();

            activator.onEnable();
        }
        
    }

    private void  OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Levier"))
        {
            var activator = other.transform.parent.GetComponent<Activator>();

            activator.onDissable();
        }
        
    }
}
