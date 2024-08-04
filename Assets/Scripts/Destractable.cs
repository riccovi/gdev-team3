using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destractable : MonoBehaviour
{
    public int MaxHealth;
    public int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth=MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void doDamage(int damage)
    {
        currentHealth=currentHealth-damage;

        if(currentHealth<=0)
        {
            Destroy(this.gameObject);
        }
    }
}
