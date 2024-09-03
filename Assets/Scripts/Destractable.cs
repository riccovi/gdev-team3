using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destractable : MonoBehaviour
{
    public bool cameraShake;
    public int MaxHealth;
    public int currentHealth;

    public string DestroySound;

    public Transform DestroyEffect;
    // Start is called before the first frame update
    void Start()
    {
        DestroySound="BoxDestroy";
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
            DestroyEffect.SetParent(null);
            DestroyEffect.GetComponent<ParticleSystem>().Play();
            GameManager.instance.DestroyGameObjectTime(DestroyEffect,3);
            
            if(cameraShake)
            {
                GameManager.instance.ShakeCamera("Medium");
            }

            Sound_Manager.instance.environmentSoundOnce(DestroySound);
            Destroy(this.gameObject);
        }
    }
}
