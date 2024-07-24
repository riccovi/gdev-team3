using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevierActivator : Activator
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void onEnable()
    {
        Debug.Log("Activate Levier");
    }

    public override void onDissable()
    {
        Debug.Log("DisActivate Levier");
    }
}
