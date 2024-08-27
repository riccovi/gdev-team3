using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactActivator : Activator
{
    public enum activateTyp
    {
        PressButtonE,
        OnObjectPlacement

    }

    public activateTyp ActivationType;
    [Header("Assign Activate Object")]
    public Enabler ObjectToActivate;

    [Header("Assign Activator Animator")]
    public Animator ActivatorAnimation;
    // Start is called before the first frame update

    [Header("On close up tip")]
    public TipsScriptableObject tipData;

    public Transform SpriteTip;

    public string soundEffectEnable="Levier_Enable";
    public string soundEffectDisable="Levier_Disable";

    public enum PlatformStatus
    {
        Disable,
        Enable
    }
    public PlatformStatus currentStatus;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void onEnable()
    {
        ObjectToActivate.Enable();
        ActivatorAnimation.SetTrigger("Push");

        currentStatus=PlatformStatus.Enable;

        Sound_Manager.instance.environmentSoundOnce(soundEffectEnable);

        if(tipData!=null)
        {
            UIHandler.instance.ActivatePopUp(tipData.Description);
        }
    }

    public override void onDissable()
    {
        ObjectToActivate.Disable();
        ActivatorAnimation.SetTrigger("Release");

        Sound_Manager.instance.environmentSoundOnce(soundEffectDisable);

        currentStatus=PlatformStatus.Disable;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && SpriteTip!=null )
        {
            SpriteTip.gameObject.SetActive(true);
        }

        if(ActivationType==activateTyp.OnObjectPlacement)
        {
            //Activate With Anything Else
            if(currentStatus==PlatformStatus.Disable)
            {
                 onEnable();
            }
            else
            {
                onDissable();
            }
        }
        
    }

    void OnTriggerStay2D(Collider2D other) {

        if(ActivationType==activateTyp.PressButtonE)
        {
            if(other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
            {
                if(currentStatus==PlatformStatus.Disable)
                {
                    onEnable();
                    currentStatus=PlatformStatus.Enable;
                }
                else
                {
                    onDissable();
                    currentStatus=PlatformStatus.Disable;
                }
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player") && SpriteTip!=null)
        {
            SpriteTip.gameObject.SetActive(false);
        }

        if(ActivationType==activateTyp.OnObjectPlacement)
        {
            //Activate With Anything Else
            if(currentStatus==PlatformStatus.Disable)
            {
                 onEnable();
                 currentStatus=PlatformStatus.Enable;
            }
            else
            {
                onDissable();
                currentStatus=PlatformStatus.Disable;
            }
        }
    }
}
