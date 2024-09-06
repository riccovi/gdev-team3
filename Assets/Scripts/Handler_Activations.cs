using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler_Activations : MonoBehaviour
{
    public Wrench wrench;
    // Start is called before the first frame update
    void Start()
    {
        wrench=transform.parent.GetComponent<Wrench>();
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
        else if(other.CompareTag("Rope") && wrench.isRotating)//is on move
        {
            other.GetComponent<RopeTrigger>().doDamage(1);
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
