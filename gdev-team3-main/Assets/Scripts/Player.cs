using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{



    public enum state
    {
        Alive,
        Dead
    }

    public state currentState;
    public int health;
    public int moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        currentState= state.Alive;   
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState==state.Alive)
        {
            InputHandler();
        }
    }

    public void InputHandler()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0)* moveSpeed *Time.fixedDeltaTime;
    }
}
