using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupHandler : MonoBehaviour
{
    public ActivatableObject WhatActivates;
    public Sprite icon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            if(WhatActivates!=null)
            {
                other.GetComponent<InventoryManager>().AddToInventory(WhatActivates,icon,this.transform.parent.gameObject);            
            }
            else
            {
                other.GetComponent<InventoryManager>().AddToInventory(null,icon,this.transform.parent.gameObject);            
            }
        }
    }
}
