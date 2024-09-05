using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupHandler : MonoBehaviour
{
    public enum objectPickupType
    {
        Activator,
        Upgrade
    }

    public objectPickupType ObjectType;
    public ActivatableObject WhatActivates;
    public Sprite icon;

    public TipsScriptableObject tipData;

    public Transform pickupEffect;

    public string pickupSound;


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
            pickupEffect.transform.SetParent(null);
            pickupEffect.GetComponent<ParticleSystem>().Play();
            Sound_Manager.instance.environmentSoundOnce(pickupSound);
            GameManager.instance.DestroyGameObjectTime(pickupEffect,1.5f);


            if(tipData!=null)
            {
                UIHandler.instance.ActivatePopUp(tipData.Description);
            }

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
