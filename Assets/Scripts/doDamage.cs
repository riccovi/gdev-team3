using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doDamage : MonoBehaviour
{
    public bool destroyOnCollision;
    public int DamageAmount=1;

    public float ThrowBackForce;


    public Transform forceDirection;
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
            other.GetComponent<PlayStats>().removeHealth(DamageAmount);
            other.GetComponent<Rigidbody2D>().AddForce(forceDirection.transform.right*ThrowBackForce,ForceMode2D.Impulse);

        }        
    }
}
