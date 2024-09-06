using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enabler : MonoBehaviour
{
    public UnityEvent OnEnable;
    public UnityEvent OnDisable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Enable()
    {
        OnEnable.Invoke();
    }

    public void Disable()
    {
        OnDisable.Invoke();
    }

    
}
