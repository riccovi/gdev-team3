using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStats : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth=MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addHealth(int health)
    {
        var CurHealth = CurrentHealth+health;
        CurrentHealth = Mathf.Clamp(CurHealth,0,MaxHealth);
        UIHandler.instance.addHealth();

    }

    public void removeHealth(int health)
    {
        var CurHealth = CurrentHealth-health;
        CurrentHealth = Mathf.Clamp(CurHealth,0,MaxHealth);

        UIHandler.instance.removeHealth();

        if(CurrentHealth==0)
        {
            GameManager.instance.GameOverSequence(false);
        }

    }
}
