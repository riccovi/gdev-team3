using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActivator : Activator
{
    public enum DoorType
    {
        KeyUnlock,
        LevierUnlock
    }
    public DoorType doorType;

    [Header("Assign Door To Activate")]
    public Animator Door;

    [Header("Assign Push Levier Animator")]
    public Animator ActivatorAnimation;
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
        Debug.Log("Activate Door");
        Door.SetTrigger("OpenDoor");
        ActivatorAnimation.SetTrigger("Push");
    }

    public override void onDissable()
    {
        Debug.Log("DisActivate Door");
        Door.SetTrigger("CloseDoor");
        ActivatorAnimation.SetTrigger("Release");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(doorType==DoorType.KeyUnlock)
        {
            if(other.CompareTag("Player") && other.GetComponent<InventoryManager>().checkItems(transform.parent.name))
            {
                onEnable();
            }
        }
    }
}
