using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
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
            var player = other.GetComponent<playerMovement>();
            player.ResetAllAnimationTrigger();
            player.anim.SetTrigger("Idle");

            Sound_Manager.instance.PlayerSoundHandler_Loop.Stop();
            Sound_Manager.instance.PlayerSoundHandler_Once.Stop();

            GameManager.instance.GameOverSequence(true);

        }
    }
}
