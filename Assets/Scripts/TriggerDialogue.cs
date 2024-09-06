using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    public Sprite Dialogue;
    public SpriteRenderer DialogueHolde;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        
        if(other.CompareTag("Player"))
        {
            Debug.Log("Chat" + other.name);
            DialogueHolde.gameObject.SetActive(true);
            DialogueHolde.sprite=Dialogue;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        
        if(other.CompareTag("Player"))
        {
            Debug.Log("Chat" + other.name);
            DialogueHolde.gameObject.SetActive(false);
            DialogueHolde.sprite=null;
        }
    }
}
