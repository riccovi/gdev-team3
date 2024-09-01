using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingToolHandler : MonoBehaviour
{
    public PoolHandler poolHandler;
    public int Damage=1;

    public float rotateSpead=100;

     public float accelerationGravity = 9.8f; // Acceleration value

    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb=transform.parent.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < 0) // Ensure the object is falling
        {
            rb.velocity += Vector2.down * accelerationGravity * Time.fixedDeltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.Rotate(0,0,rotateSpead*Time.deltaTime);
    }


    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayStats>().removeHealth(Damage);
            poolHandler.returnPool(this.transform.parent.gameObject);
        }   
        else if(other.CompareTag("Ground"))     
        {
            poolHandler.returnPool(this.transform.parent.gameObject);
        }
    }



}
